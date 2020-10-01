using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    public class FyByDrugManager
    {
        public int Insert(List<fybydrug> li)
        {
            return new DbContext().Db.Insertable<fybydrug>(li).ExecuteCommand();
        }
        public DateTime? GetMaxTime()
        {
            return new DbContext().Db.Queryable<fybydrug>().Max(it => it.fytime);
        }
    }
}
