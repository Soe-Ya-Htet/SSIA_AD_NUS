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
            List<ChargeBack> chargeBacks = ChargeBackDAO.GetChargeBackByDept(deptId, year);
            foreach(ChargeBack chargeBack in chargeBacks)
            {
                chargeBack.DeptName = DepartmentService.GetDepartmentById(deptId).DeptName;
            }
            return chargeBacks;
        }

        public static List<ChargeBack> GetChargeBackByMonth(int month)
        {            
            List<ChargeBack> chargeBacks = ChargeBackDAO.GetChargeBackByMonth(month);
            foreach (ChargeBack chargeBack in chargeBacks)
            {
                chargeBack.DeptName = DepartmentService.GetDepartmentById(chargeBack.DeptId).DeptName;
            }
            return chargeBacks;
        }

        public static ChargeBack GetDistinctChargeBack(long deptId, int month)
        {
            return ChargeBackDAO.GetDistinctChargeBack(deptId, month);
        }

        public static void CreateChargeBack(ChargeBack chargeBack)
        {
            ChargeBackDAO.CreateChargeBack(chargeBack);
        }

        public static void UpdateChargeBack(long amount, long deptId)
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