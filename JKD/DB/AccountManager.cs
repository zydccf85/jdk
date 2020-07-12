using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    class AccountManager
    {
        public List<string> GetListJKR()
        {
            List<string> li = new DbContext().accountDb.GetList(item=>item.isdelete==0).Where(item=>item != null).Select(item => item.jkr).Distinct().ToList<string>();
            return li;
        }
    }
}
