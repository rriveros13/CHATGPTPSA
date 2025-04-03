using DevExpress.Xpo;

namespace FileUploadService.model
{
    [Persistent("TipoAdjunto")]
    public class XpoTipoAdjunto : BaseObject
    {
        private string _codigo;
        private string _descripcion;
        private int _porcentajeMatching;
        private bool _usarvalidador;
        private int _vigencia;

        public XpoTipoAdjunto(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _vigencia = 0;
        }

        [Size(30)]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [Size(100)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        public int PorcentajeMatching
        {
            get => _porcentajeMatching;
            set => SetPropertyValue(nameof(PorcentajeMatching), ref _porcentajeMatching, value);
        }

        public bool UsarValidador
        {
            get => _usarvalidador;
            set => SetPropertyValue(nameof(UsarValidador), ref _usarvalidador, value);
        }

        public int Vigencia
        {
            get => _vigencia;
            set => SetPropertyValue(nameof(Vigencia), ref _vigencia, value);
        }
    }
}
