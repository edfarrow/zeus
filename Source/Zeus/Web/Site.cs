using MongoDB.Bson;
using Zeus.Configuration;

namespace Zeus.Web
{
	/// <summary>
	/// Represents a site to the Zeus engine. A site defines a start page, a root
	/// page and a host url.
	/// </summary>
	public class Site
	{
		private readonly HostNameCollection _hostNames;

		#region Constructor

		public Site(ObjectId rootItemID, ObjectId startPageID, HostNameCollection hostNames)
		{
			RootItemID = rootItemID;
			StartPageID = startPageID;
			_hostNames = hostNames;
		}

		#endregion

		#region Properties

		/// <summary>Matches hosts that ends with the site's authority, e.g. match both www.zeus.com and zeus.com.</summary>
		public bool Wildcards { get; set; }

		public ObjectId StartPageID { get; set; }
		public ObjectId RootItemID { get; set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return base.ToString() + " #" + StartPageID;
		}

		public override bool Equals(object obj)
		{
			if (obj is Site)
			{
				Site other = obj as Site;
				return other.RootItemID == RootItemID
					&& other.StartPageID == StartPageID;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return RootItemID.GetHashCode() + StartPageID.GetHashCode();
			}
		}

		public string GetHostName()
		{
			foreach (HostNameElement element in _hostNames)
			{
				if (element.Name == "*")
					return null;
				return element.Name;
			}
			return null;
		}

		#endregion
	}
}