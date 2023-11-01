#nullable disable
using System.ComponentModel.DataAnnotations;

namespace EntertainmentGuild.Models
{
    public partial class Game_genre
    {
        [Key]
        public int subGenreID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
