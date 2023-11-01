#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntertainmentGuild.Models
{
    [Table("Book_genre NEW")]
    public partial class Book_genre_NEW
    {
        [Key]
        public int subGenreID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
