using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ITTI;

namespace ITTI {
	[DefaultProperty("FileName")]
	public class FileDataITTI : BaseObject, IFileDataITTI, IEmptyCheckable {
		private string fileName = "";
        private readonly SHA256 sha = SHA256.Create();

#if MediumTrust
		private int size;
		public int Size {
			get { return size; }
			set { SetPropertyValue("Size", ref size, value); }
		}
#else
		[Persistent]
		private int size;
		public int Size {
			get { return size; }
		}
#endif
		public FileDataITTI(Session session) : base(session)
        {
        }
		public virtual void LoadFromStream(string fileName, Stream stream) {
			Guard.ArgumentNotNull(stream, "stream");
			FileName = fileName;
			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
			Content = bytes;
		}
		public virtual void SaveToStream(Stream stream) {
			if(Content != null) {
				stream.Write(Content, 0, Size);
			}
			stream.Flush();
		}
		public void Clear() {
			Content = null;
            FileName = String.Empty;
		}
		public override string ToString() {
			return FileName;
		}
		[Size(260)] 
		public string FileName {
			get { return fileName; }
			set { SetPropertyValue("FileName", ref fileName, value); }
		}


        //private string annotatios;
        //[Size(SizeAttribute.Unlimited)] 
        //public string Annotations {
        //    get { return annotatios; }
        //    set { SetPropertyValue("Annotations", ref annotatios, value); }
        //}


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

        [Persistent]
        [Size(128)] 
        [Indexed]
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

		#region IEmptyCheckable Members
		[NonPersistent, MemberDesignTimeVisibility(false)]
		public bool IsEmpty {
			get { return string.IsNullOrEmpty(FileName); }
		}

       
        #endregion
    }

}
