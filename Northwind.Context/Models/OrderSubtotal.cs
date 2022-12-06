using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models
{
    [Keyless]
    public partial class OrderSubtotal
    {
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column(TypeName = "money")]
        public decimal? Subtotal { get; set; }
    }
}
