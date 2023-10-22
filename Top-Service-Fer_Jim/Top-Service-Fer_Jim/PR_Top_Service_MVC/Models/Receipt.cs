using System;
using System.Collections.Generic;

namespace PR_Top_Service_MVC.Models
{
    public partial class Receipt
    {
        public int IdReceipt { get; set; }
        public string Description { get; set; } = null!;
        public decimal Total { get; set; }

        public virtual Service IdReceiptNavigation { get; set; } = null!;
    }
}
