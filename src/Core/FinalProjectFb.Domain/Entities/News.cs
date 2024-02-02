using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class News :BaseNameableEntity 
    {
        public DateTime DateOfNews { get; set; }
        public List<Image> Images { get; set; }
        public string Description { get; set; }
    }
}
