namespace Lumia.Models
{
    public class Job
    {
        public Job()
        {
            Teams= new List<Team>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Team> Teams { get; set; }
    }
}
