using System;
using System.Collections.Generic;
using System.Text;
using MNightWorks.Shared;
using System.Text.Json.Serialization;

namespace MNightWorks.Shared.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        // The foreign key: this column will hold the Id of whichever
        // Restaurant this menu item belongs to. It's just an int under
        // the hood — the database enforces that it must match a real row
        // in the Restaurants table.
        public int RestaurantId { get; set; }

        // The navigation property: lets C# code write menuItem.Restaurant.Name
        // instead of manually looking up the restaurant by RestaurantId yourself.
        // Tells the JSON serializer to never include this property in responses.
        // We already have RestaurantId as a plain number for "which restaurant" —
        // we rarely need the whole nested Restaurant object echoed back too,
        // and skipping it removes the cycle problem at the source.
        [JsonIgnore]
        public Restaurant? Restaurant { get; set; }
    }
}
