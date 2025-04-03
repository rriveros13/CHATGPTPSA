using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using PDNOriginacion.Module.BusinessObjects;
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
        private Solicitud _solicitud;

        public EnviarAlBack(Session session) : base(session)
        {
            this.FechaDesembolso = DateTime.Today;
        }

        [Appearance("", TargetItems = nameof(Solicitud), AppearanceItemType = nameof(ViewItem), Visibility = ViewItemVisibility.Hide)]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [XafDisplayName("Fecha para desembolso")]
        [ImmediatePostData]
        public DateTime FechaDesembolso
        {
            get => _fechaDesembolso;
            set
            {
                if (value != null) value = value.Date;
                bool cambio = SetPropertyValue(nameof(FechaDesembolso), ref _fechaDesembolso, value);
                if (IsLoading || IsSaving || !cambio || this.Solicitud == null) return;
                    this.FechaPrimerVencimiento = FechaDesembolso.AddMonths(this.Solicitud.Producto.MesesPrimerVencimiento);
                
                OnChanged(nameof(FechaPrimerVencimiento));
            }
        }

        [XafDisplayName("Fecha para el primer vencimiento")]
        public DateTime FechaPrimerVencimiento
        {
            get => _fechaPrimerVencimiento;
            set => SetPropertyValue(nameof(FechaPrimerVencimiento), ref _fechaPrimerVencimiento, value);
        }
    }
}
