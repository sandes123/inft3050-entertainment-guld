#nullable disable
using System.ComponentModel.DataAnnotations;

namespace EntertainmentGuild.Models
{
        public partial class Patron
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patron()
        {
            TOes = new HashSet<TO>();
        }

        [Key]
        public int UserID { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(32)]
        public string Salt { get; set; }

        [StringLength(64)]
        public string HashPW { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TO> TOes { get; set; }
    }

}
