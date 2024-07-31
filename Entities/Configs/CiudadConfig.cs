using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallaboo.Entities.Configs
{
    public class CiudadConfig : IEntityTypeConfiguration<Ciudad>
    {
        public void Configure(EntityTypeBuilder<Ciudad> builder)
        {
            //builder.ToTable("tblCiudad");
            builder.HasKey(ciudad => ciudad.Id);

            //builder.HasOne(ciudad => ciudad.Provincia);
        }
    }
}
