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
using JKD.Models;
using System.Reflection;

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
                foreach (XmlNode c in cf.SelectNodes("处方信息"))
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

        #region 从指定文件中导入数据
        public List<Cfhead> Import02(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            Type t = new Cfhead().GetType();
            List<PropertyInfo> lili = new Cfhead().GetType().GetRuntimeProperties().ToList();
            List<PropertyInfo> lili02 = new Cfdetail().GetType().GetRuntimeProperties().ToList();
            List<Cfhead> li = new List<Cfhead>();
            foreach (XmlNode cf in doc.SelectNodes("//NewDataSet"))
            {
                XmlNode cfinfo = cf.SelectSingleNode("诊断信息");
                //Object obj = Activator.CreateInstance(t);
                Cfhead cfhead = new Cfhead() { 
                    enable=1
                };

                lili.Where(i=>i.Name !="id" && i.Name !="enable" && i.Name != "cfDetails").ToList().ForEach(item =>
                {
                    
                        string temp = cfinfo.SelectSingleNode(Cfhead.NameMap[item.Name]).InnerText;
                    // p.SetValue(model, Convert.ChangeType(data.Rows[i][j], p.PropertyType.GetGenericArguments()[0]), null);
                    //  item.SetValue(cfhead, Convert.ChangeType(temp, item.PropertyType.GetGenericArguments()[0]), null);
                    if (item.PropertyType == typeof(double?))
                    {
                        item.SetValue(cfhead, Convert.ToDouble(temp), null);
                    }
                    else
                    {
                        item.SetValue(cfhead, temp, null);
                    }
                });
                List<Cfdetail> cfdetails = new List<Cfdetail>();

                foreach (XmlNode c in cf.SelectNodes("处方信息"))
                {
                    Cfdetail cfdetail = new Cfdetail() { 
                        enable=1
                    };
                    lili02.Where(i => i.Name != "id" && i.Name != "enable" && i.Name != "opertime").ToList().ForEach(item =>
                        {
                            string temp = c.SelectSingleNode(Cfdetail.NameMap[item.Name]).InnerText;
                            if (item.PropertyType == typeof(double?))
                            {
                                item.SetValue(cfdetail, Convert.ToDouble(temp), null);
                            }
                            else if(item.PropertyType == typeof(int?))
                            {
                                item.SetValue(cfdetail, Convert.ToInt32(temp), null);
                            }
                            else if(item.PropertyType == typeof(string))
                            {
                                item.SetValue(cfdetail, temp, null);
                            }
                        });
                    cfdetail.opertime = cfhead.opertime;
                    cfdetails.Add(cfdetail);

                }
                cfhead.cfDetails = cfdetails;
                li.Add(cfhead);

            }
            return li;
        }
        #endregion

        #region 从指定文件夹中导入数据(文件夹)
        public List<Cfhead> ImportByDir(string dirname,Func<FileInfo,bool> func)
        {
            List<Cfhead> li = new List<Cfhead>();
           new DirectoryInfo(dirname).GetFiles("*.xml").Where(func).ToList().ForEach(item=> {
               li.AddRange(Import02(item.FullName));
           });
            return li;
        }
        #endregion

        #region 根据时间,自动导入
        public Dictionary<string,int> AutoImportData()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string maxTime = new CfheadManager().GetMaxTime();
            string maxDate = string.Empty;
            if(maxTime==null || string.IsNullOrEmpty(maxTime))
            {
                maxDate = "1970-01-01";
            }
            else
            {
                maxDate = maxTime.Split(' ')[0].Replace("-", "").Trim();
            }
            
            Func<FileInfo, bool> func = item => item.Name.CompareTo(maxDate) >= 0;
            string xmlPath = ConfigurationManager.AppSettings["importXmlPath"];
            if (Directory.Exists(xmlPath))
            {
                List<Cfhead> li = ImportByDir(xmlPath, func);
               
                List<Cfhead> filList = FilterUnique(li);
                if (filList.Count == 0)
                {
                    dic.Add("处方数", 0);
                    dic.Add("处方明细数", 0);
                    return dic;
                };
                int cfs =  new DbContext().Db.Insertable<Cfhead>(filList.ToArray()).IgnoreColumns("cfDetails").IgnoreColumns("id").ExecuteCommand();
                List<Cfdetail> lili = new List<Cfdetail>();
                filList.ForEach(item => lili.AddRange(item.cfDetails) );
                int mxs = new DbContext().Db.Insertable(lili.ToArray()).ExecuteCommand();
                dic.Add("处方数", cfs);
                dic.Add("处方明细数", mxs);

                return dic;
            }
            return null;
        }
        #endregion


        #region 过滤重复项
        private List<Cfhead> FilterUnique(List<Cfhead> originList)
        {
            List<string> opertimes =new CfheadManager().GetUniqueOpertime();
            Debug.WriteLine(opertimes.Count);
            List<Cfhead> li = originList.Where(item => !opertimes.Contains(item.opertime)).ToList();
            Debug.WriteLine(li.Count);
            return li;
        }

        #endregion


    }
}
