namespace MNight_works.Models
{
    public class MenuItem
    {
        // Unique identifier for the menu item
        public int Id { get; set; }
        // Name of the menu item
        public string Name { get; set; } = string.Empty;
        // Description of the menu item
        public string? Description { get; set; }
        // Price of the menu item
        public decimal Price { get; set; }
        // Indicates whether the menu item is available for order
        public bool IsAvailable { get; set; } = true;
    }
}
