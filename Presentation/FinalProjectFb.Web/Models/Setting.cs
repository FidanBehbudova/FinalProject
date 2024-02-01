using FinalProjectFb.Domain.Entities.Common;

namespace FinalProjectFb.Web.Models
{
    public class Setting:BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
