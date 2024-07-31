using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallaboo.Entities.Configs
{
    public class PaisConfig : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
          //  builder.ToTable("tblPais");
            builder.HasKey(pais => pais.id);

          //  builder.HasMany(pais => pais.Provincias)
            //    .WithOne(provincia => provincia.Pais);
        }
    }
}
