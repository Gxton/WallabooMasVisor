using Microsoft.EntityFrameworkCore;
using Wallabo.Entities;

namespace Wallaboo.Entities.Configs
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(usuario => usuario.TenantId);

        }
    }
}
