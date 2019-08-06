using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class ChargeBackService
    {
        public static List<ChargeBack> GetChargeBackByDept(long deptId, int year)
        {
            return ChargeBackDAO.GetChargeBackByDept(deptId, year);
        }

        public static List<ChargeBack> GetChargeBackByMonth(int month)
        {
            return ChargeBackDAO.GetChargeBackByMonth(month);
        }

        public static ChargeBack GetDistinctChargeBack(long deptId, int month)
        {
            return ChargeBackDAO.GetDistinctChargeBack(deptId, month);
        }

        public static void CreateChargeBack(ChargeBack chargeBack)
        {
            ChargeBackDAO.CreateChargeBack(chargeBack);
        }

        public static void UpdateChargeBack(int amount, long deptId)
        {
            ChargeBackDAO.UpdateChargeBack(amount, deptId);
        }

        public static void UpdateChargeBack(ChargeBack chargeBack)
        {
            if(GetDistinctChargeBack(chargeBack.DeptId,chargeBack.MonthOfOrder) != null)
            {
                UpdateChargeBack(chargeBack.Amount, chargeBack.DeptId);
            }
            else
            {
                CreateChargeBack(chargeBack);
            }
        }

    }
}