using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Xml;
using System.Data;
using System.IO;
using System.Diagnostics;
namespace JKD.ViewModels
{
    [POCOViewModel]
    public class MainFormViewModel
    {
        public virtual string Jkr { get; set; }
        public void dothing()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"\\172.20.81.139\chufang\20200117cf.xml");
            foreach(XmlNode cf in doc.DocumentElement.ChildNodes)
            {
                foreach (XmlNode hh in cf.ChildNodes)
                {
                    if (hh.Name == "诊断信息")
                    {
                        Debug.WriteLine(hh.InnerText);


                    }
                }
            }
           
           
           
            
        }
    }
}