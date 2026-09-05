using System;
using System.Collections.Generic;
using System.Text;

namespace MNightWorks.Shared.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
