using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKD.DB
{
    public class DrugTJManager
    {
        public DataTable Get(string myname,string beginTime,string endTime) {

            var dt2 = new DbContext().Db.Ado.UseStoredProcedure()
                .GetDataTable("getquantity", new { myname = myname, beginTime=beginTime,endTime=endTime });
            return dt2;
        }
    }
}
