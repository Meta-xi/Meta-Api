using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class MisionsController : ControllerBase
{
    private readonly DBContext context;
    public MisionsController(DBContext dBContext)
    {
        context = dBContext;
    }
    [HttpPost("MisionRegister")]
    public async Task<IActionResult> MisionRegister(MisionRegister misionRegister)
    {
        var name = context.Missionss.FirstOrDefault(u => u.Name == misionRegister.Name);
        if (name != null)
        {
            return BadRequest("La mision ya existe en la aplicacion");
        }
        Missions missions = new Missions
        {
            Name = misionRegister.Name,
            Reward = misionRegister.Reward,
            MissionsUsers = null
        };
        await context.Missionss.AddAsync(missions);
        await context.SaveChangesAsync();
        return Ok("Mision registrada correctamente");
    }
    [HttpPost("TrendRegister")]
    public async Task<IActionResult> TrendRegister(MisionRegister misionRegister){
        var name = context.Missionss.FirstOrDefaultAsync(u => u.Name == misionRegister.Name);
        if(name != null){
            return BadRequest("La mision ya existe en la aplicacion");
        }
        Trend trend = new Trend{
            Name = misionRegister.Name,
            Reward = misionRegister.Reward,
            TrendUsers = null
        };
        await context.Trends.AddAsync(trend);
        await context.SaveChangesAsync();
        return Ok("Mision registrada correctamente");
    }
}