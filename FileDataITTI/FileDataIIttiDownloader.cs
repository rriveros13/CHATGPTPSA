using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.SystemModule;
using ITTI;
using FileDataHelper = ITTI.FileDataHelper;

namespace ITTI {
	public class FileDataIIttiDownloader {
		private static FileDataIIttiDownloader downloader;
		static FileDataIIttiDownloader() {
			downloader = new FileDataIIttiDownloader();
		}
		public static void SetDownloader(FileDataIIttiDownloader downloader) {
			Guard.ArgumentNotNull(downloader, "downloaderITTI");
			FileDataIIttiDownloader.downloader = downloader;
		}
		public static void Download(IFileDataITTI fileData) {
			downloader.DownloadCore(fileData);
		}
        public static void View(IFileDataITTI fileData) {
            int i = 0;
        }
		public virtual void DownloadCore(IFileDataITTI fileData) {
			if(!FileDataHelper.IsFileDataEmpty(fileData)) {
				ResponseWriter.WriteFileToResponse(fileData);
			}
		}
	}
}
