using Microsoft.AspNetCore.Mvc;

namespace Meta_xi.Application;
public interface IBenefitPerRefer{
    public Task<bool> RegisterBenefitLevel1(BenefitOperation benefitOperation);
    public Task<bool> RegisterBenefitLevel2(BenefitOperation benefitOperation);
    public Task<bool> RegisterBenefitLevel3(BenefitOperation benefitOperation);

}