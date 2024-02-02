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
    internal class BasicFunctionsİnfoConfiguration : IEntityTypeConfiguration<BasicFunctionsİnfo>
    {
        public void Configure(EntityTypeBuilder<BasicFunctionsİnfo> builder)
        {
            builder.Property(x => x.Function).IsRequired().HasColumnType("text");
            





        }
    }
}
