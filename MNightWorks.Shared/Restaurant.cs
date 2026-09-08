using MNightWorks.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MNightWorks.Shared
{
    public class Restaurant
    {
        //its a convenience EF core provides so the c# code ran instead of writing a separate query
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new(); 


    }
}
