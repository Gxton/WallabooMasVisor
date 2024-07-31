using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallaboo.Interfaces
{
    public interface IListar<TEntidad, TEntidadId> 
    {
        List<TEntidad> Listar();
        TEntidad SeleccionarPorId(TEntidadId entidadId);

    }
}
