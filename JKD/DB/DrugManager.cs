using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    public class DrugManager
    {
        public DbContext dbcontext = new DbContext();
        public List<Drug> GetAll()
        {
            return new DbContext().Db.SqlQueryable<Drug>("select * from drug").ToList();
        }
        public List<Drug> GetList(Drug drug)
        {
            string sqlStr = string.Format("select * from drug where name like '%{0}%' and address like '%{1}%' and form like '%{2}%' ",
                drug.name,drug.address,drug.form);
            return new DbContext().Db.SqlQueryable<Drug>(sqlStr).ToList();
        }
        public List<Drug> GetList(string searchcode)
        {
            return dbcontext.Db.Queryable<Drug>().WhereIF(!string.IsNullOrEmpty(searchcode), it => it.searchcode.Contains(searchcode)).ToList();
        }
        public List<Drug> GetListBySearchcode(string searchcode)
        {
            return dbcontext.Db.Queryable<Drug>().WhereIF(!string.IsNullOrEmpty(searchcode), it => it.searchcode.Contains(searchcode)).ToList();
        }
        public  List<Drug> GetListByName(string name)
        {
            return dbcontext.Db.Queryable<Drug>().WhereIF(!string.IsNullOrEmpty(name), it => it.name.Contains(name)).ToList();
        }
        public int Update(Drug drug)
        {
           return new DbContext().Db.Updateable(drug).ExecuteCommand();
        }
        public int Insert(List<Drug> drugs)
        {
            return new DbContext().Db.Insertable<Drug>(drugs).ExecuteCommand();
        }
    }
}
