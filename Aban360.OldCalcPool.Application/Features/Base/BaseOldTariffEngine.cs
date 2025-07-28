using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using org.matheval;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal class BaseOldTariffEngine : BaseExpressionCalculator
    {
        //todo: Do not use 'BaseException' directy

        double Ab = 0;//?
        double O_Ab = 0;//?
        double mas_fi_roz;//?
        double duration;
        double mas1_7, mas2_7;//??
        double V_vaj1_7;//??
        double V_vaj2_7;//??
        double nerkh_azad;//??
        double AB1_7;//??
        double AB2_7;//??
        int rosta_calc;//??
        double first_olgo;//??
        /// <summary>
        /// mod1_ : duration
        /// mas1_ : PartialConsumption 
        /// noe_ensh : UsageId
        /// fix_mas : ContractualCapacity
        /// noe_va : BranchType
        /// rate_ : monthlyConsumption
        /// abfar_x ==2 then Rusta , abfar_x==1 then Shahri
        /// </summary>



        /// <summary>
        /// تنها تابع با دسترسی پابلیک بابت محاسبه تک رکورد جدول نرخ
        /// </summary>
        /// <returns>مقدار خروجی بعد از اتمام نوشتن کد، اصلاح شود</returns>
        public object CalculateWaterBill()
        {
            //object parameters= new {X=consumptionAverage};
            //long _value = expression.Eval<long>();
            throw new NotImplementedException();
        }

        private ConsumptionInfo GetConsumptionInfo()
        {
            throw new NotImplementedException();
        }
        private long CalcVajExpression(string formula, double x)
        {
            object parameters = new { X = x };
            Expression expression = GetExpression(formula, parameters);
            long value = expression.Eval<long>();
            return value;
        }
        private bool IsVillage(int zoneId)
        {
            //-abfar_x==2 ? village : shahr(equal 1)
            if (zoneId > 140000)
                return true;

            return false;
        }
        private bool IsDomestic(int usageId)
        {
            List<int> condition = new List<int> { 0, 1, 3 };
            if (condition.Contains(usageId))
                return true;

            return false;
        }
        private bool IsDomesticWithoutUnspecified(int usageId)
        {
            List<int> condition = new List<int> { 1, 3 };
            if (condition.Contains(usageId))
                return true;

            return false;
        }
        private bool IsNotReligious(int usageId)
        {
            List<int> condition = new List<int> { 10, 12, 13, 32, 29 };
            if (!condition.Contains(usageId))
                return true;

            return false;
        }
        private bool IsReligious(int usageId)
        {
            List<int> condition = new List<int> { 10, 12, 13, 32, 29 };
            if (condition.Contains(usageId))
                return true;

            return false;
        }
        private bool IsNotConstruction(int branchTypeId)
        {
            List<int> condition = new List<int> { 4 };
            if (condition.Contains(branchTypeId))
                return false;

            return true;
        }
        private bool IsCharityAndSchool(int usageId)
        {
            List<int> condition = new List<int> { 8, 7, 12, 13, 29, 30, 32 };
            if (condition.Contains(usageId))
                return true;

            return false;
        }
        private bool IsReligiousWithCharity(int usageId)
        {
            List<int> condition = new List<int> { 12, 13, 2, 30, 32 };
            if (condition.Contains(usageId))
                return true;

            return false;
        }

        /// <summary>
        /// از خلاصه نویسی استفاده شود
        /// </summary>
        /// <returns>عدد محاسبه شده‌ی آب‌بها</returns>
        private long CalculateAbBaha(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo)
        {
            duration = nerkh.Duration;
            double monthlyConsumption = nerkh.DailyConsumption * 30;
            double vaj = CalcVajExpression(nerkh.Vaj, monthlyConsumption);//??
            double ab_, o_ab_ = 0;

            if (duration > 0)
            {
                if ((IsDomestic(customerInfo.UsageId) ||
                    ((customerInfo.UsageId == 34 || customerInfo.UsageId == 25) && nerkh.Date1.CompareTo("1400/12/24") >= 0)) &&
                    IsNotReligious(customerInfo.UsageId))
                {
                    double vaj = CalcVajExpression(nerkh.Vaj, monthlyConsumption);
                    ab_ = vaj * nerkh.PartialConsumption;

                    if (nerkh.Date2.CompareTo("1403/09/13") <= 0 && (int.Parse)(nerkh.OVaj.Trim()) != 0)
                    {
                        double o_vaj = CalcVajExpression(nerkh.OVaj, monthlyConsumption);
                        o_ab_ = (nerkh.PartialConsumption * o_vaj) * 1.15;
                    }
                    else
                    {
                        o_ab_ = 0;
                    }

                    if (nerkh.Date2.CompareTo("1403/09/13") <= 0 &&
                        monthlyConsumption <= nerkh.Olgo &&
                        ab_ > o_ab_ &&
                        o_ab_ > 0 &&
                        IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                        IsNotConstruction(customerInfo.BranchType))
                    {
                        ab_ = o_ab_;
                    }
                }
                else
                {
                    //c#: 141    foxpro:1139
                    if ((customerInfo.ContractualCapacity > 0 && IsNotConstruction(customerInfo.BranchType)) ||
                        IsReligious(customerInfo.UsageId))
                    {
                        mas_fi_roz = (customerInfo.ContractualCapacity / 30) * duration;

                        if (duration > mas_fi_roz || IsCharityAndSchool(customerInfo.UsageId))
                        {
                            mas2_7 = duration - mas_fi_roz;
                            mas1_7 = duration - mas2_7;

                            if (duration <= mas_fi_roz)//c#:158
                            {
                                mas1_7 = duration;
                                mas2_7 = 0;
                            }

                            if (customerInfo.ContractualCapacity == 0 && IsReligiousWithCharity(customerInfo.UsageId))
                            {
                                mas1_7 = duration;
                                mas2_7 = 0;
                            }

                            V_vaj1_7 = vaj;//???

                            if (IsReligiousWithCharity(customerInfo.UsageId))
                            {
                                if (IsNotConstruction(customerInfo.BranchType))//c#:178  foxpro:1178
                                {
                                    //method Line1178()
                                }
                                else
                                {
                                    nerkh_azad = 450000;//??
                                    V_vaj1_7 = 450000;
                                    V_vaj2_7 = 450000;
                                }
                            }
                            else
                            {
                                //method BigCase();
                            }

                            //c#: 195   foxpro: 1539
                            AB1_7 = mas1_7 * V_vaj1_7;
                            AB2_7 = mas2_7 * V_vaj2_7;
                            Ab = AB1_7 + AB2_7;//??
                        }
                        else
                        {
                            Ab = duration * vaj;//??
                        }
                    }
                    else
                    {
                        Ab = duration * vaj;//??
                    }
                }//c#:211   foxpro:1553

                rosta_calc = 1;
                if (nerkh.Date2.CompareTo("1403/12/30") <= 0)
                    first_olgo = 14;
                else
                    first_olgo = nerkh.Olgo;//is it true? olgoab==nerkh.Olgo??

                if (IsVillage(customerInfo.ZoneId) && IsDomesticWithoutUnspecified(customerInfo.UsageId) && IsNotConstruction(customerInfo.BranchType))//c#:221
                {
                    int cod_rosta = int.Parse(customerInfo.VillageId.Trim().Substring(0, 4));
                    if (!IsVillage(customerInfo.ZoneId))//c#:225   
                    {

                    }
                }

            }


            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private long CalculateAbBahaDiscount(/*long abBaha... other parameters*/)
        {
            throw new NotImplementedException();
        }

        private long CalculateBoodjePart1()
        {
            throw new NotImplementedException();
        }
        private (long, long) CalculateBoodjePart1Discount()
        {
            throw new NotImplementedException();
        }

        private long CalculateBoodjePart2()
        {
            throw new NotImplementedException();
        }
        private (long, long) CalculateBoodjePart2Discount()
        {
            throw new NotImplementedException();
        }

        private long CalculateFazelab()
        {
            throw new NotImplementedException();
        }
        private long CalculateFazelabDiscount()
        {
            throw new NotImplementedException();
        }
        private long CalculateAbonmanAb()
        {
            throw new NotImplementedException();
        }
        private long CalculateAbonmanAbDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateAbonmanFazelab()
        {
            throw new NotImplementedException();
        }
        private long CalculateAbonmanFazelabDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateJavaniJamiat()
        {
            throw new NotImplementedException();
        }
        private long CalculateJavaniJamiatDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateAvarez()
        {
            throw new NotImplementedException();
        }
        private long CalculateAvarezDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateFasleGarm()
        {
            throw new NotImplementedException();
        }
        private long CalculateFasleGarmDiscount()
        {
            throw new NotImplementedException();
        }
    }
}
