using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using DNTPersianUtils.Core;
using DNTPersianUtils.Core.IranCities;
using org.matheval;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal class BaseOldTariffEngine : BaseExpressionCalculator
    {
        //todo: Do not use 'BaseException' directy

        //double Ab = 0;//?
        //double O_Ab = 0;//?
        //double mas_fi_roz;//?
        //double duration;
        //double mas1_7, mas2_7;//??
        //double V_vaj1_7;//??
        //double V_vaj2_7;//??
        //double nerkh_azad;//??
        //double AB1_7;//??
        //double AB2_7;//??
        //int rosta_calc;//??
        //double first_olgo;//??
        //double Bmas1_7;//
        //double Bmas2_7;//
        //double Bmas_fi_roz;//
        //double v_bodjeh01;//??
        //double mas_maskoni;//??
        //double abfar_x;//??
        //double vzarib_baha;//??
        //double shandle;//??
        //double vAb_sevom;//??
        //double ab_Fas;//??
        //double vNewAb;//??
        //double vNewFa;//??
        //double VAB10;//??
        //double VAB20;//??
        //double takhfif_ab;//??
        //double takhf_10;//??
        //double ab_takh;//??
        //double mas_takh;//??
        //double fazelabAmountTemp;//??
        //double SHAHRDARI1;//??
        //double VZFASL;//??
        //int modat_;//??
        //double fazlab;//??
        //double Tmp_nFaz;//??
        //double Gr_hes_ab;//??
        //double VzTadil_1;//??
        //double Abresani1;//??
        //double V_FASBAHA1;//??
        //double takhfif_fa;//??
        //double VZFASL_olgo;//??
        //double takhf_fasL;//??
        //double ted_ejraei;//??
        //double NEW_BODJ01;//??
        //double VZARIB_D1;//??
        //bool drsd10;//??
        //bool Edareh_k_;//??

        //double ab;//??
        //double V_SHAHRDARI;//??
        //double VZARIB_D;//??
        //double V_Avarez;//??
        //double V_AB_10;//??
        //double V_AB_20;//??
        //double V_FAS_BAHA;//??
        //double vzAbresani;//??
        //double Z_FASL_;//??
        //double abresani1;//??
        //double v_bodjeh;//??
        //double V_zTadil;//??

        /// <summary>
        /// mod1_ : duration
        /// mas1_ : PartialConsumption 
        /// noe_ensh : UsageId
        /// fix_mas : ContractualCapacustomerInfo.ZoneId
        /// noe_va : BranchType
        /// rate_ : monthlyConsumption
        /// abfar_x ==2 then Rusta , abfar_x==1 then Shahri
        /// </summary>



        /// <summary>
        /// تنها تابع با دسترسی پابلیک بابت محاسبه تک رکورد جدول نرخ
        /// </summary>
        /// <returns>مقدار خروجی بعد از اتمام نوشتن کد، اصلاح شود</returns>

        private ConsumptionInfo GetConsumptionInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// محاسبه فرمول
        /// </summary>
        /// <param name="formula">متن فرمول که در آن X متوسط مصرف ماهانه است</param>
        /// <param name="monthlyAverageConsumption">متوسط مصرف ماهانه</param>
        /// <returns></returns>
        private long CalcFormulaByRate(string formula, double monthlyAverageConsumption)
        {
            object parameters = new { X = monthlyAverageConsumption };
            Expression expression = GetExpression(formula, parameters);
            long value = expression.Eval<long>();
            return value;
        }
        private bool IsVillage(int zoneId)
        {
            return zoneId > 140000;
        }
        private bool IsDomestic(int usageId)
        {
            int[] condition = [0, 1, 3];
            return condition.Contains(usageId);
        }
        private bool IsDomesticCategory(int usageId)
        {
            int[] domesticUsageIds = [0, 1, 3, 25, 34];
            return domesticUsageIds.Contains(usageId);
        }
        private bool IsDomesticWithoutUnspecified(int usageId)
        {
            int[] condition = [1, 3];
            return condition.Contains(usageId);
        }
        private bool IsNotReligious(int usageId)
        {
            int[] condition = [10, 12, 13, 32, 29];
            return !condition.Contains(usageId);
        }
        private bool IsReligious(int usageId)
        {
            int[] condition = [10, 12, 13, 32, 29];
            return condition.Contains(usageId);
        }
        private bool IsNotConstruction(int branchTypeId)
        {
            int[] condition = [4];
            return condition.Contains(branchTypeId);
        }
        private bool IsCharityAndSchool(int usageId)
        {
            int[] condition = [8, 7, 12, 13, 29, 30, 32];
            return condition.Contains(usageId);
        }
        private bool IsHandoverDiscount(int usageId)
        {
            int[] condition = [3, 6, 7];
            return condition.Contains(usageId);
        }
        private bool IsSchool(int usageId)
        {
            int[] condition = [8, 7];
            return condition.Contains(usageId);
        }
        private bool IsReligiousWithCharity(int usageId)
        {
            int[] condition = [12, 13, 29, 30, 32];
            return condition.Contains(usageId);
        }
        private bool HasDiscountBranch(int branchId)
        {
            int[] condition = [3, 6, 7];
            return condition.Contains(branchId);
        }

        /// <summary>
        /// روستاهایی که اگرچه در ناحیه روستایی هستند اما محاسبه بصورت شهری
        /// </summary>
        /// <param name="zoneId"></param>
        /// <param name="villageId"></param>
        /// <returns></returns>
        private bool RuralButIsMetro(int zoneId, ulong villageId)
        {
            ulong[] village142618 = [1037, 1038, 1039];
            ulong[] village144311 = [1090, 1093];
            ulong[] village144411 = [1016];
            ulong[] village143012 = [1010, 1013, 1016, 1017, 1029];
            ulong[] village142714 = [1019];
            ulong[] village141911 = [1034];
            ulong[] village141914 = [1061];
            ulong[] village141611 = [1006];

            return
                (zoneId == 142618 && village142618.Contains(villageId)) ||
                (zoneId == 144311 && village144311.Contains(villageId)) ||
                (zoneId == 144411 && village144411.Contains(villageId)) ||
                (zoneId == 143012 && village143012.Contains(villageId)) ||
                (zoneId == 142714 && village142714.Contains(villageId)) ||
                (zoneId == 141911 && village141911.Contains(villageId)) ||
                (zoneId == 141914 && village141914.Contains(villageId)) ||
                (zoneId == 141611 && village141611.Contains(villageId));
        }
        private bool IsBetween(string number, string start, string end)
        {
            return number.CompareTo(start) >= 0 && number.CompareTo(end) <= 0;
        }
        private bool IsBetween(int number, int min, int max)
        {
            return number >= min && number <= max;
        }
        private bool IsBetween(double number, double min, double max)
        {
            return number >= min && number <= max;
        }
        private bool IsBetween(ulong number, ulong min, ulong max)
        {
            return number >= min && number <= max;
        }
        private bool IsGardenAndResidence(int usageId)
        {
            int[] condition = [25, 34];
            return condition.Contains(usageId);
        }
        private bool IsUsageConstructor(int usageId)
        {
            int[] condition = [5, 39];
            return condition.Contains(usageId);
        }

        //todo : change it, anti pattern
        [Obsolete]
        private bool IsAbUnequal0AndDate2LestThan1403_09_13(string date2, double monthlyConsumption, double abBaha, double oldAbBaha, int firstOlgoo)
        {
            if (abBaha != 0)
            {
                if (date2.CompareTo("1403/09/13") <= 0 || monthlyConsumption <= firstOlgoo)
                {
                    MultiplyCalculation(abBaha, oldAbBaha, 0.5);
                }
                else
                {
                    MultiplyCalculation(abBaha, oldAbBaha, 0.65);
                }
            }

            return false;
        }
        private (double, double, bool) MultiplyCalculation(double abBaha, double oldAbBaha, double multiplier)
        {
            return (abBaha * multiplier, oldAbBaha * multiplier, true);
        }
        private bool IsTankerSaleAndVillage(int usageId)
        {
            int[] condition = [14, 15];
            return condition.Contains(usageId);
        }
        private bool IsDolatabadOrHabibabadWithConditionEshtrak(int zoneId, ulong readingNumber, string villageId)
        {
            return
                (zoneId == 134013 && IsBetween(readingNumber, 57000000, 57999999)) ||
                (zoneId == 134016 && IsBetween(readingNumber, 57000000, 57999999)) ||
                 MetroButIsRural(zoneId, villageId, 4);
        }
        private bool MetroButIsRural(int zoneId, string villageId, int thresholdSkip)
        {
            int villageCod = (int.Parse)(villageId.Substring(0, thresholdSkip));
            if (zoneId == 132220 &&
                (IsBetween(villageCod, 1610, 1628) ||
                IsBetween(villageCod, 1633, 1648) ||
                IsBetween(villageCod, 1651, 1661) ||
                IsBetween(villageCod, 6042, 6052) ||
                IsBetween(villageCod, 6060, 6072))
                )
                return true;

            if (zoneId == 132211 &&
                 (IsBetween(villageCod, 1103, 1108) ||
                 IsBetween(villageCod, 1109, 1113) ||
                 IsBetween(villageCod, 1143, 1165) ||
                 IsBetween(villageCod, 1161, 1184) ||
                 IsBetween(villageCod, 1403, 1499) ||
                 IsBetween(villageCod, 1450, 1472) ||
                 IsBetween(villageCod, 1574, 1599))
               )
                return true;

            return false;
        }
        private double Multiplier(ZaribGetDto zarib, int olgo, bool isDomestic, bool isVillage, double monthlyConsumption)
        {
            double zbSelection = 1;

            zbSelection = isVillage ? zarib.Zarib_baha : 1;
            zbSelection = !isDomestic && !isVillage ? zarib.Zb : 1;
            if (isDomestic && !isVillage)
            {
                zbSelection = IsBetween(monthlyConsumption, 0, 5) ? zarib.Zb1 : 1;
                zbSelection = IsBetween(monthlyConsumption, 5, 10) ? zarib.Zb2 : 1;
                zbSelection = IsBetween(monthlyConsumption, 10, olgo) ? zarib.Zb3 : 1;
                zbSelection = IsBetween(monthlyConsumption, olgo, olgo * 1.5) ? zarib.Zb4 : 1;
                zbSelection = IsBetween(monthlyConsumption, olgo * 1.5, olgo * 2) ? zarib.Zb5 : 1;
                zbSelection = IsBetween(monthlyConsumption, olgo * 2, olgo * 3) ? zarib.Zb6 : 1;
                zbSelection = monthlyConsumption > olgo * 3 ? zarib.Zb7 : 1;
            }

            return zbSelection;
        }

        //written by mhnds Gharibi
        private bool Rosta_shahr(int zoneId, int readingNumber, int number)
        {
            return true;
        }
        private double sele_zarib(int zoneId, string date1, string date2, int noe_ensh, double noe_va, double abfar_X, double shandle, string shtrak, double rate_, double olgoAB)
        {
            return 140000;
        }
        private double z_16(double ab1, double rage, string date1, string date2, int noe_ensh)
        {
            return 0;
        }
        private double javan_sazi(int zoneId, double ab1, int noe_ensh_, double partialConsumption, double duration, int branchType, double monthlyConsumption, int contractualConsumption, int domesticUnit, int otherUnit, string previousDate, string currentDate, double olgoo, string nerkhDate2)
        {
            return 0;
        }
        private double Avarez(int zoneId, double ab1, double mas1_, int noe_ensh_, double noe_va_, double rate_, string date2)
        {
            return 0;
        }


        #region
        ///// <summary>
        ///// توسط خانم اتحادی
        ///// </summary>
        ///// <returns>عدد محاسبه شده‌ی آب‌بها</returns>
        //private long CalculateAbBaha(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo)
        //{
        //    string currentDateJalali = DateTime.Now.ToShortPersianDateString();
        //    duration = nerkh.Duration;
        //    double monthlyConsumption = nerkh.DailyAverageConsumption * 30;
        //    double vaj = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption);//??
        //    double ab_ = 0, o_ab_ = 0;

        //    if (duration > 0)
        //    {
        //        if ((IsDomestic(customerInfo.UsageId) ||
        //            ((customerInfo.UsageId == 34 || customerInfo.UsageId == 25) && nerkh.Date1.CompareTo("1400/12/24") >= 0)) &&
        //            IsNotReligious(customerInfo.UsageId))
        //        {
        //            vaj = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption);
        //            ab_ = vaj * nerkh.PartialConsumption;

        //            if (nerkh.Date2.CompareTo("1403/09/13") <= 0 && (int.Parse)(nerkh.OVaj.Trim()) != 0)
        //            {
        //                double o_vaj = CalcFormulaByRate(nerkh.OVaj, monthlyConsumption);
        //                o_ab_ = (nerkh.PartialConsumption * o_vaj) * 1.15;
        //            }
        //            else
        //            {
        //                o_ab_ = 0;
        //            }

        //            if (nerkh.Date2.CompareTo("1403/09/13") <= 0 &&
        //                monthlyConsumption <= nerkh.Olgo &&
        //                ab_ > o_ab_ &&
        //                o_ab_ > 0 &&
        //                IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
        //                IsNotConstruction(customerInfo.BranchType))
        //            {
        //                ab_ = o_ab_;
        //            }
        //        }
        //        else
        //        {
        //            //c#: 141    foxpro:1139
        //            if ((customerInfo.ContractualCapacity > 0 && IsNotConstruction(customerInfo.BranchType)) ||
        //                IsReligious(customerInfo.UsageId))
        //            {
        //                mas_fi_roz = (customerInfo.ContractualCapacity / 30) * duration;

        //                if (duration > mas_fi_roz || IsCharityAndSchool(customerInfo.UsageId))
        //                {
        //                    mas2_7 = duration - mas_fi_roz;
        //                    mas1_7 = duration - mas2_7;

        //                    if (duration <= mas_fi_roz)//c#:158
        //                    {
        //                        mas1_7 = duration;
        //                        mas2_7 = 0;
        //                    }

        //                    if (customerInfo.ContractualCapacity == 0 && IsReligiousWithCharity(customerInfo.UsageId))
        //                    {
        //                        mas1_7 = duration;
        //                        mas2_7 = 0;
        //                    }

        //                    V_vaj1_7 = vaj;//???

        //                    if (IsReligiousWithCharity(customerInfo.UsageId))
        //                    {
        //                        if (IsNotConstruction(customerInfo.BranchType))//c#:178  foxpro:1178
        //                        {
        //                            //method Line1178()
        //                        }
        //                        else
        //                        {
        //                            nerkh_azad = 450000;//??
        //                            V_vaj1_7 = 450000;
        //                            V_vaj2_7 = 450000;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //method BigCase();
        //                    }

        //                    //c#: 195   foxpro: 1539
        //                    AB1_7 = mas1_7 * V_vaj1_7;
        //                    AB2_7 = mas2_7 * V_vaj2_7;
        //                    Ab = AB1_7 + AB2_7;//??
        //                }
        //                else
        //                {
        //                    Ab = duration * vaj;//??
        //                }
        //            }
        //            else
        //            {
        //                Ab = duration * vaj;//??
        //            }
        //        }//c#:211   foxpro:1553

        //        rosta_calc = 1;
        //        if (nerkh.Date2.CompareTo("1403/12/30") <= 0)
        //            first_olgo = 14;
        //        else
        //            first_olgo = nerkh.Olgo;//is it true? olgoab==nerkh.Olgo??

        //        if (IsVillage(customerInfo.ZoneId) && IsDomesticWithoutUnspecified(customerInfo.UsageId) && IsNotConstruction(customerInfo.BranchType))
        //        {
        //            int cod_rosta = int.Parse(customerInfo.VillageId.Trim().Substring(0, 4));
        //            if (RuralButIsMetro(customerInfo.ZoneId,customerInfo.ReadingNumber) || RuralButIsMetro(customerInfo.ZoneId, ulong.Parse(customerInfo.VillageId)))
        //            {
        //                //
        //            }
        //            else
        //            {//1167
        //                IsAbUnequal0AndDate2LestThan1403_09_13(nerkh.Date2, monthlyConsumption, ab_, o_ab_);//??
        //            }//c#:258  foxpro:1591
        //        }

        //        if (IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, (int.Parse)(customerInfo.ReadingNumber)))
        //        {
        //            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) && IsNotConstruction(customerInfo.BranchType))
        //            {
        //                IsAbUnequal0AndDate2LestThan1403_09_13(nerkh.Date2, monthlyConsumption, ab_, o_ab_);
        //            }
        //        }//c#:289 and foxpro:1619

        //        if ((!IsDomesticWithoutUnspecified(customerInfo.UsageId) && !IsGardenAndResidence(customerInfo.UsageId) && nerkh.Date2.CompareTo("1398/12/29") > 0) ||
        //            (IsGardenAndResidence(customerInfo.UsageId) && nerkh.Date2.CompareTo("1400/12/29") < 0))
        //        {
        //            Bmas2_7 = 0;
        //            if (customerInfo.ContractualCapacity > 0 && IsNotConstruction(customerInfo.BranchType))
        //            {
        //                Bmas_fi_roz = (customerInfo.ContractualCapacity / 30.0) * duration;
        //                if (nerkh.PartialConsumption > Bmas_fi_roz || IsSchool(customerInfo.UsageId))
        //                {
        //                    Bmas2_7 = nerkh.PartialConsumption - Bmas_fi_roz;
        //                    Bmas1_7 = nerkh.PartialConsumption - Bmas2_7;
        //                    if (nerkh.PartialConsumption <= Bmas_fi_roz)
        //                    {
        //                        Bmas1_7 = nerkh.PartialConsumption;
        //                        Bmas2_7 = 0;
        //                    }
        //                }
        //            }//c#:313  foxpro:1642

        //            if (Bmas2_7 > 0)
        //                v_bodjeh01 = nerkh.ZaribBodje * Bmas2_7;
        //            else
        //                v_bodjeh01 = 0;
        //        }
        //        else
        //        {
        //            if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) &&
        //                nerkh.Date2.CompareTo("1398/12/29") > 0)//always false because date is invalid
        //            {
        //                mas_maskoni = 0;
        //                if (IsDomesticWithoutUnspecified(customerInfo.UsageId) && monthlyConsumption >= first_olgo && customerInfo.DomesticUnit > 0)
        //                {
        //                    mas_maskoni = (((monthlyConsumption - first_olgo) * customerInfo.DomesticUnit) / 30.0) * duration;
        //                }
        //                else
        //                {
        //                    if (monthlyConsumption > first_olgo)
        //                        mas_maskoni = ((monthlyConsumption - first_olgo) / 30.0) * duration;
        //                    else
        //                        mas_maskoni = 0;
        //                }
        //                v_bodjeh01 = nerkh.ZaribBodje * customerInfo.DomesticUnit;
        //            }
        //            else
        //            {
        //                v_bodjeh01 = nerkh.ZaribBodje * nerkh.PartialConsumption;
        //            }//c#:344  foxpro:1673
        //        }
        //        v_bodjeh01 = (int)v_bodjeh01;
        //        if (!IsNotConstruction(customerInfo.BranchType))
        //            v_bodjeh01 = 0;

        //        if (IsTankerSaleAndVillage(customerInfo.UsageId))
        //            v_bodjeh01 = 0;

        //        if (customerInfo.ZoneId == 151511)
        //            v_bodjeh01 = 0;

        //        if (abfar_x == 2)
        //        {
        //            int cod_rosta = int.Parse(customerInfo.VillageId.Trim().Substring(0, 4));
        //            if (RuralButIsMetro(customerInfo.ZoneId, (int.Parse)(customerInfo.VillageId)))
        //            {
        //                //
        //            }
        //            else
        //            {
        //                if (IsDomesticWithoutUnspecified(customerInfo.UsageId))
        //                {
        //                    v_bodjeh01 = (int)(v_bodjeh01 / 2);
        //                }
        //            }
        //        }//c#:386  foxpro:1712
        //        if (IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, (int.Parse)(customerInfo.ReadingNumber)))
        //        {
        //            if (IsDomesticWithoutUnspecified(customerInfo.UsageId))
        //            {
        //                v_bodjeh01 = (int)(v_bodjeh01 / 2);
        //            }
        //        }
        //        vzarib_baha = sele_zarib(customerInfo.ZoneId, nerkh.Date1, nerkh.Date2, customerInfo.UsageId, customerInfo.BranchType, abfar_x, shandle, customerInfo.ReadingNumber, monthlyConsumption, nerkh.Olgo);
        //        if (IsGardenAndResidence(customerInfo.ZoneId))
        //        {
        //            if (nerkh.Date2.CompareTo("1401/12/28") <= 0)
        //                vzarib_baha = 1;
        //        }//c#:403  foxpro:1740

        //        ab_ = vAb_sevom + (ab_ * vzarib_baha);
        //        ab_Fas = ab_;
        //        o_ab_ = vAb_sevom + (o_ab_ * vzarib_baha);//c#:415  foxpro:1754

        //        vNewAb = (int)(vNewAb + nerkh.Date1.CompareTo("1394/06/31") > 0 ? ab_ : 0);//IIF(TMP_NERKH.date1 > '1394/06/31', ab1, 0)
        //        VAB10 = 0;
        //        VAB20 = 0;

        //        //else of if(2==3)  c#:427
        //        VAB10 = (drsd10 && nerkh.Tabsare2 && !Edareh_k_) ? (int)(FAZLAB() ? (ab_ * 0.1) : 0) : 0;


        //        if (IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, (int.Parse)(customerInfo.ReadingNumber)))
        //        {
        //            VAB10 = 0;
        //        }//c#:435 foxpro:1785
        //        if (monthlyConsumption <= 5 && IsDomesticWithoutUnspecified(customerInfo.UsageId) && nerkh.Date1.CompareTo("1399/09/30") >= 0 && nerkh.Date2.CompareTo("1401/12/27") <= 0 && IsNotConstruction(customerInfo.ZoneId))
        //        {
        //            if (IsHandoverDiscount(customerInfo.BranchType))
        //            {//?
        //                takhfif_ab += ab_;
        //                if (VAB10 != 0)
        //                    takhf_10 += ab_ * 0.1;

        //                ab_ = 0;
        //                VAB10 = 0;
        //            }
        //        }//c#:452  foxpro:1809
        //        ab_takh = 0;
        //        mas_takh = (first_olgo / 30) * duration;

        //        //c#:470  foxpro:1823
        //        if (IsDomesticWithoutUnspecified(customerInfo.UsageId) && IsNotConstruction(customerInfo.BranchType) && nerkh.Date1.CompareTo("1401/12/27") >= 0)
        //        {
        //            if (IsHandoverDiscount(customerInfo.BranchType))
        //            {
        //                if (monthlyConsumption > first_olgo)
        //                {
        //                    if (nerkh.Date2.CompareTo("1403/09/13") <= 0)
        //                    {
        //                        ab_takh = (int)((mas_takh * ((((3706 * first_olgo) - 13845) / first_olgo) * 1.15) * vzarib_baha) / rosta_calc);
        //                    }
        //                    else
        //                    {
        //                        ab_takh = (int)((mas_takh * ((((70000 * 0.01 * first_olgo)) * first_olgo) / first_olgo) * vzarib_baha) / rosta_calc);
        //                    }
        //                    takhfif_ab = takhfif_ab + ab_takh;

        //                    if (VAB10 != 0)
        //                        takhf_10 = takhf_10 + (ab_takh * 0.1);

        //                    ab_ = ab_ - ab_takh;
        //                    VAB10 = VAB10 - (ab_takh * 0.1);
        //                    if (VAB10 < 0)
        //                        VAB10 = 0;
        //                }
        //                else
        //                {
        //                    takhfif_ab = takhfif_ab + ab_;

        //                    if (VAB10 != 0)
        //                        takhf_10 = takhf_10 + (ab_ * 0.1);

        //                    ab_ = 0;
        //                    VAB10 = 0;
        //                }
        //            }
        //        }//c#:506  foxpro:1882

        //        fazelabAmountTemp = 0;
        //        VZFASL = nerkh.ZaribFasl ? Z_FASL(ab_, meterInfo.PreviousDateJalali, currentDateJalali, customerInfo.UsageId, duration, nerkh.Date1, nerkh.Date2) : 0;
        //        fazelabAmountTemp = VZFASL;

        //        SHAHRDARI1 = 0;
        //        VzTadil_1 = nerkh.ZaribTadil ? (int)z_16(ab_, monthlyConsumption, nerkh.Date1, nerkh.Date2, customerInfo.UsageId) : 0;//line -> 1900
        //        Abresani1 = 0;
        //        V_FASBAHA1 = (int)CALC_FAS(customerInfo.ZoneId, shandle, (int.Parse)(customerInfo.ReadingNumber), (ab_Fas - vAb_sevom), modat_, meterInfo.PreviousDateJalali, currentDateJalali, customerInfo.SewageInstallationDateJalali, Tmp_nFaz, Gr_hes_ab, customerInfo.BranchType, fazlab, nerkh.PartialConsumption);

        //        //c#"519  foxpro:1915
        //        if (IsDomesticWithoutUnspecified(customerInfo.UsageId))
        //        {
        //            if (takhfif_ab != 0 && V_FASBAHA1 != 0)
        //            {
        //                if (monthlyConsumption > first_olgo)
        //                {
        //                    takhfif_fa += (int)(ab_takh * 0.7);
        //                    V_FASBAHA1 -= (int)(ab_takh * 0.7);
        //                }
        //                else
        //                {
        //                    takhfif_fa += (int)(ab_Fas * 0.7);
        //                    V_FASBAHA1 = 0;
        //                }
        //            }
        //        }
        //        VZFASL_olgo = ab_takh;
        //        if (IsReligiousWithCharity(customerInfo.UsageId) && IsNotConstruction(customerInfo.BranchType))
        //        {
        //            ab_takh = (AB1_7 * vzarib_baha);
        //            VZFASL_olgo = 0;
        //            VZFASL_olgo = nerkh.ZaribFasl ? Z_FASL(AB1_7, meterInfo.PreviousDateJalali, currentDateJalali, customerInfo.UsageId, duration, nerkh.Date1, nerkh.Date2) * vzarib_baha : 0;//line->1946
        //            takhfif_ab = takhfif_ab + ab_takh;

        //            if (VAB10 != 0)
        //                takhf_10 = takhf_10 + (ab_takh * 0.1);

        //            ab_ = ab_ - ab_takh;
        //            VAB10 = VAB10 - (ab_takh * 0.1);

        //            if (VAB10 < 0)
        //                VAB10 = 0;
        //            if (VZFASL != 0)
        //            {
        //                takhf_fasL = takhf_fasL + VZFASL_olgo;
        //                VZFASL = VZFASL - VZFASL_olgo;

        //                if (VZFASL < 0)
        //                    VZFASL = 0;
        //            }
        //        }//c#:563  foxpro:1974
        //        eted_ejraei = 1;
        //        if (nerkh.Date2.CompareTo("1395/02/31") > 0 && fazelabAmountTemp != 0)
        //        {
        //            if (V_FASBAHA1 != 0)
        //            {
        //                if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) && IsNotConstruction(customerInfo.BranchType))
        //                    V_FASBAHA1 = V_FASBAHA1 + (fazelabAmountTemp * 0.7);
        //                else
        //                    V_FASBAHA1 = V_FASBAHA1 + (fazelabAmountTemp * 1);
        //            }
        //            if (VAB10 != 0)
        //                VAB10 = VAB10 + (fazelabAmountTemp * 0.1);
        //        }//c#:586  foxpro:2008
        //        if (IsReligiousWithCharity(customerInfo.UsageId))
        //        {
        //            if (ab_takh != 0 && V_FASBAHA1 != 0)
        //            {
        //                takhfif_fa += ab_takh;
        //                V_FASBAHA1 -= ab_takh;

        //                if (V_FASBAHA1 < 0)
        //                {
        //                    V_FASBAHA1 = 0;
        //                }
        //            }
        //        }
        //        if (takhfif_fa != 0)
        //        {
        //            if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) && IsNotConstruction(customerInfo.BranchType))
        //            {
        //                takhfif_fa = takhfif_fa;
        //                V_FASBAHA1 = V_FASBAHA1;
        //            }
        //            else
        //            {
        //                takhfif_fa = takhfif_fa + (VZFASL_olgo * 1);
        //                V_FASBAHA1 = V_FASBAHA1 - (VZFASL_olgo * 1);
        //            }
        //            if (V_FASBAHA1 < 0)
        //                V_FASBAHA1 = 0;
        //        }//c#:616  foxpro:2050

        //        vNewFa = (int)(vNewFa + nerkh.Date1.CompareTo("1394/06/31") > 0 ? V_FASBAHA1 : 9);
        //        NEW_BODJ01 = 0;
        //        NEW_BODJ01 = NEW_BODJ(customerInfo.ZoneId, customerInfo.UsageId, ab_, VZFASL, nerkh.PartialConsumption, duration, customerInfo.BranchType, monthlyConsumption, customerInfo.ContractualCapacity, customerInfo.DomesticUnit, customerInfo.OtherUnit, meterInfo.PreviousDateJalali, currentDateJalali, vzarib_baha, nerkh.Vaj, nerkh.Bodjeh_new, nerkh.Date1, nerkh.Date2, customerInfo.HouseholdNumber, nerkh.Olgo);//??

        //        VZARIB_D1 = 0;
        //        VZARIB_D1 = javan_sazi(customerInfo.ZoneId, ab_, customerInfo.UsageId, nerkh.PartialConsumption, duration, customerInfo.BranchType, monthlyConsumption, customerInfo.ContractualCapacity, customerInfo.DomesticUnit, customerInfo.OtherUnit, meterInfo.PreviousDateJalali, currentDateJalali, nerkh.Olgo, nerkh.Date2);

        //        if (IsDolatabadOrHabibabasWithConditionEshtrak(customerInfo.ZoneId.(int.Parse)(customerInfo.ReadingNumber)))
        //        {
        //            VZARIB_D1 = 0;
        //        }
        //        VZARIB_D = VZARIB_D + VZARIB_D1;
        //        V_Avarez = V_Avarez + Avarez(customerInfo.ZoneId, ab_, nerkh.PartialConsumption, customerInfo.UsageId, customerInfo.BranchType, monthlyConsumption, nerkh.Date2);

        //        ab = ab + ab_;//AB1??
        //        v_bodjeh = v_bodjeh + v_bodjeh01 + NEW_BODJ01;
        //        V_AB_20 = V_AB_20 + VAB20;
        //        V_AB_10 = V_AB_10 + VAB10;

        //        Z_FASL_ = Z_FASL_ + VZFASL;
        //        V_SHAHRDARI = V_SHAHRDARI + SHAHRDARI1;
        //        V_zTadil = V_zTadil + VzTadil_1;
        //        vzAbresani = vzAbresani + abresani1;//c#:646  foxpro:2111
        //        V_FAS_BAHA = V_FAS_BAHA + V_FASBAHA1;

        //    }

        //    throw new NotImplementedException();


        //}
        #endregion

        /// <summary>
        /// محاسبه آب بها 
        /// </summary>
        /// <returns>عدد محاسبه شده‌ی آب‌بها</returns>
        private double _CalculateAbBaha(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, ZaribGetDto zarib, long abAzad39, long abAzad8)
        {
            double abBahaAmount = 0, oldAbBahaAmount = 0, abBahaFromExpression = 0;
            bool isVillageCalculation = false;
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            double duration = nerkh.Duration;
            double monthlyConsumption = nerkh.DailyAverageConsumption * 30;
            abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption);
            if (duration <= 0 ||
                monthlyConsumption == 0 ||
                string.IsNullOrWhiteSpace(nerkh.Vaj))
            {
                return 0;
            }

            if ((IsDomestic(customerInfo.UsageId) || IsGardenOrDweltyAfter1400_12_24(customerInfo.UsageId, nerkh.Date1)) &&
                IsNotReligious(customerInfo.UsageId))
            {
                abBahaFromExpression = CalcFormulaByRate(nerkh.Vaj, monthlyConsumption);
                abBahaAmount = abBahaFromExpression * nerkh.PartialConsumption;

                if (!string.IsNullOrWhiteSpace(nerkh.OVaj) &&
                    nerkh.OVaj.Trim() != "0" &&
                    IsLessThan1403_09_13(nerkh.Date2))
                {
                    long oldAbBahaFromExpression = CalcFormulaByRate(nerkh.OVaj, monthlyConsumption);
                    oldAbBahaAmount = nerkh.PartialConsumption * oldAbBahaFromExpression * 1.15;
                }

                if (IsLessThan1403_09_13(nerkh.Date2) &&
                    monthlyConsumption <= nerkh.Olgo &&//todo: ask olgoab vs olgo?
                    abBahaAmount > oldAbBahaAmount &&
                    IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                    IsNotConstruction(customerInfo.BranchType))
                {
                    abBahaAmount = oldAbBahaAmount;
                }
            }
            else
            {
                // foxpro:1139
                if ((customerInfo.ContractualCapacity > 0 && IsNotConstruction(customerInfo.BranchType)) ||
                    IsReligious(customerInfo.UsageId))
                {
                    double contractualCapacityInDuration = (customerInfo.ContractualCapacity / 30) * duration;

                    if (nerkh.PartialConsumption > contractualCapacityInDuration ||
                        IsCharityAndSchool(customerInfo.UsageId))
                    {
                        double disallowedPartialConsumption = nerkh.PartialConsumption - contractualCapacityInDuration;
                        double allowedPartialConsumption = customerInfo.ContractualCapacity;

                        if (nerkh.PartialConsumption < contractualCapacityInDuration)
                        {
                            allowedPartialConsumption = nerkh.PartialConsumption;
                            disallowedPartialConsumption = 0;
                        }//L 1153
                        if (customerInfo.ContractualCapacity == 0 &&
                            IsReligiousWithCharity(customerInfo.UsageId))
                        {
                            disallowedPartialConsumption = 0;
                            allowedPartialConsumption = nerkh.PartialConsumption;
                        }

                        (long, long) _2Amount = (0, 0);
                        (double, double) abBahaValues = (0, 0);
                        if (IsReligiousWithCharity(customerInfo.UsageId))
                        {
                            if (IsNotConstruction(customerInfo.BranchType))//  foxpro:1178
                            {
                                _2Amount = Get2PartAmount(nerkh.Date2);
                            }
                            else
                            {
                                _2Amount = (450000, 450000);
                            }
                        }
                        else
                        {
                            _2Amount = BigCase(customerInfo.UsageId, nerkh.Date1, nerkh.Date2, customerInfo.IsSpecial, abAzad39);
                        }
                        abBahaValues.Item1 = _2Amount.Item1 * allowedPartialConsumption;
                        abBahaValues.Item2 = _2Amount.Item2 * disallowedPartialConsumption;
                        abBahaAmount = abBahaValues.Item1 + abBahaValues.Item2;
                    }
                    else
                    {
                        abBahaAmount = nerkh.PartialConsumption * abBahaFromExpression;//??
                    }
                }
                else
                {
                    abBahaAmount = nerkh.PartialConsumption * abBahaFromExpression;
                }
            }
            //L 1553
            isVillageCalculation = false;//L 1558
            int firstOlgoo = nerkh.Date2.CompareTo("1403/12/30") <= 0 ? 14 : nerkh.Olgo;

            if (IsVillage(customerInfo.ZoneId) &&
                IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                IsNotConstruction(customerInfo.BranchType))
            {
                int villageCode = int.Parse(customerInfo.VillageId.Trim().Substring(0, 4));
                if (RuralButIsMetro(customerInfo.ZoneId, customerInfo.ReadingNumber) ||
                    RuralButIsMetro(customerInfo.ZoneId, ulong.Parse(customerInfo.VillageId)))
                {
                    //nothing !
                }
                else
                {//1578
                    if (abBahaAmount != 0)
                    {
                        if (nerkh.Date2.CompareTo("1403/09/13") <= 0 || monthlyConsumption <= firstOlgoo)
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.5);
                        }
                        else
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.65);
                        }
                    }
                }
            }
            //L 1600 approximately
            if (IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, ulong.Parse(customerInfo.ReadingNumber), customerInfo.VillageId))
            {
                if (IsDomesticWithoutUnspecified(customerInfo.UsageId) &&
                    IsNotConstruction(customerInfo.BranchType))
                {
                    if (abBahaAmount != 0)//L 1604
                    {
                        if (nerkh.Date2.CompareTo("1403/09/13") <= 0 || monthlyConsumption <= firstOlgoo)
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.5);
                        }
                        else
                        {
                            (abBahaAmount, oldAbBahaAmount, isVillageCalculation) = MultiplyCalculation(abBahaAmount, oldAbBahaAmount, 0.65);
                        }
                    }
                }
            }//foxpro:1620

            double contractInDuration = 0;
            double allowedConsumption = 0;
            double disAllowedConsumption = 0;
            double allowedBoodje = 0;
            double disAllowedBoodje = 0;
            //L 1627
            if ((!IsDomesticWithoutUnspecified(customerInfo.UsageId) && !IsGardenAndResidence(customerInfo.UsageId) && nerkh.Date2.CompareTo("1398/12/29") > 0) ||
                (IsGardenAndResidence(customerInfo.UsageId) && nerkh.Date2.CompareTo("1400/12/29") < 0))
            {
                //Bmas2_7 = 0;

                if (customerInfo.ContractualCapacity > 0 &&
                    IsNotConstruction(customerInfo.BranchType))
                {
                    contractInDuration = (customerInfo.ContractualCapacity / 30.0) * nerkh.Duration;
                    if (nerkh.PartialConsumption > contractInDuration || IsSchool(customerInfo.UsageId))
                    {
                        disAllowedConsumption = nerkh.PartialConsumption - contractInDuration;
                        allowedConsumption = nerkh.PartialConsumption - disAllowedConsumption;
                        if (nerkh.PartialConsumption <= contractInDuration)
                        {
                            allowedConsumption = nerkh.PartialConsumption;
                            disAllowedConsumption = 0;
                        }
                    }
                }//c#:313  foxpro:1642

                if (disAllowedConsumption > 0)
                    allowedBoodje = nerkh.ZaribBodje * disAllowedConsumption;
                else
                    allowedBoodje = 0;
            }
            else
            {
                double domesticConsumption = 0;
                if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) &&
                    nerkh.Date2.CompareTo("1398/12/29") > 0)//always false because date is invalid
                {
                    // mas_maskoni = 0;

                    if (IsDomesticWithoutUnspecified(customerInfo.UsageId) && monthlyConsumption >= firstOlgoo && customerInfo.DomesticUnit > 0)
                    {
                        domesticConsumption = (((monthlyConsumption - firstOlgoo) * customerInfo.DomesticUnit) / 30.0) * nerkh.Duration;
                    }
                    else
                    {
                        if (monthlyConsumption > firstOlgoo)
                            domesticConsumption = ((monthlyConsumption - firstOlgoo) / 30.0) * nerkh.Duration;
                        else
                            domesticConsumption = 0;
                    }
                    allowedBoodje = nerkh.ZaribBodje * domesticConsumption;
                }
                else
                {
                    allowedBoodje = nerkh.ZaribBodje * nerkh.PartialConsumption;
                }//c#:344  foxpro:1673
            }
            if (!IsNotConstruction(customerInfo.BranchType))
                allowedBoodje = 0;

            if (IsTankerSaleAndVillage(customerInfo.UsageId))
                allowedBoodje = 0;

            if (customerInfo.ZoneId == 151511)
                allowedBoodje = 0;

            if (IsVillage(customerInfo.ZoneId))
            {
                int cod_rosta = int.Parse(customerInfo.VillageId.Trim().Substring(0, 4));
                if (RuralButIsMetro(customerInfo.ZoneId, ulong.Parse(customerInfo.VillageId)))
                {
                    //nothing
                }
                else
                {
                    if (IsDomesticWithoutUnspecified(customerInfo.UsageId))
                    {
                        allowedBoodje = (allowedBoodje / 2);
                    }
                }
            }//foxpro:1713
            if (IsDolatabadOrHabibabadWithConditionEshtrak(customerInfo.ZoneId, ulong.Parse(customerInfo.ReadingNumber), customerInfo.VillageId))
            {
                if (IsDomesticWithoutUnspecified(customerInfo.UsageId))
                {
                    allowedBoodje = (allowedBoodje / 2);
                }
            }//L 1724            
            double multiplierAbBaha = Multiplier(zarib, nerkh.Olgo, IsDomestic(customerInfo.UsageId), isVillageCalculation, monthlyConsumption);
            if (IsGardenAndResidence(customerInfo.ZoneId) &&
                nerkh.Date2.CompareTo("1401/12/28") <= 0)
            {
                int vzarib_baha = 1;
            }//foxpro:1740

            abBahaAmount = abBahaAmount * multiplierAbBaha;

            double fazelabAmount = abBahaAmount;
            oldAbBahaAmount = oldAbBahaAmount * multiplierAbBaha;// foxpro:1755
            return abBahaAmount;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        private int PartTime(string date1, string date2, string previousDate, string currentDate)
        {
            int partMethod = 0;
            partMethod = IsBetween(previousDate, date1, date2) && IsBetween(currentDate, date1, date2) ? (int.Parse)(CalculationDistanceDate.CalcDistance(previousDate, currentDate)) : 0;
            partMethod = previousDate.CompareTo(date1) <= 0 && IsBetween(currentDate, date1, date2) ? (int.Parse)(CalculationDistanceDate.CalcDistance(date1, currentDate)) : 0;
            partMethod = currentDate.CompareTo(date2) >= 0 && IsBetween(currentDate, date1, date2) ? (int.Parse)(CalculationDistanceDate.CalcDistance(previousDate, date2)) : 0;
            partMethod = previousDate.CompareTo(date1) <= 0 && currentDate.CompareTo(date2) >= 0 ? (int.Parse)(CalculationDistanceDate.CalcDistance(date1, date2)) : 0;

            return partMethod;
        }
        private double CalcHotSeason(double abBahaAmount, int duration, string date1, string date2)
        {
            string hotSeasonStart = date2.Substring(0, 4) + "/02/31";
            string hotSeasonEnd = date2.Substring(0, 4) + "/06/31";

            int hotSeasonDuration = PartTime(hotSeasonStart, hotSeasonEnd, date1, date2);
            return hotSeasonDuration > 0 && PartTime(hotSeasonStart, hotSeasonEnd, date1, date2) > 0 ? (int)((hotSeasonDuration * abBahaAmount / duration) * 0.2) : 0;

        }
        private double CalcHotSeasonDiscount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, double multiplierAbBaha, (double, double) abBahaValues, double hotSeasonAmount, double abBahaAmount)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            double abBahaMultiplied = 0;

            if ((IsReligiousWithCharity(customerInfo.UsageId)) && IsNotConstruction(customerInfo.BranchType))
            {
                abBahaMultiplied = (abBahaValues.Item1 * multiplierAbBaha);
                abBahaAmount = abBahaAmount - abBahaMultiplied;
                if (hotSeasonAmount != 0)
                {
                }
            }//line->1975

            throw new NotImplementedException();
        }
        private double CalculateAbBahaDiscount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo, double firstOlgoo, double monthlyConsumption, double abBahaAmount, double multiplierAbBaha, (double, double) abBahaValues)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            double fazelabAmount = abBahaAmount;
            double cosumptionDiscount = 0, abBahaDiscountTemp = 0, abBahaDiscount = 0, fazelbDiscount = 0;
            bool isVillage = IsVillage(customerInfo.ZoneId);
            int divider = isVillage ? 2 : 1;

            cosumptionDiscount = (firstOlgoo / 30) * nerkh.Duration;//L 1831

            if (IsDomesticWithoutUnspecified(customerInfo.UsageId) && IsNotConstruction(customerInfo.BranchType) && nerkh.Date1.CompareTo("1401/12/27") >= 0)//nerkh.Date1>="1401/12/27"
            {
                if (IsHandoverDiscount(customerInfo.BranchType))
                {
                    if (monthlyConsumption > firstOlgoo)
                        if (nerkh.Date2.CompareTo("1403/09/13") <= 0)//TMP_NERKH.date2<="1403/09/13"
                        {
                            abBahaDiscountTemp = (int)((cosumptionDiscount * ((((3706 * firstOlgoo) - 13845) / firstOlgoo) * 1.15) * multiplierAbBaha) / divider);
                        }
                        else
                        {
                            abBahaDiscountTemp = (int)((cosumptionDiscount * ((((70000 * 0.01 * firstOlgoo)) * firstOlgoo) / firstOlgoo) * multiplierAbBaha) / divider);
                        }
                    abBahaDiscount = abBahaDiscount + abBahaDiscountTemp;
                    abBahaAmount = abBahaAmount - abBahaDiscountTemp;
                }
                else
                {
                    abBahaDiscount = abBahaDiscount + abBahaAmount;
                    abBahaAmount = 0;
                }//line -> 1879
            }//L 1883

            return abBahaDiscount;
        }

        private bool IsGardenOrDweltyAfter1400_12_24(int usageId, string nerkhDate1)
        {
            string baseDate = "1400/24/12";
            int[] usageIds = [25, 34];
            return usageIds.Contains(usageId) && nerkhDate1.CompareTo(baseDate) >= 0;
        }
        private bool IsLessThan1403_09_13(string nerkhDate2)
        {
            string baseDate = "1403/09/13";
            return nerkhDate2.CompareTo(baseDate) <= 0;
        }
        private bool StringConditionMoreThan(string fromDate, string toDate)
        {
            DateOnly? from = fromDate.ToGregorianDateOnly();
            DateOnly? to = toDate.ToGregorianDateOnly();
            if (!from.HasValue && !to.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }

            if (from.Value >= to.Value)
                return true;

            return false;
        }
        private (long, long) Get2PartAmount(string nerkhDate2)
        {
            if (StringConditionMoreThan("1400/12/25", nerkhDate2))//nerkhDate2 <= '1400/12/25'
            {
                return (3776, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, "1400/12/25") && StringConditionMoreThan("1402/04/23", nerkhDate2))//nerkhDate2 > '1400/12/25'   AND nerkhDate2 <= '1402/04/23'
            {
                return (4040, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, "1402/04/23") && StringConditionMoreThan("1403/06/25", nerkhDate2))//nerkhDate2 > '1402/04/23' AND nerkhDate2 <= '1403/06/25'
            {
                return (4040, 168110);
            }
            else if (StringConditionMoreThan(nerkhDate2, "1403/06/25") && StringConditionMoreThan("1403/09/13", nerkhDate2))//(nerkhDate2 > '1403/06/25' AND nerkhDate2 <= '1403/09/13'
            {
                return (4323, 350000);
            }
            else if (StringConditionMoreThan(nerkhDate2, "1403/09/13") && StringConditionMoreThan("1404/02/31", nerkhDate2))//nerkhDate2 > '1403/09/13' AND nerkhDate2 <= '1404/02/31'
            {
                return (7000, 350000);
            }
            else if (StringConditionMoreThan(nerkhDate2, "1404/02/31")) //nerkhDate2 > '1404/02/31'
            {
                return (9000, 450000);
            }
            return (0, 0);
        }
        private static bool IsEducation(int usageId)
        {
            int[] collection = [7, 8, 41];
            return collection.Contains(usageId);

        }
        private static bool IsEducationOrBath(int usageId)
        {
            int[] collection = [7, 8, 41, 11];
            return collection.Contains(usageId);
        }
        private static (long, long) BigCase(int usageId, string nerkhDate1, string nerkhDate2, bool isSpecial, long abAzad39)
        {
            //start line 1228
            //1                                                  
            if ((IsEducation(usageId) &&
            nerkhDate2.CompareTo("1399/01/31") > 0 &&//TMP_NERKH.Date2 > '1399/01/31'
            nerkhDate2.CompareTo("1400/01/31") <= 0))//TMP_NERKH.Date2 <= '1400/01/31'
            {
                if (usageId == 9 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (9525, 45000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3529, 45000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (3529, 45000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (10953, 45000);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (3529, 45000);
                }
            }
            //2
            else if (IsEducation(usageId) &&
                     nerkhDate2.CompareTo("1400/01/31") > 0 &&//TMP_NERKH.Date2 > '1400/01/31'
                     nerkhDate2.CompareTo("1400/12/24") <= 0)//TMP_NERKH.Date2 <= '1400/12/24'
            {
                if (usageId == 9 && isSpecial)
                {
                    return (11720, 133255);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (11720, 133255);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (11720, 133255);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (3776, 133255);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (3776, 133255);
                }
            }
            // 3
            else if (IsEducation(usageId) &&
                     nerkhDate2.CompareTo("1400/12/24") > 0 &&
                     nerkhDate2.CompareTo("1401/12/27") <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (3776, 168110);
                }
            }

            // 4
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo("1401/12/27") > 0 &&
                     nerkhDate2.CompareTo("1402/04/23") <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (33622, 168110);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (3776, 168110);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (4040, 168110);
                }
                else if (usageId == 11)
                {
                    return (8644, 8644);
                }
            }

            // 5
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo("1402/04/23") > 0 &&
                     nerkhDate2.CompareTo("1403/06/25") <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 225000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (4323, 225000);
                }
                else if (usageId == 11)
                {
                    return (8644, 8644);
                }
            }

            // 6
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo("1403/06/25") > 0 &&
                     nerkhDate2.CompareTo("1403/09/13") <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 350000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 7 && !isSpecial)
                {
                    return (4323, 350000);
                }
                else if (usageId == 11)
                {
                    return (8644, 8644);
                }
            }

            // 7
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo("1403/09/13") > 0 &&
                     nerkhDate2.CompareTo("1404/02/31") <= 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 350000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (7000, 350000);
                }
                else if (usageId == 11)
                {
                    return (7000, 350000);
                }
            }

            // 8
            else if (IsEducationOrBath(usageId) &&
                     nerkhDate2.CompareTo("1404/02/31") > 0)
            {
                if (usageId == 9 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 9 && !isSpecial)
                {
                    return (45000, 450000);
                }
                else if (usageId == 41 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 41 && !isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 8 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 8 && !isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 7 && isSpecial)
                {
                    return (9000, 450000);
                }
                else if (usageId == 11)
                {
                    return (9000, 450000);
                }
            }


            else
            {
                //long nerkh_azad = CalculateAzad(nerkhDate1, nerkhDate2, 39);//&& ab azad sakht va saz  && ab azad omomi kargahi** dar  tarikh 1398 / 01 / 31
                return (0, abAzad39);

            }
            return (0, 0);
            //end line 1532
        }


        private static int CalculateAzad(string date1, string date2, int kar)
        {
            return 150000;
        }
        private static bool IsBetween(int baseZoneId, int zoneIdParam, string readingNumber, string fromNumber, string toNumber)
        {
            return
                baseZoneId == zoneIdParam &&
                readingNumber.Trim().CompareTo(fromNumber) >= 0 &&
                readingNumber.Trim().CompareTo(toNumber) <= 0;
        }
        private static bool RuralButIsMetro(int zoneId, string readingNumber)
        {
            return
                IsBetween(141911, zoneId, readingNumber, "10340005001", "10340908000") ||
                IsBetween(141914, zoneId, readingNumber, "10610001000", "10610800000") ||
                IsBetween(144015, zoneId, readingNumber, "60000000000", "60999999999") ||
                IsBetween(144015, zoneId, readingNumber, "62000000000", "62999999999") ||
                IsBetween(144016, zoneId, readingNumber, "22000000000", "22999999999") ||
                IsBetween(144016, zoneId, readingNumber, "24000000000", "24999999999") ||
                IsBetween(141611, zoneId, readingNumber, "10060001000", "10060797000") ||
                IsBetween(144411, zoneId, readingNumber, "10160001000", "10161024000") ||
                IsBetween(143411, zoneId, readingNumber, "10930000000", "10939999999") ||
                IsBetween(143411, zoneId, readingNumber, "71093000000", "71093999999") ||
                IsBetween(143411, zoneId, readingNumber, "81093000000", "81093999999") ||
                IsBetween(143411, zoneId, readingNumber, "10900000000", "10909999999") ||
                IsBetween(143411, zoneId, readingNumber, "71090000000", "71090999999") ||
                IsBetween(143411, zoneId, readingNumber, "81090000000", "81090999999") ||
                IsBetween(143012, zoneId, readingNumber, "10100000000", "10109999999") ||
                IsBetween(143012, zoneId, readingNumber, "10170000000", "10179999999") ||
                IsBetween(143012, zoneId, readingNumber, "10160000000", "10169999999") ||
                IsBetween(143012, zoneId, readingNumber, "10290000000", "10299999999") ||
                IsBetween(143012, zoneId, readingNumber, "10130000000", "10139999999") ||
                IsBetween(142211, zoneId, readingNumber, "10340000000", "10349999999") ||
                IsBetween(142211, zoneId, readingNumber, "10370000000", "10379999999") ||
                IsBetween(142211, zoneId, readingNumber, "10380000000", "10389999999") ||
                IsBetween(142215, zoneId, readingNumber, "10220000000", "10229999999");

        }
        private (double, double) CalculateBoodjePart1(int zoneId, int usageId, double partialConsumption, double duration, int branchType, int contractualConsumption, int domesticUnit,string nerkhDate2, int otherUnit, int householdNumber, double olgoab, int emptyUnit)
        {
            if (IsGardenAndResidence(usageId))
            {
                domesticUnit += otherUnit;
                domesticUnit = domesticUnit == 0 ? 1 : domesticUnit;
            }

            domesticUnit = domesticUnit > otherUnit && IsDomesticWithoutUnspecified(usageId) ? householdNumber : domesticUnit;

            domesticUnit = IsDomesticWithoutUnspecified(usageId) && emptyUnit != 0 ? domesticUnit - emptyUnit : domesticUnit;
            domesticUnit = domesticUnit < 0 ? 1 : domesticUnit;

            if (nerkhDate2.CompareTo("1403/12/30") <= 0)
                return (0, 0);

            if (zoneId == 151511)
                return (0, 0);

            double B_mod_Feli = duration;
            double B_MAS_FEli = partialConsumption;
            double mas_z1 = 0;
            double mas_z2 = 0;

            if (IsDomesticWithoutUnspecified(usageId) || IsGardenAndResidence(usageId))
            {
                double z01 = (B_mod_Feli * olgoab * domesticUnit) / 30;
                if (z01 > B_MAS_FEli)
                    z01 = B_MAS_FEli;

                double z14 = B_MAS_FEli - z01;
                mas_z1 = z01;
                mas_z2 = z14;
            }
            if (!IsDomesticWithoutUnspecified(usageId) || !IsGardenAndResidence(usageId))
            {
                double z01 = (B_mod_Feli * contractualConsumption) / 30;
                if (z01 > B_MAS_FEli)
                    z01 = B_MAS_FEli;

                double z14 = B_MAS_FEli - z01;
                if (z14 < 0)
                    z14 = 0;

                mas_z1 = z01;
                mas_z2 = z14;
            }

            double bha1 = 2000 * mas_z1;//L 2403
            double bha2;

            if (!IsNotConstruction(branchType) || IsUsageConstructor(usageId))
                bha2 = 2000 * mas_z2;
            else
                bha2 = 4000 * mas_z2;

            if (HasDiscountBranch(branchType) || IsReligiousWithCharity(usageId))
            {
                bha1 = 0;
            }

            return (bha1, bha2);

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

        private double CalculateFazelab(NerkhGetDto nerkh, double ab_Fas, int duration, string currentDate, string sewageInstallatonDatejalali, int sewageCount, double sewagePercent, double partialConsumption)
        {
            double sewageAmount = 0;
            sewagePercent = sewagePercent / 100;

            //has foreach
            if (sewageCount == 0)
            {
                sewageAmount = 0;
            }
            else if (sewageCount == 1 && string.Compare(currentDate, sewageInstallatonDatejalali) > 0)
            {
                if (string.Compare(nerkh.Date2, "1389/09/27") >= 0 && string.Compare(nerkh.Date2, "1394/06/31") <= 0)
                {
                    string vajFazStr = string.IsNullOrWhiteSpace(nerkh.VajFaz) ? "0" : nerkh.VajFaz.Trim();
                    double vajFaz = double.TryParse(vajFazStr, out var parsedVajFaz) ? parsedVajFaz : 0;

                    if (vajFaz.ToString().Length > 8)
                        vajFaz = 0;

                    sewageAmount = partialConsumption * vajFaz;

                    if (sewageAmount > 0)
                    {
                        int mod_as_nasb = PartTime(sewageInstallatonDatejalali, nerkh.Date2, sewageInstallatonDatejalali, currentDate);
                        sewageAmount = (sewageAmount / nerkh.Duration) * mod_as_nasb;
                    }
                    else
                    {
                        sewageAmount = 0;
                    }
                }
                else
                {
                    int zaman = (int.Parse)(CalculationDistanceDate.CalcDistance(sewageInstallatonDatejalali, currentDate));
                    sewageAmount = (ab_Fas / duration) * zaman * sewagePercent;
                }

                if (string.Compare(sewageInstallatonDatejalali, nerkh.Date2) <= 0)
                {
                    sewageCount = 2;
                }
            }
            else if (sewageCount == 2 || currentDate == sewageInstallatonDatejalali)
            {
                if (string.Compare(nerkh.Date2, "1389/09/27") >= 0 && string.Compare(nerkh.Date2, "1394/06/31") <= 0)
                {
                    string vajFazStr = string.IsNullOrWhiteSpace(nerkh.VajFaz) ? "0" : nerkh.VajFaz.Trim();
                    double vajFaz = double.TryParse(vajFazStr, out var parsedVajFaz) ? parsedVajFaz : 0;

                    if (vajFaz.ToString().Length > 8)
                        vajFaz = 0;

                    sewageAmount = partialConsumption * vajFaz;
                }
                else
                {
                    sewageAmount = ab_Fas * sewagePercent;
                }
            }

            return sewageAmount;
        }
        private long CalculateFazelabDiscount(NerkhGetDto nerkh, CustomerInfoOutputDto customerInfo, double monthlyConsumption, double firstOlgoo, double abBahaDiscount, double fazelabAmount, double ab_Fas)
        {
            double fazelbDiscount = 0;
            //line->1916
            if (IsDomesticWithoutUnspecified(customerInfo.UsageId))
            {
                if (abBahaDiscount != 0 && fazelabAmount != 0)
                {
                    if (monthlyConsumption > firstOlgoo)
                    {
                        fazelbDiscount += (int)(abBahaDiscount * 0.7);
                        fazelabAmount -= (int)(abBahaDiscount * 0.7);
                    }
                    else
                    {
                        fazelbDiscount += (int)(ab_Fas * 0.7);
                        fazelabAmount = 0;
                    }
                }
            }


            //TMP_NERKH.date2 > "1395/02/31" 
            //if (nerkh.Date2.CompareTo("1395/02/31") > 0 && zaribFaslAmountTemp != 0)
            //{
            //    if (V_FASBAHA1 != 0)
            //    {
            //        if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) && IsNotConstruction(customerInfo.BranchType))
            //            V_FASBAHA1 = V_FASBAHA1 + (zaribFaslAmountTemp * 0.7);
            //        else
            //            V_FASBAHA1 = V_FASBAHA1 + (zaribFaslAmountTemp * 1);
            //    }

            //}//line -> 2009

            //if (IsReligiousWithCharity(customerInfo.UsageId))
            //{
            //    if (abBahaDiscountTemp != 0 && V_FASBAHA1 != 0)
            //    {
            //        fazelbDiscount += abBahaDiscountTemp;
            //        V_FASBAHA1 -= abBahaDiscountTemp;

            //        if (V_FASBAHA1 < 0)
            //        {
            //            V_FASBAHA1 = 0;
            //        }
            //    }
            //}

            //if (fazelbDiscount != 0)
            //{
            //    if ((IsDomesticWithoutUnspecified(customerInfo.UsageId) || IsGardenAndResidence(customerInfo.UsageId)) && IsNotConstruction(customerInfo.BranchType))
            //    {
            //        fazelbDiscount = fazelbDiscount;
            //        V_FASBAHA1 = V_FASBAHA1;
            //    }
            //    else
            //    {
            //        fazelbDiscount = fazelbDiscount + (VZFASL_olgo * 1);
            //        V_FASBAHA1 = V_FASBAHA1 - (VZFASL_olgo * 1);
            //    }
            //    if (V_FASBAHA1 < 0)
            //        V_FASBAHA1 = 0;
            //}//line-> 2050
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
            //L 2073 javan_sazi
            throw new NotImplementedException();
        }
        private long CalculateJavaniJamiatDiscount()
        {
            throw new NotImplementedException();
        }

        private long CalculateAvarez()
        {
            //Avarez

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
