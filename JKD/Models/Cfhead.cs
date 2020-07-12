using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace JKD.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Cfhead
    {
        public static Dictionary<string, string> NameMap = new Dictionary<string, string>()
        {
            { "id","编号" },{ "cftype","处方类型" },{ "hospital","单位名称" },{ "cardid","卡号" },{ "pid","门诊号" },{ "patient","患者" },
            { "age","年龄" },{ "doctor","医生" },{ "disease","诊断" },{ "disease2","辅助诊断" },{ "feibie","费别" },{ "phone","联系电话" },
            { "address","居住地址" },{ "totalprice","金额" },{ "opertime","操作时间" },{ "did","身份证号" },{ "enable","是否作废" },
        };
           public Cfhead(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string cftype {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string hospital {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string cardid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string pid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string patient {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string age {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string doctor {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string disease {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string disease2 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string feibie {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string phone {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string address {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? totalprice {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string opertime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string did {get;set;}
           public int enable { get; set; }
    }
}
