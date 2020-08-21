using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using JKD.DB;
using JKD.Models;
using DevExpress.XtraCharts;
using DevExpress.XtraTab.Buttons;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab.ViewInfo;

namespace JKD.CenterView
{
    public partial class CftongjiControl : DevExpress.XtraEditors.XtraUserControl
    {
        public List<HuiZong> ds;
        public List<HuiZong> all;
        public CftongjiControl()
        {
            InitializeComponent();
            all = new HuiZongManager().GetListAll();
            xtraTabControl1.CustomHeaderButtonClick += CustomHeaderButtonClick;
            ds = all;
            FreshData();
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"Cfcount","处方数量" },{"MzCount","门诊人次数量" },{"JdCount","静滴数量"},{"ZcyCount","中成药数量"  },
                { "JeCount","精二处方数量"}, {"EkCount","儿科处方数量" }
            };
           
            foreach (var key in dic)
            {
                Series ser = new Series()
                {

                    // View = new LineSeriesView(),
                    View = new SplineSeriesView(),
                    DataSource = ds,
                    Name = key.Value,
                    ArgumentDataMember = "riqi",
                    
                };
                ser.ValueDataMembers[0] = key.Key;
                ser.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl1.Series.Add(ser);

            }
          
            ChartTitle chartTitle1 = new ChartTitle() {
                Text = "门诊处方数量统计（全部）",
                
            };
            chartControl1.Titles.Add(chartTitle1);
            Legend le = new Legend();
            chartControl1.Legend.Title.Text = "处方数量统计";
            chartControl1.Legend.MarkerMode = LegendMarkerMode.CheckBoxAndMarker;
            chartControl1.CustomDrawSeriesPoint += (s, e) =>
            {
               
                PointSeriesLabel psl = e.Series.Label as PointSeriesLabel;
                psl.Position = PointLabelPosition.Center;
                psl.Angle = 60;

                if (e.SeriesPoint.Values[0] > 1)
                {
                    e.LabelText = e.SeriesPoint.Values[0].ToString();
                }

            };
        }
        public List<HuiZong> allList;
        public void CustomHeaderButtonClick(Object o,CustomHeaderButtonEventArgs e)
        {
            CustomHeaderButton btn = e.Button;
            string begin = string.Empty;
            string end = string.Empty;
            if (btn.Caption == "全部")
            {
                begin = "2020-01-01";
                end = "2025-01-01";
                chartControl1.Titles[0].Text = string.Format("门诊处方数量统计（{0}）", "全部");

            }
            else if (btn.Caption == "本月")
            {
                begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                end = DateTime.Now.ToString("yyyy-MM-dd");
                chartControl1.Titles[0].Text = string.Format("门诊处方数量统计（{0}）", "本月");

            }
            else if (btn.Caption == "上月")
            {
                begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1).ToString("yyyy-MM-dd");
                end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1.0).ToString("yyyy-MM-dd");
                chartControl1.Titles[0].Text = string.Format("门诊处方数量统计（{0}）", "上月");
            }
            else if (btn.Caption == "近一月")
            {
                begin = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                end = DateTime.Now.ToString("yyyy-MM-dd");
                chartControl1.Titles[0].Text = string.Format("门诊处方数量统计（{0}）", "近一月");
            }
            else if (btn.Caption == "刷新")
            {
                begin = "2020-01-01";
                end = "2025-01-01";
                chartControl1.Titles[0].Text = string.Format("门诊处方数量统计（{0}）", "全部");
                all =  new HuiZongManager().GetListAll();
            }
            ds = all.Where(item => string.Compare(item.Riqi, begin) >= 0 && string.Compare(item.Riqi, end) <= 0).ToList();
            FreshData();
            
        }
        public void FreshData()
        {
            foreach(Series ser in chartControl1.Series)
            {
                ser.DataSource = ds;
            }
        }
    }
}
