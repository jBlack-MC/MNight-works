using System.Text.Json.Serialization;

namespace MNightWorks.Shared.Models
{
    // Represents someone who can log in — for now, specifically a restaurant owner.
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        // A securely hashed password, never the real one.
        // [JsonIgnore] means this can NEVER accidentally be sent back in an API response —
        // even if a future endpoint returns a User object by mistake, this field just won't be there.
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
