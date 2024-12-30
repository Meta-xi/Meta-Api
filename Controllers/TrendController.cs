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
        int plan3 = 0, plan4 = 0, plan5 = 0, plan6 = 0;
        var tendency = await context.Trends.ToListAsync();
        var TrendUser = await context.TrendsUsers.Where(u => u.IdUser == user.Id).ToListAsync();
        var refer = await context.ReferLevel1s.Where(u => u.UniqueCodeReferrer == user.Code).Select(u => u.UniqueCodeReFerred).ToListAsync();
        var users = await context.Users.Where(u => refer.Contains(u.Code)).Select(u => u.Email ?? u.PhoneNumber).ToListAsync();
        var plans = await context.Plans.ToListAsync();
        var userPlans = await context.UserPlans.Where(u => users.Contains(u.Username)).ToListAsync();
        List<MisionsBack> misionsBack = new List<MisionsBack>();
        foreach (var i in userPlans)
        {
            foreach (var j in plans)
            {
                if (j.Name == i.NamePlan && j.IDPlan == 3)
                {
                    plan3 += 1;
                }
                else if (j.Name == i.NamePlan && j.IDPlan == 4)
                {
                    plan4 += 1;
                }
                else if (j.Name == i.NamePlan && j.IDPlan == 5)
                {
                    plan5 += 1;
                }
                else if (j.Name == i.NamePlan && j.IDPlan == 6)
                {
                    plan6 += 1;
                }
            }
        }
        foreach (var i in tendency)
        {
            var findmision = TrendUser.FirstOrDefault(u => u.IDTrend == i.IdTendency);
            if (findmision == null)
            {
                TrendUser trendUser = new TrendUser
                {
                    IDTrend = i.IdTendency,
                    IdUser = user.Id,
                    IsClaimed = false,
                    Trend = null,
                    User = null
                };
                await context.TrendsUsers.AddAsync(trendUser);
                await context.SaveChangesAsync();
                switch(i.IdTendency){
                    case 1:
                        MisionsBack misionsBack1 = new MisionsBack{
                            Id = i.IdTendency,
                            Title = i.Name,
                            Goal = i.Goal,
                            Progress = plan3,
                            Claimed = trendUser.IsClaimed,
                            Reward = i.Reward,
                        };
                        misionsBack.Add(misionsBack1);
                        break;
                    case 2:
                        MisionsBack misionsBack2 = new MisionsBack{
                            Id = i.IdTendency,
                            Title = i.Name,
                            Goal = i.Goal,
                            Progress = plan4,
                            Claimed = trendUser.IsClaimed,
                            Reward = i.Reward,
                        };
                        misionsBack.Add(misionsBack2);
                        break;
                    case 3:
                        MisionsBack misionsBack3 = new MisionsBack{
                            Id = i.IdTendency,
                            Title = i.Name,
                            Goal = i.Goal,
                            Progress = plan5,
                            Claimed = trendUser.IsClaimed,
                            Reward = i.Reward,
                        };
                        misionsBack.Add(misionsBack3);
                        break;
                    case 4:
                        MisionsBack misionsBack4 = new MisionsBack{
                            Id = i.IdTendency,
                            Title = i.Name,
                            Goal = i.Goal,
                            Progress = plan6,
                            Claimed = trendUser.IsClaimed,
                            Reward = i.Reward,
                        };
                        misionsBack.Add(misionsBack4);
                        break;
                        }
                }
                else{
                    switch(i.IdTendency){
                        case 1:
                            MisionsBack misionsBack1 = new MisionsBack{
                                Id = i.IdTendency,
                                Title = i.Name,
                                Goal = i.Goal,
                                Progress = plan3,
                                Claimed = findmision.IsClaimed,
                                Reward = i.Reward
                            };
                            misionsBack.Add(misionsBack1);
                            break;
                        case 2:
                            MisionsBack misionsBack2 = new MisionsBack{
                                Id = i.IdTendency,
                                Title = i.Name,
                                Goal = i.Goal,
                                Progress = plan4,
                                Claimed = findmision.IsClaimed,
                                Reward = i.Reward
                            };
                            misionsBack.Add(misionsBack2);
                            break;
                        case 3:
                            MisionsBack misionsBack3 = new MisionsBack{
                                Id = i.IdTendency,
                                Title = i.Name,
                                Goal = i.Goal,
                                Progress = plan5,
                                Claimed = findmision.IsClaimed,
                                Reward = i.Reward
                            };
                            misionsBack.Add(misionsBack3);
                            break;
                        case 4:
                            MisionsBack misionsBack4 = new MisionsBack{
                                Id = i.IdTendency,
                                Title = i.Name,
                                Goal = i.Goal,
                                Progress = plan6,
                                Claimed = findmision.IsClaimed,
                                Reward = i.Reward
                            };
                            misionsBack.Add(misionsBack4);
                            break;
                    }
                }
            }
            return Ok(misionsBack);
        }
    

    [HttpPost("LogToClaim")]
    public async Task<IActionResult> LogToClaim(MisionToClaim misionToClaim)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == misionToClaim.Username || u.PhoneNumber == misionToClaim.Username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var mision = await context.Trends.FirstOrDefaultAsync(u => u.IdTendency == misionToClaim.IDMission);
        var disponibility = await context.DisponibilityToClaims.FirstOrDefaultAsync(u => u.UserID == user.Id);
        if (mision == null)
        {
            return NotFound(new { message = "La mision no existe en la aplicacion" });
        }
        IsClaimed isClaimed = new IsClaimed
        {
            IDMission = mision.IdTendency,
            UserID = user.Id,
            DateClaimed = DateTime.UtcNow,
            Missions = null,
            User = null
        };
        await context.IsClaimeds.AddAsync(isClaimed);
        await context.SaveChangesAsync();
        if (disponibility == null)
        {
            DisponibilityToClaim disponibilityToClaim = new DisponibilityToClaim
            {
                Disponibility = mision.Reward,
                UserID = user.Id,
                User = null
            };
            await context.DisponibilityToClaims.AddAsync(disponibilityToClaim);
            await context.SaveChangesAsync();
        }
        else
        {
            disponibility.Disponibility += mision.Reward;
            context.Entry(disponibility).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        var misionUser = await context.TrendsUsers.FirstOrDefaultAsync(u => u.IdUser == user.Id && u.IDTrend == mision.IdTendency);
        if (misionUser == null)
        {
            return NotFound(new { message = "El usuario no esta en la mision" });
        }
        misionUser.IsClaimed = true;
        context.Entry(misionUser).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(new { message = "Mision Relamada" });
    }
    [HttpGet("GetCompletedMissions/{username}")]
    public async Task<IActionResult> GetCompletedMissions(string username){
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
        int id = 0 ;
        if(user == null){
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var userMissions = await context.MissionsUsers.Where(u => u.UserID == user.Id && u.IsClaimed == true).Select(u => u.IDMission).ToListAsync();
        var mision = await context.Missionss.Where(u => userMissions.Contains(u.IDMission)).ToListAsync();
        var userTrends = await context.TrendsUsers.Where(u => u.IdUser == user.Id && u.IsClaimed == true).Select(u => u.IDTrend).ToListAsync();
        var trend = await context.Trends.Where(u => userTrends.Contains(u.IdTendency)).ToListAsync();
        List<CompletedMisionsBack> completedMisionsBacks = new List<CompletedMisionsBack>();
        foreach( var i in mision){
            CompletedMisionsBack completedMisionsBack = new CompletedMisionsBack{
                Id = id + 1,
                Title = i.Name,
                Reward = i.Reward
            };
            id++;
            completedMisionsBacks.Add(completedMisionsBack);
        }
        foreach(var i in trend){
            CompletedMisionsBack completedMisionsBack = new CompletedMisionsBack{
                Id = id + 1,
                Title = i.Name,
                Reward = i.Reward
            };
            id++;
            completedMisionsBacks.Add(completedMisionsBack);
        }

        return Ok(completedMisionsBacks);
    }
}