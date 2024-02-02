using FinalProjectFb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    internal class HomeVM
    {
        public List<Category> Categories { get; set; }
        public List<Job> Jobs { get; set; }
        public List<News> Newss { get; set; }
        public List<Company> Companies { get; set; }

    }
}
