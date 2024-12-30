using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class UserPlansController : ControllerBase
{
    private readonly DBContext context;
    private readonly IBenefitPerRefer benefitPerRefer;
    public UserPlansController(DBContext dBContext, IBenefitPerRefer _benefitPerRefer)
    {
        context = dBContext;
        benefitPerRefer = _benefitPerRefer;
    }
    [HttpPost("UserBuyPlans")]
    public async Task<IActionResult> UserBuyPlans(BuyPlans buyPlans)
    {
        var user = await context.Users.FirstOrDefaultAsync(option => option.Email == buyPlans.Username || option.PhoneNumber == buyPlans.Username);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        var plan = await context.Plans.FirstOrDefaultAsync(option => option.Name == buyPlans.IdPlan);
        if (plan == null)
        {
            return NotFound(new { message = "Plan no encontrado" });
        }
        var userPlans = await context.UserPlans.Where(option => option.Username == user.Email && option.NamePlan == plan.Name).ToListAsync();
        Console.WriteLine(userPlans.Count);
        if (userPlans.Count >= plan.MaxQuantity)
        {
            return NotFound(new { message = "El usuario ya tiene el maximo de compras para este plan" });
        }
        Console.WriteLine("Puede comprar un plan de este tipo");
        var wallet = await context.Wallets.FirstOrDefaultAsync(option => option.Email == buyPlans.Username);
        if (wallet == null)
        {
            return NotFound(new { message = "Wallet no encontrada" });
        }
        if (wallet.Balance > plan.Price)
        {
            Console.WriteLine("Tiene saldo suficiente para comprar este plan");
            UserPlans userPlans1 = new UserPlans
            {
                Username = buyPlans.Username,
                NamePlan = plan.Name,
                DatePlan = DateTime.UtcNow,
                Percentage = 0
            };
            await context.UserPlans.AddAsync(userPlans1);
            await context.SaveChangesAsync();
            float balance = (float)plan.Price;
            wallet.Balance -= balance;
            Console.WriteLine(wallet.Balance);
            context.Entry(wallet).State = EntityState.Modified;
            await context.SaveChangesAsync();
            BenefitOperation benefitOperation = new BenefitOperation
            {
                PricePlan = plan.Price,
                Username = buyPlans.Username
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

        }
        else
        {
            return NotFound(new { message = "No tiene saldo suficiente" });
        }
        return Ok(new { message = "Plan comprado con exito" });
    }
    [HttpGet("GetUserPlans/{username}")]
    public async Task<IActionResult> UserPlans(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(option => option.Email == username || option.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        var userPlans = await context.UserPlans.Where(i => i.Username == username).ToListAsync();
        if (!userPlans.Any())
        {
            return NotFound(new { message = "El usuario no ha comprado ningún plan" });
        }
        var plans = await context.Plans.ToListAsync();
        List<MyPlan> myPlans = new List<MyPlan>();
        foreach (var i in userPlans)
        {
            foreach (var j in plans)
            {
                if (j.Name == i.NamePlan)
                {
                    DateTime dateTime = DateTime.UtcNow;
                    TimeSpan diference = dateTime.Subtract(i.DatePlan);
                    int daysRemaining = diference.Days;
                    double HourBenefit = Math.Round(j.DailyBenefit / 24, 2);
                    MyPlan myPlan = new MyPlan
                    {
                        DailyBenefit = j.DailyBenefit,
                        DaysRemaining = j.DaysActive - daysRemaining,
                        HourBenefit = HourBenefit,
                        Percentage = i.Percentage,
                        TotalBenefit = j.TotalBenefit,
                        IdPlan = j.IDPlan,
                        Name = j.Name
                    };
                    myPlans.Add(myPlan);
                }
            }
        }
        return Ok(userPlans);
    }
    [HttpGet("GetBalaceToUser/{username}")]
    public async Task<IActionResult> GetBalanceToUser(string username)
    {
        var benefit = await context.UpdatePlansForUser.FirstOrDefaultAsync(option => option.Username == username);
        if (benefit == null)
        {
            return NotFound(new { message = "El usuario no tiene ninguna información de beneficios" });
        }
        else
        {
            return Ok(benefit);
        }
    }
    [HttpGet("GetVrsToUser/{username}")]
    public async Task<IActionResult> GetVrsToUser(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(option => option.Email == username || option.PhoneNumber == username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var userVrs = await context.UserPlans.Where(option => option.Username == username).ToListAsync();
        if (!userVrs.Any())
        {
            return NotFound(new { message = "El usuario no tiene ningun plan" });
        }
        var plans = await context.Plans.ToListAsync();
        List<string> vrs = new List<string>();
        foreach (var i in userVrs)
        {
            foreach (var j in plans)
            {
                if (i.NamePlan == j.Name)
                {
                    vrs.Add("Vr" + j.IDPlan + ",");
                }
            }
        }
        return Ok(vrs);
    }
    [HttpPost("AdminPlanToUser")]
    public async Task<IActionResult> AdminPlanToUSer(AdminPlanToUser adminPlanToUser)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == adminPlanToUser.Username || u.PhoneNumber == adminPlanToUser.Username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var plan = await context.Plans.FirstOrDefaultAsync(u => u.IDPlan == adminPlanToUser.Vr);
        if (plan == null)
        {
            return NotFound(new { message = "El plan no existe en la aplicacion" });
        }
        var userPlans = await context.UserPlans.Where(u => u.Username == user.Email && u.NamePlan == plan.Name).ToListAsync();
        if (userPlans.Count >= plan.MaxQuantity)
        {
            return BadRequest(new { message = "El usuario ya tiene el maximo de compras para este plan" });
        }
        UserPlans userPlans1 = new UserPlans
        {
            Username = adminPlanToUser.Username,
            NamePlan = plan.Name,
            DatePlan = DateTime.UtcNow,
            Percentage = 0
        };
        await context.AddAsync(userPlans1);
        await context.SaveChangesAsync();
        return Ok(new { message = "Plan comprado con exito" });
    }
    [HttpDelete("DeleteUserPlans")]
    public async Task<IActionResult> DeleteUserPlans(AdminPlanToUserDelete adminPlanToUserDelete)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == adminPlanToUserDelete.Username || u.PhoneNumber == adminPlanToUserDelete.Username);
        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe en la aplicacion" });
        }
        var plan = await context.Plans.FirstOrDefaultAsync(u => u.IDPlan == adminPlanToUserDelete.Vr);
        if (plan == null)
        {
            return NotFound(new { message = "El plan no existe en la aplicacion" });
        }
        var userPlans = await context.UserPlans.Where(u => u.Username == adminPlanToUserDelete.Username && u.NamePlan == plan.Name).ToListAsync();
        if (userPlans.Count < adminPlanToUserDelete.Quantity)
        {
            return BadRequest(new { message = "El usuario no tiene esa cantidad de compras de este plan" });
        }
        var plansdelete = userPlans.Take(adminPlanToUserDelete.Quantity);
        context.UserPlans.RemoveRange(plansdelete);
        await context.SaveChangesAsync();
        if (adminPlanToUserDelete.Quantity == 1)
        {
            return Ok(new { message = "Plan eliminado con exito" });
        }
        return Ok(new { message = "Planes eliminados con exito" });
    }
}