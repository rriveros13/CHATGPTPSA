using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Web;
using ITTI;

namespace ITTI {
	public sealed class FileDataEditIttiControl : Table, INamingContainer, IXafCallbackHandler {
        const string FileAnchorStyleClassName = "XafFileDataAnchor";
		const string ClearCommandId = "Clear";
		const string LocalizationGroup = "FileAttachments";
		private ASPxButton fileDownloadLink;
        private ASPxButton fileViewLink;
		private ASPxButton editButton;
		private ASPxButton clearButton;
		private ASPxUploadControl uploadControl;
		private bool isPrerendered;
		private void FileDownloadLink_Click(object sender, EventArgs e) {
			FileDataIIttiDownloader.Download(FileData);
		}

        private void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e) {
			FileDataLoadingEventArgs args = new FileDataLoadingEventArgs(e);
            FileDataLoading?.Invoke(this, args);
            if(!args.Cancel) {
				if(e.UploadedFile != null) {
					string fullFileName = e.UploadedFile.FileName;
					if(!string.IsNullOrEmpty(fullFileName)) {
						if(FileData == null) {
							FileData = OnCreateCustomFileDataObject();
						}
						if(FileData != null) {
							string fileName = Path.GetFileName(fullFileName);
							FileDataHelper.LoadFromStream(FileData, fileName, e.UploadedFile.FileContent, fullFileName);
							e.CallbackData = fileName;
						}
						else {
							e.IsValid = false;
							e.ErrorText = "Upload failed due to missing target FileData.";
						}
					}
				}
			}
		}

		private void CreateControls() {
			fileDownloadLink = CreateFileDownloadLink();
			fileDownloadLink.Click += FileDownloadLink_Click;
			editButton = CreateButton("Edit", CaptionHelper.GetLocalizedText(LocalizationGroup,"Editor_Edit"), "Editor_Edit");
			clearButton = CreateButton("Clear", CaptionHelper.GetLocalizedText(LocalizationGroup, "Editor_Clear"), "Editor_Clear");
            //fileViewLink = CreateViewLink();
            fileViewLink = CreateButton("VL", "Ver", "State_ItemVisibility_Show");
			uploadControl = CreateUploadControl();
			SetBackColor(uploadControl, BackColor);
			uploadControl.FileUploadComplete += UploadControl_FileUploadComplete;
            UploadControlCreated?.Invoke(this, EventArgs.Empty);}

        private void SetupClientSideEvents() {
            string urlVisor = System.Configuration.ConfigurationManager.AppSettings["urlVisor"];
            fileDownloadLink.ClientSideEvents.Click = "function(s, e) { cancelProgress(); e.cancelEventAndBubble = true; }";
            fileViewLink.ClientSideEvents.Click = @"function(s, e) 
            {
                window.open('" + urlVisor + @"?src=' + s.cpOid,'_blank'); cancelProgress(); e.cancelEventAndBubble = true;
            }";
           
            editButton.ClientSideEvents.Click = GetEditButtonClickScript();
			string fileUploadCompleteScript = GetFileUploadCompleteScript();
            ClientSideEventsHelper.AssignClientHandlerSafe(clearButton, "Click", @"function(s, e) { " + CallbackManager.GetScript(UniqueID, "'" + ClearCommandId + "'") + " }", "FileDataEditITTI");
			ClientSideEventsHelper.AssignClientHandlerSafe(uploadControl, "FileUploadComplete", fileUploadCompleteScript, "FileDataEditITTI");}

