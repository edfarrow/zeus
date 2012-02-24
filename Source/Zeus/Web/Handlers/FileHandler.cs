using System.IO;
using System.Web;
using Zeus.Admin;
using File = Zeus.FileSystem.File;

namespace Zeus.Web.Handlers
{
	public class FileHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			File file = Context.Current.Resolve<Navigator>().Navigate(context.Request.QueryString["Path"]) as File;
			if (file == null)
				return;

			context.Response.ContentType = file.Data.Data.ContentType;

			// TODO: Replace this with an MVC handler.
			// grab chunks of data and write to the output stream 
			const int bufferSize = 0x1000;
			Stream outputStream = context.Response.OutputStream;
			using (file.Data.Data.Content)
			{
				var buffer = new byte[bufferSize];

				while (true)
				{
					int bytesRead = file.Data.Data.Content.Read(buffer, 0, bufferSize);
					if (bytesRead == 0)
					{
						// no more data 
						break;
					}

					outputStream.Write(buffer, 0, bytesRead);
				}
			}

			context.Response.Flush();
		}
	}
}
