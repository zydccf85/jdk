using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using System.Configuration;
using JKD.Models;
namespace JKD.DB
{
    public  class DbContext
    {
        public SqlSugarClient Db;
        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig() {
                ConnectionString = ConfigurationManager.ConnectionStrings["mysqlConnectionStr"].ConnectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection=true
            
            });
        }
         public DbSet<Drug> DrugDb { get { return new DbSet<Drug>(Db); } }
        public DbSet<Cfdetail> cfdetailDb { get { return new DbSet<Cfdetail>(Db); } }
        public DbSet<Cfhead> cfheadDb { get { return new DbSet<Cfhead>(Db); } }
        public DbSet<Chufang> chufangDb { get { return new DbSet<Chufang>(Db); } }
        public DbSet<JKD.Models.Account> accountDb{ get { return new DbSet<JKD.Models.Account>(Db); } }
        public DbSet<Vaccount> vaccountDb { get { return new DbSet<Vaccount>(Db); } }
    }
}
