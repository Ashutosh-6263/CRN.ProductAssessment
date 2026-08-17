using CRN.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRN.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.Property(x => x.ModifiedBy)
                .HasMaxLength(100);

            builder.Property(x => x.ModifiedOn);

            builder.HasIndex(x => x.ProductName).IsUnique();
        }
    }
}
