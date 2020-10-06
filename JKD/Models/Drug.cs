using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JKD.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Drug
    {
           public Drug(){


           }
        public static Dictionary<string, string> TitleMapFiled = new Dictionary<string, string>()
        {
            {"code","药品代码" },{"name","药品名称" },{"spci","规格" },{"form","剂型" },{"unitprice","常用价" },
            { "unit","常用单位" },
            {"cate","帐目类别" },{"cata","药品归类" }
        };
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int? id {get;set;}
          
           public string code {get;set;}

                    
           public string name {get;set;}
         
           public string spci {get;set;}

                   
           public string form {get;set;}

                   
           public string address {get;set;}

                   
           public string unit {get;set;}

                   
           public double? unitprice {get;set;}
         
          
           public string searchcode {get;set;}
         
           public string cate {get;set;}
          public string cata { get; set; }
        public override string ToString()
        {
            return string.Format("id:{0},code:{1},name:{2},form:{3},spci:{4},unit:{5},unitprice:{6},searchcode:{7},cate:{8}",
                id,code,name,form,spci,unit,unitprice,searchcode,cate);
        }
        public override bool Equals(object obj)
        {
            Drug d = obj as Drug;
            return this.code == d.code && this.unitprice == d.unitprice;
        }

    }
}
