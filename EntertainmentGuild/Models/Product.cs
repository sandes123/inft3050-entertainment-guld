#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentGuild.Models
{

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Stocktakes = new HashSet<Stocktake>();
        }

        public int ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Author { get; set; }

        public string Description { get; set; }

        public int? Genre { get; set; }

        public int? subGenre { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Published { get; set; }

        [StringLength(50)]
        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdated { get; set; }

        public virtual Genre Genre1 { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stocktake> Stocktakes { get; set; }
    }
}
