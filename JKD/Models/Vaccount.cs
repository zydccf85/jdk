using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace JKD.Models
{
    
    public partial class Vaccount
    {
        public static Dictionary<string, string> MapField = new Dictionary<string, string>()
        {
            {"aid","银行编号" },{"hm","收款人户名" },{"zh","收款人帐号" },{"yh","开户银行" },{"id","编号" },{"kxly","款项来源" },
            {"bz","币种" },{"lrsj","录入时间" },{"dysj","打印时间" },{"jkrq","缴款日期" },{"jkr","缴款人" },{"je","金额" }, {"isdelete","是否删除"}
        };
           public Vaccount(){


           }
           /// <summary>
           /// Desc:
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int aid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string hm {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string zh {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string yh {get;set;}

           /// <summary>
           /// Desc:
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? accid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string kxly {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string bz {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? lrsj {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? dysj {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? jkrq {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string jkr {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? je {get;set;}
         public int? isdelete { get; set; }

    }
}
