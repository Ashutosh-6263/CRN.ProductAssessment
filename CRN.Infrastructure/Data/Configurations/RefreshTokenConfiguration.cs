using CRN.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRN.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(
        EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ExpiresOn)
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.Property(x => x.RevokedOn);

            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Token)
                .IsUnique();

            builder.HasIndex(x => x.UserId);
        }
    }
}
