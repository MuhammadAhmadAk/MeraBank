using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeraBank.Models
{
    public class M_ActDetail
    {
        public int Account_ID { get; set; }
        public string Name { get; set; }
        public string CNIC { get; set; }
        public double Balance { get; set; }
    }
}