 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
using SqlSugar;
namespace JKD.DB
{
    class CfheadManager
    {
        public List<Cfhead> GetAll()
        {
            List<string> jeList = new DbContext().cfdetailDb
                .GetList(item => item.drug.Contains("艾司唑仑"))
                .Select(item => item.opertime).Distinct().ToList();
            return new DbContext().Db.Queryable<Cfhead>()
                .Mapper(it =>
                {
                    if (double.Parse(it.age.Replace('岁', '.').TrimEnd('月')) <= 12)
                    {
                        it.cftype = "儿科";
                    }
                    else if (jeList.Contains(it.opertime))
                    {
                        it.cftype = "精二";
                    }
                    else
                    {
                        it.cftype = "普通";
                    }
                    it.disease = (it.disease + "," + it.disease2).TrimEnd(',');

                })
                .Where(it => it.enable == 1).OrderBy(it => it.opertime, SqlSugar.OrderByType.Desc).ToList(); 
        }
        public List<Cfhead> GetListByDid(string did)
        {
            return GetAll().Where(item => item.did == did).ToList();
        }
        public List<Cfhead> GetListByPatientAndAddress(string patient,string address)
        {
            return GetAll().Where(item => item.patient==patient && item.address == address).ToList();
        }
        public List<Cfhead> GetListByCondition(string BeginTime,string EndTime,string Doctor,string Patient)
        {
            if (string.IsNullOrEmpty(BeginTime) || string.IsNullOrEmpty(EndTime)) return null;
            string bt = BeginTime + " 00:00:00";
            string et = EndTime + " 23:59:59";
            return GetAll()
                 .Where(it => String.Compare(it.opertime,bt)>=0 && String.Compare(it.opertime,et)<=0)
                 .Where(it => string.IsNullOrEmpty(Doctor) ? true : it.doctor.Contains(Doctor))
                 .Where(it => string.IsNullOrEmpty(Patient) ? true : it.patient.Contains(Patient)).ToList();
        }
        public List<Cfhead> GetListByConditionNotUnique(string BeginTime, string EndTime, string Doctor, string Patient)
        {
            string strSql = string.Format(@"SELECT * FROM cfhead WHERE  opertime >= '{0}' AND opertime <= '{1}' AND doctor LIKE '%{2}%' AND patient LIKE '%{3}%' AND enable =1 AND 
                pid IN (SELECT pid FROM cfhead where enable = 1 GROUP BY pid, totalprice HAVING COUNT(pid) > 1) ",BeginTime,EndTime,Doctor,Patient);


            return new DbContext().Db.SqlQueryable<Cfhead>(strSql).ToList();
        }
        public List<string> GetPidNotUnique()
        {
            return new DbContext().Db.SqlQueryable<Cfhead>("select pid FROM cfhead where enable = 1 GROUP BY pid, totalprice HAVING COUNT(pid) > 1")
                .Select(item => item.pid).ToList();
        }
        public string GetMaxTime()
        {
            return new DbContext().cfdetailDb.GetList().Max(item => item.opertime);
        }

    }
        
}
