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
        public  DataTable GetDataTable(string BeginTime,String EndTime,string doctor ,string patient)
        {
             BeginTime += " 00:00:00";
            EndTime += " 23:59:59";
            if (doctor == null) doctor = string.Empty;
            if (patient == null) patient = string.Empty;
            string mysql = $@"select date_format(g.opertime, '%Y-%m-%d') AS '日期',department AS '科室',doctor AS '医生',
                            count(distinct pid) AS '人次数',
                            count(feibie) as '处方数',
                            convert(sum(totalprice),decimal(12,2)) as '金额',
                            convert(max(totalprice),decimal(12,2)) as '最大金额',
                            convert(min(totalprice),decimal(12,2)) as '最小金额',
                            convert(avg(totalprice),decimal(12,2)) as '平均金额',
                            count(if (feibie = '医保',1,null)) as '医保',
                            count(distinct if (feibie = '医保', pid,null)) as '医保人次数',
                            count(if (feibie = '自费',1,null)) as '自费',
                             count(distinct if (feibie = '自费', pid,null)) as '自费人次数',
                            count(if (cftype = '普通',1,null)) as '普通',
                            count(distinct if (cftype = '普通', pid,null)) as '普通人次数',
                            count(if (cftype = '儿科',1,null)) as '儿科',
                            count(distinct if (cftype = '儿科', pid,null)) as '儿科人次数',
                            count(if (cftype = '精二',1,null)) as '精二',
                            count(distinct if (cftype = '精二', pid,null)) as '精二人次数',
                            count(if (cftype = '麻醉',1,null)) as '麻醉',
                            count(distinct if (cftype = '麻醉', pid,null)) as '麻醉人次数',
                            count(if (cftype = '急诊',1,null)) as '急诊',
                            count(distinct if (cftype = '急诊', pid,null)) as '急诊人次数',
                            count(if (zcy > 0,1,null)) AS '中成药',
                            count(distinct if (zcy > 0,pid,null)) as '中成药人次数',
                            count(if (jingdi > 0,1,null)) AS '静滴' ,
                            count(distinct if (jingdi > 0,pid,null)) as '静滴人次数',
                            count(if (js > 0,1,null)) AS '激素' ,
                            count(distinct if (js > 0,pid,null)) as '激素人次数',
                            count(if (kjs > 0,1,null)) AS '抗菌素',
                            count(distinct if (kjs > 0,pid,null)) as '抗菌素人次数'
                            from
                             (
                                select f.opertime,totalprice, department, f.pid, f.cftype, feibie, doctor, zcy, jingdi, js, kjs from 
                                    (select * from cfhead where opertime >='{BeginTime}' and opertime <= '{EndTime}' and doctor like concat('%','{doctor}','%' ) and enable=1 
                            and  patient like concat('%','{patient}','%')  and enable=1 )
                                f left  join
                            (

                                select a.opertime, count(if (a.yongfa = '静滴',1,null)) jingdi ,count(if (b.cate = '中成药',1,null)) zcy, count(if (b.cata = '激素',1,null)) js,
	                            count(if (b.cata = '抗菌素',1,null)) kjs
                                   from cfdetail a left join drug b on a.drug = b.name and a.unitprice = b.unitprice

                                group by a.opertime
	                            )  e on f.opertime = e.opertime
                            ) g
                            group by date_format(g.opertime, '%Y-%m-%d'),g.department,g.doctor order by date_format(g.opertime, '%Y-%m-%d') desc ";
            DataTable dt = new DbContext().Db.SqlQueryable<Object>(mysql.ToString()).ToDataTable();
            return dt;
        }

        public DataTable GetDataTable02(string BeginTime, String EndTime, string doctor, string patient,string drugtype)
        {
            BeginTime += " 00:00:00";
            EndTime += " 23:59:59";
            if (doctor == null) doctor = string.Empty;
            if (patient == null) patient = string.Empty;
            string sqlstr = $"select b.drug,b.spci,b.unitprice,b.unit ,a.opertime ,a.patient,b.quantity" +
                           $" from cfhead a  left join cfdetail b on a.opertime = b.opertime"+
                           $" where a.enable = 1 and  a.opertime >='{BeginTime}' and a.opertime <= '{EndTime}' and a.doctor like concat('%','{doctor}','%') and  a.patient like concat('%','{patient}','%' )"+
                           $"and b.drug in( select name from drug where cata = '{drugtype}' )";
            DataTable dt = new DbContext().Db.SqlQueryable<Object>(sqlstr).ToDataTable();
            System.Diagnostics.Debug.WriteLine(sqlstr);
            return dt;

        }
    }
}
