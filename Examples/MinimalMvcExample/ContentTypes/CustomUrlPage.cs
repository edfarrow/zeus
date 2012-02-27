using System.Collections.Generic;
using MongoDB.Bson;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.Editors.Attributes;
using Zeus.Examples.MinimalMvcExample.Design.Editors;
using Zeus.Examples.MinimalMvcExample.Enums;
using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
    [ContentType("Custom Url Page")]
    [RestrictParents(typeof(CustomUrlContainer))]
    [Panel("Images", "Images", 340)]
    [AllowedChildren(typeof(Page))]
	public class CustomUrlPage : BasePage
	{
        public override string Url
        {
			get { return Parent.Url + "/" + "moomooBabyBoo" + "/" + Title.ToSafeUrl(); }
        }

        public override bool HasCustomUrl
        {
			get { return true; }
        }

		[PageListBoxEditor("Selection Of Pages", 110)]
		public List<ObjectId> PagesSelection { get; set; }

        /// <summary>
        /// Banner image
        /// </summary>
		[EmbeddedCroppedImageEditor("Banner", 200, FixedWidthValue = 400, FixedHeightValue = 200)]
		public virtual EmbeddedCroppedImage Banner { get; set; }

		[EmbeddedItemEditor("MyPage", 210)]
		public virtual MyLittleType MyPage { get; set; }

		[EnumEditor("Vegetable", 300, typeof(Vegetable))]
		public virtual Vegetable Vegetable { get; set; }

		[HtmlTextBoxEditor("Tiny MCE Content", 350)]
		public virtual string TinyMCEContent { get; set; }

        public override bool UseProgrammableSEOAssets
        {
			get { return true; }
        }

        public override string UseProgrammableSEOAssetsExplanation
        {
			get { return "no SEO assets cos you suck balls!"; }
        }

        public override string ProgrammableHtmlTitle
        {
			get { return "moo"; }
        }

        public override string ProgrammableMetaDescription
        {
			get { return "This is a programmatical desc"; }
        }
	}
}
