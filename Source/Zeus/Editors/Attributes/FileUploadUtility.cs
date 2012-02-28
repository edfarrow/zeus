using System.Drawing;
using System.IO;
using System.Web;
using Ormongo;
using Zeus.BaseLibrary.Web;
using Zeus.Editors.Controls;
using Zeus.FileSystem;
using Zeus.Web.Handlers;
using File = System.IO.File;

namespace Zeus.Editors.Attributes
{
	internal static class FileUploadUtility
	{
		public static Bitmap GetUploadedImage(FancyImageUpload fileUpload)
		{
			return new Bitmap(GetUploadedFilePath(fileUpload));
		}

		public static void HandleUpload(FancyFileUpload fileUpload, EmbeddedFile embeddedFile)
		{
			// Populate File object.
			string uploadedFile = GetUploadedFilePath(fileUpload);
			using (var fs = new FileStream(uploadedFile, FileMode.Open))
			{
				var bytes = ReadAllBytes(fs);
				fs.Position = 0;
				embeddedFile.Data = Attachment.Create(new Attachment
				{
					Content = fs,
					FileName = fileUpload.FileName,
					ContentType = MimeUtility.GetMimeType(bytes)
				});
			}

			// Delete temp folder.
			File.Delete(uploadedFile);
			Directory.Delete(PostedFileUploadHandler.GetUploadFolder(fileUpload.Identifier));
		}

		private static string GetUploadedFilePath(FancyFileUpload fileUpload)
		{
			string uploadFolder = PostedFileUploadHandler.GetUploadFolder(fileUpload.Identifier);
			return Path.Combine(uploadFolder, HttpUtility.UrlDecode(fileUpload.FileName));
		}

		private static byte[] ReadAllBytes(Stream stream)
		{
			using (var ms = new MemoryStream((int) stream.Length))
			{
				byte[] buffer = new byte[4096];
				int bytesRead;
				while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
				{
					ms.Write(buffer, 0, bytesRead);
				}
				return ms.ToArray();
			}
		}
	}
}