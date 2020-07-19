using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using JKD.Models;
namespace JKD.DB
{
    public class HzByDoctor
    {
        public  DataTable GetDataTable(string BeginTime,String EndTime)
        {
            List<string> li = new DbContext().Db.SqlQueryable<Cfhead>("select distinct doctor from cfhead order by doctor")
                .Select<string>(item => item.doctor).ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT SUBSTR(opertime,1,10) as '日期' ,feibie as '费别'");
            li.ForEach(item => sb.Append($",SUM(if(doctor='{item}',1,NULL)) AS '{item}'"));
            sb.Append($@"FROM cfhead where opertime >= concat('{BeginTime}', ' 00:00:01') and opertime <= concat('{ EndTime}', ' 23:59:59')
                        GROUP BY SUBSTR(opertime, 1, 10), feibie order by SUBSTR(opertime, 1, 10) desc ");

            //string sqlstr = $@"SELECT SUBSTR(opertime,1,10) as '日期' ,feibie as '费别',SUM(if(doctor='陈刚',1,NULL)) AS '陈刚',
            //            SUM(if(doctor='徐春华',1,NULL)) AS '徐春华',SUM(if(doctor='倪小备',1,NULL)) AS '倪晓备',
            //            SUM(if (doctor = '陆惠琳',1,NULL)) AS '陆惠琳',SUM(if (doctor = '胡云丹',1,NULL)) AS '胡云丹',
            //            SUM(if (doctor = '胡培红',1,NULL)) AS '胡培红',SUM(if (doctor = '陈要刚',1,NULL)) AS '陈要刚'
            //            FROM cfhead where opertime >= concat('{BeginTime}',' 00:00:01') and opertime <= concat('{ EndTime}',' 23:59:59')
            //            GROUP BY SUBSTR(opertime, 1, 10),feibie order by SUBSTR(opertime, 1, 10) desc ";
            DataTable dt = new DbContext().Db.SqlQueryable<Object>(sb.ToString()).ToDataTable();
            return dt;
        }
    }
}
