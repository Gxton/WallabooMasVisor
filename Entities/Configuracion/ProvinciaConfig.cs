using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallaboo.Entities.Configs
{
    public class ProvinciaConfig : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            //builder.ToTable("tblProvincia");
            builder.HasKey(provincia => provincia.id);

            //builder.HasMany(provincia => provincia.Ciudades)
            //    .WithOne(ciudad => ciudad.Provincia);            
        }
    }
}
