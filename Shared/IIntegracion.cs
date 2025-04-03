using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IIntegracion
    {
        OpeRespuesta CreatePersona(object PersonaOri);
        OpeRespuesta CreateSolicitud(object SolicitudOri);
    }
}
