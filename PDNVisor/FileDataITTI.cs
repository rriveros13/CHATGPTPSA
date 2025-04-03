using System;
using System.ComponentModel;
using System.IO;
using DevExpress.Xpo;
using PDNVisor.model;

namespace PDNVisor {
	[DefaultProperty("FileName")]
	public class FileDataITTI : BaseObject, IFileData, IEmptyCheckable {
		private string fileName = "";
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
		public FileDataITTI(Session session) : base(session) { }
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
				if(value != null) {
					size = value.Length;
				}
				else {
					size = 0;
				}
				SetDelayedPropertyValue("Content", value);
				OnChanged("Size", oldSize, size);
			}
		}
		#region IEmptyCheckable Members
		[NonPersistent, MemberDesignTimeVisibility(false)]
		public bool IsEmpty {
			get { return string.IsNullOrEmpty(FileName); }
		}
		#endregion
	}
}
