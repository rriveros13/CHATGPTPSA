using DevExpress.Xpo;

namespace FileUploadService.model
{
    [Persistent("Persona")]
    public class XpoPersona : BaseObject
    {
        public XpoPersona(Session session) : base(session)
        {
        }

        private string _documento;
        [Size(30)]
        public string Documento
        {
            get => _documento;
            set => SetPropertyValue(nameof(Documento), ref _documento, value);
        }

    }
}
