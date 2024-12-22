using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class ReferController : ControllerBase
{
    private readonly DBContext context;
    public ReferController(DBContext dbcontext)
    {
        context = dbcontext;
    }
    [HttpGet("GetReferrer/{username}")]
    public async Task<IActionResult> GetReferrer(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(option => option.Email == username || option.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var refer = await context.ReferLevel1s.Where(option => option.UniqueCodeReferrer == user.Code).ToListAsync();
        var refer2 = await context.ReferLevel2s.Where(option => option.UniqueCodeReferrer == user.Code).ToListAsync();
        var refer3 = await context.ReferLevel3s.Where(option => option.UniqueCodeReferrer == user.Code).ToListAsync();
        var Profits = await context.BenefitPerRefers.FirstOrDefaultAsync(option => option.Username == user.Email || option.Username == user.PhoneNumber);
        if (Profits != null)
        {
            QuantityRefersLevelsBack quantityRefersLevelsBack = new QuantityRefersLevelsBack
            {
                Level1Earnings = Profits.Nivel1,
                Level2Earnings = Profits.Nivel2,
                Level3Earnings = Profits.Nivel3,
                QuantityRefersLevel1 = refer.Count,
                QuantityRefersLevel2 = refer2.Count,
                QuantityRefersLevel3 = refer3.Count,
            };
            return Ok(quantityRefersLevelsBack);
        }
        QuantityRefersLevelsBack quantityRefersLevels = new QuantityRefersLevelsBack
        {
            Level1Earnings = 0,
            Level2Earnings = 0,
            Level3Earnings = 0,
            QuantityRefersLevel1 = refer.Count,
            QuantityRefersLevel2 = refer2.Count,
            QuantityRefersLevel3 = refer3.Count,
        };
        return Ok(quantityRefersLevels);
    }
    [HttpGet("GetTeamParameters/{username}")]
    public async Task<IActionResult> GetTeamParameters(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(option => option.Email == username || option.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var refer = await context.Users.Include(option => option.referLevel1s).Include(option => option.referLevel2s).Include(option => option.referLevel3s).Include(option => option.rechargeLogs).Include(u => u.withdrawLogs).FirstOrDefaultAsync(option => option.Id == user.Id);
        int TeamSize = 0;
        float TeamRecharge = 0;
        DateTime today = DateTime.UtcNow;
        int TeamSizeToday = 0;
        float TeamRechargeToday = 0;
        float TeamWithdrawToday = 0;
        float TeamWithdraw = 0;
        if (refer != null)
        {
            TeamSize = (refer.referLevel1s?.Count ?? 0) +
            (refer.referLevel2s?.Count ?? 0) +
            (refer.referLevel3s?.Count ?? 0);
            if (refer.referLevel1s != null)
            {
                var referLevel1 = refer.referLevel1s.Select(option => option.UniqueCodeReFerred).ToList();
                var referLevel1_id = await context.Users.Where(u => referLevel1.Contains(u.Code)).Select(u => u.Id).ToListAsync();
                var recharge = await context.RechargeLogs.Where(log => referLevel1_id.Contains(log.IdUser)).ToListAsync();
                var withdraw = await context.WithdrawLogs.Where(log => referLevel1_id.Contains(log.UserId)).ToListAsync();
                TeamSizeToday += await context.Users.Where(u => referLevel1.Contains(u.Code) && u.Date.Date == today.Date).CountAsync();
                TeamRecharge += recharge.Sum(u => u.Recharge);
                TeamWithdraw += withdraw.Sum(u => u.Withdraw);
                TeamRechargeToday += recharge.Where(u => u.Date.Date == today.Date).Sum(u => u.Recharge);
                TeamWithdrawToday += withdraw.Where(u => u.Date.Date == today.Date).Sum(u => u.Withdraw);
            }
            if (refer.referLevel2s != null)
            {
                var referLevel2 = refer.referLevel2s.Select(u => u.UniqueCodeReFerred).ToList();
                var referLevel2_id = await context.Users.Where(u => referLevel2.Contains(u.Code)).Select(u => u.Id).ToListAsync();
                var recharge = await context.RechargeLogs.Where(log => referLevel2_id.Contains(log.IdUser)).ToListAsync();
                var withdraw = await context.WithdrawLogs.Where(u => referLevel2_id.Contains(u.UserId)).ToListAsync();
                TeamSizeToday += await context.Users.Where(u => referLevel2.Contains(u.Code) && u.Date.Date == today.Date).CountAsync();
                TeamWithdraw += withdraw.Sum(u => u.Withdraw);
                TeamRecharge += recharge.Sum(u => u.Recharge);
                TeamRechargeToday += recharge.Where(u => u.Date.Date == today.Date).Sum(u => u.Recharge);
                TeamWithdrawToday += withdraw.Where(u => u.Date.Date == today.Date).Sum(u => u.Withdraw);
            }
            if (refer.referLevel3s != null)
            {
                var referLevel3 = refer.referLevel3s.Select(u => u.UniqueCodeReFerred).ToList();
                var referLevel3_id = await context.Users.Where(u => referLevel3.Contains(u.Code)).Select(u => u.Id).ToListAsync();
                var recharge = await context.RechargeLogs.Where(log => referLevel3_id.Contains(log.IdUser)).ToListAsync();
                var withdraw = await context.WithdrawLogs.Where(u => referLevel3_id.Contains(u.UserId)).ToListAsync();
                TeamSizeToday += await context.Users.Where(u => referLevel3.Contains(u.Code) && u.Date.Date == today.Date).CountAsync();
                TeamRecharge += recharge.Sum( u => u.Recharge);
                TeamWithdraw += withdraw.Sum( u => u.Withdraw);
                TeamRechargeToday += recharge.Where(u => u.Date.Date == today.Date).Sum( u => u.Recharge);
                TeamWithdrawToday += withdraw.Where(u => u.Date.Date == today.Date).Sum( u => u.Withdraw);
            }
            if (refer.rechargeLogs != null && refer.withdrawLogs != null)
            {
                TeamRecharge += refer.rechargeLogs.Sum(log => log.Recharge);
                TeamRechargeToday += refer.rechargeLogs.Where(u => u.Date.Date == today.Date).Sum(u => u.Recharge);
                TeamWithdraw += refer.withdrawLogs.Sum(u => u.Withdraw);
                TeamWithdrawToday += refer.withdrawLogs.Where( u=> u.Date.Date == today.Date).Sum(u => u.Withdraw);
            }
        }
        ParametersOfRefers parametersOfRefers = new ParametersOfRefers{
            NewTeamToday = TeamSizeToday,
            NewTeamTodayRecharge = TeamRechargeToday,
            TeamRecharge = TeamRecharge,
            TeamSize = TeamSize,
            TeamWithdraw = TeamWithdraw,
            TeamWithdrawToday = TeamWithdrawToday
        };
        return Ok(parametersOfRefers);
    }
}