		private void UpdateControls() {
			bool isFileDataEmpty = FileDataHelper.IsFileDataEmpty(FileData);
			if(!isFileDataEmpty) {
				fileDownloadLink.Text = FileData.FileName;
                if (fileViewLink != null)
                {
                    if (fileViewLink.JSProperties.ContainsKey("cpOid"))
                    {
                        fileViewLink.JSProperties["cpOid"] = ParametroVisor;
                    }
                    else
                    {
                        fileViewLink.JSProperties.Add("cpOid", ParametroVisor);
                    }
                }
              

            }
			if(ReadOnly) {
				if(isFileDataEmpty) {
					fileDownloadLink.Visible = false;
                    fileViewLink.Visible = false;
					string nullText = HttpUtility.HtmlEncode(NullText);
					Rows[0].Cells[0].Controls.Add(new LiteralControl(nullText));
				}
				editButton.Visible = false;
				clearButton.Visible = false;
				uploadControl.Visible = false;
            }
			else {
				fileDownloadLink.ClientVisible = !isFileDataEmpty;
                if (fileViewLink != null) fileViewLink.ClientVisible = false;
                editButton.ClientVisible = !isFileDataEmpty;
				clearButton.ClientVisible = !isFileDataEmpty;
				uploadControl.ClientVisible = isFileDataEmpty;
                
            }
		}
		internal static void AddTableRow(Table table, params Control[] controls) {
			TableRow row = new TableRow();
			TableCell cell = new TableCell();
			cell.Wrap = false;
			foreach(Control control in controls) {
				cell.Controls.Add(control);
			}
			row.Cells.Add(cell);
			table.Rows.Add(row);
		}

		private static ASPxButton CreateFileDownloadLink() {
			ASPxButton fileDownloadLink = RenderHelper.CreateASPxButton();
			fileDownloadLink.ID = "HA";
			fileDownloadLink.RenderMode = ButtonRenderMode.Link;
			fileDownloadLink.CausesValidation = false;
			fileDownloadLink.CssClass = FileAnchorStyleClassName;
			return fileDownloadLink;
		}

        private static ASPxButton CreateButton(string id, string caption, string imageName) {
			ASPxButton button = RenderHelper.CreateASPxButton();
			button.ID = id;
			button.AutoPostBack = false;
			button.CssClass = "xafLookupButton";
			ASPxImageHelper.SetImageProperties(button.Image, imageName, 16, 16);
			if(button.Image.IsEmpty) {
				button.Text = string.IsNullOrEmpty(caption) ? id : caption;
			}
			button.ToolTip = caption;
			return button;
		}
		private static ASPxUploadControl CreateUploadControl() {
			ASPxUploadControl uploadControl = new ASPxUploadControl();
			uploadControl.ID = "UPC";
			uploadControl.ShowProgressPanel = true;
			uploadControl.AutoStartUpload = true;
			return uploadControl;
		}
		private static void SetBackColor(ASPxUploadControl uploadControl, Color color) {
			uploadControl.BrowseButtonStyle.BackColor = color;
			uploadControl.TextBoxStyle.BackColor = color;
		}

