using System.ComponentModel.DataAnnotations;

namespace BetSystem.Model
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EventResult> Results { get; set; }
    }
}
