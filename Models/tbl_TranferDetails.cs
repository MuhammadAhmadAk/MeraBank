//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeraBank.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_TranferDetails
    {
        public int TrsID { get; set; }
        public Nullable<int> ActId { get; set; }
        public string TrsCategory { get; set; }
        public string TrsDate { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal Balance { get; set; }
    
        public virtual tbl_AccountHolder tbl_AccountHolder { get; set; }
    }
}