using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using DNTPersianUtils.Core;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal class FoxproTarifAb
    {
        int noe_ensh_ = 1;//UsageId
        int fix_mas_ = 0;//Contractual Capacity
        double AB1 = 1, VAJ_ = 1, O_VAJ_ = 1;
        double mas1_ = 12.3, O_AB1 = 1, mas_fi_roz;
        double rate_, olgoab, noe_va_, V_vaj1_7, V_vaj2_7, mod1_, mas1_7, mas2_7;
        bool Edareh_k_;
        double noe_va;
        NerkhGetDto TMP_NERKH;
        int nerkh_azad;

        private void TestMethod()
        {
            //from line:1105
            if ((
                    (
                        (noe_ensh_ == 1 || noe_ensh_ == 3) || fix_mas_ == 0
                    )
                     ||
                     (noe_ensh_ == 34 && StringConditionMoreThanEqual(TMP_NERKH.Date1, "1400/12/24")) //TMP_NERKH.Date1 >= "1400/12/24"
                     ||
                     (noe_ensh_ == 25 && StringConditionMoreThanEqual(TMP_NERKH.Date1, "1400/12/24"))//TMP_NERKH.Date1 >= "1400/12/24"
                )
                &&
                (
                    noe_ensh_ != 10 &&
                    noe_ensh_ != 12 &&
                    noe_ensh_ != 13 &&
                    noe_ensh_ != 32 &&
                    noe_ensh_ != 29
                ))
            {
                AB1 = mas1_ * VAJ_;
                //TMP_NERKH.Date2 <= "1403/09/13"
                if (StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2) && O_VAJ_ != 0)
                {
                    O_AB1 = (mas1_ * O_VAJ_) * 1.15;
                }
                else
                {
                    O_AB1 = 0;
                }


                if (
                    StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2) &&//TMP_NERKH.Date2 <= "1403/09/13"
                    rate_ <= olgoab &&
                    AB1 > O_AB1 &&
                    O_AB1 > 0 &&
                    (noe_ensh_ == 1 || noe_ensh_ == 3) &&
                    noe_va_ != 4
                )
                {
                    AB1 = O_AB1;
                }
            }
            else
            {
                //line=1139
                if ((fix_mas_ > 0 && noe_va_ != 4) ||
                      (noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 30 || noe_ensh_ == 29 || noe_ensh_ == 32))  // && If5 &&  TMP_NERKH.date1>='1389/09/27'
                {
                    mas_fi_roz = (fix_mas_ / 30) * mod1_;



                    if ((mas1_ > mas_fi_roz) ||
                        (noe_ensh_ == 7 || noe_ensh_ == 8) ||
                        (noe_ensh_ == 30 || noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 29 || noe_ensh_ == 32))
                    {
                        mas2_7 = mas1_ - mas_fi_roz;
                        mas1_7 = mas1_ - mas2_7;


                        if (mas1_ <= mas_fi_roz)//ok ->1150
                        {
                            mas1_7 = mas1_;
                            mas2_7 = 0;
                        }


                        if ((fix_mas_ == 0) &&//ok
                            (noe_ensh_ == 30 || noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 29 || noe_ensh_ == 32))
                        {
                            mas1_7 = mas1_;
                            mas2_7 = 0;

                        }


                        V_vaj1_7 = VAJ_;



                        if (noe_ensh_ == 30 || noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 29 || noe_ensh_ == 32)
                            //  ab  masajed , hoseinieh  ,darl quran  ,  olom dini


                            if (noe_va != 4)//switch case line->1178
                            {
                                if (StringConditionMoreThan("1400/12/25", TMP_NERKH.Date2))//TMP_NERKH.Date2 <= '1400/12/25'
                                {
                                    V_vaj1_7 = 3776;
                                    V_vaj2_7 = 168110;
                                }
                                else if (StringConditionMoreThan(TMP_NERKH.Date2, "1400/12/25") && StringConditionMoreThan("1402/04/23", TMP_NERKH.Date2))//TMP_NERKH.Date2 > '1400/12/25'   AND TMP_NERKH.date2 <= '1402/04/23'
                                {
                                    V_vaj1_7 = 4040;
                                    V_vaj2_7 = 168110;
                                }
                                else if (StringConditionMoreThan(TMP_NERKH.Date2, "1402/04/23") && StringConditionMoreThan("1403/06/25", TMP_NERKH.Date2))//TMP_NERKH.date2 > '1402/04/23' AND TMP_NERKH.date2 <= '1403/06/25'
                                {
                                    V_vaj1_7 = 4040;
                                    V_vaj2_7 = 168110;
                                }
                                else if (StringConditionMoreThan(TMP_NERKH.Date2, "1403/06/25") && StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2))//(TMP_NERKH.date2 > '1403/06/25' AND TMP_NERKH.date2 <= '1403/09/13'
                                {
                                    V_vaj1_7 = 4323;
                                    V_vaj2_7 = 350000;
                                }
                                else if (StringConditionMoreThan(TMP_NERKH.Date2, "1403/09/13") && StringConditionMoreThan("1404/02/31", TMP_NERKH.Date2))//TMP_NERKH.date2 > '1403/09/13' AND TMP_NERKH.date2 <= '1404/02/31'
                                {
                                    V_vaj1_7 = 7000;
                                    V_vaj2_7 = 350000;
                                }
                                else if (StringConditionMoreThan(TMP_NERKH.Date2, "1404/02/31")) //TMP_NERKH.date2 > '1404/02/31'
                                {
                                    V_vaj1_7 = 9000;
                                    V_vaj2_7 = 450000;
                                }
                            }
                            else
                            {
                                nerkh_azad = 450000;   //&& ab azad sakht va  saz  �� ����� 14040231
                                V_vaj1_7 = 450000; //&&  ab azad
                                V_vaj2_7 = 450000;  //&&  ab azad

                            }


                        else//line => 1225
                        {

                            BigCase();

                            //big case
                        }

                        //line -> 1539
                        //اکولاد های بسته شده بعد اکسپشن مثالی هستند  و صحیح نیستند
                        throw new BaseException("");

                    }
                }
            }
        }
        private void BigCase()
        {
            //start line 1228
            //1                                                  
            if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41) &&
                     StringConditionMoreThan(TMP_NERKH.Date2, "1399/01/31") &&//TMP_NERKH.Date2 > '1399/01/31'
                     StringConditionMoreThan("1400/01/31", TMP_NERKH.Date2))//TMP_NERKH.Date2 <= '1400/01/31'
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 10953;
                    V_vaj2_7 = 45000;
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 9525;
                    V_vaj2_7 = 45000;
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 10953;
                    V_vaj2_7 = 45000;
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 3529;
                    V_vaj2_7 = 45000;
                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 10953;
                    V_vaj2_7 = 45000;
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 3529;
                    V_vaj2_7 = 45000;
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 10953;
                    V_vaj2_7 = 45000;
                }
                else if (noe_ensh_ == 7 && !Edareh_k_)
                {
                    V_vaj1_7 = 3529;
                    V_vaj2_7 = 45000;
                }
            }
            //2
            else if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41) &&
                     StringConditionMoreThan(TMP_NERKH.Date2, "1400/01/31") &&//TMP_NERKH.Date2 > '1400/01/31'
                     StringConditionMoreThan("1400/12/24", TMP_NERKH.Date2))//TMP_NERKH.Date2 <= '1400/12/24'
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 11720; // 10953
                    V_vaj2_7 = 133255;
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 11720;//&& 9525
                    V_vaj2_7 = 133255;
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 3776;//&& 11720 && 10953
                    V_vaj2_7 = 133255;
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 3776;//&& 3529
                    V_vaj2_7 = 133255;


                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 11720;// && 10953
                    V_vaj2_7 = 133255;
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 3776;// && 3529
                    V_vaj2_7 = 133255;
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 3776;
                    V_vaj2_7 = 133255;
                }
                else if (noe_ensh_ == 7 && !Edareh_k_)
                {
                    V_vaj1_7 = 3776;
                    V_vaj2_7 = 133255;
                }


            }
            //3
            else if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41) &&
                     StringConditionMoreThan(TMP_NERKH.Date2, "1400/12/24") && //TMP_NERKH.Date2 > '1400/12/24'
                     StringConditionMoreThan("1401/12/27", TMP_NERKH.Date2))//TMP_NERKH.Date2 <= '1401/12/27'
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 33622;//&& 11720 && 10953
                    V_vaj2_7 = 168110;//&& 133255
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 33622;//&& 11720 && 9525
                    V_vaj2_7 = 168110;//&& 133255
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 3776;//&& 11720 && 10953
                    V_vaj2_7 = 168110;//&& 133255
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 3776;// && 3529
                    V_vaj2_7 = 168110;//&& 133255
                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 3776;//&& 33622 && 11720 && 10953
                    V_vaj2_7 = 168110;// && 133255
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 3776;//&& 3529
                    V_vaj2_7 = 168110;// && 133255
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 3776;
                    V_vaj2_7 = 168110;//&& 133255
                }
                else if (noe_ensh_ == 7 && !Edareh_k_)
                {
                    V_vaj1_7 = 3776;
                    V_vaj2_7 = 168110;//&& 133255
                }
            }
            //4
            else if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41 || noe_ensh_ == 11) &&
                     StringConditionMoreThan(TMP_NERKH.Date2, "1401/12/27") &&//TMP_NERKH.date2 > '1401/12/27' 
                     StringConditionMoreThan("1402/04/23", TMP_NERKH.Date2))//TMP_NERKH.date2 <= '1402/04/23'
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 33622;//&& 11720 && 10953
                    V_vaj2_7 = 168110;// && 133255
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 33622; //&& 11720 && 9525
                    V_vaj2_7 = 168110; //&& 133255
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 4040; //&& 3776 && 11720 && 10953
                    V_vaj2_7 = 168110; //&& 133255
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 3776; //&& 3529
                    V_vaj2_7 = 168110; //&& 133255
                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 4040; // && 3776 && 33622 && 11720 && 10953
                    V_vaj2_7 = 168110; //&& 133255
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 4040; //&& 3776 && 3529
                    V_vaj2_7 = 168110; //&& 133255
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 4040; //&& 3776
                    V_vaj2_7 = 168110; //&& 133255
                }
                else if (noe_ensh_ == 7 && !Edareh_k_)
                {
                    V_vaj1_7 = 4040; //&& 3776
                    V_vaj2_7 = 168110;// && 133255

                }
                else if (noe_ensh_ == 11)
                {
                    V_vaj1_7 = 8644;
                    V_vaj2_7 = 8644;
                }
            }
            //5
            else if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41 || noe_ensh_ == 11) &&
                     StringConditionMoreThan(TMP_NERKH.Date2, "1402/04/23") &&//TMP_NERKH.date2 > '1402/04/23' 
                     StringConditionMoreThan("1403/06/25", TMP_NERKH.Date2))//TMP_NERKH.date2 <= '1403/06/25' 
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 4323;//&& 11720 && 10953
                    V_vaj2_7 = 225000;//&& 168110 && 133255
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 45000;//&& 33622 && 11720 && 9525
                    V_vaj2_7 = 225000;//&& 168110 && 133255
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 4323;//&& 3776 && 11720 && 10953
                    V_vaj2_7 = 225000;//&& 168110 && 133255
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 4323;//&& 3776 && 3529
                    V_vaj2_7 = 225000;// && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 4323;//&& 3776 && 33622 && 11720 && 10953
                    V_vaj2_7 = 225000;// && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 4323;//&& 3776 && 3529
                    V_vaj2_7 = 225000;//&& 168110 && 133255
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 4323;//&& 3776
                    V_vaj2_7 = 225000;//&& 168110 && 133255
                }
                else if (noe_ensh_ == 7 && !Edareh_k_)
                {
                    V_vaj1_7 = 4323;//&& 3776
                    V_vaj2_7 = 225000;//&& 168110 && 133255
                }
                else if (noe_ensh_ == 11)
                {
                    V_vaj1_7 = 8644;
                    V_vaj2_7 = 8644;
                }
            }
            //6
            else if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41 || noe_ensh_ == 11) &&
                     StringConditionMoreThan(TMP_NERKH.Date2, "1403/06/25") &&//TMP_NERKH.date2 > '1403/06/25'
                     StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2))//TMP_NERKH.date2 <= '1403/09/13'
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 4323; // && 11720 && 10953
                    V_vaj2_7 = 350000; // && 225000 && 168110 && 133255
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 45000; // && 33622 && 11720 && 9525
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 4323; // && 3776 && 11720 && 10953
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 4323; // && 3776 && 3529
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 4323; // && 3776 && 33622 && 11720 && 10953
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 4323; // && 3776 && 3529
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 4323; // && 3776
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 7 && !Edareh_k_)
                {
                    V_vaj1_7 = 4323; // && 3776
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 11 /* && تاریخ >= 1403/09/14 */)
                {
                    V_vaj1_7 = 8644;
                    V_vaj2_7 = 8644;
                }
            }
            //7
            else if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41 || noe_ensh_ == 11) &&
                   StringConditionMoreThan(TMP_NERKH.Date2, "1403/09/13") &&//TMP_NERKH.date2 > '1403/09/13'  
                   StringConditionMoreThan("1404/02/31", TMP_NERKH.Date2))// TMP_NERKH.date2 <= '1404/02/31'
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 7000;//&& 4323 && 11720 && 10953
                    V_vaj2_7 = 350000; // && 225000 && 168110 && 133255
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 45000;//&& 33622 && 11720 && 9525
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 7000;//&& 4323 && 3776 && 11720 && 10953
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 7000;//&& 4323 && 3776 && 3529
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 7000;// && 4323 && 3776 && 33622 && 11720 && 10953
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 7000;//&& 4323 && 3776 && 3529
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 7000;//&& 4323 && 3776
                    V_vaj2_7 = 350000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 11)
                {
                    V_vaj1_7 = 7000;
                    V_vaj2_7 = 350000;
                }
            }
            //8
            else if ((noe_ensh_ == 7 || noe_ensh_ == 8 || noe_ensh_ == 41 || noe_ensh_ == 11) &&
                     StringConditionMoreThan(TMP_NERKH.Date2, "1404/02/31"))//TMP_NERKH.date2 > '1404/02/31'
            {
                if (noe_ensh_ == 9 && Edareh_k_)
                {
                    V_vaj1_7 = 9000; // && 4323 && 11720 && 10953
                    V_vaj2_7 = 450000; // && 225000 && 168110 && 133255
                }
                else if (noe_ensh_ == 9 && !Edareh_k_)
                {
                    V_vaj1_7 = 45000; // && 33622 && 11720 && 9525
                    V_vaj2_7 = 450000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 41 && Edareh_k_)
                {
                    V_vaj1_7 = 9000; // && 4323 && 3776 && 11720 && 10953
                    V_vaj2_7 = 450000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 41 && !Edareh_k_)
                {
                    V_vaj1_7 = 9000; // && 4323 && 3776 && 3529
                    V_vaj2_7 = 450000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && Edareh_k_)
                {
                    V_vaj1_7 = 9000; // && 4323 && 3776 && 33622 && 11720 && 10953
                    V_vaj2_7 = 450000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 8 && !Edareh_k_)
                {
                    V_vaj1_7 = 9000; // && 4323 && 3776 && 3529
                    V_vaj2_7 = 450000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 7 && Edareh_k_)
                {
                    V_vaj1_7 = 9000; // && 4323 && 3776
                    V_vaj2_7 = 450000; // && 168110 && 133255
                }
                else if (noe_ensh_ == 11 /* && تاریخ >= 1403/09/14 */)
                {
                    V_vaj1_7 = 9000;
                    V_vaj2_7 = 450000;
                }
            }
            else
            {
                nerkh_azad = abazad(TMP_NERKH.Date1, TMP_NERKH.Date2, 39);//&& ab azad sakht va saz  && ab azad omomi kargahi** dar  tarikh 1398 / 01 / 31
                V_vaj2_7 = nerkh_azad;//&& ab azad

            }
            //end line 1532
        }
        private int abazad(string date1, string date2, int kar)
        {
            return 102;
        }
        private bool StringConditionMoreThanEqual(string fromDate, string toDate)
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
        private bool StringConditionMoreThan(string fromDate, string toDate)
        {
            DateOnly? from = fromDate.ToGregorianDateOnly();
            DateOnly? to = toDate.ToGregorianDateOnly();
            if (!from.HasValue && !to.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }

            if (from.Value > to.Value)
                return true;

            return false;
        }
    }
}

