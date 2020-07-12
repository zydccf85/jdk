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
namespace JKD.Service
{
    public class DataLoad
    {
        #region 从硬盘导入药品
        public int ImportDrug(string filePath)
        {
            
            // string path = ConfigurationManager.AppSettings["importExcelPath"];
            DataTable dt = ExcelHelper.ImportExceltoDt(@filePath);
            List<Drug> list = new List<Drug>();
            DrugManager dm = new DrugManager();
            List<Drug> all = dm.GetAll();
            PinyinHelper ph = new PinyinHelper();
            foreach (DataRow dataRow in dt.Rows) 
            {
                string code = dataRow.Field<string>("code");
                if (string.IsNullOrEmpty(code)) continue;
                string name = dataRow.Field<string>("name");
                System.Diagnostics.Debug.WriteLine(new PinyinHelper().GetFirstLetter(name));
                string spci = dataRow.Field<string>("spci");
                string form = dataRow.Field<string>("form");
                string address = dataRow.Field<string>("address");
                string unit = dataRow.Field<string>("unit");
                string unitprice = dataRow.Field<string>("unitprice");
                if (string.IsNullOrEmpty(unitprice)) System.Diagnostics.Debug.WriteLine(name);
                Drug drug = new Drug()
                {
                    code = code,
                    name = name,
                    spci = spci,
                    form = form,
                    address = address,
                    unit = unit,
                    unitprice = double.Parse(unitprice),
                    searchcode = ph.GetFirstLetter(name)
                };
                if (!all.Contains(drug))
                {
                    list.Add(drug);
                    System.Diagnostics.Debug.WriteLine(drug.name +":"+drug.ToString());
                }
               
                
            }
            
            int count = dm.Insert(list);
            return count;
        }
        #endregion

    }
}
