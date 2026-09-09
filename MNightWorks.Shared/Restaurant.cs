using MNightWorks.Shared.Models;
using System.Text.Json.Serialization;
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

        // Which user owns this restaurant. Nullable, because your existing restaurant
        // (id 2) doesn't have an owner assigned yet — we'll set that in a moment.
        public int? OwnerId { get; set; }

        // [JsonIgnore] here from the start — this is the exact kind of two-way link
        // (Restaurant → Owner → [their other restaurants, if we ever add that] → ...)
        // that caused the infinite-loop bug earlier. Marking it now avoids repeating that.
        [JsonIgnore]
        public User? Owner { get; set; }

    }
}
