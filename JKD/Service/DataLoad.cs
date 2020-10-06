using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.utils;
using JKD.Models;
using JKD.DB;
using Microsoft.International.Converters.PinYinConverter;
using JKD.utils;
using System.IO;

namespace JKD.Service
{
    public class DataLoad
    {
        #region 从硬盘导入药品
        public int ImportDrug(string filePath)
        {
            DataTable dtDrug = ExcelHelper.ImportExceltoDt(filePath);
            List<Drug> allDrug = new DrugManager().GetAll();
            List<Drug> li = DataTableToModel.ToListModel<Drug>(dtDrug).Where(item=>!allDrug.Contains(item)).ToList();
            PinyinHelper ph = new PinyinHelper();
            li.ForEach(item => item.searchcode =ph.GetFirstLetter(item.name));
            int count = new DrugManager().Insert(li);

            return count;
         
        }
        private Drug Datarow2Drug(DataRow dr)
        {
            
            return null;
        }
        #endregion

        public  List<T> GetEntityFromDataTable<T>(DataTable sourceDT,Dictionary<string,string> dic) where T : class
        {
            List<T> list = new List<T>();
            // 获取需要转换的目标类型
            Type type = typeof(T);
            foreach (DataRow dRow in sourceDT.Rows)
            {
                if (string.IsNullOrEmpty(dRow[0].ToString())) break;
                // 实体化目标类型对象
                object obj = Activator.CreateInstance(type);
                
                foreach (System.Reflection.PropertyInfo prop in type.GetProperties())
                {

                    //prop.SetValue(obj, dRow[dic[prop.Name]], null);
                    //给目标类型对象的各个属性值赋值
                    if (prop.PropertyType   == typeof(double?))
                    {
                        prop.SetValue(obj, Convert.ToDouble(dRow[dic[prop.Name]]), null);
                    }
                    else if (prop.PropertyType == typeof(int?))
                    {
                        prop.SetValue(obj, Convert.ToInt32(dRow[dic[prop.Name]]), null);
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(obj, Convert.ToString(dRow[dic[prop.Name]]), null);
                    }

                }
                System.Diagnostics.Debug.WriteLine(obj as T);
                list.Add(obj as T);
            }
            return list;
        }
        public int ImportDrug02(string filepath)
        {
            DataTable dt = ExcelHelper.ImportExceltoDt(@filepath);
            dt.TableName = "mydrug";
            //dt.WriteXmlSchema(new FileStream(@"C:\Users\Administrator\Desktop\drug.xml",FileMode.OpenOrCreate));
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"code","药品代码" },{"name","药品名称"},
                {"spci","规格"},{"form","剂型" },{"address","厂商"},
                {"unit","单位"},{"unitprice","单价" },{"searchcode","搜索码"},
                { "cate","药品性质"} ,{"cata","药品分类" },{"enable","是否启用"} 

            };
           List<Drug> li =  GetEntityFromDataTable<Drug>(dt, dic);
            System.Diagnostics.Debug.WriteLine(li.Count);
            li.ForEach(item => System.Diagnostics.Debug.WriteLine(item));

            return 0;
        }
    }
}
