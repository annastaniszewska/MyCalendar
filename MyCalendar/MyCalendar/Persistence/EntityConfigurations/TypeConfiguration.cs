using System;
using System.Data.Entity.ModelConfiguration;

namespace MyCalendar.Persistence.EntityConfigurations
{
    public class TypeConfiguration : EntityTypeConfiguration<Type>
    {
        public TypeConfiguration()
        {
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}