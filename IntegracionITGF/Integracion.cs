using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionITGF
{
    public class Integracion
    {

        string conectionStringOracle = myConfig["ConectionStringOracle"];
        Dl = XpoDefault.GetDataLayer(conectionStringOracle, AutoCreateOption.DatabaseAndSchema);

            XpoDefault.DataLayer = Dl;
            Uow = new UnitOfWork(Dl);

    }
}
