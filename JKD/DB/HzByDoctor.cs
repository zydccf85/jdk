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
            string mysql = $@"select substr(g.opertime,1,10) AS '日期',department AS '科室',doctor AS '医生',
                            count(distinct pid) AS '人次数',
                            count(feibie) as '处方数',
                            convert(sum(totalprice),decimal(12,2)) as '金额',
                            convert(max(totalprice),decimal(12,2)) as '最大金额',
                            convert(min(totalprice),decimal(12,2)) as '最小金额',
                            convert(avg(totalprice),decimal(12,2)) as '平均金额',
                            count(if (feibie = '医保',1,null)) as '医保',
                            count(if (feibie = '自费',1,null)) as '自费',
                            count(if (cftype = '普通',1,null)) as '普通',
                            count(if (cftype = '儿科',1,null)) as '儿科',
                            count(if (cftype = '精二',1,null)) as '精二',
                            count(if (cftype = '麻醉',1,null)) as '麻醉',
                            count(if (cftype = '急诊',1,null)) as '急诊',
                            count(if (zcy > 0,1,null)) AS '中成药',
                            count(if (jingdi > 0,1,null)) AS '静滴' ,
                            count(if (js > 0,1,null)) AS '激素' ,
                            count(if (kjs > 0,1,null)) AS '抗菌素'
                            from
                             (
                                select f.opertime,totalprice, department, f.pid, f.cftype, feibie, doctor, zcy, jingdi, js, kjs from 
                                    (select * from cfhead where opertime >='{BeginTime}' and opertime <= '{EndTime}' and doctor like concat('%','{doctor}','%')
                            and  patient like concat('%','{patient}','%')  and enable=1 )
                                f left  join
                            (

                                select a.opertime, count(if (a.yongfa = '静滴',1,null)) jingdi ,count(if (b.cate = '中成药',1,null)) zcy, count(if (b.cata = '激素',1,null)) js,
	                            count(if (b.cata = '抗菌素',1,null)) kjs
                                   from cfdetail a left join drug b on a.drug = b.name and a.unitprice = b.unitprice

                                group by a.opertime
	                            )  e on f.opertime = e.opertime
                            ) g
                            group by substr(g.opertime,1,10),g.department,g.doctor order by substr(g.opertime,1,10) desc ";
            DataTable dt = new DbContext().Db.SqlQueryable<Object>(mysql.ToString()).ToDataTable();
            return dt;
        }
    }
}
