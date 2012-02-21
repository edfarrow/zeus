﻿using System;
using System.Text;
using Ext.Net;
using MongoDB.Bson;
using Ormongo;
using Ormongo.Ancestry;
using Zeus.Admin;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Integrity;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Linq;
using Zeus.Linq;
using Zeus.Web;
using Zeus.Security;
using System.Security.Principal;
using Zeus.Web.Hosting;
using System.Threading;

namespace Zeus
{
    [RestrictParents(typeof(ContentItem))]
    [System.Serializable]
    public abstract class ContentItem : OrderedAncestryDocument<ContentItem>, IUrlParserDependency, INode, IEditableObject
	{
		#region Private Fields

		private IList<AuthorizationRule> _authorizationRules;
        private string _name;
        private DateTime? _expires;
        private IDictionary<string, PropertyData> _details = new Dictionary<string, PropertyData>();
        private IDictionary<string, PropertyCollection> _detailCollections = new Dictionary<string, PropertyCollection>();
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

        /// <summary>Gets or sets the sort order of this item.</summary>
        public virtual int SortOrder { get; set; }

        /// <summary>Gets or sets whether this item is visible. This is normally used to control it's visibility in the site map provider.</summary>
		public virtual bool Visible { get; set; }

        /// <summary>Gets or sets the name of the identity who saved this item.</summary>
        public virtual string SavedBy { get; set; }

        /// <summary>Gets or sets the details collection. These are usually accessed using the e.g. item["Detailname"]. This is a place to store content data.</summary>
        public IDictionary<string, PropertyData> Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>Gets or sets the details collection collection. These are details grouped into a collection.</summary>
        public IDictionary<string, PropertyCollection> DetailCollections
        {
            get { return _detailCollections; }
            set { _detailCollections = value; }
        }

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
        public virtual IList<AuthorizationRule> AuthorizationRules
        {
            get
            {
                if (_authorizationRules == null)
                    _authorizationRules = new List<AuthorizationRule>();
                return _authorizationRules;
            }
            set { _authorizationRules = value; }
        }

        #region this[]

        /// <summary>Gets or sets the detail or property with the supplied name. If a property with the supplied name exists this is always returned in favour of any detail that might have the same name.</summary>
        /// <param name="detailName">The name of the propery or detail.</param>
        /// <returns>The value of the property or detail. If now property exists null is returned.</returns>
        public virtual object this[string detailName]
        {
            get
            {
                if (detailName == null)
                    throw new ArgumentNullException("detailName");

                switch (detailName)
                {
                    case "ID":
                        return ID;
                    case "Title":
                        return Title;
                    case "Name":
                        return Name;
                    case "Url":
                        return Url;
                    default:
                        return Utility.Evaluate(this, detailName)
                            ?? GetDetail(detailName)
                            ?? GetDetailCollection(detailName, false);
                }
            }
            set
            {
                if (string.IsNullOrEmpty(detailName))
                    throw new ArgumentNullException("detailName", "Parameter 'detailName' cannot be null or empty.");

                PropertyInfo info = GetType().GetProperty(detailName);
                if (info != null && info.CanWrite)
                {
                    if (value != null && info.PropertyType != value.GetType())
                        value = Utility.Convert(value, info.PropertyType);
                    info.SetValue(this, value, null);
                }
                else if (value is PropertyCollection)
                {
                    DetailCollections[detailName] = (PropertyCollection)value;
                    //throw new ZeusException("Cannot set a detail collection this way, add it to the DetailCollections collection instead.");
                }
                else
                {
                    SetDetail(detailName, value);
                }
            }
        }

        #endregion

        protected ContentItem()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Published = DateTime.Now;
            Visible = true;
        }

        #region GetDetail & SetDetail<T> Methods

        /// <summary>Gets a detail from the details bag.</summary>
        /// <param name="detailName">The name of the value to get.</param>
        /// <returns>The value stored in the details bag or null if no item was found.</returns>
        public virtual object GetDetail(string detailName)
        {
            return Details.ContainsKey(detailName)
                ? Details[detailName].Value
                : null;
        }

