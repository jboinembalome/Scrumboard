using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Infrastructure.Persistence.Configurations
{
    public class AdherentConfiguration : IEntityTypeConfiguration<Adherent>
    {
        public void Configure(EntityTypeBuilder<Adherent> builder)
        {
            builder.Property(a => a.IdentityGuid)
                .IsRequired();
        }
    }
}
