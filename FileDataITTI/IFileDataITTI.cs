using System.IO;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;

namespace ITTI {
	public interface IFileDataITTI : IFileData {
		void LoadFromStream(string fileName, Stream stream);
		void SaveToStream(Stream stream);
		void Clear();
		string FileName {
			get;
			set;
		}
		int Size {
			get;
		}
        string Hash {
            get;
        }
    }
	public interface ISupportFullName {
		string FullName { get; set; }
	}
	public sealed class FileDataHelper {
		public static bool IsFileDataEmpty(IFileDataITTI fileData) {
			return (fileData == null) || (string.IsNullOrEmpty(fileData.FileName) ); 
		}
		public static void LoadFromStream(IFileDataITTI fileData, string fileName, Stream stream, string fullName) {
			Guard.ArgumentNotNull(fileData, "fileData");
			if(fileData is ISupportFullName) {
				((ISupportFullName)fileData).FullName = fullName;
			}
			fileData.LoadFromStream(fileName, stream);
		}
	}
}
