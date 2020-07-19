using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Data;
using JKD.DB;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Threading;
using DevExpress.XtraSplashScreen;
using JKD.CenterView;
using JKD.DB;
using System.IO;
using System.Configuration;
namespace JKD.Service
{
   public  class ImportData
    {
        #region 根据时间，自动导入
        public Dictionary<string, int> AutoImport()
        {
            string maxTime = new CfheadManager().GetMaxTime();
            //SplashScreenManager.Default.SendCommand(null, string.Format("最新更新时间为{0},1", maxTime));
            string maxDate = maxTime.Split(' ')[0].Replace("-", "").Trim();
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("处方数", 0);
            result.Add("处方明细数", 0);
            string xmlPath = ConfigurationManager.AppSettings["importXmlPath"];
            if (Directory.Exists(xmlPath))
            {
                Directory.GetFiles(xmlPath).ToList()
                .Where(item => item.Substring(item.IndexOf("cf.xml") - 8, 8).CompareTo(maxDate) >= 0).OrderByDescending(f => f.ToString()).ToList()
                .ForEach(item => {
                    Dictionary<string, int> res = Import(item);
                    result["处方数"] += res["处方数"];
                    result["处方明细数"] += res["处方明细数"];
                });
            }
            else
            {
                return null;
            }
            
            return result;
        }
        #endregion
        #region 从指定文件中导入数据到数据库
        public Dictionary<string,int> Import(string filename)
        {
            DataTable dt = SqlHelper.ExecuteTable("select opertime from cfhead");
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach (DataRow dr in dt.Rows)
            {
                string val = Convert.ToString(dr[0]);
                dic[val] = true;
            }
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);
            int num01 = 0;
            int num02 = 0;
            foreach (XmlNode cf in doc.SelectNodes("//NewDataSet"))
            {
                XmlNode cfinfo = cf.SelectSingleNode("诊断信息");
                string opertime = cfinfo.SelectSingleNode("操作时间").InnerText;
                bool flag = false;
                flag = dic.TryGetValue(opertime, out flag);
                if (flag) continue;
                string cftype = cfinfo.SelectSingleNode("处方类型名称").InnerText;
                string hospital = cfinfo.SelectSingleNode("单位名称").InnerText;
                string cardid = cfinfo.SelectSingleNode("卡号").InnerText;
                string pid = cfinfo.SelectSingleNode("门诊号").InnerText;
                string patient = cfinfo.SelectSingleNode("姓名").InnerText;
                string age = cfinfo.SelectSingleNode("年龄").InnerText;
                string doctor = cfinfo.SelectSingleNode("医生").InnerText;
                string disease = cfinfo.SelectSingleNode("诊断").InnerText;
                string disease2 = cfinfo.SelectSingleNode("辅助信息").InnerText;
                string feibie = cfinfo.SelectSingleNode("费别").InnerText;
                string phone = cfinfo.SelectSingleNode("联系电话").InnerText;
                string address = cfinfo.SelectSingleNode("居住地址").InnerText;
                double totalprice = Convert.ToDouble(cfinfo.SelectSingleNode("总金额").InnerText);

                string did = cfinfo.SelectSingleNode("身份证号").InnerText;
                string sql01 = $@"insert into  cfhead(cftype,hospital,cardid,pid,patient,age,doctor,disease,disease2,feibie,phone,address,totalprice,opertime,did)
                values('{cftype}','{hospital}','{cardid}','{pid}','{patient}','{age}','{doctor}','{disease}','{disease2}','{feibie}','{phone}','{address}',{totalprice},'{opertime}','{did}')";
                Debug.WriteLine(sql01);
                num01 += SqlHelper.ExecuteNoQuery(sql01);
                XmlNodeList cfdetail = cf.SelectNodes("处方信息");
                string sql02 = string.Empty;
                foreach (XmlNode c in cfdetail)
                {
                    string gid = c.SelectSingleNode("组号").InnerText;
                    string drug = c.SelectSingleNode("名称").InnerText;
                    string spci = c.SelectSingleNode("规格").InnerText;
                    string cishu = c.SelectSingleNode("每日次数").InnerText;
                    double yongliang = Convert.ToDouble(c.SelectSingleNode("每次用量").InnerText);
                    string danwei = c.SelectSingleNode("用量单位").InnerText;
                    string yongfa = c.SelectSingleNode("用法").InnerText;
                    double quantity = Convert.ToDouble(c.SelectSingleNode("数量").InnerText);
                    string unit = c.SelectSingleNode("单位").InnerText;
                    double unitprice = Convert.ToDouble(c.SelectSingleNode("单价").InnerText);
                    sql02 += $"('{gid}','{drug}','{spci}','{cishu}',{yongliang},'{danwei}','{yongfa}',{quantity},'{unit}',{unitprice},'{opertime}'),";
                }
                string sql = "insert into cfdetail(gid,drug,spci,cishu,yongliang,danwei,yongfa,quantity,unit,unitprice,opertime) values";
                Debug.WriteLine((sql + sql02).TrimEnd(new Char[] { ',' }));
                string newsql = (sql + sql02).TrimEnd(new Char[] { ',' });
                num02 += SqlHelper.ExecuteNoQuery(newsql);

            }
            //if (num01 == 0) return string.Format("从({0})中,没有数据进行更新", filename);
            //return string.Format("从({2})中,导入处方数为：{0},处方明细数为：{1}", num01, num02, filename);
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("处方数", num01);
            result.Add("处方明细数", num02);
            return result;

        }
        #endregion


    }
}
