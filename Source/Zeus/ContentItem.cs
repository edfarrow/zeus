using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using Ext.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Ormongo.Ancestry;
using Zeus.Integrity;
using Zeus.Linq;
using Zeus.Persistence;
using Zeus.Security;
using Zeus.Web;

namespace Zeus
{
    [RestrictParents(typeof(ContentItem))]
    [Serializable]
	[BsonDiscriminator(RootClass = true)]
    public /*abstract*/ class ContentItem : OrderedAncestryDocument<ContentItem>, IUrlParserDependency, INode
	{
		#region Private Fields

		private List<AuthorizationRule> _authorizationRules;
        private string _name;
        private DateTime? _expires;
        private string _url;

        private IUrlParser _urlParser;

        #endregion

        #region Public Properties (persisted)

        /// <summary>Gets or sets the item's title. This is used in edit mode and probably in a custom implementation.</summary>
        public virtual string Title { get; set; }

        /// <summary>Gets or sets the item's name. This is used to compute the item's url and can be used to uniquely identify the item among other items on the same level.</summary>
        public virtual string Name
        {
            get
            {
                return _name ?? (!IsNewRecord ? ID.ToString() : null);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _name = null;
                else
                    _name = value;
                _url = null;
            }
        }

        /// <summary>Gets or sets when this item was initially created.</summary>
        public virtual DateTime Created { get; set; }

        /// <summary>Gets or sets the date this item was updated.</summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>Gets or sets the publish date of this item.</summary>
        public virtual DateTime? Published { get; set; }

        /// <summary>Gets or sets the expiration date of this item.</summary>
        public virtual DateTime? Expires
        {
            get { return _expires; }
            set { _expires = value != DateTime.MinValue ? value : null; }
        }

        /// <summary>Gets or sets whether this item is visible. This is normally used to control it's visibility in the site map provider.</summary>
		public virtual bool Visible { get; set; }

        /// <summary>Gets or sets the name of the identity who saved this item.</summary>
        public virtual string SavedBy { get; set; }

		/// <summary>
		/// Data store for arbitrary key/value pairs
		/// </summary>
		public Dictionary<string, object> ExtraData { get; set; }

        #endregion

        #region Public Properties (generated)

        /// <summary>The default file extension for this content item, e.g. ".aspx".</summary>
        public virtual string Extension
        {
            get { return BaseLibrary.Web.Url.DefaultExtension; }
        }

        /// <summary>Gets whether this item is a page. This is used for site map purposes.</summary>
        public virtual bool IsPage
        {
            get { return true; }
        }

        /// <summary>Needs to be overridden and set to true for the code needed to match a Custom Url to kick in</summary>
        public virtual bool HasCustomUrl
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the public url to this item. This is computed by walking the 
        /// parent path and prepending their names to the url.
        /// </summary>
        public virtual string Url
        {
            get
            {
                if (_url == null)
                {
                	if (_urlParser != null)
                		return _urlParser.BuildUrl(this);
                	else
                		_url = FindPath(PathData.DefaultAction).RewrittenUrl;
                }
                return _url;
            }
        }

        public string HierarchicalTitle
        {
            get
            {
                string result = this.Title;
                if (Parent != null)
                    result = Parent.HierarchicalTitle + " - " + result;
                return result;
            }
        }

        /// <summary>Gets the icon of this item. This can be used to distinguish item types in edit mode.</summary>
        public virtual string IconUrl
        {
            get { return Utility.GetCooliteIconUrl(Icon); }
        }

        protected virtual Icon Icon
        {
            get { return (IsPage) ? Icon.Page : Icon.PageWhite; }
        }

        /// <summary>The logical path to the node from the root node.</summary>
        public string Path
        {
            get
            {
                string path = "/";
                ContentItem startingParent = Parent;
                if (startingParent != null)
                    path += Name;
                for (ContentItem item = startingParent; item != null && item.Parent != null; item = item.Parent)
                    path = "/" + item.Name + path;
                return path;
            }
        }

        #endregion

        /// <summary>Gets an array of roles allowed to read this item. Null or empty list is interpreted as this item has no access restrictions (anyone may read).</summary>
        public virtual List<AuthorizationRule> AuthorizationRules
        {
            get { return _authorizationRules ?? (_authorizationRules = new List<AuthorizationRule>()); }
        	set { _authorizationRules = value; }
        }

        #region this[]

        /// <summary>Used primarily by editors to provide untyped access to this item's properties. If this class
        /// does not contain the specified property, it falls back to the ExtraData dictionary.</summary>
        /// <param name="detailName">The name of the propery or detail.</param>
        /// <returns>The value of the property. If no property exists, null is returned.</returns>
        public virtual object this[string detailName]
        {
            get
            {
				if (detailName == null)
					throw new ArgumentNullException("detailName");

				// If we have a class property matching this name, get the property value.
				// TODO: Cache this reflection
				var propertyInfo = GetType().GetProperty(detailName);
				if (propertyInfo != null && propertyInfo.CanRead)
					return propertyInfo.GetValue(this, null);

				if (ExtraData.ContainsKey(detailName))
					return ExtraData[detailName];

				return null;
            }
            set
            {
				if (string.IsNullOrEmpty(detailName))
					throw new ArgumentNullException("detailName");

				// If we have a class property matching this name, set the property value.
				// TODO: Cache this reflection
				var propertyInfo = GetType().GetProperty(detailName);
            	if (propertyInfo != null && propertyInfo.CanWrite)
            		propertyInfo.SetValue(this, value, null);
            	else
            		ExtraData[detailName] = value;

            	if (string.IsNullOrEmpty(detailName))
                    throw new ArgumentNullException("detailName", "Parameter 'detailName' cannot be null or empty.");
            }
        }

        #endregion

        protected ContentItem()
        {
        	ExtraData = new Dictionary<string, object>();
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Published = DateTime.Now;
            Visible = true;
        }

        #region Methods

		/// <summary>Creats a copy of this item including details, authorization rules, while resetting ID.</summary>
		/// <returns>The cloned item with or without cloned child items.</returns>
        public virtual ContentItem Clone()
		{
			var bsonDocument = this.ToBsonDocument();
			var cloned = (ContentItem) BsonSerializer.Deserialize(bsonDocument, GetType());
			foreach (ContentItem child in Children)
			{
				ContentItem clonedChild = child.Clone();
				clonedChild.Parent = cloned;
			}
			cloned.ID = ObjectId.Empty;
			cloned._url = null;
			return cloned;
        }

        /// <summary>
        /// Gets child items that the user is allowed to access.
        /// It doesn't have to return the same collection as
        /// the Children property.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ContentItem> GetChildren()
        {
            return Children.Authorized(HttpContext.Current.User, Context.SecurityManager, Operations.Read);
        }

        public virtual IEnumerable<T> GetChildren<T>()
        {
			return Children.OfType<T>();
        }

        /// <summary>Finds children based on the given url segments. The method supports convering the last segments into action and parameter.</summary>
        /// <param name="remainingUrl">The remaining url segments.</param>
        /// <returns>A path data object which can be empty (check using data.IsEmpty()).</returns>
        public virtual PathData FindPath(string remainingUrl)
        {
            if (remainingUrl == null)
                return GetTemplate(string.Empty);

            remainingUrl = remainingUrl.TrimStart('/');

            if (remainingUrl.Length == 0)
                return GetTemplate(string.Empty);

            int slashIndex = remainingUrl.IndexOf('/');
            string nameSegment = slashIndex < 0 ? remainingUrl : remainingUrl.Substring(0, slashIndex);
            foreach (ContentItem child in Children)
            {
                if (child.Equals(nameSegment))
                {
                    remainingUrl = slashIndex < 0 ? null : remainingUrl.Substring(slashIndex + 1);
                    return child.FindPath(remainingUrl);
                }
            }

            return GetTemplate(remainingUrl);
        }

        private PathData GetTemplate(string remainingUrl)
        {
            IPathFinder[] finders = PathDictionary.GetFinders(GetType());

            foreach (IPathFinder finder in finders)
            {
                PathData data = finder.GetPath(this, remainingUrl);
                if (data != null)
                    return data;
            }

            return PathData.Empty;
        }

        /// <summary>
        /// Checks whether this content item contains any properties or property collections.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty()
        {
			// TODO: Check if anything is broken if this only checks Title
			return string.IsNullOrEmpty(Title); //&& !Details.Any() && !DetailCollections.Any();
        }

        /// <summary>
        /// Tries to get a child item with a given name. This method ignores
        /// user permissions and any trailing '.aspx' that might be part of
        /// the name.
        /// </summary>
        /// <param name="childName">The name of the child item to get.</param>
        /// <returns>The child item if it is found otherwise null.</returns>
        /// <remarks>If the method is passed an empty or null string it will return itself.</remarks>
		public virtual ContentItem GetChild(string childName)
        {
        	if (string.IsNullOrEmpty(childName))
        		return null;

        	int slashIndex = childName.IndexOf('/');
        	if (slashIndex == 0) // starts with slash
        	{
        		if (childName.Length == 1)
        			return this;
        		return GetChild(childName.Substring(1));
        	}

        	if (slashIndex > 0) // contains a slash further down
        	{
        		string nameSegment = childName.Substring(0, slashIndex);
        		foreach (ContentItem child in Children)
        			if (child.Equals(nameSegment))
        				return child.GetChild(childName.Substring(slashIndex));
        		return null;
        	}

        	// no slash, only a name
			foreach (ContentItem child in Children)
        		if (child.Equals(childName))
					return child;
        	return null;
        }

        protected virtual bool Equals(string name)
        {
            if (Name == null)
                return false;
            return Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                || HttpUtility.UrlDecode(Name).Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>Gets wether a certain user is authorized to view this item.</summary>
        /// <param name="user">The user to check.</param>
        /// <param name="operation"></param>
        /// <returns>True if the item is open for all or the user has the required permissions.</returns>
        public virtual bool IsAuthorized(IPrincipal user, string operation)
        {
            if (AuthorizationRules == null || AuthorizationRules.Count == 0)
                return true;

            // Iterate rules to find a rule that matches
            foreach (AuthorizationRule auth in AuthorizationRules)
                if (auth.IsAuthorized(user, operation))
                    return true;

            return false;
        }

        public virtual bool IsPublished()
        {
            return (Published != null && Published.Value <= DateTime.Now)
                && !(Expires != null && Expires.Value < DateTime.Now);
        }

        /// <summary>
        /// Return something other than null to group items in the admin site tree.
        /// </summary>
        /// <returns></returns>
        public virtual string FolderPlacementGroup
        {
            get { return null; }
        }

        #endregion

        void IUrlParserDependency.SetUrlParser(IUrlParser parser)
        {
            _urlParser = parser;
        }

        #region INode Members

        string INode.PreviewUrl
        {
            get
            {
                if (IsPage)
                    return Url;
            	throw new NotSupportedException();
            }
        }

        string INode.ClassNames
        {
            get
            {
                StringBuilder className = new StringBuilder();

                if (!Published.HasValue || Published > DateTime.Now)
                    className.Append("unpublished ");
                else if (Published > DateTime.Now.AddDays(-1))
                    className.Append("day ");
                else if (Published > DateTime.Now.AddDays(-7))
                    className.Append("week ");
                else if (Published > DateTime.Now.AddMonths(-1))
                    className.Append("month ");

                if (Expires.HasValue && Expires <= DateTime.Now)
                    className.Append("expired ");

                if (!Visible)
                    className.Append("invisible ");

                if (AuthorizationRules != null && AuthorizationRules.Count > 0)
                    className.Append("locked ");

                return className.ToString();
            }
        }

        #endregion

        #region ILink Members

        string ILink.Contents
        {
            get { return Title; }
        }

        string ILink.ToolTip
        {
            get { return string.Empty; }
        }

        string ILink.Target
        {
            get { return string.Empty; }
        }

        #endregion

		public ContentItem CopyTo(ContentItem destination)
		{
			if (!OnBeforeCopy(destination))
				throw new ZeusException("Could not copy item");

			ContentItem cloned = Clone();
			cloned.Parent = destination;
			cloned.Save();

			OnAfterCopy(destination);

			return cloned;
		}

		#region Callbacks

		protected override bool OnBeforeSave()
		{
			Updated = DateTime.Now;
			return base.OnBeforeSave();
		}

		protected virtual bool OnBeforeCopy(ContentItem newParent)
		{
			return ExecuteCancellableObservers<IContentItemObserver>(o => o.BeforeCopy(this, newParent));
		}

		protected virtual void OnAfterCopy(ContentItem newParent)
		{
			ExecuteObservers<IContentItemObserver>(o => o.AfterCopy(this, newParent));
		}

		#endregion
	}
}
