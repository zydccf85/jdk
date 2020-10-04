using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SqlSugar;

namespace JKD.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Cfdetail
    {
        public static Dictionary<string, string> NameMap = new Dictionary<string, string>()
        {
            { "id","编号" },{ "gid","组号" },{ "drug","名称" },{ "spci","规格" },{ "cishu","每日次数" },{ "yongliang","每次用量" },
            { "danwei","用量单位" },{ "yongfa","用法" },{ "quantity","数量" },{ "unit","单位" },{ "unitprice","单价" },{ "opertime","操作时间" },
            { "enable","是否作废" }
        };
        public Cfdetail(){


           }
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id {get;set;}

           public int? gid {get;set;}
         
           public string drug {get;set;}
        
           public string spci {get;set;}
          
           public string cishu {get;set;}
         
           public double? yongliang {get;set;}

                 
           public string danwei {get;set;}

           public string yongfa {get;set;}
           public double? quantity {get;set;}

           public string unit {get;set;}

           public double? unitprice {get;set;}

           public string opertime {get;set;}
          public int enable { get; set; }
    }
}
