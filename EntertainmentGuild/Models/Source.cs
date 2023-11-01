#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentGuild.Models
{

    [Table("Source")]
    public partial class Source
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Source()
        {
            Stocktakes = new HashSet<Stocktake>();
        }

        public int sourceid { get; set; }

        public string Source_name { get; set; }

        public string externalLink { get; set; }

        public int? Genre { get; set; }

        public virtual Genre Genre1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stocktake> Stocktakes { get; set; }
    }
}
