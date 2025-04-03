using System;
using DevExpress.Xpo;

namespace FileUploadService.model
{
    [Persistent("Adjunto")]
    public class XpoAdjunto : BaseObject
    {
        private bool _adjuntado;
        private XpoFileDataItti _archivo;
        private string _descripcion;
        private DateTime _fecha = DateTime.Now;
        private int _solicitud;
        private Guid? _tipoAdjunto;

        public XpoAdjunto(Session session) : base(session)
        {
        }

        public bool Adjuntado
        {
            get => _adjuntado;
            set => SetPropertyValue(nameof(Adjuntado), ref _adjuntado, value);
        }

        public XpoFileDataItti Archivo
        {
            get => _archivo;
            set => SetPropertyValue(nameof(Archivo), ref _archivo, value);
        }

        [Size(250)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
        }

        public int Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        public Guid? TipoAdjunto
        {
            get => _tipoAdjunto;
            set => SetPropertyValue(nameof(TipoAdjunto), ref _tipoAdjunto, value);
        }

        private Guid _adjuntadoPor;
        public Guid AdjuntadoPor
        {
            get => _adjuntadoPor;
            set => SetPropertyValue(nameof(AdjuntadoPor), ref _adjuntadoPor, value);
        }

        private Guid? _persona;
        public Guid? Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

    }
}

