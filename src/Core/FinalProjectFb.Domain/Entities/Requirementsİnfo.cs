﻿using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Domain.Entities
{
    public class Requirementsİnfo:BaseEntity
    {
        public string Requirement { get; set; }
        public int? JobId { get; set; }
        public Job Job { get; set; }
    }
}
