using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using JKD.Models;
using JKD.Service;
using SqlSugar;
using System.Data;
using System.Xml;
using System.Diagnostics;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using JKD.Reports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using JKD.DB;
using DevExpress.Data.Helpers;

namespace JKD.ViewModels
{
    [POCOViewModel]
    public class FyFormViewModel
    {
        public virtual string BeginTime { get; set; }
        public virtual string EndTime { get; set; }
        public virtual string Doctor { get; set; } = string.Empty;
        public virtual string Patient { get; set; } = string.Empty;
        public virtual List<Cfhead> cfhead { get; set; } = new List<Cfhead>();
        public void OncfheadChanged()
        {
            if (cfhead.Count >0)
            {
                SelectRow = cfhead.ToArray()[0] as Cfhead;
            }
        }
        public virtual List<Cfdetail> cfdetail { get; set; }
        public virtual List<Chufang> listchufang { get; set; }
      
        public virtual string Cfcount { get; set; }
        public virtual string Mzcount { get; set; }
        public virtual string Totalprice { get; set; }
        public virtual string Zcycount { get; set; }
        public virtual string Jdcount { get; set; }
        public virtual string Jeycount { get; set; }
        public virtual string Ekcount { get; set; }
        public virtual string Jscount { get; set; }
        public virtual string SelectDate { get; set; } = "当天";
        public virtual string YibaoAndzifeicount { get; set; }
        public virtual string Huizong { get; set; }
        public virtual Cfhead SelectRow { get; set; }
        public void OnSelectRowChanged()
        {
            if(SelectRow == null)
            {
                IsEnable = false;
                CfDetailList = null;
            }
            else
            {
                IsEnable = true;
                CfDetailList = new CfdeatilManager().GetListByOpertime(SelectRow.opertime);
            }
            
        }
        public virtual bool IsEnable { get; set; }
        public virtual List<HuiZong> Dv { get; set; }
        public virtual DataTable HzByDoctor { get; set; }
        public virtual HuiZong HZ { get; set; }
        public virtual Dictionary<string, decimal> HZdata { get; set; }
        public virtual Boolean IsAll { get; set; }
        public virtual List<Cfdetail> CfDetailList { get; set; }
        
        public void OnSelectDateChanged(string mm)
        {
            //当日 昨天 前天 当月 上月 当年
            switch (SelectDate)
            {
                case "当日": BeginTime = EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "昨天":
                    EndTime = BeginTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    // EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "前天":
                    EndTime = BeginTime = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                    // EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "当月":
                    BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "上月":
                    BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("yyyy-MM-dd");
                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                case "当年":
                    BeginTime = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                default:
                    break;
            }
        }

