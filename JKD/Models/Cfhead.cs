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
            { "id","编号" },{ "cftype","处方类型名称" },{ "hospital","单位名称" },{ "cardid","卡号" },{ "pid","门诊号" },{ "patient","姓名" },{ "gender","性别" },
            { "age","年龄" },{"department","科室" },{ "doctor","医生" },{ "disease","诊断" },{ "disease2","辅助信息" },{ "feibie","费别" },{ "phone","联系电话" },
            { "address","居住地址" },{ "totalprice","总金额" },{ "opertime","操作时间" },{ "did","身份证号" },{ "enable","是否作废" },
        };
           public Cfhead(){

           }
                    
           public int id {get;set;}
         
           public string cftype {get;set;}
       
           public string hospital {get;set;}
         
           public string cardid {get;set;}
          public string gender { get; set; }

          public string pid {get;set;}
          
           public string patient {get;set;}

           public string age {get;set;}
         public string department { get; set; }

        public string doctor {get;set;}
        
           public string disease {get;set;}
        
           public string disease2 {get;set;}
      
           public string feibie {get;set;}
          
           public string phone {get;set;}
         
           public string address {get;set;}

                      
           public double? totalprice {get;set;}
         
           public string opertime {get;set;}
        
           public string did {get;set;}
           public int enable { get; set; }
           public List<Cfdetail> cfDetails { get; set; }
    }
}
