namespace Lumia.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Description { get; set; }
        public string ImageName { get; set; }=null!;
        public int JobId { get; set; }
        public Job? Jobs { get; set; }
    }
}
