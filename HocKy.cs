//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhanMemQuanLyHocSinhTHPT_ADO
{
    using System;
    using System.Collections.Generic;
    
    public partial class HocKy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HocKy()
        {
            this.DiemSoes = new HashSet<DiemSo>();
        }
    
        public string MaHK { get; set; }
        public string TenHK { get; set; }
        public Nullable<int> HeSo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiemSo> DiemSoes { get; set; }
    }
}
