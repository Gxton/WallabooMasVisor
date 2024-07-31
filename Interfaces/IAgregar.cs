using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallaboo.Interfaces
{
    //recibe valores genericos, en este caso las entidades de Dominio
    public interface IAgregar<TEntidad>
    {
        TEntidad Agregar(TEntidad entidad); 
    }
}
