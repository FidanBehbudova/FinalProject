using FinalProjectFb.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Configurations
{
    internal class RequirementsİnfoConfiguration : IEntityTypeConfiguration<Requirementsİnfo>
    {
        public void Configure(EntityTypeBuilder<Requirementsİnfo> builder)
        {
            builder.Property(x => x.Requirement).IsRequired().HasColumnType("text");
           

           



        }
    }
}
