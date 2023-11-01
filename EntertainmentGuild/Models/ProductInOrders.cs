#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertainmentGuild.Models
{
    public partial class ProductInOrders
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }    
        public int produktId { get; set; }
        [ForeignKey("produktId")]
        public Product Product { get; set; }    
        public int Quantity { get; set; }
    }
}
