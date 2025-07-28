using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    public static class FoxproTarifAb
    {
        static int noe_ensh_ = 1;
        static int fix_mas_ = 0;
        static double AB1 = 1, VAJ_ = 1, O_VAJ_ = 1;
        static double mas1_ = 12.3, O_AB1 = 1, mas_fi_roz;

        //consumptionAverage -->rate                      //duration   //mas1->masrag maskoni
        static double rate_, olgoab, noe_va_, V_vaj1_7, V_vaj2_7, mod1_, mas1_7, mas2_7;//mas2-> masraf tejari
        static bool Edareh_k_;
        static double noe_va, AB1_7, Mas1_7, v_vaj1_7, AB2_7, Mas2_7, v_vaj2_7;
        static NerkhGetDto TMP_NERKH;
        static int oldvaj, tedad_khane_;
        //nerkh -> oldvaj

        static int NEWMAS, NEWMOD, mah_hot, nerkh_azad, city, modat_;
        static double V_FAS_BAHA, V_zTadil, V_SHAHRDARI, Z_FASL_, V_AB_20, v_bodjeh;
        static double VZARIB_D, SHAHRDARI1, VZFASL, mas_takh, takhf_10, takhfif_ab;
        static double mas_maskoni, vzarib_baha, shandle, vAb_sevom, rad, ab_Fas;
        static double abresani1, vzAbresani, V_Avarez, ab, NEW_BODJ01, vNewFa;
        static double eted_ejraei, vAb_sevom1, VZFASL_olgo, V_FASBAHA1, Abresani1;
        static double VzTadil_1, ab_takh, rosta_calc, first_olgo, abfar_x, vNewAb;

        static string VillageId;
        static string Eshtrak;
        static double jarime_, V_AB_10, VZARIB_D1, takhf_fasL, takhfif_fa, Gr_hes_ab;
        static double TMP_VZFASL, Bmas_fi_roz, Bmas2_7, Bmas1_7, v_bodjeh01, masraf_;
        static double VAB10, VAB20, Tmp_nFaz;

        static int zrb_bodjeh;
        static int tedad_mas;
        static string date_Ga_, date_Fe_;
        static string c20;//members.c20   
        static string emrooz;
        static bool TABSARE2;
        static bool drsd10;
        static bool zaribfasl;   
        static bool zTadil;
        static double radif;
        static string inst_fas;
        static double fazlab;
        static int ted_khane;
        static int TEDAD_vahd;
        static double Total_Ab = 0;
        private static void MappingProperties(NerkhGetDto currentNerkh, CustomerInfoOutputDto customerInfo)
        {
            mod1_ = currentNerkh.Duration;
            zTadil = currentNerkh.ZaribTadil;
            TABSARE2 = currentNerkh.Tabsare2;
            zaribfasl = currentNerkh.ZaribFasl;
            emrooz = DateTime.Now.ToShortPersianDateString();
            zrb_bodjeh = currentNerkh.Bodjeh_new;
            oldvaj = (int.Parse)(currentNerkh.OVaj);
            rate_ = currentNerkh.PartialConsumption;
            date_Ga_ = currentNerkh.Date1;
            date_Fe_ = currentNerkh.Date2;

            TEDAD_vahd = customerInfo.OtherUnit;
            tedad_khane_ = customerInfo.HouseholdNumber;
            inst_fas = customerInfo.SewageInstallationDateJalali;
            radif = customerInfo.Radif;
            tedad_mas = customerInfo.DomesticUnit;
            Eshtrak = customerInfo.ReadingNumber;
            noe_ensh_ = customerInfo.UsageId;
            fix_mas_ = customerInfo.ContractualCapacity;
            noe_va_ = customerInfo.BranchType;
            VillageId = customerInfo.VillageId;
            Edareh_k_ = customerInfo.IsSpecial;

        }

        public static double CalclNerkhAb(IEnumerable<NerkhGetDto> nerkhs, CustomerInfoOutputDto customerInfo)
        {
            foreach (NerkhGetDto item in nerkhs)
            {
                MappingProperties(item, customerInfo);

                TMP_NERKH = item;
                if (mod1_ > 0)//line->1036
                {
                    mas1_ = 00000000.00000000;
                    mas1_ = (masraf_ * mod1_) / modat_;

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
                        //ok
                        if (StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2) && O_VAJ_ != 0)
                        {
                            O_AB1 = (mas1_ * O_VAJ_) * 1.15;
                        }
                        else
                        {
                            O_AB1 = 0;
                        }


                        //ok
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


                                //ok ->1150
                                if (mas1_ <= mas_fi_roz)
                                {
                                    mas1_7 = mas1_;
                                    mas2_7 = 0;
                                }

                                //ok
                                if ((fix_mas_ == 0) &&
                                    (noe_ensh_ == 30 || noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 29 || noe_ensh_ == 32))
                                {
                                    mas1_7 = mas1_;
                                    mas2_7 = 0;

                                }


                                V_vaj1_7 = VAJ_;


                                //line --> 1167
                                if (noe_ensh_ == 30 || noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 29 || noe_ensh_ == 32)
                                {   //  ab  masajed , hoseinieh  ,darl quran  ,  olom dini
                                    if (noe_va != 4)//switch case line->1178
                                    {
                                        Line1178();
                                    }
                                    else
                                    {
                                        nerkh_azad = 450000;   //&& ab azad sakht va  saz  �� ����� 14040231
                                        V_vaj1_7 = 450000; //&&  ab azad
                                        V_vaj2_7 = 450000;  //&&  ab azad

                                    }
                                }
                                else//line => 1225
                                {
                                    BigCase();
                                }

                                //line -> 1539;
                                AB1_7 = Mas1_7 * v_vaj1_7;
                                AB2_7 = Mas2_7 * v_vaj2_7;
                                AB1 = AB1_7 + AB2_7;

                            }
                            else//If4
                            {
                                AB1 = (mas1_ * VAJ_);
                            }// && If4

                        }
                        else
                        {
                            AB1 = (mas1_ * VAJ_);
                        }// && If5
                    }//first if    --  line -> 1553


                    rosta_calc = 1;
                    if (StringConditionMoreThan("1403/12/30", TMP_NERKH.Date2))//TMP_NERKH.Date2 <= '1403/12/30'
                        first_olgo = 14;
                    else
                        first_olgo = olgoab;

                    //line 1568
                    if (abfar_x == 2 && (noe_ensh_ == 1 || noe_ensh_ == 3) && noe_va_ != 4)
                    {
                        int cod_rosta = int.Parse(VillageId.Trim().Substring(0, 4));

                        if (Shahri(city, Eshtrak.Trim()) ||
                       ((city == 142618 && Between2Number(cod_rosta, 1037, 1039)) ||
                        (city == 144311 && cod_rosta == 1090) ||
                        (city == 144311 && cod_rosta == 1093)) ||
                        (city == 144411 && cod_rosta == 1016) ||
                        (city == 143012 && cod_rosta == 1010) ||
                        (city == 143012 && cod_rosta == 1013) ||
                        (city == 143012 && Between2Number(cod_rosta, 1016, 1017)) ||
                        (city == 143012 && cod_rosta == 1029) ||
                        (city == 142714 && cod_rosta == 1019) ||
                        (city == 141911 && cod_rosta == 1034) ||
                        (city == 141914 && cod_rosta == 1061) ||
                        (city == 141611 && cod_rosta == 1006))
                        {
                            //
                        }
                        else
                        {
                            if (AB1 != 0)
                            {   //TMP_NERKH.date2<='1403/09/13'
                                if (StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2) || rate_ <= first_olgo)
                                {
                                    AB1 = (int)(AB1 / 2);
                                    O_AB1 = (int)(O_AB1 / 2);
                                    rosta_calc = 2;
                                }
                                else
                                {
                                    AB1 = (int)(AB1 * 0.65);
                                    O_AB1 = (int)(O_AB1 * 0.65);
                                    rosta_calc = 2;
                                }
                            }
                        }//line ->1591
                        if (2 == 3)
                        {
                            AB1 = 0;
                            O_AB1 = 0;
                        }
                    }


                    //line->1599
                    if ((city == 134013 && Between2Number(int.Parse(Eshtrak), 57000000, 57999999)) ||
                        (city == 134016 && Between2Number(int.Parse(Eshtrak), 28000000, 28999999)) ||
                         Rosta_shahr(city, Eshtrak, 4))
                    {
                        if ((noe_ensh_ == 1 || noe_ensh_ == 3) && noe_va_ != 4)
                        {
                            if (AB1 != 0)
                            {    //TMP_NERKH.date2<='1403/09/13'
                                if (StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2) || rate_ <= first_olgo)
                                {
                                    AB1 = (int)(AB1 / 2);
                                    O_AB1 = (int)(O_AB1 / 2);
                                    rosta_calc = 2;
                                }
                                else
                                {
                                    AB1 = (int)(AB1 * 0.65);
                                    O_AB1 = (int)(O_AB1 * 0.65);
                                    rosta_calc = 2;
                                }
                            }
                        }
                    }//line -> 1619


                    if (((noe_ensh_ != 1 && noe_ensh_ != 3) && (noe_ensh_ != 34 && noe_ensh_ != 25) && StringConditionMoreThan(TMP_NERKH.Date2, "1398/12/29")) //Tmp_nerkh.date2>"1398/12/29"
                        ||
                       ((noe_ensh_ == 34 || noe_ensh_ == 25) && StringConditionMoreThan("1400/12/29", TMP_NERKH.Date2)))//Tmp_nerkh.date2<"1400/12/29"
                    {
                        Bmas2_7 = 0;
                        if (fix_mas_ > 0 && noe_va_ != 4)
                        {
                            Bmas_fi_roz = (fix_mas_ / 30.0) * mod1_;

                            if (mas1_ > Bmas_fi_roz || (noe_ensh_ == 7 || noe_ensh_ == 8))
                            {
                                Bmas2_7 = mas1_ - Bmas_fi_roz;
                                Bmas1_7 = mas1_ - Bmas2_7;

                                if (mas1_ <= Bmas_fi_roz)
                                {
                                    Bmas1_7 = mas1_;
                                    Bmas2_7 = 0;
                                }
                            }
                        }//line->1642

                        if (Bmas2_7 > 0)//agar masraf  bishtar  az zarfiat  bod
                            v_bodjeh01 = zrb_bodjeh * Bmas2_7;
                        else
                            v_bodjeh01 = 0;
                    }
                    else
                    {
                        if ((noe_ensh_ == 1 || noe_ensh_ == 3 || noe_ensh_ == 34 || noe_ensh_ == 25) &&
                             StringConditionMoreThan(TMP_NERKH.Date2, "1398/12/29"))
                        {
                            mas_maskoni = 0;

                            if ((noe_ensh_ == 1 || noe_ensh_ == 3) && rate_ >= first_olgo && tedad_mas > 0)
                            {
                                mas_maskoni = (((rate_ - first_olgo) * tedad_mas) / 30.0) * mod1_;
                            }
                            else
                            {
                                if (rate_ > first_olgo)
                                    mas_maskoni = ((rate_ - first_olgo) / 30.0) * mod1_;
                                else
                                    mas_maskoni = 0;
                            }

                            v_bodjeh01 = zrb_bodjeh * mas_maskoni;
                        }
                        else
                        {
                            v_bodjeh01 = zrb_bodjeh * mas1_;
                        }
                    }
                    v_bodjeh01 = (int)v_bodjeh01;
                    if (noe_va_ == 4)
                        v_bodjeh01 = 0;

                    if (noe_ensh_ == 14 || noe_ensh_ == 15)
                        v_bodjeh01 = 0;


                    if (city == 151511)
                        v_bodjeh01 = 0;


                    if (abfar_x == 2)
                    {
                        int cod_rosta = int.Parse(VillageId.Trim().Substring(0, 4));

                        if (
                            (city == 142618 && cod_rosta >= 1037 && Between2Number(cod_rosta, 1037, 1039)) ||
                            (city == 144311 && cod_rosta == 1090) ||
                            (city == 144311 && cod_rosta == 1093) ||
                            (city == 144411 && cod_rosta == 1016) ||
                            (city == 143012 && cod_rosta == 1010) ||
                            (city == 143012 && cod_rosta == 1013) ||
                            (city == 143012 && cod_rosta >= 1016 && Between2Number(cod_rosta, 1016, 1017)) ||
                            (city == 143012 && cod_rosta == 1029) ||
                            (city == 142714 && cod_rosta == 1019) ||
                            (city == 141911 && cod_rosta == 1034) ||
                            (city == 141914 && cod_rosta == 1061) ||
                            (city == 141611 && cod_rosta == 1006)
                        )
                        {
                            //
                        }
                        else
                        {
                            if (noe_ensh_ == 1 || noe_ensh_ == 3)
                            {
                                v_bodjeh01 = (int)(v_bodjeh01 / 2);
                            }
                        }
                    }//line -> 1712

                    if ((city == 134013 && Between2Number(int.Parse(Eshtrak), 57000000, 57999999)) ||
                         (city == 134016 && Between2Number(int.Parse(Eshtrak), 28000000, 28999999)) ||
                         Rosta_shahr(city, Eshtrak, 4))
                    {
                        if (noe_ensh_ == 1 || noe_ensh_ == 3)
                        {
                            v_bodjeh01 = (int)(v_bodjeh01 / 2);
                        }
                    }//line -> 1723


                    vzarib_baha = sele_zarib(city, TMP_NERKH.Date1, TMP_NERKH.Date2, noe_ensh_, noe_va_, abfar_x, shandle, Eshtrak, rate_, olgoab);
                    if (noe_ensh_ == 25 || noe_ensh_ == 34)
                    {
                        if (StringConditionMoreThan("1401/12/28", TMP_NERKH.Date2))//TMP_NERKH.date2 <= "1401/12/28"
                            vzarib_baha = 1;
                    }//line -> 1740


                    if (2 == 3)// ted_ejraei = 0 &&   ����� �� ������� ����� ��ʐ���� ������
                    {
                        vAb_sevom = sevom_nerkh(shandle, rad, AB1, modat_, date_Ga_, date_Fe_, noe_va_, masraf_, VAJ_, fix_mas_, noe_ensh_, c20);
                        vAb_sevom = (int)vAb_sevom * vzarib_baha;
                    }

                    AB1 = vAb_sevom + (AB1 * vzarib_baha);
                    ab_Fas = AB1;
                    O_AB1 = vAb_sevom + (O_AB1 * vzarib_baha);//line->1754

                    vNewAb = (int)(vNewAb + (StringConditionMoreThan(TMP_NERKH.Date1, "1394/06/31") ? AB1 : 0));//IIF(TMP_NERKH.date1 > '1394/06/31', ab1, 0)
                    VAB10 = 0;
                    VAB20 = 0;




                    if (2 == 3 && emrooz == "1402/04/04")
                        VAB10 = (drsd10 && TABSARE2 && !Edareh_k_) ? (int)((FAZLAB2(Tmp_nFaz, date_Fe_)) ? (AB1 * 0.1) : 0) : 0;
                    else
                        VAB10 = (drsd10 && TABSARE2 && !Edareh_k_) ? (int)(FAZLAB() ? (AB1 * 0.1) : 0) : 0;


                    if ((city == 134013 && Between2Number(int.Parse(Eshtrak), 57000000, 57999999)) ||
                        (city == 134016 && Between2Number(int.Parse(Eshtrak), 28000000, 28999999)) ||
                        Rosta_shahr(city, Eshtrak, 4))
                    {
                        VAB10 = 0;
                    }//line -> 1785


                    if (rate_ <= 5 && (noe_ensh_ == 1 || noe_ensh_ == 3) &&
                        StringConditionMoreThanEqual(TMP_NERKH.Date1, "1399/09/30") &&// TMP_NERKH.date1>="1399/09/30"
                        StringConditionMoreThan("1401/12/27", TMP_NERKH.Date2) &&// TMP_NERKH.date2<="1401/12/27"
                        noe_va_ != 4)
                    {
                        if (noe_va_ == 6 || noe_va_ == 7 || noe_va_ == 3)
                        {
                            takhfif_ab += AB1;
                            if (VAB10 != 0)
                                takhf_10 += AB1 * 0.1;

                            AB1 = 0;
                            VAB10 = 0;
                        }
                    }//line-> 1809


                    if (2 == 3)
                    {
                        if ((noe_va_ == 6 || noe_va_ == 7 || noe_va_ == 3))
                        {
                            takhfif_ab = takhfif_ab + AB1;
                            if (VAB10 != 0)
                                takhf_10 += AB1 * 0.1;

                            AB1 = 0;
                            VAB10 = 0;
                        }
                    }
                    ab_takh = 0;
                    mas_takh = (first_olgo / 30) * mod1_;

                    //line->1823
                    if ((noe_ensh_ == 1 || noe_ensh_ == 3) && noe_va_ != 4 && StringConditionMoreThanEqual(TMP_NERKH.Date1, "1401/12/27"))//TMP_NERKH.date1>="1401/12/27"
                    {
                        if (noe_va_ == 6 || noe_va_ == 7 || noe_va_ == 3)
                        {
                            if (rate_ > first_olgo)
                            {
                                if (StringConditionMoreThan("1403/09/13", TMP_NERKH.Date2))//TMP_NERKH.date2<="1403/09/13"
                                {
                                    ab_takh = (int)((mas_takh * ((((3706 * first_olgo) - 13845) / first_olgo) * 1.15) * vzarib_baha) / rosta_calc);
                                }
                                else
                                {
                                    ab_takh = (int)((mas_takh * ((((70000 * 0.01 * first_olgo)) * first_olgo) / first_olgo) * vzarib_baha) / rosta_calc);
                                }
                                takhfif_ab = takhfif_ab + ab_takh;

                                if (VAB10 != 0)
                                    takhf_10 = takhf_10 + (ab_takh * 0.1);

                                AB1 = AB1 - ab_takh;
                                VAB10 = VAB10 - (ab_takh * 0.1);
                                if (VAB10 < 0)
                                    VAB10 = 0;
                            }
                            else
                            {
                                takhfif_ab = takhfif_ab + AB1;

                                if (VAB10 != 0)
                                    takhf_10 = takhf_10 + (AB1 * 0.1);

                                AB1 = 0;
                                VAB10 = 0;
                            }//line -> 1879
                        }
                    }//line-> 1882


                    TMP_VZFASL = 0;
                    VZFASL = zaribfasl ? Z_FASL(AB1, date_Ga_, date_Fe_, noe_ensh_, mod1_, TMP_NERKH.Date1, TMP_NERKH.Date2) : 0;
                    TMP_VZFASL = VZFASL;

                    SHAHRDARI1 = 0;
                    VzTadil_1 = zTadil ? (int)z_16(AB1, rate_, TMP_NERKH.Date1, TMP_NERKH.Date2, noe_ensh_) : 0;//line -> 1900
                    Abresani1 = 0;
                    V_FASBAHA1 = (int)CALC_FAS(city, shandle, radif, (ab_Fas - vAb_sevom), modat_, date_Ga_, date_Fe_, inst_fas, Tmp_nFaz, Gr_hes_ab, noe_va_, fazlab, mas1_);


                    //line->1915
                    if (noe_ensh_ == 1 || noe_ensh_ == 3)
                    {
                        if (takhfif_ab != 0 && V_FASBAHA1 != 0)
                        {
                            if (rate_ > first_olgo)
                            {
                                takhfif_fa += (int)(ab_takh * 0.7);
                                V_FASBAHA1 -= (int)(ab_takh * 0.7);
                            }
                            else
                            {
                                takhfif_fa += (int)(ab_Fas * 0.7);
                                V_FASBAHA1 = 0;
                            }
                        }
                    }
                    VZFASL_olgo = ab_takh;

                    if ((noe_ensh_ == 30 || noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 29 || noe_ensh_ == 32) && noe_va_ != 4)
                    {
                        ab_takh = (AB1_7 * vzarib_baha);
                        VZFASL_olgo = 0;
                        VZFASL_olgo = zaribfasl ? Z_FASL(AB1_7, date_Ga_, date_Fe_, noe_ensh_, mod1_, TMP_NERKH.Date1, TMP_NERKH.Date2) * vzarib_baha : 0;//line->1946
                        takhfif_ab = takhfif_ab + ab_takh;


                        if (VAB10 != 0)
                            takhf_10 = takhf_10 + (ab_takh * 0.1);

                        AB1 = AB1 - ab_takh;
                        VAB10 = VAB10 - (ab_takh * 0.1);

                        if (VAB10 < 0)
                            VAB10 = 0;

                        if (VZFASL != 0)
                        {
                            takhf_fasL = takhf_fasL + VZFASL_olgo;
                            VZFASL = VZFASL - VZFASL_olgo;

                            if (VZFASL < 0)
                                VZFASL = 0;
                        }
                    }//line->1974


                    if (2 == 3)
                    {
                        vAb_sevom1 = vAb_sevom;
                        V_FASBAHA1 = V_FASBAHA1 + vAb_sevom1;
                    }
                    eted_ejraei = 1;

                    //TMP_NERKH.date2 > "1395/02/31" 
                    if (StringConditionMoreThan(TMP_NERKH.Date2, "1395/02/31") && TMP_VZFASL != 0)
                    {
                        if (V_FASBAHA1 != 0)
                        {
                            if ((noe_ensh_ == 1 || noe_ensh_ == 3 || noe_ensh_ == 34 | noe_ensh_ == 25) && noe_va_ != 4)
                                V_FASBAHA1 = V_FASBAHA1 + (TMP_VZFASL * 0.7);
                            else
                                V_FASBAHA1 = V_FASBAHA1 + (TMP_VZFASL * 1);
                        }

                        if (VAB10 != 0)
                            VAB10 = VAB10 + (TMP_VZFASL * 0.1);
                    }//line -> 2008

                    if (noe_ensh_ == 30 || noe_ensh_ == 12 || noe_ensh_ == 13 || noe_ensh_ == 29 || noe_ensh_ == 32)
                    {
                        if (ab_takh != 0 && V_FASBAHA1 != 0)
                        {
                            takhfif_fa += ab_takh;
                            V_FASBAHA1 -= ab_takh;

                            if (V_FASBAHA1 < 0)
                            {
                                V_FASBAHA1 = 0;
                            }
                        }
                    }

                    if (takhfif_fa != 0)
                    {
                        if ((noe_ensh_ == 1 || noe_ensh_ == 3 || noe_ensh_ == 34 || noe_ensh_ == 25) && noe_va_ != 4)
                        {
                            takhfif_fa = takhfif_fa;
                            V_FASBAHA1 = V_FASBAHA1;
                        }
                        else
                        {
                            takhfif_fa = takhfif_fa + (VZFASL_olgo * 1);
                            V_FASBAHA1 = V_FASBAHA1 - (VZFASL_olgo * 1);
                        }
                        if (V_FASBAHA1 < 0)
                            V_FASBAHA1 = 0;
                    }//line-> 2050

                    //TMP_NERKH.Date1 > "1394/06/31"
                    vNewFa = (int)(vNewFa + (StringConditionMoreThan(TMP_NERKH.Date1, "1394/06/31") ? V_FASBAHA1 : 0));

                    NEW_BODJ01 = 0;
                    NEW_BODJ01 = NEW_BODJ(city, noe_ensh_, ab, VZFASL, mas1_, mod1_, noe_va_, rate_, fix_mas_, tedad_mas, TEDAD_vahd, date_Ga_, date_Fe_, vzarib_baha, VAJ_, TMP_NERKH.Bodjeh_new, TMP_NERKH.Date1, TMP_NERKH.Date2, ted_khane, olgoab);

                    VZARIB_D1 = 0;
                    VZARIB_D1 = javan_sazi(city, AB1, noe_ensh_, mas1_, mod1_, noe_va_, rate_, fix_mas_, tedad_mas, TEDAD_vahd, date_Ga_, date_Fe_, olgoab, TMP_NERKH.Date2);


                    if ((city == 134013 && Between2Number(int.Parse(Eshtrak), 57000000, 57999999)) ||
                        (city == 134016 && Between2Number(int.Parse(Eshtrak), 28000000, 28999999)) ||
                         Rosta_shahr(city, Eshtrak, 4))
                    {
                        VZARIB_D1 = 0;
                    }

                    VZARIB_D = VZARIB_D + VZARIB_D1;
                    V_Avarez = V_Avarez + Avarez(city, AB1, mas1_, noe_ensh_, noe_va_, rate_, TMP_NERKH.Date2);

                    ab = ab + AB1;
                    v_bodjeh = v_bodjeh + v_bodjeh01 + NEW_BODJ01;
                    V_AB_20 = V_AB_20 + VAB20;
                    V_AB_10 = V_AB_10 + VAB10;

                    Z_FASL_ = Z_FASL_ + VZFASL;
                    V_SHAHRDARI = V_SHAHRDARI + SHAHRDARI1;
                    V_zTadil = V_zTadil + VzTadil_1;
                    vzAbresani = vzAbresani + abresani1;//line-> 2111
                    V_FAS_BAHA = V_FAS_BAHA + V_FASBAHA1;

                }//line -> 2118
                 //??


                ab = (int)ab;
                V_FAS_BAHA = (int)V_FAS_BAHA;


                if (noe_ensh_ == 19)
                {
                    modat_ = 30;
                    date_Fe_ = emrooz;
                    mah_hot = int.Parse(emrooz.Substring(3, 2));//VAL(SUBSTR(emrooz,4,2))
                    if (Between2Number(mah_hot, 2, 6))
                    {
                        Z_FASL_ = ab * 0.2;
                    }
                }

                if (noe_ensh_ == 14)
                {
                    modat_ = 30;
                    date_Fe_ = emrooz;
                    mah_hot = int.Parse(emrooz.Substring(3, 2));//VAL(SUBSTR(emrooz,4,2))
                    if (Between2Number(mah_hot, 2, 6))
                    {
                        Z_FASL_ = ab * 0.2;
                    }
                }


                if (2 == 3)
                {
                    if (noe_ensh_ == 1 || noe_ensh_ == 3)
                    {
                        if (noe_va_ != 4)
                            jarime_ = JAR_BASE(noe_ensh_, rate_, (masraf_ - NEWMAS), (modat_ - NEWMOD), tedad_mas, tedad_khane_, date_Ga_, date_Fe_, olgoab);
                    }
                    else
                    {
                        //select tmp_nerkh
                        //go bott
                        if (Convert.ToInt32(city.ToString().Trim().Substring(0, 2)) == 13 && jarime_ == 0)
                        {
                            if (fix_mas_ == 0 && rate_ > 20 && !Edareh_k_)
                                jarime_ = 0;
                            else
                            {
                                if (fix_mas_ > 0)
                                    jarime_ = JARIME_3((masraf_ - NEWMAS), (modat_ - NEWMOD), oldvaj, fix_mas_);
                            }
                        }
                    }
                }//line-> 2181
                Total_Ab += ab;
            }
            return Total_Ab;
        }

        private static double JAR_GM(int noe_ensh_, double rate_, double olgo_)
        {
            return 0;
        }
        private static double JAR_SR(int noe_ensh_, double rate_, double olgo_)
        {
            return 0;
        }
        private static double JARIME_3(double mas_, double V_MODAT_, double V_VAJ_, int fix_mas_)
        {
            double mas_more = 0;
            double ms_mah_mor = (mas_ * 30 / V_MODAT_) - fix_mas_;
            if (mas_ == 0 || V_MODAT_ == 0)
                ms_mah_mor = 0;
            if (ms_mah_mor > 0)
                mas_more = (ms_mah_mor / 30) * V_MODAT_;

            int bahajar = (int)(mas_more * (V_VAJ_ * 2));

            return bahajar;
        }
        private static double JAR_BASE(int noe_ensh_, double rate_, double masraf, double modat_, int ted_vahd_, int v_ted_khane, string date_Ga_, string date_Fe_, double olgo_)
        {
            double x = rate_;
            double vajsard2 = JAR_SR(noe_ensh_, x, olgo_




                );
            double vajGarm2 = JAR_GM(noe_ensh_, x, olgo_);

            //

            return 0;
        }
        private static double Avarez(int city, double ab1, double mas1_, int noe_ensh_, double noe_va_, double rate_, string date2)
        {
            return 0;
        }
        private static double javan_sazi(int city, double ab1, int noe_ensh_, double mas1_, double mod1_, double noe_va_, double rate_, int fix_mas_, int tedad_mas, int tedad_vahd, string date_Ga_, string date_Fe_, double olgoab, string date2)
        {
            return 0;
        }
        private static double NEW_BODJ(int city, int noe_ensh_, double ab, double VZFASL, double MAS1_, double mod1_, double noe_va_, double rate_, int fix_mas_, int tedad_mas, int tedad_vahd, string date_Ga_, string date_Fe_, double vzarib_baha, double VAJ_, double bodjeh_new, string date1, string date2, int ted_khane, double olgoab)
        {
            return 0;
        }
        private static double CALC_FAS(int city, double shandle, double radif, double ab_Fas, int modat, string date_Ga, string date_Fe, string inst_fas, double Tmp_nFaz, double Gr_hes_ab, double noe_va_, double fazlab, double mas1_)
        {
            return 0;
        }
        private static double z_16(double ab1, double rage, string date1, string date2, int noe_ensh)
        {
            return 0;
        }
        private static double Z_FASL(double ab1, string date_ga, string date_fe, int noe_ensh, double mod1, string date1, string date2)
        {
            return 0;
        }
        private static bool FAZLAB2(double Tmp_nFaz, string date_Fe_)
        {
            return false;
        }
        private static bool FAZLAB()
        {
            return false;
        }
        private static double sevom_nerkh(double shandle, double rad, double ab1, int modat, string date_ga, string date_fa, double noe_va, double masraf, double vaj, int fix_mas, int noe_ens, string c20)
        {
            return 140000;
        }
        private static double sele_zarib(int zoneId, string date1, string date2, int noe_ensh, double noe_va, double abfar_X, double shandle, string shtrak, double rate_, double olgoAB)
        {
            return 140000;
        }
        private static bool Rosta_shahr(int city, string eshtrak, int rosta)
        {
            return false;
        }
        private static bool Between2Number(int number, int a, int b)
        {
            if (number >= a && number <= b)
                return true;

            return false;
        }
        private static bool Shahri(int zoneId, string eshtrak)
        {
            if (zoneId > 14000)
                return false;
            return true;
        }
        private static void Line1178()
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
        private static void BigCase()
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
        private static int abazad(string date1, string date2, int kar)
        {
            return 150000;
        }
        private static bool StringConditionMoreThanEqual(string fromDate, string toDate)
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
        private static bool StringConditionMoreThan(string fromDate, string toDate)
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

