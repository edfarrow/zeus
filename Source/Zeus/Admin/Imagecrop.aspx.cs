using System;
using System.IO;
using MongoDB.Bson;
using Ormongo;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.FileSystem.Images;

namespace Zeus.Admin
{
    public partial class Imagecrop : System.Web.UI.Page
	{
        public EmbeddedCroppedImage ImageToEdit;
        public string selectedForForm;
        public double aspectRatio;
        public int minWidth;
        public int minHeight;        

		protected void Page_Load(object sender, EventArgs e)
		{
            selectedForForm = Request.QueryString["selected"];
			int fixedWidthValue = Convert.ToInt32(Request.QueryString["w"]);
			int fixedHeightValue = Convert.ToInt32(Request.QueryString["h"]);

			//ltlAdminName.Text = ((AdminSection) ConfigurationManager.GetSection("zeus/admin")).Name;
            if (Request.Form["id"] == null)
            {
                ObjectId id = ObjectId.Parse(Request.QueryString["id"]);
            	string name = Request.QueryString["name"];
            	ContentItem item = ContentItem.Find(id);
				ImageToEdit = (EmbeddedCroppedImage) item[name];
                bFixedAspectRatio = fixedWidthValue > 0 && fixedHeightValue > 0;
                if (bFixedAspectRatio)
                    aspectRatio = (double)fixedWidthValue / (double)fixedHeightValue;
                
                //need to set the min and max sizes...this will stop people upscaling their images

				System.Drawing.Image image = System.Drawing.Image.FromStream(ImageToEdit.Data.Content);
                int ActualWidth = image.Width;
                int ActualHeight = image.Height;
                image.Dispose();

                if ((800 / ActualWidth) > (600 / ActualHeight))
                {
                    //resized, leaving width @ 800
                    double percChange = (double)800 / (double)ActualWidth;
                    minWidth = Convert.ToInt32(Math.Round(percChange * fixedWidthValue, 0));
                    minHeight = Convert.ToInt32(Math.Round(percChange * fixedHeightValue, 0));
                }
                else
                {
                    //resized, leaving height @ 600
                    double percChange = (double)600 / (double)ActualHeight;
                    minWidth = Convert.ToInt32(Math.Round(percChange * fixedWidthValue, 0));
                    minHeight = Convert.ToInt32(Math.Round(percChange * fixedHeightValue, 0));
                }

                //ImageToEdit.GetUrl(800, 600, true, DynamicImageFormat.Jpeg, true);
            }
            else
            {
                ObjectId id = ObjectId.Parse(Request.Form["id"]);
				string name = Request.Form["name"];
				int x1 = Convert.ToInt32(Request.Form["x1"]);
                int y1 = Convert.ToInt32(Request.Form["y1"]);
                int w = Convert.ToInt32(Request.Form["w"]);
                int h = Convert.ToInt32(Request.Form["h"]);
                string selected = Request.Form["selected"];

				ContentItem item = ContentItem.Find(id);
				ImageToEdit = (EmbeddedCroppedImage)item[name];

                System.Drawing.Image image = System.Drawing.Image.FromStream(ImageToEdit.Data.Content);
                int ActualWidth = image.Width;
                int ActualHeight = image.Height;
                image.Dispose();

                //we know that for display purposes before cropping the image was resized to 800 x 600, so do some calcs...

                if (ActualWidth <= 800 && ActualHeight <= 600)
                {
                    //no resizing happened
                }
                else if ((800 / ActualWidth) > (600 / ActualHeight))
                {
                    //resized, leaving width @ 800
                    double percChange = (double)ActualWidth / (double)800;
                    x1 = Convert.ToInt32(Math.Round(percChange * x1, 0));
                    y1 = Convert.ToInt32(Math.Round(percChange * y1, 0));
                    w = Convert.ToInt32(Math.Round(percChange * w, 0));
                    h = Convert.ToInt32(Math.Round(percChange * h, 0));
                }
                else
                {
                    //resized, leaving height @ 600
                    double percChange = (double)ActualHeight / (double)600;
                    x1 = Convert.ToInt32(Math.Round(percChange * x1, 0));
                    y1 = Convert.ToInt32(Math.Round(percChange * y1, 0));
                    w = Convert.ToInt32(Math.Round(percChange * w, 0));
                    h = Convert.ToInt32(Math.Round(percChange * h, 0));
                }

                ImageToEdit.TopLeftXVal = x1;
                ImageToEdit.TopLeftYVal = y1;
                ImageToEdit.CropWidth = w;
                ImageToEdit.CropHeight = h;
            	item.Save();

                Response.Redirect("/admin/plugins.edit-item.default.aspx?selected=" + selected);
            }
		}

        protected bool bFixedAspectRatio { get; set; }
 
		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();
            Page.ClientScript.RegisterJavascriptResource(typeof(Imagecrop), "Zeus.Admin.Assets.JS.jcrop_jquery.js");
			
            //Page.ClientScript.RegisterCssResource(typeof(Login), "Zeus.Admin.Assets.Css.jcrop.demos.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.reset.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.login.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.jcrop_demos.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.jcrop_jquery.css");
            
			base.OnPreRender(e);
		}
         
	}
}
