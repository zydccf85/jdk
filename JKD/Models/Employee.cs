using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKD.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public int Eid { get; set; }
        public string Cardid { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public byte[] Pic { get; set; }
    }
}
