using FinalProjectFb.Domain.Entities.Common;

namespace FinalProjectFb.Web.Models
{
    public class Category:BaseNameableEntity
    {
        public string Icon { get; set; }
        public int Count { get; set; }

    }
}
