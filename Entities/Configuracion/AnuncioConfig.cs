using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallaboo.Entities.Configs
{
    public class AnuncioConfig : IEntityTypeConfiguration<Anuncio>
    {
        public void Configure(EntityTypeBuilder<Anuncio> builder)
        {
           // builder.ToTable("tblAnuncio");
            builder.HasKey(anuncio => anuncio.Id);

           // builder.HasOne(anuncio => anuncio.Cliente);
        }
    }
}
