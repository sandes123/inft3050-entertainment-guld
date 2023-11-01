#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentGuild.Models
{
    public partial class Order
    {
        public int OrderID { get; set; }

        public int? customer { get; set; }

        [StringLength(255)]
        public string StreetAddress { get; set; }

        public int? PostCode { get; set; }

        [StringLength(255)]
        public string Suburb { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        public virtual TO TO { get; set; }
    }
}
