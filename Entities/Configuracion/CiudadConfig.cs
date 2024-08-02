using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallaboo.Entities.Configs
{
    public class CiudadConfig : IEntityTypeConfiguration<Ciudad>
    {
        public void Configure(EntityTypeBuilder<Ciudad> builder)
        {
            builder.HasKey(ciudad => ciudad.Id);
        }
    }
}