		private string GetEditButtonClickScript() {
			return $@"function(s, e) {{
    var uploadControl = ASPx.GetControlCollection().Get('{uploadControl.ClientID}');
    if(uploadControl) {{
        uploadControl.SetVisible(!uploadControl.GetVisible());
    }}
}}";
		}
		private string GetFileUploadCompleteScript() {
			return $@"function(s, e) {{
    if(e.isValid && e.callbackData) {{
        if(xaf.ConfirmUnsavedChangedController) {{
            xaf.ConfirmUnsavedChangedController.EditorValueChanged(s, e);
        }}
        s.SetVisible(false);
        var fileAnchorControl = ASPx.GetControlCollection().Get('{fileDownloadLink.ClientID}');
        if(fileAnchorControl) {{
            fileAnchorControl.SetVisible(true);
            fileAnchorControl.SetText(e.callbackData);
        }}
        var changeButton = ASPx.GetControlCollection().Get('{editButton.ClientID}');
        if(changeButton) {{ changeButton.SetVisible(true); }}
        var clearButton = ASPx.GetControlCollection().Get('{clearButton.ClientID}');
        if(clearButton) {{ clearButton.SetVisible(true); }}
        var viewButton = ASPx.GetControlCollection().Get('{fileViewLink.ClientID}');
        if(viewButton) {{ viewButton.SetVisible(false); }}
    }}
}}";
		}

		protected override void OnInit(EventArgs e) {
			CallbackManager.RegisterHandler(UniqueID, this);
			CreateControls();
			AddTableRow(this, fileDownloadLink, editButton, clearButton, fileViewLink);
			AddTableRow(this, uploadControl);
			SetupClientSideEvents();
			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e) {
			isPrerendered = true;
			UpdateControls();
            base.OnPreRender(e);
		}
		protected override void Render(HtmlTextWriter writer) {
			if(!isPrerendered) {
				OnPreRender(EventArgs.Empty);
			}
			base.Render(writer);
		}

        private IFileDataITTI OnCreateCustomFileDataObject() {
			CreateCustomFileDataObjectEventArgs fileDataArgs = new CreateCustomFileDataObjectEventArgs();
            CreateCustomFileDataObject?.Invoke(this, fileDataArgs);
            return fileDataArgs.FileData;
		}
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "postDataImmediately")]
		public FileDataEditIttiControl(ViewEditMode viewEditMode, IFileDataITTI fileData, bool readOnly, bool postDataImmediately) {
            FileData = fileData;
			ReadOnly = readOnly || viewEditMode == ViewEditMode.View;
			CellPadding = 0;
			CellSpacing = 0;
		}

		private XafCallbackManager CallbackManager {
			get {
				Page page = Page;
				if(page == null) {
					return null;
				}
				Guard.TypeArgumentIs(typeof(ICallbackManagerHolder), page.GetType(), "Page");
				return ((ICallbackManagerHolder)page).CallbackManager;
			}
		}

		internal IFileDataITTI FileData { get; set; }

        private string parametroVisor;

        public string ParametroVisor
        {
            get => parametroVisor;
            set
            {
                parametroVisor = value;
                if (!fileViewLink.JSProperties.ContainsKey("cpOid"))
                {
                    fileViewLink?.JSProperties.Add("cpOid", value);
                }
                else
                {
                    fileViewLink.JSProperties["cpOid"] = value;
                }
                
            }
        }

        public override Color BackColor {
			get => base.BackColor;
            set {
				base.BackColor = value;
				if(UploadControl != null) {
					SetBackColor(uploadControl, value);
				}
			}
		}
		public bool ReadOnly { get; set; }
		public string NullText { get; set; }
		public ASPxUploadControl UploadControl => uploadControl;

        public ASPxButton ClearButton => clearButton;
        public event EventHandler<EventArgs> UploadControlCreated;
		public event EventHandler<FileDataLoadingEventArgs> FileDataLoading;
		public event EventHandler<CreateCustomFileDataObjectEventArgs> CreateCustomFileDataObject;
		#region IXafCallbackHandler Members
		public void ProcessAction(string parameter) {
			if(parameter == ClearCommandId) {
				FileData.Clear();
			}
		}
		#endregion
	}
	public class CreateCustomFileDataObjectEventArgs : EventArgs {
		public IFileDataITTI FileData { get; set; }
	}
	public class FileDataLoadingEventArgs : CancelEventArgs {
		public FileDataLoadingEventArgs(FileUploadCompleteEventArgs fileUploadCompleteEventArgs) {
			Guard.ArgumentNotNull(fileUploadCompleteEventArgs, "fileUploadCompleteEventArgs");
			FileUploadCompleteEventArgs = fileUploadCompleteEventArgs;
		}
		public string CallbackData {
			get => FileUploadCompleteEventArgs.CallbackData;
            set => FileUploadCompleteEventArgs.CallbackData = value;
        }
		public string ErrorText {
			get => FileUploadCompleteEventArgs.ErrorText;
            set => FileUploadCompleteEventArgs.ErrorText = value;
        }
		public bool IsValid {
			get => FileUploadCompleteEventArgs.IsValid;
            set => FileUploadCompleteEventArgs.IsValid = value;
        }
		public UploadedFile UploadedFile => FileUploadCompleteEventArgs.UploadedFile;

        [EditorBrowsable(EditorBrowsableState.Never)] 
		public FileUploadCompleteEventArgs FileUploadCompleteEventArgs { get; private set; }

        
	}
}
