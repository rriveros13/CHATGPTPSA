using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDNOriginacion.Module.Web.Controllers.NonPersistentClasses
{
    [NonPersistent]
    public class EnviarAlBack : BaseObject
    {
        private DateTime _fechaDesembolso;
        private DateTime _fechaPrimerVencimiento;

        public EnviarAlBack(Session session) : base(session)
        {
            this.FechaDesembolso = DateTime.Today;
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [XafDisplayName("Fecha para desembolso")]
        public DateTime FechaDesembolso
        {
            get => _fechaDesembolso;
            set => SetPropertyValue(nameof(FechaDesembolso), ref _fechaDesembolso, value);
        }

        [XafDisplayName("Fecha para el primer vencimiento")]
        public DateTime FechaPrimerVencimiento
        {
            get => _fechaPrimerVencimiento;
            set => SetPropertyValue(nameof(FechaPrimerVencimiento), ref _fechaPrimerVencimiento, value);
        }
    }
}
