﻿using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis.Web.UI;
using Zeus.Admin;

namespace Zeus.Web.UI.WebControls
{
	public class FileDataEditor : HtmlContainerControl
	{
		#region Fields

		private HtmlGenericControl _beforeUpload, _duringUpload, _afterUpload;
		private HiddenField _hiddenFileNameField, _hiddenIdentifierField;
		private HtmlAnchor _beforeUploadAnchor;
		private string _currentFileName;

		#endregion

		#region Properties

		public override string TagName
		{
			get { return "div"; }
		}

		public FileUploadImplementation FileUploadImplementation { get; set; }

		public string CurrentFileName
		{
			set
			{
				_currentFileName = value;
				EnsureChildControls();
			}
		}

		public bool Enabled
		{
			get { return (bool) (ViewState["Enabled"] ?? true); }
			set { ViewState["Enabled"] = value; }
		}

		public string FileName
		{
			get
			{
				EnsureChildControls();
				return _hiddenFileNameField.Value;
			}
		}

		public string Identifier
		{
			get
			{
				EnsureChildControls();
				return _hiddenIdentifierField.Value;
			}
		}

		public bool HasNewOrChangedFile
		{
			get { return (!string.IsNullOrEmpty(FileName)); }
		}

		public string BeforeUploadText
		{
			get { return (!string.IsNullOrEmpty(_currentFileName)) ? "Change" : "Upload file"; }
		}

		#endregion

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (Page.IsPostBack)
				EnsureChildControls();
		}

		protected override void CreateChildControls()
		{
			_hiddenFileNameField = new HiddenField { ID = ID + "hdnFileName" };
			Controls.Add(_hiddenFileNameField);

			_hiddenIdentifierField = new HiddenField { ID = ID + "hdnIdentifier" };
			Controls.Add(_hiddenIdentifierField);

			_beforeUpload = new HtmlGenericControl("div") { ID = ID + "beforeUpload" };
			if (FileUploadImplementation.RendersSelf)
				_beforeUpload.Style["float"] = "left";
			Controls.Add(_beforeUpload);

			if (!string.IsNullOrEmpty(_currentFileName))
				_beforeUpload.Controls.Add(new LiteralControl(_currentFileName + "&nbsp;"));

			_beforeUploadAnchor = new HtmlAnchor { Disabled = !Enabled };
			_beforeUpload.Controls.Add(_beforeUploadAnchor);
			_beforeUploadAnchor.HRef = "#";
			_beforeUploadAnchor.InnerText = BeforeUploadText;
			_beforeUploadAnchor.Visible = !FileUploadImplementation.RendersSelf;

			_duringUpload = new HtmlGenericControl("div") { ID = ID + "duringUpload", InnerText = "Uploading..." };
			Controls.Add(_duringUpload);
			_duringUpload.Style[HtmlTextWriterStyle.Display] = "none";
			if (FileUploadImplementation.RendersSelf)
				_duringUpload.Style["float"] = "left";

			_afterUpload = new HtmlGenericControl("div") { ID = ID + "afterUpload", InnerText = "Finished Upload", Disabled = !Enabled };
			if (FileUploadImplementation.RendersSelf)
				_afterUpload.Style["float"] = "left";
			Controls.Add(_afterUpload);

			FileUploadImplementation.AddChildControls();

			/*
				<!-- shown during upload -->
				<div id="duringUpload">Uploading MyFile.pdf: <span id="percentage">0</span>%</div>

				<!-- shown after upload -->
				<div id="afterUpload">Uploaded: MyFile.pdf (<a href="#">remove</a>)</div>
			 * */
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (!string.IsNullOrEmpty(_hiddenFileNameField.Value))
				_beforeUpload.Style[HtmlTextWriterStyle.Display] = "none";

			if (string.IsNullOrEmpty(_hiddenFileNameField.Value))
				_afterUpload.Style[HtmlTextWriterStyle.Display] = "none";
			else
			{
				_afterUpload.Style[HtmlTextWriterStyle.Display] = string.Empty;
				_afterUpload.InnerText = _hiddenFileNameField.Value;
			}

			if (Enabled)
				_beforeUploadAnchor.Attributes["onclick"] = FileUploadImplementation.StartUploadJavascriptFunction + "; return false;";
			_beforeUploadAnchor.Disabled = !Enabled;

			ScriptManager.RegisterClientScriptResource(this, typeof(FileDataEditor), "Zeus.Web.Resources.FileDataEditor.FileDataEditor.js");

			string script = string.Format(@"$('#{0}').fileDataEditor(
				{{
					hiddenFilenameField : '#{1}',
					hiddenIdentifierField : '#{2}',
					beforeUploadDiv : '#{3}',
					duringUploadDiv : '#{4}',
					afterUploadDiv : '#{5}'
				}});", ClientID,
						 _hiddenFileNameField.ClientID,
						 _hiddenIdentifierField.ClientID,
						 _beforeUpload.ClientID,
						 _duringUpload.ClientID,
						 _afterUpload.ClientID);
			ScriptManager.RegisterStartupScript(this, GetType(), ClientID, script, true);

			FileUploadImplementation.Initialize();

			base.OnPreRender(e);
		}

		#endregion
	}
}
