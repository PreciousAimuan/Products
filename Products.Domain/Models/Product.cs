using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
