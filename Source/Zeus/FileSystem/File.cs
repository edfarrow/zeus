using Ext.Net;
using Zeus.Editors.Attributes;
using Zeus.Integrity;

namespace Zeus.FileSystem
{
	[ContentType(Description = "A node that represents a file.")]
	[RestrictParents(typeof(Folder))]
	public class File : FileSystemNode
	{
		protected override Icon Icon
		{
			get
			{
				string fileExtension = (!string.IsNullOrEmpty(FileExtension)) ? FileExtension.ToLower() : null;
				switch (fileExtension)
				{
					case ".pdf":
						return Icon.PageWhiteAcrobat;
					case ".zip":
						return Icon.PageWhiteCompressed;
					case ".xls":
					case ".xlsx":
						return Icon.PageWhiteExcel;
					case ".jpg":
					case ".jpeg":
					case ".gif":
					case ".png":
					case ".bmp":
						return Icon.PageWhitePicture;
					case ".ppt":
					case ".pptx":
						return Icon.PageWhitePowerpoint;
					case ".doc":
					case ".docx":
						return Icon.PageWhiteWord;
					default:
						return Icon.PageWhite;
				}
			}
		}

		public string FileExtension
		{
			get { return System.IO.Path.GetExtension(FileName); }
		}

		public override bool IsPage
		{
			get { return true; }
		}

		[EmbeddedFileEditor("File", 100)]
		public virtual EmbeddedFile Data { get; set; }

		public long? Size { get; set; }

		[TextBoxEditor("Caption", 200)]
		public string Caption { get; set; }

		public string FileName { get; set; }

		public override bool IsEmpty()
		{
			return Data == null;
		}
	}
}
