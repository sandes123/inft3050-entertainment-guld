#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentGuild.Models
{
    [Table("Stocktake")]
    public partial class Stocktake
    {
        [Key]
        public int ItemId { get; set; }

        public int? SourceId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public virtual Product Product { get; set; }

        public virtual Source Source { get; set; }
    }
}
