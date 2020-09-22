using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Diagnostics;
using JKD.DB;
using JKD.Models;
using JKD.ViewModels;
using DevExpress.XtraGrid.Columns;
using JKD.Reports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;
using JKD.CenterView;
namespace JKD
{
    public partial class FyForm : DevExpress.XtraEditors.XtraForm
    {
        public FyFormViewModel FFVM;
        public MyLayoutControl LC;
        public Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            {"gid","组号" }, { "drug","药品名称"}, { "spci","规格"}, { "unitprice","单价"}, { "quantity","数量"}, { "yongliang","用量"}
            , { "danwei","剂量单位"}, { "unit","单位"}, { "yongfa","用法"}, { "cishu","次数"}, { "opertime","操作时间"}
        };
        public FyForm()
        {
            InitializeComponent();
            InitFilterPanel();
             FFVM = mvvmContext.GetViewModel<FyFormViewModel>();
            mvvmContext.SetBinding(this.dateBegin, e => e.Text, "BeginTime");
            mvvmContext.SetBinding(this.dateEnd, e => e.Text, "EndTime");
            mvvmContext.SetBinding(this.txtDoctor, e => e.EditValue, "Doctor");
            mvvmContext.SetBinding(this.txtPatient, e => e.EditValue, "Patient");
            mvvmContext.SetBinding(this.gridControl1, e => e.DataSource, "cfhead");
            mvvmContext.SetBinding(this.gridControl2, e => e.DataSource, "Dv");
            mvvmContext.BindCommand<FyFormViewModel>(this.btnQuery, x => x.Query());
            mvvmContext.BindCommand<FyFormViewModel>(this.btnImport, x => x.Import());
            this.btnExportExcel.Click += (x, y) =>
            {
                XlsxExportOptionsEx op = new XlsxExportOptionsEx() {
                    ShowGridLines = false,
                    SheetName="门诊处方汇总表",
                    ShowPageTitle=DevExpress.Utils.DefaultBoolean.True,
                    ShowTotalSummaries= DevExpress.Utils.DefaultBoolean.True,

                };
                op.BandedLayoutMode = DevExpress.Export.BandedLayoutMode.LinearBandsAndColumns;
              
                this.gridControl2.ExportToXlsx(@"C:\Users\Lenovo\Desktop\处方明细汇总表.xlsx",op);
            };
            mvvmContext.SetBinding(this.comDoctor, e => e.Text, "SelectDate");
            mvvmContext.SetBinding(this.lblTotlprice, e => e.Text, "Huizong");
            this.gridView1.FocusedRowChanged += (x, y) => {
                Cfhead obj;
                if (y.FocusedRowHandle < 0) obj = null;
                   obj  = this.gridView1.GetRow(y.FocusedRowHandle) as Cfhead;
                mvvmContext.GetViewModel<FyFormViewModel>().SelectRow = obj;
               // MessageBox.Show(obj.opertime);
            };
            this.gridView1.RowClick += (s, e) =>
            {
                if(e.Clicks == 2)
                {
                    GridView gv = s as GridView;
                    Cfhead dr = gv.GetRow(e.RowHandle) as Cfhead;
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
            };
           
            mvvmContext.BindCommand<FyFormViewModel>(this.btnDelete, x => x.DeleteRow());
            this.gridView3.RowClick += (x, y) =>
            {
                MessageBox.Show("hehe");
            };
            Init();
            this.toggleSwitch1_Toggled(this.toggleSwitch1, new EventArgs());
        }

        public void InitFilterPanel()
        {
            
            DateEdit deBegin = new DateEdit();
            DateEdit deEnd = new DateEdit();
            Dictionary<string, Control> dic = new Dictionary<string, Control>();
            ComboBoxEdit cbeDate = new ComboBoxEdit();
            ToggleSwitch ts = new ToggleSwitch();
            ComboBoxEdit cbeDoctor = new ComboBoxEdit();
            TextEdit tePatient = new TextEdit();
            dic["日期，起"] = deBegin;
            dic["讫"] = deEnd;
            dic["日期快捷选择"]= cbeDate;
            dic["切换"]= ts;
            dic["医生"] = cbeDoctor;
            dic["患者"] = tePatient;
            LC = new MyLayoutControl(dic);
            LC.Items.ToList().ForEach(item => item.AppearanceItemCaption.TextOptions.HAlignment=DevExpress.Utils.HorzAlignment.Far);
            this.panel3.Controls.Add(LC);
        }
        private void Init()
        {
            GridFormatRule gridFormatRule = new GridFormatRule();
            FormatConditionRuleExpression formatConditionRuleExpression = new FormatConditionRuleExpression();
            gridFormatRule.Column = this.gridView1.Columns["cftype"];
           // gridFormatRule.ApplyToRow = true;
            formatConditionRuleExpression.Appearance.BackColor = Color.LightGreen;
            formatConditionRuleExpression.Expression = "[cftype]='儿科'";
            gridFormatRule.Rule = formatConditionRuleExpression;
            gridView1.FormatRules.Add(gridFormatRule);
            this.gridView1.CustomDrawCell += (a, b) =>
            {
                if(b.Column.FieldName == "cftype")
                {
                    string val = b.CellValue.ToString();
                    if (val == "精二")
                    {
                        b.Appearance.BackColor = Color.DarkRed;
                        b.Appearance.ForeColor = Color.White;
                        b.Appearance.Font = new Font("宋体", 12, FontStyle.Bold);
                        
                    }
                    else if (val == "儿科")
                    {
                        b.Appearance.BackColor = Color.Green;
                        b.Appearance.ForeColor = Color.White;
                        b.Appearance.Font = new Font("宋体", 12, FontStyle.Bold);
                    }
                }
               
            };


            this.gridView3.FormatRules.AddDataBar(this.gridView3.Columns["TotalPrice"]);
            
            this.gridView1.MasterRowGetRelationCount += (x, y) =>
            {
                y.RelationCount = 1;
            };
            this.gridView1.MasterRowGetChildList += (sender, e) =>
            {
                GridView gv = sender as GridView;
                gv.DetailTabHeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Top;
             
                string opertime = (gv.GetRow(e.RowHandle) as Cfhead).opertime;
                //string sqlstr = $"SELECT gid 组号,drug 药品名称,spci 规格,unitprice 单价,CONCAT('Sig：',yongliang,danwei,'  ',yongfa,'  ',cishu) 用法,Concat(round(quantity),unit) 数量  FROM cfdetail where opertime = '{opertime}'";
                //e.ChildList = SqlHelper.ExecuteTable(sqlstr).DefaultView;
               // DataTable dt = mvvmContext.GetViewModel<FyFormViewModel>().GetChild(opertime);
                e.ChildList = mvvmContext.GetViewModel<FyFormViewModel>().GetChild(opertime);
            };
            this.gridView1.MasterRowGetRelationName += (x, y) =>
            {
                y.RelationName = "处方明细";
            };
            this.gridView1.MasterRowGetLevelDefaultView += (x, y) =>
            {
                y.DefaultView = this.gridView2;
            };
            this.gridView3.IndicatorWidth = 30;
            this.gridView1.CustomDrawRowIndicator += (sender, e) =>
            {
                GridView view = (GridView)sender;
                //Check whether the indicator cell belongs to a data row
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    e.Info.Appearance.BackColor = Color.Azure;
                    e.Info.Appearance.ForeColor = Color.Silver;
                    e.Info.Appearance.Font = new Font("楷体", 12, FontStyle.Regular);

                }
            };
            this.gridView3.CustomDrawRowIndicator += (sender, e) =>
            {
                GridView view = (GridView)sender;
                //Check whether the indicator cell belongs to a data row
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    e.Info.Appearance.BackColor = Color.Silver;
                    e.Info.Appearance.ForeColor = Color.Silver;
                    e.Info.Appearance.Font = new Font("楷体",12, FontStyle.Regular);

                }
            };
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
                e.Info.Appearance.ForeColor = Color.Blue;

            }

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void lblTotlprice_Click(object sender, EventArgs e)
        {

        }

        private void lblCfcount_Click(object sender, EventArgs e)
        {

        }

        private void lblMzcount_Click(object sender, EventArgs e)
        {

        }

        private void lblJdcount_Click(object sender, EventArgs e)
        {

        }
        //打印处方封面
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            FyFormViewModel ffvm = mvvmContext.GetViewModel<FyFormViewModel>();
          //  FMRpt0 rep= new FMRpt0(ffvm.HZ );
            //rep.ExportToPdf(@"C:\Users\Public\处方封面\"+ffvm.BeginTime+"至"+ffvm.EndTime+".pdf");
            ////rep.ShowPreview();
            //rep.ShowPreviewDialog();
            
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch ts = sender as ToggleSwitch;
            if (ts.IsOn)
            {
                dateBegin.Enabled = dateEnd.Enabled = labelControl1.Enabled = labelControl2.Enabled = true;
                comDoctor.Enabled = labelControl5.Enabled = false;
                FyFormViewModel ffvm = mvvmContext.GetViewModel<FyFormViewModel>();
                ffvm.SelectDate = string.Empty;
            }
            else
            {
                dateBegin.Enabled = dateEnd.Enabled = labelControl1.Enabled = labelControl2.Enabled = false;
                comDoctor.Enabled = labelControl5.Enabled = true;
            }
        }
        //private void LoadData()
        //{
        //    string sql = @"SELECT opertime 发药时间,cftype 处方类型,cardid 卡号,
        //pid 门诊号,patient 患者,age 年龄,doctor 医生,concat(disease,' ',disease2) 诊断,
        //feibie 费别,totalprice 金额 FROM cfhead
        //where opertime >= @begintime and opertime <= @endtime and doctor like @doctor and patient like @patient order by opertime desc";
        //    MySqlParameter[] pars = {
        //           new MySqlParameter("@begintime",string.IsNullOrEmpty(this.dateBegin.Text)?string.Empty:(this.dateBegin.Text+" 00:00:00")),
        //           new MySqlParameter("@endtime",string.IsNullOrEmpty(this.dateEnd.Text)?string.Empty:(this.dateEnd.Text+" 23:59:59")),
        //           new MySqlParameter("@doctor","%"+this.txtDoctor.EditValue??string.Empty+"%"),
        //           new MySqlParameter("@patient","%"+this.txtPatient.EditValue??string.Empty+"%")
        //    };
        //    System.Diagnostics.Debug.WriteLine(sql);
        //    System.Diagnostics.Debug.WriteLine(this.dateBegin.Text ?? string.Empty);
        //    System.Diagnostics.Debug.WriteLine(this.dateEnd.Text ?? string.Empty);
        //    this.gridControl1.DataSource = SqlHelper.ExecuteTable(sql, pars).DefaultView;
        //    //从指定日期范围内，查询中成药处方数量
        //    string sql02 = @"SELECT count(DISTINCT opertime) FROM 
        //        (SELECT a.opertime,b.cate FROM cfdetail a,drug b WHERE a.drug=b.name AND a.unitprice=b.unitprice and b.cate='中成药' 
        //        and opertime >= @begintime and opertime <= @endtime ) c";
        //    string sql03 = @"SELECT COUNT(*) FROM cfhead where opertime >= @begintime and opertime <= @endtime";
        //    string sql04 = @"SELECT count(DISTINCT opertime) FROM cfdetail where yongfa LIKE '%静滴%' 
        //             and opertime >= @begintime and opertime <= @endtime   ";
        //    string sql05 = @"SELECT count(DISTINCT opertime) FROM cfdetail where drug LIKE '%地塞米松%'
        //             and opertime >= @begintime and opertime <= @endtime   ";
        //    string sql06 = @"SELECT count(DISTINCT opertime) FROM cfdetail where drug LIKE '%艾司唑%'
        //             and opertime >= @begintime and opertime <= @endtime   ";
        //    int zcyCount = Convert.ToInt32(SqlHelper.ExecuteScaler(sql02, pars));
        //    int total = Convert.ToInt32(SqlHelper.ExecuteScaler(sql03, pars));
        //    int jdCount = Convert.ToInt32(SqlHelper.ExecuteScaler(sql04, pars));
        //    int jsCount = Convert.ToInt32(SqlHelper.ExecuteScaler(sql05, pars));
        //    int jeCount = Convert.ToInt32(SqlHelper.ExecuteScaler(sql06, pars));
        //    System.Diagnostics.Debug.WriteLine(zcyCount + ":" + total);
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("日期范围", typeof(string));
        //    dt.Columns.Add("处方数", typeof(int));
        //    dt.Columns.Add("中成药处方数", typeof(int));
        //    dt.Columns.Add("中成药处方占比", typeof(double));
        //    dt.Columns.Add("静滴处方数", typeof(int));
        //    dt.Columns.Add("激素处方数", typeof(int));
        //    dt.Columns.Add("精二处方数", typeof(int));
        //    DataRow dr = dt.NewRow();
        //    dr[0] = this.dateBegin.Text + "至" + this.dateEnd.Text;
        //    dr[1] = total;
        //    dr[2] = zcyCount;
        //    dr[3] = (double)zcyCount / total;
        //    dr[4] = jdCount;
        //    dr[5] = jsCount;
        //    dr[6] = jeCount;
        //    dt.Rows.Add(dr);
        //    var ds = new DbContext().Db.Queryable<Cfdetail>();
        //    this.gridControl2.DataSource = dt.DefaultView;
        //    List<string> li = new List<string>();
        //    li.Add("静滴处方数："+ds.Clone().Where(it => it.yongfa.Contains("静滴")).Select(x => x.opertime).Distinct().ToList().Count().ToString());
        //    li.Add("精二处方数："+ds.Clone().Where(it => it.drug.Contains("艾司唑仑")).Select(x => x.opertime).Distinct().ToList().Count().ToString());
        //    li.Add("激素处方数："+ds.Clone().Where(it => it.drug.Contains("地塞米松")).Select(x => x.opertime).Distinct().ToList().Count().ToString());
        //    this.gridControl3.DataSource = li;
        //    Debug.Write(ds.Clone().Where(it => it.yongfa.Contains("静滴")).Select(x => x.opertime).Distinct().Count().ToString()); ;
        //}

        //private void btnQuery_Click(object sender, EventArgs e)
        //{
        //  //  MessageBox.Show()
        //    LoadData();
        //}

        //private void btnImport_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog ofd = new OpenFileDialog() {
        //        Filter = "XML文件|*.xml",
        //        InitialDirectory = @"\\172.20.81.139\chufang"
        //    };
        //    DialogResult dr = ofd.ShowDialog();
        //    if (dr == DialogResult.OK)
        //    {
        //        string info =ImportData(ofd.FileName);
        //        MessageBox.Show(info,"信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //    }
        //}
        //private string ImportData(string filename)
        //{
        //    DataTable dt = SqlHelper.ExecuteTable("select opertime from cfhead");
        //    Dictionary<string, bool> dic = new Dictionary<string, bool>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string val = Convert.ToString(dr[0]);
        //        dic[val] = true;
        //    }
        //    XmlDocument doc = new XmlDocument();

        //    doc.Load(filename);
        //    int num01 = 0;
        //    int num02 = 0;
        //    foreach (XmlNode cf in doc.SelectNodes("//NewDataSet"))
        //    {
        //        XmlNode cfinfo = cf.SelectSingleNode("诊断信息");
        //        string opertime = cfinfo.SelectSingleNode("操作时间").InnerText;
        //        bool flag = false;
        //        flag = dic.TryGetValue(opertime, out flag);
        //        if (flag) continue;
        //        string cftype = cfinfo.SelectSingleNode("处方类型名称").InnerText;
        //        string hospital = cfinfo.SelectSingleNode("单位名称").InnerText;
        //        string cardid = cfinfo.SelectSingleNode("卡号").InnerText;
        //        string pid = cfinfo.SelectSingleNode("门诊号").InnerText;
        //        string patient = cfinfo.SelectSingleNode("姓名").InnerText;
        //        string age = cfinfo.SelectSingleNode("年龄").InnerText;
        //        string doctor = cfinfo.SelectSingleNode("医生").InnerText;
        //        string disease = cfinfo.SelectSingleNode("诊断").InnerText;
        //        string disease2 = cfinfo.SelectSingleNode("辅助信息").InnerText;
        //        string feibie = cfinfo.SelectSingleNode("费别").InnerText;
        //        string phone = cfinfo.SelectSingleNode("联系电话").InnerText;
        //        string address = cfinfo.SelectSingleNode("居住地址").InnerText;
        //        double totalprice = Convert.ToDouble(cfinfo.SelectSingleNode("总金额").InnerText);

        //        string did = cfinfo.SelectSingleNode("身份证号").InnerText;
        //        string sql01 = $@"insert into  cfhead(cftype,hospital,cardid,pid,patient,age,doctor,disease,disease2,feibie,phone,address,totalprice,opertime,did)
        //        values('{cftype}','{hospital}','{cardid}','{pid}','{patient}','{age}','{doctor}','{disease}','{disease2}','{feibie}','{phone}','{address}',{totalprice},'{opertime}','{did}')";
        //        Debug.WriteLine(sql01);
        //        num01 += SqlHelper.ExecuteNoQuery(sql01);
        //        XmlNodeList cfdetail = cf.SelectNodes("处方信息");
        //        string sql02 = string.Empty;
        //        foreach (XmlNode c in cfdetail)
        //        {
        //            string gid = c.SelectSingleNode("组号").InnerText;
        //            string drug = c.SelectSingleNode("名称").InnerText;
        //            string spci = c.SelectSingleNode("规格").InnerText;
        //            string cishu = c.SelectSingleNode("每日次数").InnerText;
        //            double yongliang = Convert.ToDouble(c.SelectSingleNode("每次用量").InnerText);
        //            string danwei = c.SelectSingleNode("用量单位").InnerText;
        //            string yongfa = c.SelectSingleNode("用法").InnerText;
        //            double quantity = Convert.ToDouble(c.SelectSingleNode("数量").InnerText);
        //            string unit = c.SelectSingleNode("单位").InnerText;
        //            double unitprice = Convert.ToDouble(c.SelectSingleNode("单价").InnerText);
        //            sql02 += $"('{gid}','{drug}','{spci}','{cishu}',{yongliang},'{danwei}','{yongfa}',{quantity},'{unit}',{unitprice},'{opertime}'),";
        //        }
        //        string sql = "insert into cfdetail(gid,drug,spci,cishu,yongliang,danwei,yongfa,quantity,unit,unitprice,opertime) values";
        //        Debug.WriteLine((sql + sql02).TrimEnd(new Char[] { ',' }));
        //        string newsql = (sql + sql02).TrimEnd(new Char[] { ',' });
        //         num02 += SqlHelper.ExecuteNoQuery(newsql);

        //    }
        //    return string.Format("导入处方数为：{0},处方明细数为：{1}", num01, num02);
        //}
    }
    }