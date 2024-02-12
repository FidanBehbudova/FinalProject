using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectFb.ViewModels
{
    public class CreateJobVM
    {
       
        public string Requirement { get; set; }
        public IFormFile Photo { get; set; }

        public string Function { get; set; }
        public string Name { get; set; }

        public int? CompanyId { get; set; }

  
        public int? CategoryId { get; set; }

       
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

     
        public string JobNature { get; set; }

       
        public string Experience { get; set; }

     
        public string Vacancy { get; set; }

    
        [DataType(DataType.Date)]
        public DateTime DiscontinuationDate { get; set; }

       
        public string CreatedByCompanyId { get; set; }
    }
}
