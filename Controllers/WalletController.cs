using Meta.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly DBContext context;
    private readonly IBenefitPerRefer benefitPerRefer;
    public WalletController(DBContext _context, IBenefitPerRefer _benefitPerRefer)
    {
        context = _context;
        benefitPerRefer = _benefitPerRefer;
    }
    [HttpPost("UpdateBalance")]
    public async Task<IActionResult> UpdateBalance(UpdateBalance updateBalance)
    {
        GetMoneyValues getMoneyValues = new GetMoneyValues();
        var user = await context.Users.FirstOrDefaultAsync(option => option.Email == updateBalance.Email || option.PhoneNumber == updateBalance.Email);
        if (user == null)
        {
            return NotFound(new { message = "No existe ninguna cuenta con ese correo" });
        }
        var wallet = await context.Wallets.FirstOrDefaultAsync(option => option.Email == updateBalance.Email);
        if (wallet == null)
        {
            return NotFound(new { message = "No existe ninguna cartera con ese correo" });
        }
        BenefitOperation benefitOperation = new BenefitOperation
        {
            PricePlan = updateBalance.Balance,
            Username = updateBalance.Email
        };
        bool result3 = await benefitPerRefer.RegisterBenefitLevel3(benefitOperation);
        if (!result3)
        {
            Console.WriteLine("No se actualizo para level3");
        }
        else
        {
            Console.WriteLine("Se actualizo para level3");
        }
        bool result2 = await benefitPerRefer.RegisterBenefitLevel2(benefitOperation);
        if (!result2)
        {
            Console.WriteLine("No se actualizo para level2");
        }
        else
        {
            Console.WriteLine("Se actualizo para level2");
        }
        bool result = await benefitPerRefer.RegisterBenefitLevel1(benefitOperation);
        if (!result)
        {
            Console.WriteLine("No se actualizo para level1");
        }
        else
        {
            Console.WriteLine("Se actualizo para level3");
        }
        string token = updateBalance.Token.ToLower();
        switch (token)
        {
            case "nequi":
                wallet.Balance += updateBalance.Balance;
                context.Entry(wallet).State = EntityState.Modified;
                await context.SaveChangesAsync();
                RechargeLog rechargeLog = new RechargeLog
                {
                    IdUser = user.Id,
                    Recharge = updateBalance.Balance,
                    Date = DateTime.UtcNow,
                    User = null
                };
                await context.RechargeLogs.AddAsync(rechargeLog);
                await context.SaveChangesAsync();
                return Ok(new { message = "Balance actualizada correctamente" });
            case "trx":
                decimal balance = await getMoneyValues.GetMoneyValueAsync("trx");
                float value = (float)balance;
                Console.WriteLine(value);
                decimal usdToCop = await getMoneyValues.GetMoneyValueAsync("cop");
                float usd = (float)usdToCop;
                Console.WriteLine(usd);
                wallet.Balance = wallet.Balance + (value * updateBalance.Balance * usd);
                context.Entry(wallet).State = EntityState.Modified;
                await context.SaveChangesAsync();
                RechargeLog rechargeLogtrx = new RechargeLog
                {
                    IdUser = user.Id,
                    Recharge = updateBalance.Balance * value * usd,
                    Date = DateTime.UtcNow,
                    User = null
                };
                await context.RechargeLogs.AddAsync(rechargeLogtrx);
                await context.SaveChangesAsync();
                return Ok(new { message = "Balance actualizado correctamente" });
            case "usdt_trc20":
                decimal balance2 = await getMoneyValues.GetMoneyValueAsync("tether");
                float value2 = (float)balance2;
                decimal usdToCop2 = await getMoneyValues.GetMoneyValueAsync("cop");
                float usd2 = (float)usdToCop2;
                wallet.Balance = wallet.Balance + (value2 * updateBalance.Balance * usd2);
                context.Entry(wallet).State = EntityState.Modified;
                await context.SaveChangesAsync();
                RechargeLog rechargeLogusdt_trc20 = new RechargeLog
                {
                    IdUser = user.Id,
                    Recharge = updateBalance.Balance,
                    Date = DateTime.UtcNow,
                    User = null
                };
                await context.RechargeLogs.AddAsync(rechargeLogusdt_trc20);
                await context.SaveChangesAsync();
                return Ok(new { message = "Balance actualizado correctamente" });
            case "paypal":
                decimal balance3 = await getMoneyValues.GetMoneyValueAsync("cop");
                float value3 = (float)balance3;
                wallet.Balance = wallet.Balance + (value3 * updateBalance.Balance);
                context.Entry(wallet).State = EntityState.Modified;
                await context.SaveChangesAsync();
                RechargeLog rechargeLogpaypal = new RechargeLog
                {
                    IdUser = user.Id,
                    Recharge = updateBalance.Balance,
                    Date = DateTime.UtcNow,
                    User = null
                };
                await context.RechargeLogs.AddAsync(rechargeLogpaypal);
                await context.SaveChangesAsync();
                return Ok(new { message = "Balance actualizado correctamente" });
            default:
                return NotFound(new { message = "Token no soportado" });
        }
        

    }
    [HttpGet("GetBalance/{username}")]
    public async Task<IActionResult> GetBalance(string username)
    {
        var wallet = await context.Wallets.FirstOrDefaultAsync(option => option.Email == username);
        if (wallet == null)
        {
            return NotFound(new { message = "El usuario no posee ninguna cartera" });
        }
        return Ok(wallet.Balance);
    }
    [HttpPatch("WithdrawBalance")]
    public async Task<IActionResult> WithdrawalBalance(WitdrawBalance witdrawBalance)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == witdrawBalance.Username || u.PhoneNumber == witdrawBalance.Username);
        if (user == null)
        {
            return NotFound(new { message = "No existe ninguna cuenta con ese correo" });
        }
        var wallet = await context.Wallets.FirstOrDefaultAsync(u => u.Email == witdrawBalance.Username);
        if (wallet == null)
        {
            return NotFound(new { message = "El usuario no posee ninguna cartera" });
        }
        if (wallet.Balance < witdrawBalance.Withdraw)
        {
            return NotFound(new { message = "No hay suficiente saldo" });
        }
        wallet.Balance -= witdrawBalance.Withdraw;
        context.Entry(wallet).State = EntityState.Modified;
        await context.SaveChangesAsync();
        WithdrawLog withdrawLog = new WithdrawLog
        {
            Date = DateTime.UtcNow,
            User = null,
            Withdraw = witdrawBalance.Withdraw,
            UserId = user.Id
        };
        await context.WithdrawLogs.AddAsync(withdrawLog);
        await context.SaveChangesAsync();
        return Ok(new { message = "Balance actualizado correctamente" });
    }
    [HttpGet("GetBalanceUsdAndCop/{username}")]
    public async Task<IActionResult> GetBalanceUsdAndCop(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.Email == username);
        if (wallet == null)
        {
            return NotFound(new { message = "Billetera no encontrada" });
        }
        GetMoneyValues getMoneyValues = new GetMoneyValues();
        float balanceToUsd = 0;
        decimal currenttlyBalance = await getMoneyValues.GetMoneyValueAsync("cop");
        if (wallet.Balance > 0)
        {
            balanceToUsd = wallet.Balance / (float)currenttlyBalance;
        }
        GetUsdAndCop getUsdAndCop = new GetUsdAndCop
        {
            BalanceInCop = (float)Math.Round(wallet.Balance, 2),
            BalanceInUsd = (float)Math.Round(balanceToUsd, 2)
        };
        return Ok(getUsdAndCop);
    }
    [HttpGet("GetRechargeAndWithdraw/{username}")]
    public async Task<IActionResult> GetRechargeAndWithdraw(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        var recharge = await context.RechargeLogs.Where(u => u.IdUser == user.Id).SumAsync(u => u.Recharge);
        var withdraw = await context.WithdrawLogs.Where(u => u.UserId == user.Id).SumAsync(u => u.Withdraw);
        GetRechargeAndWithdraw getParameters = new GetRechargeAndWithdraw
        {
            Recharge = recharge,
            Withdraw = withdraw
        };
        return Ok(getParameters);
    }
    [HttpGet("ClaimWelcomeBonus/{username}")]
    public async Task<IActionResult> ClaimWelcomeBonus(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == username || u.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        var bonus = await context.WelcomeBonuss.FirstOrDefaultAsync(u => u.UserID == user.Id);
        if (bonus == null)
        {
            return NotFound(new { message = "Bonus no encontrado" });
        }
        if (bonus.IsClaimed)
        {
            return NotFound(new { message = "Bonus ya ha sido reclamado" });
        }
        return Ok();
    }
    [HttpPost("ClaimWelcomeBonus")]
    public async Task<IActionResult> ClaimWelcomeBonus(UpdateBalance updateBalance)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == updateBalance.Email || u.PhoneNumber == updateBalance.Email);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        var bonus = await context.WelcomeBonuss.FirstOrDefaultAsync(u => u.UserID == user.Id);
        if (bonus == null)
        {
            return NotFound(new { message = "Bonus no encontrado" });
        }
        if (bonus.IsClaimed)
        {
            return NotFound(new { message = "Bonus ya ha sido reclamado" });
        }
        var wallet = await context.Wallets.FirstOrDefaultAsync(u => u.Email == updateBalance.Email);
        if (wallet == null)
        {
            return NotFound(new { message = "Billetera no encontrada" });
        }
        wallet.Balance += updateBalance.Balance;
        bonus.IsClaimed = true;
        context.Entry(bonus).State = EntityState.Modified;
        context.Entry(wallet).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok();
    }
}