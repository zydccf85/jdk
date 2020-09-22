using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKD.Models
{
    public class HuiZong
    {
        public string Riqi { get; set; }
        public double? TotalPrice { get; set; } = 0;
        public double? MaxPrice { get; set; } = 0;
        public double? MinPrice { get; set; } = 0;
        public double? AvgPrice { get; set; } = 0;
        public int? CfCount { get; set; } = 0;
        public int? MzCount { get; set; } = 0;
        public int? YbCount { get; set; } = 0;
        public int? ZfCount { get; set; } = 0;
        public int? ZcyCount { get; set; } = 0;
        public int? JdCount { get; set; } = 0;
        public int? JeCount { get; set; } = 0;
        public int? EkCount { get; set; } = 0;
        public int? JsCount { get; set; } = 0;
        public int? KjyCount { get; set; } = 0;
        public int? JzCount { get; set; } = 0;
        public string MaxDate { get; set; }
        public override string ToString()
        {
            return string.Format(@"处方数量：{0:n0}（医保处方数：{1:n0}，自费处方数：{2:n0}），门诊人次数：{3:n0}，金额：{4:c2}（最大金额：{5:c2}，" +
        "最小金额：{6:c2}，平均金额：{7:c2}），中成药处方数：{8:n0}，静滴处方数：{9:n0}，精二处方数：{10:n0}，儿科处方数：{11:n0}，激素处方数：{12:n0}," +
        "抗菌药处方数：{13:n0},急诊处方数：{14:n0}, ",
        CfCount,YbCount,ZfCount,MzCount,TotalPrice,MaxPrice,MinPrice,AvgPrice,ZcyCount,JdCount,JeCount,EkCount,JsCount,KjyCount,JzCount );
        }


    }
}