        /// <summary>Gets a detail from the details bag.</summary>
        /// <param name="detailName">The name of the value to get.</param>
        /// <param name="defaultValue">The default value to return when no detail is found.</param>
        /// <returns>The value stored in the details bag or null if no item was found.</returns>
        public virtual T GetDetail<T>(string detailName, T defaultValue)
        {
            IDictionary<string, PropertyData> details = Details;

            //try inserted to stop "illegal access" error
            bool? bContains;
            bContains = tryAndWaitIfNecessary(details, detailName);

            //if failed try again
            if (bContains == null)
            {
                bContains = tryAndWaitIfNecessary(details, detailName);
                //if still failed, then another second has already passed so try a final time with no catch
                if (bContains == null)
                    bContains = details.ContainsKey(detailName);
            }

            if (bContains.Value)
                return Utility.Convert<T>(details[detailName].Value);
            return defaultValue;
        }

        private bool? tryAndWaitIfNecessary(IDictionary<string, PropertyData> details, string detailName)
        {
            try
            {
                return details.ContainsKey(detailName);
            }
            catch (System.Exception ex)
            {
                if (ex.Message.ToLower().IndexOf("illegal access to loading collection") > -1)
                {
                    //wait for a second and try again
                    Thread.Sleep(1000);
                }
                else if (ex.Message.ToLower().IndexOf("could not initialize a collection batch") > -1)
                {
                    //wait for a second and try again
                    Thread.Sleep(1000);
                }
                else if (ex.Message.ToLower().IndexOf("was deadlocked on lock resources with another process and has been chosen as the deadlock victim") > -1)
                {
                    //wait for a second and try again
                    Thread.Sleep(1000);
                }
                else
                {
                    throw (ex);
                }
                return null;
            }
        }

        public virtual void SetDetail(string detailName, object value)
        {
            if (string.IsNullOrEmpty(detailName))
                throw new ArgumentNullException("detailName");

            PropertyData detail = Details.ContainsKey(detailName) ? Details[detailName] : null;

            if (detail != null && value != null && value.GetType().IsAssignableFrom(detail.ValueType))
            {
                // update an existing detail
                detail.Value = value;
            }
            else
            {
                if (detail != null)
                    // delete detail or remove detail of wrong type
                    Details.Remove(detailName);
                if (value != null)
                {
                    // add new detail
                    PropertyData propertyData = Context.ContentTypes.GetContentType(GetType()).GetProperty(detailName, value).CreatePropertyData(this, value);
                    Details.Add(detailName, propertyData);
                }
            }
        }

        /// <summary>Set a value into the <see cref="Details"/> bag. If a value with the same name already exists it is overwritten. If the value equals the default value it will be removed from the details bag.</summary>
        /// <param name="detailName">The name of the item to set.</param>
        /// <param name="value">The value to set. If this parameter is null or equal to defaultValue the detail is removed.</param>
        /// <param name="defaultValue">The default value. If the value is equal to this value the detail will be removed.</param>
        protected virtual void SetDetail<T>(string detailName, T value, T defaultValue)
        {
            if (value == null || !value.Equals(defaultValue))
                SetDetail(detailName, value);
            else if (Details.ContainsKey(detailName))
                Details.Remove(detailName);
        }

        /// <summary>Set a value into the <see cref="Details"/> bag. If a value with the same name already exists it is overwritten.</summary>
        /// <param name="detailName">The name of the item to set.</param>
        /// <param name="value">The value to set. If this parameter is null the detail is removed.</param>
        protected virtual void SetDetail<T>(string detailName, T value)
        {
            SetDetail(detailName, (object)value);
        }

        #endregion

        #region GetDetailCollection

        /// <summary>Gets a named detail collection.</summary>
        /// <param name="collectionName">The name of the detail collection to get.</param>
        /// <param name="createWhenEmpty">Wether a new collection should be created if none exists. Setting this to false means null will be returned if no collection exists.</param>
        /// <returns>A new or existing detail collection or null if the createWhenEmpty parameter is false and no collection with the given name exists..</returns>
        public virtual PropertyCollection GetDetailCollection(string collectionName, bool createWhenEmpty)
        {
            if (DetailCollections.ContainsKey(collectionName))
                return DetailCollections[collectionName];
            else if (createWhenEmpty)
            {
                PropertyCollection collection = new PropertyCollection(this, collectionName);
                DetailCollections.Add(collectionName, collection);
                return collection;
            }
            else
                return null;
        }

        #endregion

        #region Methods

