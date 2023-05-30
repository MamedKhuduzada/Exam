using Lumia.Models;

namespace Lumia.ViewModels.TeamVM
{
    public class EditTeamVM
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public int JobId { get; set; }
        public List<Job>? Jobs { get; set; }
    }
}
