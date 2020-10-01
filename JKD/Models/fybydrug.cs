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
    public partial class fybydrug
    {
           public fybydrug(){


           }
        public static Dictionary<string, string> TitleMapFiled = new Dictionary<string, string>()
        {
            {"code","药品代码" },{"name","药品名称" },{"spci","药品规格" },{"unitprice","单价" },{"quantity","数量" },{"price","总金额" },
            {"patient","姓名" },{"gender","性别" },{"pid","门诊/住院号" },{"fytime","发/退药时间" },{"fytype","类型" },
        };
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id {get;set;}
       
           public string code {get;set;}
        public string name { get; set; }
        public string spci { get; set; }
        public double? unitprice { get; set; }

        public double? price {get;set;}
        public string quantity { get; set; }
           public string patient {get;set;}
        public string gender { get; set; }
           public string pid {get;set;}
         
           public DateTime? fytime {get;set;}
      
           public string fytype {get;set;}
           [SugarColumn(IsIgnore =true)]
           public DateTime? updatetime {get;set;}

    }
}
