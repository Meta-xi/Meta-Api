
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class PlansController : ControllerBase{
    private readonly DBContext context;

    public PlansController(DBContext dBContext){
        context = dBContext;
    }
    //Endpoint para colocar algun plan nuevo
    [HttpPost("PlanRegister")]
    public async Task<IActionResult> PlanRegister(PlanRegister planRegister){
        Plan plan = new Plan{
            Name = planRegister.Name,
            Price = planRegister.Price,
            MaxQuantity = planRegister.MaxQuantity,
            DailyBenefit = planRegister.DailyBenefit,
            DaysActive = planRegister.DaysActive,
            TotalBenefit = planRegister.TotalBenefit
        };
        await context.Plans.AddAsync(plan);
        await context.SaveChangesAsync();
        return Ok("Plan registrado correctamente");
    }
    //Endpoint para obtener todos los planes disponibles en la aplicación
    [HttpGet("Plans")]
    public async Task<IActionResult> Plans(){
        var plan = await context.Plans.ToListAsync();
        if(!plan.Any()){
            return NotFound("No hay planes registrados");
        }
        return Ok(plan);
    }

}