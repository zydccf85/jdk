using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    public class FyByPatientManager
    {
        public int Insert(List<fybypatient> li)
        {
            return new DbContext().Db.Insertable<fybypatient>(li).ExecuteCommand();
        }
        public DateTime? GetMaxTime()
        {
            return new DbContext().Db.Queryable<fybypatient>().Max<DateTime?>(it => it.fytime);
        }
        public List<fybypatient> GetListByCondition(string beginTime,string endTime,string doctor,string code)
        {
            string sqlstr = $"select a.* from fybypatient a,fybydrug b " +
                $"where a.fytime = b.fytime and b.pid not in( select pid from fybydrug where price<0)"+
                $" and a.fytime between '{beginTime}' and '{endTime}' and a.doctor = '{doctor}' and  b.code = '{code}' ";
            return new DbContext().Db.SqlQueryable<fybypatient>(sqlstr).ToList();
        }
    }
}
