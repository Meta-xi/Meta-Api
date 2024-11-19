using Microsoft.EntityFrameworkCore;


namespace Meta_xi.Application
{
    public class UpdatePlans : IUpdatePlansPerHour
    {
        public readonly DBContext context;

        public UpdatePlans(DBContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task UpdatePlansPerHour()
        {
            var userPlans = await context.UserPlans.ToListAsync();
            var plans = await context.Plans.ToDictionaryAsync(p => p.Name); // Diccionario para evitar múltiples consultas
            List<UserBenefitDaily> userBenefitDailies = new List<UserBenefitDaily>();

            foreach (var i in userPlans)
            {
                if (plans.TryGetValue(i.NamePlan, out var plan))
                {
                    var userBenefitDaily = userBenefitDailies.FirstOrDefault(ub => ub.Username == i.Username);
                    if (userBenefitDaily != null)
                    {
                        userBenefitDaily.AcumulatedBenefitperDay += (float)Math.Round(plan.DailyBenefit, 2);
                    }
                    else
                    {
                        userBenefitDailies.Add(new UserBenefitDaily
                        {
                            Username = i.Username,
                            AcumulatedBenefitperDay = (float)Math.Round(plan.DailyBenefit, 2)
                        });
                    }
                }
                else
                {
                    Console.WriteLine($"Plan no encontrado para el usuario {i.Username} con el plan {i.NamePlan}");
                }
            }

            foreach (var i in userPlans)
            {
                if (!plans.TryGetValue(i.NamePlan, out var plan)) continue;

                if (i.Percentage < 100)
                {
                    var userPlan = await context.UpdatePlansForUser.FirstOrDefaultAsync(u => u.Username == i.Username);
                    double benefitPerHour = Math.Round(plan.DailyBenefit / 24, 2);

                    // Actualización o creación de registro de beneficios para el usuario
                    if (userPlan == null)
                    {
                        var newUserPlan = new UpdatePlansForUser
                        {
                            Username = i.Username,
                            AcumulatedBenefitperHour = Math.Round(benefitPerHour, 2),
                            AcumulatedTotalBenefit = Math.Round(benefitPerHour, 2)
                        };
                        await context.UpdatePlansForUser.AddAsync(newUserPlan);
                        Console.WriteLine($"Nuevo registro de beneficios creado para {i.Username}");
                    }
                    else
                    {
                        var userBenefitDaily = userBenefitDailies.FirstOrDefault(ub => ub.Username == i.Username);
                        if (userBenefitDaily != null && userPlan.AcumulatedBenefitperHour < userBenefitDaily.AcumulatedBenefitperDay)
                        {
                            userPlan.AcumulatedBenefitperHour = Math.Round(userPlan.AcumulatedBenefitperHour + benefitPerHour, 2);
                            userPlan.AcumulatedTotalBenefit = Math.Round(userPlan.AcumulatedTotalBenefit + benefitPerHour, 2);
                            Console.WriteLine($"Beneficio por hora actualizado para {i.Username}: {userPlan.AcumulatedBenefitperHour}");
                        }
                        else
                        {
                            userPlan.AcumulatedBenefitperHour = Math.Round(benefitPerHour, 2);
                            userPlan.AcumulatedTotalBenefit = Math.Round(userPlan.AcumulatedTotalBenefit + benefitPerHour, 2);
                            Console.WriteLine($"Beneficio acumulado diario alcanzado para {i.Username}, restableciendo beneficio por hora.");
                        }
                    }

                    i.Percentage += float.Parse(Math.Round(benefitPerHour / plan.TotalBenefit * 100, 2).ToString());
                    i.Percentage = (float)Math.Round(i.Percentage, 2);  // Redondeo final del porcentaje
                    Console.WriteLine($"Porcentaje actualizado para {i.Username}: {i.Percentage}%");

                    var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.Email == i.Username);
                    if (wallet != null)
                    {
                        wallet.Balance = (float)Math.Round(wallet.Balance + benefitPerHour, 2);
                        Console.WriteLine($"Saldo actualizado para {i.Username}: {wallet.Balance}");
                    }
                }
                else
                {
                    context.UserPlans.Remove(i);
                    Console.WriteLine($"Plan completado y eliminado para {i.Username}");
                }
                await context.SaveChangesAsync();
            }

            // Limpieza final de la lista
            userBenefitDailies.Clear();
            Console.WriteLine($"userBenefitDailies limpia, total elementos: {userBenefitDailies.Count}");
        }
    }
}

