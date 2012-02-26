using System.Linq;
using Zeus.Web.Security.Items;
using Zeus.Security.ContentTypes;

namespace Zeus.Persistence
{
	public abstract class GenericFind<TRoot, TStart>
		where TRoot : ContentItem
		where TStart : ContentItem
	{
		/// <summary>Gets the site's root items.</summary>
		public static TRoot RootItem
		{
			//get { return (TStart) Context.Current.UrlParser.RootItem; }
			get { return (TRoot) ContentItem.Find(Context.Current.Host.CurrentSite.RootItemID); }
		}

		/// <summary>Gets the current start page (this may vary depending on host url).</summary>
		public static TStart StartPage
		{
			get { return (TStart) Context.Current.UrlParser.StartPage; }
		}

        public static UserContainer UserContainer()
        {
            return RootItem.GetChildren<SystemNode>().Single().GetChildren<SecurityContainer>().Single().GetChildren<UserContainer>().Single();
        }
	}
}