using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class TrendUserController : ControllerBase
{
    private readonly DBContext context;
    public readonly IMisionsFunction misionsFunction;
    public TrendUserController(DBContext dBContext, IMisionsFunction misions)
    {
        context = dBContext;
        misionsFunction = misions;
    }
    [HttpGet("GetTendency/{username}")]
    public async Task<IActionResult> GetTendency(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var tendency = await context.Trends.ToListAsync();
        var TrendUser = await context.TrendsUsers.Where(u => u.IdUser == user.Id).ToListAsync();
        var refer = await context.ReferLevel1s.Where(u => u.UniqueCodeReferrer == user.Code).Select(u => u.UniqueCodeReFerred).ToListAsync();
        var usersid = await context.Users.Where(u => refer.Contains(u.Code)).Select(u => u.Id).ToListAsync();
        var rechargeLogs = await context.RechargeLogs.Where(u => usersid.Contains(u.IdUser)).CountAsync();
        List<MisionsBack> misionsBack = new List<MisionsBack>();
        foreach (var i in tendency)
        {
            var findmision = TrendUser.FirstOrDefault(u => u.IDTrend == i.IdTendency);
            if (findmision == null)
            {
                MissionsUser missionsUser = new MissionsUser
                {
                    UserID = user.Id,
                    IDMission = i.IdTendency,
                    IsClaimed = false,
                    Missions = null,
                    User = null
                };
                await context.MissionsUsers.AddAsync(missionsUser);
                await context.SaveChangesAsync();
                MisionsBack misionsBack1 = new MisionsBack
                {
                    Id = i.IdTendency,
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
                    Id = i.IdTendency,
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
            var mision = await context.Trends.FirstOrDefaultAsync(u => u.IdTendency == misionToClaim.IDMission);
            var disponibility = await context.DisponibilityToClaims.FirstOrDefaultAsync(u => u.UserID == user.Id);
            if(mision == null){
                return NotFound(new { message = "La mision no existe en la aplicacion" });
            }
            IsClaimed isClaimed = new IsClaimed{
                IDMission = mision.IdTendency,
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
            var misionUser = await context.TrendsUsers.FirstOrDefaultAsync(u => u.IdUser == user.Id && u.IDTrend == mision.IdTendency);
            if(misionUser == null){
                return NotFound(new { message = "El usuario no esta en la mision" });
            }
            misionUser.IsClaimed = true;
            context.Entry(misionUser).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(new { message = "Mision Relamada" });
            }
}