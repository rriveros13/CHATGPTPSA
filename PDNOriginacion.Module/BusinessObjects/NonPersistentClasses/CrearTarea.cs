using DevExpress.ExpressApp.DC;
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
    public class CrearTarea : BaseObject
    {
        private Usuario _usuario;
        private string _descripcion;
        private PrioridadTarea _prioridad;
        private WFTarea _tipoTarea;
        private Solicitud _solicitud;

        public CrearTarea(Session session) : base(session)
        {
            this.Prioridad = PrioridadTarea.Alta;
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [XafDisplayName("Usuario al que se asignará la tarea")]
        public Usuario Usuario
        {
            get => _usuario;
            set => SetPropertyValue(nameof(Usuario), ref _usuario, value);
        }

        [XafDisplayName("Descripción")]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        [VisibleInDetailView(false)]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        [DataSourceCriteria("Producto='@This.Solicitud.Producto'")]
        public WFTarea TipoTarea
        {
            get => _tipoTarea;
            set => SetPropertyValue(nameof(TipoTarea), ref _tipoTarea, value);
        }

        public PrioridadTarea Prioridad
        {
            get => _prioridad;
            set => SetPropertyValue(nameof(Prioridad), ref _prioridad, value);
        }
    }
}