        #region 删除选中行
        public void DeleteRow()
        {
            if (SelectRow == null) return;
            string id = SelectRow.opertime;
            DialogResult dr = XtraMessageBox.Show(text: string.Format("是否删除发药日期为{0}的记录？", id), caption: "警告", buttons: MessageBoxButtons.OKCancel, icon: MessageBoxIcon.Warning);
            if (dr == DialogResult.OK)
            {
                SelectRow.enable = 0;
                bool flag = new DbContext().cfheadDb.Update(SelectRow);
                List<Cfdetail> list = new DbContext().cfdetailDb.GetList(it => it.opertime == id);
                bool flag02 = new DbContext().cfdetailDb.UpdateRange(list);
                if (flag && flag02)
                {
                    XtraMessageBox.Show(caption: "信息提醒", text: "删除成功！", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    Query();
                }
            }
        }
       
        #endregion

        public FyFormViewModel()
        {
            // this.BeginTime= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            BeginTime = DateTime.Now.ToString("yyyy-MM-dd");
            Query();
        }

        #region 单击查询按钮
        public void Query()
        {
            //SplashScreenManager.ShowForm(typeof(WaitForm1));
            SelectRow = null;
            cfhead = new CfheadManager().GetListByCondition(BeginTime, EndTime, Doctor, Patient);
            if (this.IsAll)
            {
                List<string> pids = new CfheadManager().GetPidNotUnique();
                cfhead = cfhead.Where(item => pids.Contains(item.pid)).ToList();
            }

            Dv = new HuiZongManager().GetListByCondition(this.BeginTime, this.EndTime, this.Patient, this.Doctor);
            HzByDoctor = new HzByDoctor().GetDataTable(BeginTime, EndTime);
            
            if (HzByDoctor.Rows.Count == 0) return;
            HZ = new HuiZong()
            {
                Riqi = cfhead.Select(it => it.opertime).Min() + "至" + cfhead.Select(it => it.opertime).Max(),
                MaxPrice = Convert.ToDouble(HzByDoctor.Compute("MAX(最大金额)", "")),
                MinPrice = Convert.ToDouble(HzByDoctor.Compute("MIN(最小金额)", "")),


            };
            //HZ = new HuiZong()
            //{
            //    Riqi = cfhead.Select(it => it.opertime).Min() + "至" + cfhead.Select(it => it.opertime).Max(),
            //    TotalPrice = Dv.Sum(i => i.TotalPrice),
            //    MaxPrice = Dv.Max(i => i.MaxPrice),
            //    MinPrice = Dv.Min(i => i.MinPrice),
            //    AvgPrice = Dv.Average(i => i.AvgPrice),
            //    CfCount = Dv.Sum(i => i.CfCount),
            //    MzCount = Dv.Sum(i => i.MzCount),
            //    YbCount = Dv.Sum(i => i.YbCount),
            //    ZfCount = Dv.Sum(i => i.ZfCount),
            //    ZcyCount = Dv.Sum(i => i.ZcyCount),
            //    JdCount = Dv.Sum(i => i.JdCount),
            //    JeCount = Dv.Sum(i => i.JeCount),
            //    EkCount = Dv.Sum(i => i.EkCount),
            //    JsCount = Dv.Sum(i => i.JsCount)

            //};
            Huizong = HZ.ToString();
            //cfhead.ForEach(item => item.cfDetails = new CfdeatilManager().GetListByOpertime(item.opertime));
            //var temp = cfhead.GroupBy(i => new { opertime = i.opertime.Substring(0, 10), department = i.department, doctor = i.doctor })
            //    .Select(x => new { opertime = x.Key.opertime, doctor = x.Key.doctor, department = x.Key.department,
            //        renci = x.GroupBy(z => z.pid).Count(),
            //       total = x.Count(),
            //        yibao = x.Count(z => z.feibie == "医保"), zifei = x.Count(z => z.feibie == "自费"),
            //        butong = x.Count(z => z.cftype == "普通"),
            //        erke = x.Count(z => z.cftype == "儿科"),
            //        jinger = x.Count(z => z.cftype == "精二"),
            //        mazui = x.Count(z => z.cftype == "麻醉"),
            //        jizhen = x.Count(z => z.cftype == "急诊"),
            //        jingdi = x.Count(z => z.cfDetails.Any(u => u.yongfa == "静滴") == true)
            //    }).OrderByDescending(item=>item.opertime).ThenByDescending(item=>item.department);
            //HzByDoctor = new DataTable();
            //HzByDoctor.Columns.Add("发药日期", typeof(String));
            //HzByDoctor.Columns.Add("科室", typeof(String));
            //HzByDoctor.Columns.Add("医生", typeof(String));
            //HzByDoctor.Columns.Add("人次数", typeof(int));
            //HzByDoctor.Columns.Add("处方数", typeof(int));
            //HzByDoctor.Columns.Add("自费", typeof(int));
            //HzByDoctor.Columns.Add("医保", typeof(int));
            //HzByDoctor.Columns.Add("普通", typeof(int));
            //HzByDoctor.Columns.Add("儿科", typeof(int));
            //HzByDoctor.Columns.Add("精二", typeof(int));
            //HzByDoctor.Columns.Add("麻醉", typeof(int));
            //HzByDoctor.Columns.Add("急诊", typeof(int));
            //HzByDoctor.Columns.Add("静滴", typeof(int));
            //foreach (var l in temp)
            //{
            //     DataRow dr = HzByDoctor.NewRow();
            //    dr.SetField("发药日期",l.opertime);
            //    dr.SetField("科室", l.department);
            //    dr.SetField("医生", l.doctor);
            //    dr.SetField("人次数", l.renci);
            //    dr.SetField("处方数", l.total);
            //    dr.SetField("自费", l.zifei);
            //    dr.SetField("医保", l.yibao);
            //    dr.SetField("普通", l.butong);
            //    dr.SetField("儿科", l.erke);
            //    dr.SetField("精二", l.jinger);
            //    dr.SetField("麻醉", l.mazui);
            //    dr.SetField("急诊", l.jizhen);
            //    dr.SetField("静滴", l.jingdi);
            //    HzByDoctor.Rows.Add(dr);


            //}

        }
        #endregion

        #region 查询明细
        public List<Cfdetail> GetChild(string opertime)
        {
            //string sql=$" SELECT gid 组号, drug 药品名称,spci 规格, unitprice 单价,CONCAT('Sig：', yongliang, danwei, '  ', yongfa, '  ', cishu) 用法,Concat(round(quantity), unit) 数量 FROM cfdetail where opertime = '{opertime}'";
            //return new DbContext().Db.SqlQueryable<Cfdetail>(sql).ToDataTable();
            return new DbContext().Db.Queryable<Cfdetail>().Where(it => it.opertime == opertime)
                .Mapper(it => it.yongfa = string.Format(" {0}{1}    {2}    {3}", it.yongliang, it.danwei, it.cishu, it.yongfa)).ToList();

        }
        #endregion

        #region 单击导入按钮
        public void Import()
        {
            Dictionary<string, int> result= new ImportData().AutoImportData();
            MessageBox.Show(string.Format("处方数:{0}，处方明细数:{1}", result["处方数"], result["处方明细数"]), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //OpenFileDialog ofd = new OpenFileDialog()
            //{
            //    Filter = "XML文件|*.xml",
            //    InitialDirectory = @"\\172.20.81.139\chufang"
            //};
            //DialogResult dr = ofd.ShowDialog();
            //if (dr == DialogResult.OK)
            //{
            //    //string info = ImportData(ofd.FileName);
            //    //MessageBox.Show(info, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    Dictionary<string, int> result = new ImportData().Import(ofd.FileName);
            //    MessageBox.Show(string.Format("处方数：{0}，处方明细数{1}", result["处方数"], result["处方明细数"]), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
        #endregion

        #region 打印处方封面
        public void Print()
        {
            SplashScreenManager.ShowForm(typeof(WaitForm1));
            FMRpt0 rep = new FMRpt0(HZ);
            rep.ExportToPdf(@"C:\Users\Public\处方封面\" + BeginTime + "至" + EndTime + ".pdf");
            //rep.ShowPreview();
            SplashScreenManager.CloseForm();
            rep.ShowPreviewDialog();
        }
        #endregion

        #region 同步数据
        public void Refresh()
        {

             Dictionary<string,int> result = new ImportData().AutoImportData();
            MessageBox.Show(string.Format("处方数:{0}，处方明细数:{1}", result["处方数"], result["处方明细数"]), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region 导出至Excel表
        public void Export(GridControl gc)
        {
           // SplashScreenManager.ShowForm(typeof(WaitForm1));
            XlsxExportOptionsEx op = new XlsxExportOptionsEx()
            {
                ShowGridLines = false,
                SheetName = gc.Tag as string,
                ShowPageTitle = DevExpress.Utils.DefaultBoolean.True,
                ShowTotalSummaries = DevExpress.Utils.DefaultBoolean.True,

            };
            op.BandedLayoutMode = DevExpress.Export.BandedLayoutMode.LinearBandsAndColumns;
            string ml = string.Format(@"C:\Users\Lenovo\Desktop\{2}({0}至{1}).xlsx",BeginTime,EndTime,gc.Tag as string);
            gc.ExportToXlsx(ml, op);
           // SplashScreenManager.CloseForm();
            MessageBox.Show("成功导出至桌面", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        #endregion

        #region 双击行，弹出历史处方框
        public void ShowHistory()
        {
            Cfhead dr = this.SelectRow;
            XtraDialogForm di = new XtraDialogForm();
            di.CancelButton = new SimpleButton();
            di.AutoSize = true;
            di.StartPosition = FormStartPosition.CenterParent;
            di.ShowInTaskbar = false;
            di.Text = "患者处方历史记录";
            di.MaximizeBox = di.MinimizeBox = false;
            di.FormBorderStyle = FormBorderStyle.Fixed3D; ;
            TableControl tc = new TableControl(dr);
            di.Controls.Add(tc);
            di.ShowDialog();
        }
        public bool CanShowHistory()
        {
            return SelectRow != null;
        }
        #endregion

        #region 表焦点改变时，所选行数据进行变化
        public void ChangeRow(GridView gv)
        {
            int rownum = gv.FocusedRowHandle;
            if (rownum < 0) {
                SelectRow = null;
            }
            else
            {
                SelectRow = gv.GetRow(rownum) as Cfhead;
            }
            

        }

        #endregion

    }
}
