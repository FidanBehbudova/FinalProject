﻿using FinalProjectFb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class ConfirmationFormVM
    {
        //public List<Image>? Images { get; set; }
        //[Required]
        //public List<CompanyCity>? CompanyCities { get; set; }
        [Required]

        public IFormFile Photo { get; set; }
        public ICollection<int>? CityIds { get; set; }
        public ICollection<City>? Cities { get; set; }
        [Required]

        public bool? IsDeleted { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FacebookLink { get; set; }
        [Required]
        public string InstagramLink { get; set; }
        [Required]
        public string WebsiteLink { get; set; }
        [Required]
        public string TwitterLink { get; set; }
        [Required]
        public string GmailLink { get; set; }
    }
}