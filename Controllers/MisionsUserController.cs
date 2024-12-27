using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
[ApiController]
[Route("api/[controller]")]
public class MisionsUserController : ControllerBase
{
    private readonly DBContext context;
    public readonly IMisionsFunction misionsFunction;
    public MisionsUserController(DBContext dBContext ,IMisionsFunction misions){
        context = dBContext;
        misionsFunction = misions;
    }
    
    }
