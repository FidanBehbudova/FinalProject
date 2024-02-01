using FinalProjectFb.Domain.Entities.Common;

namespace FinalProjectFb.Web.Models
{
    public class Image:BaseNameableEntity
    {
        public string Url { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
