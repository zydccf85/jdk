using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    public class JkdManager
    {
        public List<Jkd> GetList(string beginTime,string endTime,string jkr)
        {
            jkr = (string.IsNullOrEmpty(jkr) || jkr=="所有") ? "" : jkr;
            string sqlstr = $"select * from acctitle a inner join account b on a.aid = b.accid and" +
                $" lrsj between '{beginTime} 00:00:00' and '{endTime} 23:59:59' and jkr like '%{jkr}%'";
            return new DbContext().Db.SqlQueryable<Jkd>(sqlstr).ToList();
        }
       public List<Jkd> GetList(int? id)
        {
            return new DbContext().Db.SqlQueryable<Jkd>($"select * from acctitle a inner join account b on a.aid = b.accid and b.id = {id}").ToList();
        }
    }
}
