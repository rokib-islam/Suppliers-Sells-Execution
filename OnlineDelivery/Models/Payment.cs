//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineDelivery.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public partial class Payment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Payment()
        {
            this.OrderMains = new HashSet<OrderMain>();
        }
    
        public int Payment_ID { get; set; }
		[Required]
		[DisplayName("Payment Type")]
		public string Payment_Type { get; set; }

		[Required]
		[DisplayName("Payment Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public Nullable<int> Payment_Phone { get; set; }

		[Required]
		[DisplayName("Payment Transaction Id")]
		public Nullable<int> Payment_Code { get; set; }

		[Required]
		[DisplayName("Payment Amount")]
		public Nullable<int> Payment_Amount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderMain> OrderMains { get; set; }
    }
}
