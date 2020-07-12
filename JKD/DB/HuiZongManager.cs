using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.Models;
namespace JKD.DB
{
    public class HuiZongManager
    {
        public List<HuiZong> GetListByCondition(string BeginDate,string EndDate,string Patient,string Doctor)
        {
            string BeginTime = BeginDate + " 00:00:00";
            string EndTime = EndDate + " 23:59:59";
            string doc = string.IsNullOrEmpty(Doctor) ? "%%" : Doctor + "%";
            string pat = string.IsNullOrEmpty(Patient) ? "%%" : Patient + "%";
            string sqlStr = $@"SELECT SUBSTRING(g.opertime,1,10) RiQi ,SUM(g.totalprice)  TotalPrice,max(g.totalprice) MaxPrice,min(g.totalprice) MinPrice,
                    round(avg(g.totalprice),2) AvgPrice,
                sum(g.cou) CfCount,COUNT(distinct g.pid) MzCount,SUM(g.isyibao) YbCount,sum(g.cou)-SUM(g.isyibao) ZfCount,SUM(g.zcycount) ZcyCount,
                    SUM(g.jdcount) JdCount,SUM(g.jecount) JeCount,SUM(g.ekcount) EkCount,SUM(g.jscount) JsCount
                    FROM(
                    SELECT f.opertime, totalprice, count(distinct opertime) cou,pid,SUM(f.isyibao) > 0 isyibao, SUM(f.iszifei) > 0 isyifei, SUM(f.zcycount) > 0 zcycount,
                    SUM(f.jdcount) > 0 jdcount, SUM(jecount) > 0 jecount, SUM(ekcount) > 0 ekcount, SUM(jscount) > 0 jscount
                     FROM(SELECT a.opertime, a.totalprice,a.pid,
                    case when a.feibie = '医保' then 1 END isyibao,
                    case when a.feibie = '自费' then 1 END iszifei,
                    case when b.drug IN(SELECT d.NAME FROM drug d WHERE d.cate = '中成药') then 1 END  zcycount,
                    case when b.yongfa = '静滴' then 1 END  jdcount,
                    case when b.drug LIKE '%艾司唑%'  then 1 END  jecount,
                    case when left(a.age,INSTr(a.age,'岁')-1) <= 12 then 1 END ekcount,
                    case when b.drug LIKE '%地塞米松%'   then 1 END  jscount
                    FROM cfhead a INNER JOIN cfdetail b
                    ON a.opertime = b.opertime
                    WHERE a.patient like '{pat}' and a.doctor like '{doc}' and a.enable= 1 and a.opertime >='{BeginTime}' and a.opertime <= '{EndTime}' ) f
                    GROUP BY f.opertime, f.totalprice,f.pid) g
                     GROUP BY SUBSTRING(g.opertime, 1, 10) 
                    ORDER BY SUBSTRING(g.opertime, 1, 10) DESC";
            return new DbContext().Db.SqlQueryable<HuiZong>(sqlStr).ToList();
        }
        public List<HuiZong> GetListAll()
        {
            string sqlStr = $@"SELECT SUBSTRING(g.opertime,1,10) RiQi ,SUM(g.totalprice)  TotalPrice,max(g.totalprice) MaxPrice,min(g.totalprice) MinPrice,
                    round(avg(g.totalprice),2) AvgPrice,
                sum(g.cou) CfCount,COUNT(distinct g.pid) MzCount,SUM(g.isyibao) YbCount,sum(g.cou)-SUM(g.isyibao) ZfCount,SUM(g.zcycount) ZcyCount,
                    SUM(g.jdcount) JdCount,SUM(g.jecount) JeCount,SUM(g.ekcount) EkCount,SUM(g.jscount) JsCount
                    FROM(
                    SELECT f.opertime, totalprice, count(distinct opertime) cou,pid,SUM(f.isyibao) > 0 isyibao, SUM(f.iszifei) > 0 isyifei, SUM(f.zcycount) > 0 zcycount,
                    SUM(f.jdcount) > 0 jdcount, SUM(jecount) > 0 jecount, SUM(ekcount) > 0 ekcount, SUM(jscount) > 0 jscount
                     FROM(SELECT a.opertime, a.totalprice,a.pid,
                    case when a.feibie = '医保' then 1 END isyibao,
                    case when a.feibie = '自费' then 1 END iszifei,
                    case when b.drug IN(SELECT d.NAME FROM drug d WHERE d.cate = '中成药') then 1 END  zcycount,
                    case when b.yongfa = '静滴' then 1 END  jdcount,
                    case when b.drug LIKE '%艾司唑%'  then 1 END  jecount,
                    case when left(a.age,INSTr(a.age,'岁')-1) <= 12 then 1 END ekcount,
                    case when b.drug LIKE '%地塞米松%'   then 1 END  jscount
                    FROM cfhead a INNER JOIN cfdetail b
                    ON a.opertime = b.opertime ) f
                    GROUP BY f.opertime, f.totalprice,f.pid) g
                     GROUP BY SUBSTRING(g.opertime, 1, 10) 
                    ";
            return new DbContext().Db.SqlQueryable<HuiZong>(sqlStr).ToList();
        }
    }
}
