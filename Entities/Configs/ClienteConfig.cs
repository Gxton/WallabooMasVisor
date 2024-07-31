using Microsoft.EntityFrameworkCore;
using Wallabo.Entities;

namespace Wallaboo.Entities.Configs
{
    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
        {
           // builder.ToTable("tblCliente");
            builder.HasKey(cliente => cliente.Id);

           // builder.HasMany(cliente => cliente.Anuncios)
           //     .WithOne(anuncio => anuncio.Cliente);
        }
    }
}
