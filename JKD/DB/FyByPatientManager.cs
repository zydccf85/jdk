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
    }
}
