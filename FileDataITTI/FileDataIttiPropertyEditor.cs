using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.TestScripts;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Web;
using ITTI;

namespace ITTI {
    [PropertyEditor(typeof(ITTI.FileDataITTI), true)]
	public class FileDataIttiPropertyEditor : WebPropertyEditor, IComplexViewItem, ITestable {
        private class FileDataWrapper : IFileDataITTI, ISupportFullName {
			private string fileName;
			private string fullName;
			private Stream innerStream;
			public IFileDataITTI OriginalFileData { get; }

			public string FileName {
				get => OriginalFileData.FileName;
                set => fileName = value;
            }
			public int Size => OriginalFileData.Size;
            public string Hash => OriginalFileData.Hash;

            public string FullName {
				get => OriginalFileData is ISupportFullName name ? name.FullName : null;
                set => fullName = value;
            }
			public FileDataWrapper(IFileDataITTI originalFileData) {
				OriginalFileData = originalFileData;
			}
			public void LoadFromStream(string pfileName, Stream stream) {
				ClearChanges();
				fileName = pfileName;
				if(stream is FileStream) { 
					innerStream = stream;
				}
				else {
					innerStream = new MemoryStream();
					stream.CopyTo(innerStream);
					innerStream.Seek(0, SeekOrigin.Begin);
				}
			}
			public void SaveToStream(Stream stream) {
				OriginalFileData.SaveToStream(stream);
			}
			public void Clear() {
				ClearChanges();
				OriginalFileData.Clear();
			}
			public void ClearChanges() {
				fileName = null;
                if (innerStream == null) return;
                string tempFileName = null;
                if(innerStream is FileStream stream) {
                    try {
                        tempFileName = stream.Name;
                    }
                    catch
                    {
                        // ignored
                    }
                }
                innerStream.Dispose();
                innerStream = null;
                if(!string.IsNullOrEmpty(tempFileName)) {
                    try {
                        File.Delete(tempFileName);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
			public void FlushChanges() {
				if(innerStream != null) {
					if(OriginalFileData is ISupportFullName name) {
						name.FullName = fullName;
					}
					OriginalFileData.LoadFromStream(fileName, innerStream);
					ClearChanges();
				}
			}
		}

		private IObjectSpace objectSpace;
		private FileDataWrapper fileDataWrapper;
		private ASPxTextBox hiddenTextBox;
		private FileDataEditIttiControl CreateFileDataEdit() {
            FileDataEditIttiControl fileDataEditItti =
                new FileDataEditIttiControl(ViewEditMode, GetFileDataWrapper(), !AllowEdit, ImmediatePostData)
                {
                    NullText = NullText
                };

            fileDataEditItti.CreateCustomFileDataObject += CreateFileDataObject;
			fileDataEditItti.Init += FileDataEdit_Init;
            
			return fileDataEditItti;
		}
		private XafCallbackManager CallbackManager => ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;

        private IFileDataITTI FileData => (IFileDataITTI)PropertyValue;

        private FileDataWrapper GetFileDataWrapper() {
			if(fileDataWrapper != null && fileDataWrapper.OriginalFileData != FileData) {
				fileDataWrapper.ClearChanges();
				fileDataWrapper = null;
			}
			if(fileDataWrapper == null && FileData != null) {
				fileDataWrapper = new FileDataWrapper(FileData);
			}
			return fileDataWrapper;
		}

        private static string EncryptData(string plain)
        {
            string aesPassword = string.Concat("ytfILO!655577vvcvd333", System.Configuration.ConfigurationManager.AppSettings["aesPassword"]);

            string encrypted = CipherUtility.Encrypt<AesManaged>(plain, aesPassword, "77654");

            return encrypted;
        }

		private void FileDataEdit_Init(object sender, EventArgs e) {
            var hiddenTextB = new ASPxTextBox {ID = "TBHF", ClientVisible = false, Value = ""};
            hiddenTextB.ValueChanged += EditValueChangedHandler;
			FileDataEditIttiControl fileDataEditItti = (FileDataEditIttiControl)sender;
			FileDataEditIttiControl.AddTableRow(fileDataEditItti, hiddenTextB);
            fileDataEditItti.ParametroVisor = GenerateUrlParam();
            
            string immediatePostDataScript = ImmediatePostData ? CallbackManager.GetScript() : string.Empty;
			ClientSideEventsHelper.AssignClientHandlerSafe(fileDataEditItti.UploadControl, "FileUploadComplete",
                $@"function(s, e) {{
    var hiddenTextBox = ASPx.GetControlCollection().Get('{hiddenTextB.ClientID}');
    if(hiddenTextBox) {{ hiddenTextBox.SetText(new Date().getUTCMilliseconds()); }}
    {immediatePostDataScript}
}}", "FileDataPropertyEditor");
		}


		public override void BreakLinksToControl(bool unwireEventsOnly) {
			FileDataEditIttiControl fileDataEditItti = ViewEditMode == ViewEditMode.Edit ? Editor : InplaceViewModeEditor as FileDataEditIttiControl;
			if(fileDataEditItti != null) {
				fileDataEditItti.CreateCustomFileDataObject -= CreateFileDataObject;
				fileDataEditItti.Init -= FileDataEdit_Init;
			}
			if(hiddenTextBox != null) {
				hiddenTextBox.ValueChanged -= EditValueChangedHandler;
				hiddenTextBox = null;
			}
			base.BreakLinksToControl(unwireEventsOnly);
		}
		protected override void WriteValueCore()
        {
            fileDataWrapper?.FlushChanges();
        }
		protected override WebControl CreateViewModeControlCore() {
			return CreateEditModeControlCore();}
        
        protected override void ReadViewModeValueCore() {
			((FileDataEditIttiControl)InplaceViewModeEditor).FileData = FileData;
            ((FileDataEditIttiControl) InplaceViewModeEditor).ParametroVisor = GenerateUrlParam();
        }

        private string GenerateUrlParam()
        {
            //Obtener el Oid de la propiedad de tipo FileDataItti para enviar al visor
            var fileDataProperty = CurrentObject.GetType().GetProperty(PropertyName)?.GetValue(CurrentObject,null);
            var oid = fileDataProperty?.GetType().GetProperty("Oid")?.GetValue(fileDataProperty, null);

            if (oid != null)
                return HttpUtility.UrlEncode(EncryptData(string.Concat(oid.ToString(), "|",
                    DateTime.UtcNow.ToString(new CultureInfo("es-PY")), "|", GetUserIp())));
            return "Error";
            //return HttpUtility.UrlEncode(EncryptData(string.Concat((View.ObjectSpace.GetKeyValue(View.CurrentObject)).ToString(),"|", DateTime.UtcNow.ToString(new CultureInfo("es-PY")), "|", GetUserIp())));
        }

        private static string GetUserIp()
        {
            HttpContext context = HttpContext.Current;

            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            return !string.IsNullOrEmpty(ipList) ? ipList.Split(',')[0] : context.Request.ServerVariables["REMOTE_ADDR"];
        }

		protected override IJScriptTestControl GetInplaceViewModeEditorTestControlImpl() {
			return new JSFileDataPropertyEditorTestControl();
		}
		protected override WebControl CreateEditModeControlCore() {
			return CreateFileDataEdit();
		}
		protected override void ReadEditModeValueCore() {
			Editor.FileData = GetFileDataWrapper();
		}
		protected override IJScriptTestControl GetEditorTestControlImpl() {
			return new JSFileDataPropertyEditorTestControl();
		}
		protected override void ApplyReadOnly() {
			if(ViewEditMode == ViewEditMode.Edit && Editor != null) {
				Editor.ReadOnly = !AllowEdit;
			}
		}
		protected override object GetControlValueCore() {
			return null;
		}
		protected virtual void CreateFileDataObject(object sender, CreateCustomFileDataObjectEventArgs e) {
			IFileDataITTI fileData = (IFileDataITTI)FileAttachmentsAspNetModule.CreateFileData(objectSpace, MemberInfo);
			MemberInfo.SetValue(CurrentObject, fileData);
			e.FileData = GetFileDataWrapper();
		}
		protected override void Dispose(bool disposing) {
			if(fileDataWrapper != null) {
				fileDataWrapper.ClearChanges();
				fileDataWrapper = null;
                
			}
			base.Dispose(disposing);
		}
		public FileDataIttiPropertyEditor(Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
		public new FileDataEditIttiControl Editor => (FileDataEditIttiControl)base.Editor;

        #region IComplexViewItem Members
		public void Setup(IObjectSpace pobjectSpace, XafApplication application) {
			objectSpace = pobjectSpace;
		}
		#endregion
	}
}
