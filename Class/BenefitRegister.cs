using Meta_xi.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
public class BenefitRegister : IBenefitPerRefer
{
    private readonly DBContext context;
    public BenefitRegister(DBContext dBContext){
        context = dBContext;
    }
    public async Task<bool> RegisterBenefitLevel1(BenefitOperation benefitOperation) { 
        var user = await context.Users.FirstOrDefaultAsync(option=> option.Email == benefitOperation.Username || option.PhoneNumber == benefitOperation.Username);
        if(user == null){
            return false;
        }
        var level1 = await context.ReferLevel1s.FirstOrDefaultAsync(option=> option.UniqueCodeReFerred == user.Code);
        if(level1 == null){
            return false;
        }
        var userLevel1 = await context.Users.FirstOrDefaultAsync(option => option.Code == level1.UniqueCodeReferrer);
        if(userLevel1 == null){
            return false;
        }
        var walletToLevel1 = await context.Wallets.FirstOrDefaultAsync(option => option.Email == userLevel1.Email || option.Email == userLevel1.PhoneNumber);
        if(walletToLevel1 == null){
            return false;
        }
        float level1Comision = (float)(benefitOperation.PricePlan * 8 / 100);
        walletToLevel1.Balance = (float)Math.Round(walletToLevel1.Balance + level1Comision  , 2);
        context.Entry(walletToLevel1).State = EntityState.Modified;
        await context.SaveChangesAsync();
        var BenefitPerrefer = await context.BenefitPerRefers.FirstOrDefaultAsync(option => option.Username == walletToLevel1.Email);
        if(BenefitPerrefer == null){
            BenefitPerRefer benefitPerRefer = new BenefitPerRefer{
                Username = walletToLevel1.Email,
                Nivel1 = Math.Round(level1Comision , 2),
                Nivel2 = 0,
                Nivel3 = 0,
            };
            await context.BenefitPerRefers.AddAsync(benefitPerRefer);
            await context.SaveChangesAsync();
        }else{
             BenefitPerrefer.Nivel1 = Math.Round(BenefitPerrefer.Nivel1 + level1Comision , 2);
             context.Entry(BenefitPerrefer).State = EntityState.Modified;
             await context.SaveChangesAsync();
        }
        return true;
     }
     public async Task<bool> RegisterBenefitLevel2(BenefitOperation benefitOperation){
        var user = await context.Users.FirstOrDefaultAsync(option=>option.Email == benefitOperation.Username || option.PhoneNumber == benefitOperation.Username);
        if(user == null){
            return false;
        }
        var level2 = await context.ReferLevel2s.FirstOrDefaultAsync(option => option.UniqueCodeReFerred == user.Code);
        if(level2 == null){
            return false;
        }
        var userLevel2 = await context.Users.FirstOrDefaultAsync(option => option.Code == level2.UniqueCodeReferrer);
        if(userLevel2 == null){
            return false;
        }
        var wallet2 = await context.Wallets.FirstOrDefaultAsync(option => option.Email == userLevel2.Email || option.Email == userLevel2.PhoneNumber);
        if(wallet2 == null){
            return false;
        }
        float level2Comision = (float)(benefitOperation.PricePlan * 5 / 100);
        wallet2.Balance = (float)Math.Round(wallet2.Balance + level2Comision , 2);
        context.Entry(wallet2).State = EntityState.Modified;
        await context.SaveChangesAsync();
        var BenefitPerrefer = await context.BenefitPerRefers.FirstOrDefaultAsync(option => option.Username == wallet2.Email);
        if(BenefitPerrefer == null){
            BenefitPerRefer benefitPerRefer = new BenefitPerRefer{
                Username = wallet2.Email,
                Nivel1 = 0,
                Nivel2 =Math.Round(level2Comision , 2),
                Nivel3 = 0,
            };
            await context.BenefitPerRefers.AddAsync(benefitPerRefer);
            await context.SaveChangesAsync();
        }else{
            BenefitPerrefer.Nivel2 = Math.Round(BenefitPerrefer.Nivel2 + level2Comision , 2);
            context.Entry(BenefitPerrefer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        return true;
     }

    public async Task<bool> RegisterBenefitLevel3(BenefitOperation benefitOperation)
    {
        var user = await context.Users.FirstOrDefaultAsync(option=> option.Email == benefitOperation.Username || option.PhoneNumber == benefitOperation.Username);
        if(user == null){
            return false;
        }
        var level3 = await context.ReferLevel3s.FirstOrDefaultAsync(option => option.UniqueCodeReFerred == user.Code);
        if(level3 == null){
            return false;
        }
        var userLevel3 = await context.Users.FirstOrDefaultAsync(option => option.Code == level3.UniqueCodeReferrer);
        if(userLevel3 == null){
            return false;
        }
        var wallet3 = await context.Wallets.FirstOrDefaultAsync(option => option.Email == userLevel3.Email || option.Email == userLevel3.PhoneNumber);
        if(wallet3 == null){
            return false;
        }
        float level3Comision = (float)(benefitOperation.PricePlan * 2 / 100);
        wallet3.Balance = (float)Math.Round(wallet3.Balance + level3Comision , 2);
        context.Entry(wallet3).State = EntityState.Modified;
        await context.SaveChangesAsync();
        var BenefitPerrefer = await context.BenefitPerRefers.FirstOrDefaultAsync(option => option.Username == wallet3.Email);
        if(BenefitPerrefer == null){
            BenefitPerRefer benefitPerRefer = new BenefitPerRefer{
                Username = wallet3.Email,
                Nivel1 = 0,
                Nivel2 = 0,
                Nivel3 = Math.Round(level3Comision , 2),
            };
            await context.BenefitPerRefers.AddAsync(benefitPerRefer);
            await context.SaveChangesAsync();
        }else{
            BenefitPerrefer.Nivel3 = Math.Round(BenefitPerrefer.Nivel3 + level3Comision , 2);
            context.Entry(BenefitPerrefer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        return true;
    }
}