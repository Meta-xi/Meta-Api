
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
public class MisionFunction : IMisionsFunction
{
    private readonly DBContext context;
    public MisionFunction(DBContext _context)
    {
        context = _context;
    }
    public async Task<int> InviteRefers( string code){
        var refer = await context.ReferLevel1s.Where(u => u.UniqueCodeReferrer == code).CountAsync();
        return refer;
    }

    public async Task<int> QuantityGafas(string username)
    {
        var userPlans = await context.UserPlans.Where(u => u.Username == username).CountAsync();
        return userPlans;
    }

    public async Task<bool> QuantityReferGafas(string code )
    {
        var refer = await context.ReferLevel1s.Where(u => u.UniqueCodeReferrer == code).Select(u => u.UniqueCodeReFerred).ToListAsync();
        var username = await context.Users.Where(u => refer.Contains(u.Code)).Select(u => u.Email).ToListAsync();
        var userPlans = await context.UserPlans.ToListAsync();
        foreach(var i in username){
            bool result = userPlans.Any(p => p.Username == i);
            if(result){
                return true;
            }
        }
        return false;
    }

}