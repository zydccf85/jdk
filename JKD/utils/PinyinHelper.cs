using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.International.Converters.PinYinConverter;
namespace JKD.utils
{
    public class PinyinHelper
    {
        public string GetFirstLetter(string str)
        {
            StringBuilder sb = new StringBuilder();
            str.ToList().ForEach(item => {
                if (ChineseChar.IsValidChar(item))
                {
                    sb.Append(new ChineseChar(item).Pinyins[0][0]);
                }
                else
                {
                    sb.Append(item);
                }
               
            });

            return sb.ToString().ToLower();
        }
    }
}
