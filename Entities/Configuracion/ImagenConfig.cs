using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallaboo.Entities.Configuracion
{
    public class ImagenConfig : IEntityTypeConfiguration<Imagen>
    {
        public void Configure(EntityTypeBuilder<Imagen> builder)
        {
            builder.HasKey(imagen => imagen.Id);
        }
    }
}
