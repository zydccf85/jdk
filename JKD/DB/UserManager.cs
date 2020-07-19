using JKD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKD.DB
{
    public class UserManager
    {
        public DbContext dbcontext = new DbContext();
        public User GetUser(string username)
        {
            User user = dbcontext.Db.SqlQueryable<User>("select * from user").Where(it=>it.username==username).First();
            return user;
        }
        public int Insert(User u)
        {
            return dbcontext.Db.Insertable<User>(u).ExecuteCommand();
        }
        public List<User> GetList()
        {
            return dbcontext.Db.SqlQueryable<User>("select * from user").ToList();
        }
    }
}
