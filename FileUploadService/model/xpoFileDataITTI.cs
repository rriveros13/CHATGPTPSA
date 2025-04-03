using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using DevExpress.Xpo;

namespace FileUploadService.model {
	[DefaultProperty("FileName")]
    [Persistent("FileDataITTI")]
	public class XpoFileDataItti : BaseObject {
		private string fileName = "";
        private readonly SHA256 sha = SHA256.Create();
		[Persistent]
		private int size;
		public int Size => size;
        public XpoFileDataItti(Session session) : base(session) { }
		
		public override string ToString() {
			return FileName;
		}
		[Size(260)] 
		public string FileName {
			get => fileName;
            set => SetPropertyValue("FileName", ref fileName, value);
        }

		[Persistent, Delayed(true)]
		[ValueConverter(typeof(CompressionConverter))]
		[MemberDesignTimeVisibility(false)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] Content {
            get { return GetDelayedPropertyValue<byte[]>("Content"); }
            set {
                int oldSize = size;
                string oldHash = hash;
                if(value != null) {
                    size = value.Length;
                    hash = GetHash(sha.ComputeHash(value));
                }
                else {
                    size = 0;
                    hash = string.Empty;
                }
                SetDelayedPropertyValue("Content", value);
                OnChanged("Size", oldSize, size);
                OnChanged("Hash", oldHash, hash);
            }
        }

        [Size(128)] 
        [Indexed]
        [Persistent]
        private string hash;
        public string Hash {
            get { return hash; }
        }

        private static string GetHash(byte[] data)
        {
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

		[NonPersistent, MemberDesignTimeVisibility(false)]
		public bool IsEmpty => string.IsNullOrEmpty(FileName);
    }
}
