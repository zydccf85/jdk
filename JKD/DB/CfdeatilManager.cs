using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    public class CfdeatilManager
    {
        public List<Cfdetail> GetListByOpertime(string oper)
        {
            return new DbContext().cfdetailDb.GetList(item => item.opertime == oper);
        }
    }
}
