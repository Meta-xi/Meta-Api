using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;

public class UpdatePlans : IUpdatePlansPerHour
{
    public readonly DBContext context;

    public UpdatePlans(DBContext dbcontext)
    {
        context = dbcontext;
    }

    public async Task UpdatePlansPerHour()
    {
        var userplans = await context.UserPlans.ToListAsync();
        List<UserBenefitDaily> userBenefitDailies = new List<UserBenefitDaily>();

        foreach (var i in userplans)
        {
            var plan = await context.Plans.FirstOrDefaultAsync(option => option.Name == i.NamePlan);

            UserBenefitDaily? userBenefitDaily = userBenefitDailies.FirstOrDefault(option => option.Username == i.Username);
            if (userBenefitDaily != null && plan != null)
            {
                userBenefitDaily.AcumulatedBenefitperDay += (float)Math.Round(plan.DailyBenefit, 2);
                Console.WriteLine($"AcumulatedBenefitperDay actualizado para {i.Username}: {userBenefitDaily.AcumulatedBenefitperDay}");
            }
            else if (plan != null)
            {
                UserBenefitDaily userBenefit = new UserBenefitDaily
                {
                    Username = i.Username,
                    AcumulatedBenefitperDay = (float)Math.Round(plan.DailyBenefit, 2)
                };
                userBenefitDailies.Add(userBenefit);
                Console.WriteLine($"Nuevo UserBenefitDaily agregado para {i.Username}");
            }

        }

        foreach (var i in userplans)
        {
            Console.WriteLine($"Entrando a la actualización del usuario {i.Username}");
            var plan = await context.Plans.FirstOrDefaultAsync(option => option.Name == i.NamePlan);
            Console.WriteLine(i.Percentage);
            if (i.Percentage < 100)
            {
                var userPlan = await context.UpdatePlansForUser.FirstOrDefaultAsync(option => option.Username == i.Username);
                Console.WriteLine(userPlan == null);
                if (userPlan == null)
                {
                    if (plan != null)
                    {
                        double benefitPerHour = Math.Round(plan.DailyBenefit / 24, 2);
                        UpdatePlansForUser updatePlansForUser = new UpdatePlansForUser
                        {
                            Username = i.Username,
                            AcumulatedBenefitperHour = Math.Round(benefitPerHour, 2),
                            AcumulatedTotalBenefit = Math.Round(benefitPerHour, 2)
                        };
                        await context.UpdatePlansForUser.AddAsync(updatePlansForUser);
                        await context.SaveChangesAsync();
                        Console.WriteLine($"Nuevo UpdatePlansForUser creado para {i.Username} con beneficio por hora: {benefitPerHour}");

                        i.Percentage += float.Parse(Math.Round(benefitPerHour / plan.TotalBenefit * 100, 2).ToString());
                        context.Entry(i).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        Console.WriteLine($"Porcentaje actualizado para {i.Username}: {i.Percentage}");

                        var wallet = await context.Wallets.FirstOrDefaultAsync(option => option.Email == i.Username);
                        if (wallet != null)
                        {
                            wallet.Balance += (float)benefitPerHour;
                            context.Entry(wallet).State = EntityState.Modified;
                            await context.SaveChangesAsync();
                            Console.WriteLine($"Balance de wallet actualizado para {i.Username}: {wallet.Balance}");
                        }
                    }
                }
                else
                {
                    if (plan != null)
                    {
                        UserBenefitDaily? userBenefitDaily = userBenefitDailies.FirstOrDefault(option => option.Username == i.Username);
                        Console.WriteLine(userBenefitDaily == null);
                        if (userBenefitDaily != null)
                        {
                            Console.WriteLine("userBenefitDaily es distinto de null");
                            if (userPlan.AcumulatedBenefitperHour < userBenefitDaily.AcumulatedBenefitperDay)
                            {
                                double benefitPerHour = Math.Round(plan.DailyBenefit / 24, 2);
                                userPlan.AcumulatedBenefitperHour += Math.Round(benefitPerHour, 2, MidpointRounding.AwayFromZero);
                                userPlan.AcumulatedTotalBenefit += Math.Round(benefitPerHour, 2, MidpointRounding.AwayFromZero);
                                context.Entry(userPlan).State = EntityState.Modified;
                                await context.SaveChangesAsync();
                                Console.WriteLine($"AcumulatedBenefitperHour y AcumulatedTotalBenefit actualizados para {i.Username}");

                                i.Percentage += float.Parse(Math.Round(benefitPerHour / plan.TotalBenefit * 100, 2).ToString());
                                context.Entry(i).State = EntityState.Modified;
                                await context.SaveChangesAsync();
                                Console.WriteLine($"Porcentaje actualizado para {i.Username}: {i.Percentage}");

                                var wallet = await context.Wallets.FirstOrDefaultAsync(option => option.Email == i.Username);
                                if (wallet != null)
                                {
                                    wallet.Balance += (float)benefitPerHour;
                                    context.Entry(wallet).State = EntityState.Modified;
                                    await context.SaveChangesAsync();
                                    Console.WriteLine($"Balance de wallet actualizado para {i.Username}: {wallet.Balance}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("userBenefitDaily es igual a null");
                                double benefitPerHour = Math.Round(plan.DailyBenefit / 24, 2);
                                userPlan.AcumulatedBenefitperHour += Math.Round(benefitPerHour, 2, MidpointRounding.AwayFromZero);
                                userPlan.AcumulatedTotalBenefit += Math.Round(benefitPerHour, 2, MidpointRounding.AwayFromZero);
                                context.Entry(userPlan).State = EntityState.Modified;
                                await context.SaveChangesAsync();
                                Console.WriteLine($"AcumulatedBenefitperHour restablecido y AcumulatedTotalBenefit actualizado para {i.Username}");
                                i.Percentage += float.Parse(Math.Round(benefitPerHour / plan.TotalBenefit * 100, 2).ToString());
                                context.Entry(i).State = EntityState.Modified;
                                await context.SaveChangesAsync();
                                Console.WriteLine($"Porcentaje actualizado para {i.Username}: {i.Percentage}");

                                var wallet = await context.Wallets.FirstOrDefaultAsync(option => option.Email == i.Username);
                                if (wallet != null)
                                {
                                    wallet.Balance += (float)benefitPerHour;
                                    context.Entry(wallet).State = EntityState.Modified;
                                    await context.SaveChangesAsync();
                                    Console.WriteLine($"Balance de wallet actualizado para {i.Username}: {wallet.Balance}");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                context.UserPlans.Remove(i);
                await context.SaveChangesAsync();
                Console.WriteLine($"Plan de usuario {i.Username} eliminado porque alcanzó el 100% del beneficio.");
            }
        }

        userBenefitDailies.Clear();
        Console.WriteLine("Lista de beneficios diarios limpia: " + userBenefitDailies.Count);
    }
}


