using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    public class EmployeeManager
    {
        public DbContext dbcontext = new DbContext();
        public List<Employee> GetList()
        {
            return dbcontext.Db.SqlQueryable<Employee>("select * from employee").ToList();
        }
    }
}
