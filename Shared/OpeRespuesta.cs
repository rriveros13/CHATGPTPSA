using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class OpeRespuesta
    {
        public OpeRespuesta()
        {
            this.Error = false;
        }

        public bool Error { get; set; }
        public string Mensaje { get; set; }
        public Exception Excepcion { get; set; }
    }
}