        private const int SORT_ORDER_THRESHOLD = 9999;

        /// <summary>Adds an item to the children of this item updating it's parent refernce.</summary>
        /// <param name="newParent">The new parent of the item. If this parameter is null the item is detached from the hierarchical structure.</param>
        public virtual void AddTo(ContentItem newParent)
        {
			// TODO: Don't think this is necessary, but not sure.
			//if (Parent != null && Parent != newParent && Parent.Children.Contains(this))
			//    Parent.Children.Remove(this);

            Parent = newParent;

            if (newParent != null && !newParent.Children.Contains(this))
            {
                var siblings = newParent.Children.ToList();
                if (siblings.Count > 0)
                {
                    int lastOrder = siblings[siblings.Count - 1].SortOrder;

                    for (int i = siblings.Count - 2; i >= 0; i--)
                    {
                        if (siblings[i].SortOrder < lastOrder - SORT_ORDER_THRESHOLD)
                        {
                            siblings.Insert(i + 1, this);
                            return;
                        }
                        lastOrder = siblings[i].SortOrder;
                    }

                    if (lastOrder > SORT_ORDER_THRESHOLD)
                    {
                        siblings.Insert(0, this);
                        return;
                    }
                }

                siblings.Add(this);
            }
        }

		/// <summary>Creats a copy of this item including details, authorization rules, while resetting ID.</summary>
		/// <param name="includeChildren">Specifies whether this item's child items also should be cloned.</param>
		/// <returns>The cloned item with or without cloned child items.</returns>
        public virtual ContentItem Clone(bool includeChildren)
        {
            ContentItem cloned = (ContentItem)MemberwiseClone();
            cloned.ID = ObjectId.Empty;
            cloned._url = null;

            CloneDetails(cloned);
            CloneChildren(includeChildren, cloned);
            CloneAuthorizationRules(cloned);

            return cloned;
        }

        #region Clone Helper Methods

        private void CloneAuthorizationRules(ContentItem cloned)
        {
            if (AuthorizationRules != null)
            {
                cloned.AuthorizationRules = new List<AuthorizationRule>();
                foreach (AuthorizationRule rule in AuthorizationRules)
                {
                    AuthorizationRule clonedRule = rule.Clone();
                    clonedRule.EnclosingItem = cloned;
                    cloned.AuthorizationRules.Add(clonedRule);
                }
            }
        }

        private void CloneChildren(bool includeChildren, ContentItem cloned)
        {
            if (includeChildren)
                foreach (ContentItem child in Children)
                {
                    ContentItem clonedChild = child.Clone(true);
                    clonedChild.AddTo(cloned);
                }
        }

        private void CloneDetails(ContentItem cloned)
        {
            cloned.Details = new Dictionary<string, PropertyData>();
            foreach (var detail in Details.Values)
                cloned[detail.Name] = detail.Value;

            cloned.DetailCollections = new Dictionary<string, PropertyCollection>();
            foreach (PropertyCollection collection in DetailCollections.Values)
            {
                PropertyCollection clonedCollection = collection.Clone();
                clonedCollection.EnclosingItem = cloned;
                cloned.DetailCollections[collection.Name] = clonedCollection;
            }
        }

        #endregion

        public TAncestor FindFirstAncestor<TAncestor>()
            where TAncestor : ContentItem
        {
            return FindFirstAncestorRecursive<TAncestor>(this);
        }

        private static TAncestor FindFirstAncestorRecursive<TAncestor>(ContentItem contentItem)
            where TAncestor : ContentItem
        {
            if (contentItem == null)
                return null;

            if (contentItem is TAncestor)
                return (TAncestor)contentItem;

            return FindFirstAncestorRecursive<TAncestor>(contentItem.Parent);
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
            return string.IsNullOrEmpty(Title) && !Details.Any() && !DetailCollections.Any();
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

    	public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (obj.GetType() != GetType())) return false;
            ContentItem item = obj as ContentItem;
            if (!IsNewRecord && !item.IsNewRecord)
                return ID == item.ID;
            else
                return ReferenceEquals(this, item);
        }

        /// <summary>Gets a hash code based on the item's id.</summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
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
                return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(Context.Current.Resolve<IAdminAssemblyManager>().Assembly, "Zeus.Admin.View.aspx") + "?selected=" + Path;
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
    }
}
