using FinalProjectFb.Domain.Entities.Common;

namespace FinalProjectFb.Web.Models
{
    public class News :BaseNameableEntity
    {
        public DateTime DateOfNews { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
