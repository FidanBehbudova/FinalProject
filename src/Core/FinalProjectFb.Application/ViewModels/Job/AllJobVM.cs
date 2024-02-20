using FinalProjectFb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class AllJobVM
    {
        public List<Job> Jobs{ get; set; }
        public List<Category> Categories { get; set; }
     

    }
}
