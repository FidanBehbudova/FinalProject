using FinalProjectFb.Domain.Entities.Common;
using FinalProjectFb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class Company : BaseNameableEntity
    {
        public List<Image>? Images { get; set; }

        public string Description { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string WebsiteLink { get; set; }
        public string TwitterLink { get; set; }
        public string GmailLink { get; set; }      
        public ICollection<CompanyCity> CompanyCities { get; set; }

    }
}
