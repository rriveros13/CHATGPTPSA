using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using IntegracionITGF.DataAccess;
using PDNOriginacion.Module.BusinessObjects;
using Shared;
using Xunit;

namespace OriginacionTests
{
    public class IntegracionITGFPersonasTest
    {
        private Persona personaOri;
        private Solicitud solicitudOri;
        public IntegracionITGFPersonasTest()
        {           
            ConnectionHelper.Connect(DevExpress.Xpo.DB.AutoCreateOption.None);
            Session session = new Session();
            solicitudOri = session.FindObject<Solicitud>(CriteriaOperator.Parse("Oid = 5269"));
            personaOri = solicitudOri.Titular;
        }

        /*[Fact]
        public void CrearPersona()
        {
            ITGFAccess iTGFAccess = new ITGFAccess();
            OpeRespuesta resp = iTGFAccess.CreatePersona(personaOri);
            Assert.False(resp.Error);
        }*/

        [Fact]
        public void CrearSolicitud()
        {
            ITGFAccess iTGFAccess = new ITGFAccess();
            OpeRespuesta resp = iTGFAccess.CreateSolicitud(solicitudOri);
            Assert.False(resp.Error);
        }
    }
}
