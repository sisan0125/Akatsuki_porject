//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customers()
        {
            this.SOM = new HashSet<SOM>();
        }
    
        public string CUID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Address { get; set; }
        public string ReceiptAddress { get; set; }
        public string Phone { get; set; }
        public string VAT { get; set; }
        public string CP { get; set; }
        public string CPphone { get; set; }
        public string PIC { get; set; }
        public System.DateTime bDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOM> SOM { get; set; }
    }
}