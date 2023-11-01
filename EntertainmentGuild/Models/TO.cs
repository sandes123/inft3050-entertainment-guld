#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentGuild.Models
{
    [Table("TO")]
    public partial class TO
    {
        

        [Key]
        public int customerID { get; set; }

        public int? PatronId { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string StreetAddress { get; set; }

        public int? PostCode { get; set; }

        [StringLength(50)]
        public string Suburb { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(50)]
        public string CardOwner { get; set; }

        [StringLength(5)]
        public string Expiry { get; set; }

        public int? CVV { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual Patron Patron { get; set; }
    }
}
