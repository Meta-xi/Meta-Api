using System.Runtime.CompilerServices;

namespace Meta_xi.Application;
public interface IMisionsFunction
{
    public Task<int> InviteRefers(string code);
    public Task<int> QuantityGafas(string username);
    public Task<bool> QuantityReferGafas(string code);
    
}