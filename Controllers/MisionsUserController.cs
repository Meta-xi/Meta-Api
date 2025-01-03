using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class MisionsUserController : ControllerBase
{
    private readonly DBContext context;
    public readonly IMisionsFunction misionsFunction;
    public MisionsUserController(DBContext dBContext, IMisionsFunction misions)
    {
        context = dBContext;
        misionsFunction = misions;
    }
    [HttpGet("GetMissions/{username}")]
    public async Task<IActionResult> GetMisions(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var misions = await context.Missionss.ToListAsync();
        var misionsUser = await context.MissionsUsers.Where(u => u.UserID == user.Id).ToListAsync();
        var refer = await context.ReferLevel1s.Where(u => u.UniqueCodeReferrer == user.Code).Select(u => u.UniqueCodeReFerred).ToListAsync();
        var usersid = await context.Users.Where(u => refer.Contains(u.Code)).Select(u => u.Id).ToListAsync();
        var rechargeLogs = await context.RechargeLogs.Where(u => usersid.Contains(u.IdUser)).CountAsync();
        List<MisionsBack> misionsBack = new List<MisionsBack>();
        foreach (var i in misions)
        {
            var findmision = misionsUser.FirstOrDefault(u => u.IDMission == i.IDMission);
            if (findmision == null)
            {
                MissionsUser missionsUser = new MissionsUser
                {
                    UserID = user.Id,
                    IDMission = i.IDMission,
                    IsClaimed = false,
                    Missions = null,
                    User = null
                };
                await context.MissionsUsers.AddAsync(missionsUser);
                await context.SaveChangesAsync();
                MisionsBack misionsBack1 = new MisionsBack
                {
                    Id = i.IDMission,
                    Title = i.Name,
                    Goal = i.Goal,
                    Progress = rechargeLogs,
                    Claimed = missionsUser.IsClaimed,
                    Reward = i.Reward,
                };
                misionsBack.Add(misionsBack1);
            }
            else
            {
                MisionsBack misionsBack1 = new MisionsBack
                {
                    Id = i.IDMission,
                    Title = i.Name,
                    Goal = i.Goal,
                    Progress = rechargeLogs,
                    Claimed = findmision.IsClaimed,
                    Reward = i.Reward
                };
                misionsBack.Add(misionsBack1);
            }
        }
        return Ok(misionsBack);
    }
    [HttpPost("LogToClaim")]
    public async Task<IActionResult> LogToClaim(MisionToClaim misionToClaim){
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == misionToClaim.Username || u.PhoneNumber == misionToClaim.Username);
        if (user == null){
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
            var mision = await context.Missionss.FirstOrDefaultAsync(u => u.IDMission == misionToClaim.IDMission);
            var disponibility = await context.DisponibilityToClaims.FirstOrDefaultAsync(u => u.UserID == user.Id);
            if(mision == null){
                return NotFound(new { message = "La mision no existe en la aplicacion" });
            }
            IsClaimed isClaimed = new IsClaimed{
                IDMission = mision.IDMission,
                UserID = user.Id,
                DateClaimed = DateTime.UtcNow,
                Missions = null,
                User = null
            };
            await context.IsClaimeds.AddAsync(isClaimed);
            await context.SaveChangesAsync();
            if(disponibility == null){
                DisponibilityToClaim disponibilityToClaim = new DisponibilityToClaim{
                    Disponibility = mision.Reward,
                    UserID = user.Id,
                    User = null
                };
                await context.DisponibilityToClaims.AddAsync(disponibilityToClaim);
                await context.SaveChangesAsync();
            }
            else{
                disponibility.Disponibility += mision.Reward;
                context.Entry(disponibility).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            var misionUser = await context.MissionsUsers.FirstOrDefaultAsync(u => u.UserID == user.Id && u.IDMission == mision.IDMission);
            if(misionUser == null){
                return NotFound(new { message = "El usuario no esta en la mision" });
            }
            misionUser.IsClaimed = true;
            context.Entry(misionUser).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(new { message = "Mision Relamada" });
            }
            [HttpGet("GetDates/{username}")]
            public async Task<IActionResult> GetDates(string username){
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
                if (user == null){
                    return NotFound(new { message = "El usuario no existe en la aplicacion" });
                }
                var usermisions = await context.IsClaimeds.Where(u => u.UserID == user.Id).CountAsync();
                var userMisionToday = await context.IsClaimeds.Where(u => u.UserID == user.Id && u.DateClaimed.Date == DateTime.UtcNow.Date).CountAsync();
                var disponibility = await context.DisponibilityToClaims.FirstOrDefaultAsync(u => u.UserID == user.Id);
                if(disponibility == null){
                    return NotFound(new { message = "No tienes disponibilidad para la mision" });
                }
                DatesBack datesBack = new DatesBack{
                    Disponibility = disponibility.Disponibility,
                    QuantityMisions = usermisions,
                    QuantityMisionsToday = userMisionToday
                };
                return Ok(datesBack);
            }
            [HttpGet("UpdateWallet/{username}")]
            public async Task<IActionResult> UpdateWallet(string username){
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
                if (user == null){
                    return NotFound(new { message = "El usuario no existe en la aplicacion" });
                }
                var wallet = await context.Wallets.FirstOrDefaultAsync(u => u.Email ==username);
                if(wallet == null){
                    return NotFound(new { message = "El usuario no tiene wallet" });
                }
                var disponibility = await context.DisponibilityToClaims.FirstOrDefaultAsync(u => u.UserID == user.Id);
                if(disponibility == null){
                    return NotFound(new { message = "No tienes disponibilidad para la mision" });
                }
                wallet.Balance += disponibility.Disponibility;
                context.Entry(wallet).State = EntityState.Modified;
                await context.SaveChangesAsync();
                disponibility.Disponibility = 0;
                context.Entry(disponibility).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "Billetera actualizada" });
            }

        }
    

