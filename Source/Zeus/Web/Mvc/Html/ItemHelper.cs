using System.Web.Mvc;
using Zeus.Engine;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public abstract class ItemHelper
	{
		public HtmlHelper HtmlHelper { get; set; }
		private readonly IContentItemContainer _itemContainer;

		protected ItemHelper(HtmlHelper htmlHelper, IContentItemContainer itemContainer)
		{
			HtmlHelper = htmlHelper;
			_itemContainer = itemContainer;
			CurrentItem = itemContainer.CurrentItem;
		}

		protected ItemHelper(HtmlHelper htmlHelper, IContentItemContainer itemContainer, ContentItem item)
		{
			_itemContainer = itemContainer;
			HtmlHelper = htmlHelper;
			CurrentItem = item;
		}

		protected virtual ContentEngine Engine
		{
			get { return Context.Current; }
		}

		protected IContentItemContainer Container
		{
			get { return _itemContainer; }
		}

		protected ContentItem CurrentItem { get; private set; }
	}
}