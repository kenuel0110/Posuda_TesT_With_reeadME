//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArtemNikita
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderPickupPoints
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderPickupPoints()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int id { get; set; }
        public int address_index { get; set; }
        public string address_city { get; set; }
        public string address_street { get; set; }
        public Nullable<int> address_home { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}