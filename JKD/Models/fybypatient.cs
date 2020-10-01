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
    public partial class fybypatient
    {
           public fybypatient(){


           }
        public static Dictionary<string, string> TitleMapFiled = new Dictionary<string, string>()
        {
            {"patient","病人姓名" },{"gender","性别" },{"age","年龄" },{"doctor","医生" },{"disease","诊断" },{"fytime","发药时间" },
            {"fytype","类别" }
        };
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id {get;set;}
         
           public string patient {get;set;}
          
           public string gender {get;set;}
           public string age {get;set;}

           public string doctor {get;set;}

           public string disease {get;set;}

           public DateTime? fytime {get;set;}

           public string fytype {get;set;}

        [SugarColumn(IsIgnore =true)]
        public DateTime? updatetime {get;set;}

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            var fybypatient = obj as fybypatient;
            return fybypatient != null &&
                   id == fybypatient.id &&
                   patient == fybypatient.patient &&
                   gender == fybypatient.gender &&
                   age == fybypatient.age &&
                   doctor == fybypatient.doctor &&
                   disease == fybypatient.disease &&
                   fytime == fybypatient.fytime &&
                   fytype == fybypatient.fytype &&
                   EqualityComparer<DateTime?>.Default.Equals(updatetime, fybypatient.updatetime);
        }

        public override int GetHashCode()
        {
            var hashCode = -443031009;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(patient);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(gender);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(age);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(doctor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(disease);
            hashCode = hashCode * -1521134295 + fytime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(fytype);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(updatetime);
            return hashCode;
        }
        
    }
}
