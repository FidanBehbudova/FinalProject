﻿using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class Job:BaseNameableEntity
    {
        public List<Image>? Images { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set;}

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime Deadline { get; set; }

       
        public List<Requirementsİnfo> Requirementsİnfos { get; set; }
      
        public List<BasicFunctionsİnfo> BasicFunctionsİnfos { get; set; }


        public decimal Salary { get; set; }
        public string JobNature { get; set; }
        public string Experience { get; set; }

        public string Vacancy { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime DiscontinuationDate { get; set; }





    }
}
