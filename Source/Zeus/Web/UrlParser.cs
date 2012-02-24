using System;
using System.IO;
using System.Web;
using MongoDB.Bson;
using Ninject;
using Ormongo;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace Zeus.Web
{
    public class UrlParser : Observer<ContentItem>, IUrlParser, IStartable
    {
        #region Fields

        private readonly IHost _host;
        protected readonly IWebContext _webContext;
        private readonly bool _ignoreExistingFiles;
        private readonly CustomUrlsSection _configUrlsSection;

        #endregion

        #region Constructors

        public UrlParser(IHost host, IWebContext webContext, HostSection config, CustomUrlsSection urls)
        {
            _host = host;
            _webContext = webContext;

            _ignoreExistingFiles = config.Web.IgnoreExistingFiles;

            DefaultDocument = "default";

            _configUrlsSection = urls;
        }

        #endregion

        #region Events

        public event EventHandler<PageNotFoundEventArgs> PageNotFound;

        #endregion

        #region Properties

        protected IHost Host
        {
            get { return _host; }
        }

        /// <summary>Gets or sets the default content document name. This is usually "/default.aspx".</summary>
        public string DefaultDocument { get; set; }

        /// <summary>Gets the current start page.</summary>
        public virtual ContentItem StartPage
        {
            get { return ContentItem.Find(_host.CurrentSite.StartPageID); }
        }

        public ContentItem CurrentPage
        {
            get { return _webContext.CurrentPage ?? (_webContext.CurrentPage = (ResolvePath(_webContext.Url).CurrentItem)); }
        }

        #endregion

        #region Methods

        public virtual string BuildUrl(ContentItem item)
        {
            ContentItem startPage;
            return BuildUrlInternal(item, out startPage);
        }

        protected string BuildUrlInternal(ContentItem item, out ContentItem startPage)
        {
            startPage = null;
            ContentItem current = item;
            Url url = new Url("/");

            // Walk the item's parent items to compute it's url
            do
            {
                if (IsStartPage(current))
                {
                    startPage = current;

                    // we've reached the start page so we're done here
                    return VirtualPathUtility.ToAbsolute("~" + url.PathAndQuery);
                }

                url = url.PrependSegment(current.Name, current.Extension);

                current = current.Parent;
            } while (current != null);

            // If we didn't find the start page, it means the specified
            // item is not part of the current site.
            return "/?" + PathData.PageQueryKey + "=" + item.ID;
            //return item.FindPath(PathData.DefaultAction).RewrittenUrl;
        }

        protected virtual bool IsStartPage(ContentItem item)
        {
            return IsStartPage(item, _host.CurrentSite);
        }

        protected static bool IsStartPage(ContentItem item, Site site)
        {
            return item.ID == site.StartPageID;
        }

        /// <summary>Handles virtual directories and non-page items.</summary>
        /// <param name="url">The relative url.</param>
        /// <param name="item">The item whose url is supplied.</param>
        /// <returns>The absolute url to the item.</returns>
        protected virtual string ToAbsolute(string url, ContentItem item)
        {
            if (string.IsNullOrEmpty(url) || url == "/")
                url = _webContext.ToAbsolute("~/");
            else
                url = _webContext.ToAbsolute("~" + url + item.Extension);

            return url;
        }

        /// <summary>Checks if an item is startpage or root page</summary>
        /// <param name="item">The item to compare</param>
        /// <returns>True if the item is a startpage or a rootpage</returns>
        public virtual bool IsRootOrStartPage(ContentItem item)
        {
            return item.ID == _host.CurrentSite.RootItemID || IsStartPage(item);
        }

		public override void AfterInitialize(ContentItem document)
		{
			((IUrlParserDependency) document).SetUrlParser(this);
		}

        private ObjectId? FindQueryStringReference(string url, params string[] parameters)
        {
            string queryString = Url.QueryPart(url);
            if (!string.IsNullOrEmpty(queryString))
            {
                string[] queries = queryString.Split('&');

                foreach (string parameter in parameters)
                {
                    int parameterLength = parameter.Length + 1;
                    foreach (string query in queries)
                    {
                        if (query.StartsWith(parameter + "=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            ObjectId id;
                            if (ObjectId.TryParse(query.Substring(parameterLength), out id))
                            {
                                return id;
                            }
                        }
                    }
                }
            }
            return null;
        }

        protected virtual ContentItem TryLoadingFromQueryString(string url, params string[] parameters)
        {
            ObjectId? itemID = FindQueryStringReference(url, parameters);
            if (itemID.HasValue)
				return ContentItem.Find(itemID.Value);
            return null;
        }

        protected virtual ContentItem Parse(ContentItem current, string url)
        {
            if (current == null) throw new ArgumentNullException("current");

            url = CleanUrl(url);

            if (url.Length == 0)
                return current;

            return current.GetChild(url) ?? NotFoundPage(url);
        }

        /// <summary>May be overridden to provide custom start page depending on authority.</summary>
        /// <param name="url">The host name and path information.</param>
        /// <returns>The configured start page.</returns>
        protected virtual ContentItem GetStartPage(Url url)
        {
            return StartPage;
        }

        /// <summary>Finds an item by traversing names from the start page.</summary>
        /// <param name="url">The url that should be traversed.</param>
        /// <returns>The content item matching the supplied url.</returns>
        public virtual ContentItem Parse(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");

            ContentItem startingPoint = GetStartPage(url);

            return TryLoadingFromQueryString(url, PathData.ItemQueryKey, PathData.PageQueryKey) ?? Parse(startingPoint, url);
        }

        private string CleanUrl(string url)
        {
            url = Url.PathPart(url);
            url = _webContext.ToAppRelative(url);
            url = url.TrimStart('~', '/');
            if (url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase))
                url = url.Substring(0, url.Length - ".aspx".Length);
            return url;
        }

        protected virtual ContentItem NotFoundPage(string url)
        {
            if (url.Equals(DefaultDocument, StringComparison.InvariantCultureIgnoreCase))
                return StartPage;

            PageNotFoundEventArgs args = new PageNotFoundEventArgs(url);
            if (PageNotFound != null)
                PageNotFound(this, args);
            return args.AffectedItem;
        }

        private bool isFile(string path, bool includeImages)
        {
            path = path.ToLower();
            if (path.EndsWith(".css"))
                return true;
            else if (path.EndsWith(".gif") && includeImages)
                return true;
            else if (path.EndsWith(".png") && includeImages)
                return true;
            else if (path.EndsWith(".jpg") && includeImages)
                return true;
            else if (path.EndsWith(".jpeg") && includeImages)
                return true;
            else if (path.EndsWith(".js"))
                return true;
            else if (path.EndsWith(".axd"))
                return true;
            else if (path.EndsWith(".ashx"))
                return true;
            else if (path.EndsWith(".ico"))
                return true;
            else if (path.EndsWith(".css"))
                return true;
            else if (path.EndsWith(".swf"))
                return true;

            return false;
        }

        private void LogIt(string what)
        {
            // create a writer and open the file
            TextWriter tw = new StreamWriter("c:\\sites\\zeus\\debugger.txt", true);

            // write a line of text to the file
            tw.WriteLine(System.DateTime.Now + " // " + what);

            // close the stream
            tw.Close();
        }/*
                        LogIt("In the cache all section : session says " + 
                            (System.Web.HttpContext.Current.Application["customUrlCacheActivated"] == null ? "No setting" : "Setting Found") +
                            " : requestedUrl.Path = " + requestedUrl.Path +
                            " : _webContext.Url.Path = " + _webContext.Url.Path +
                            " : isFile? = " + isFile(_webContext.Url.Path));
                         */

        public PathData ResolvePath(string url)
        {
            Url requestedUrl = url;

            //look for files etc and ignore
            bool bNeedsProcessing = true;
            if (requestedUrl.Path.ToLower().StartsWith("/assets/"))
                bNeedsProcessing = false;
            else if (!requestedUrl.Path.StartsWith("/"))
                bNeedsProcessing = false;
            else if (isFile(requestedUrl.Path, false))
                bNeedsProcessing = false;

            if (!bNeedsProcessing)
            {
                PathData data = new PathData();
                data.IsRewritable = false;
                return data;
            }
            else
            {
                ContentItem item = TryLoadingFromQueryString(requestedUrl, PathData.PageQueryKey);
                if (item != null)
                    return item.FindPath(requestedUrl["action"] ?? PathData.DefaultAction)
                            .SetArguments(requestedUrl["arguments"])
                            .UpdateParameters(requestedUrl.GetQueries());

                ContentItem startPage = GetStartPage(requestedUrl);
                string path = Url.ToRelative(requestedUrl.Path).TrimStart('~');
                PathData data = startPage.FindPath(path).UpdateParameters(requestedUrl.GetQueries());

                if (data.IsEmpty())
                {
                    if (path.EndsWith(DefaultDocument, StringComparison.OrdinalIgnoreCase))
                    {
                        // Try to find path without default document.
                        data = StartPage
                                .FindPath(path.Substring(0, path.Length - DefaultDocument.Length))
                                .UpdateParameters(requestedUrl.GetQueries());
                    }

                    //cache data first time we go through this
                    if ((_configUrlsSection.ParentIDs.Count > 0) && (System.Web.HttpContext.Current.Application["customUrlCacheActivated"] == null))
                    {
                        //the below takes resource and time, we only want one request doing this at a time, so set the flag immediately
                        System.Web.HttpContext.Current.Application["customUrlCacheActivated"] = "true";
                        System.Web.HttpContext.Current.Application["customUrlCacheInitialLoopLastRun"] = System.DateTime.Now;

                        foreach (CustomUrlsIDElement id in _configUrlsSection.ParentIDs)
                        {
                            // First check that the page referenced in web.config actually exists.
							ContentItem customUrlPage = ContentItem.Find(id.ID);
                            if (customUrlPage == null)
                                continue;

                            //need to check all children of these nodes to see if there's a match
                            IEnumerable<ContentItem> AllContentItemsWithCustomUrls = Find.EnumerateAccessibleChildren(customUrlPage, id.Depth);
                            foreach (ContentItem ci in AllContentItemsWithCustomUrls)
                            {
                                if (ci.HasCustomUrl)
                                {
                                    System.Web.HttpContext.Current.Application["customUrlCache_" + ci.Url] = ci.ID;
                                    System.Web.HttpContext.Current.Application["customUrlCacheAction_" + ci.Url] = "";
                                }
                            }
                        }

                        System.Web.HttpContext.Current.Application["customUrlCacheInitialLoopComplete"] = System.DateTime.Now;
                    }

                    bool bTryCustomUrls = false;
                    {
                        if (!isFile(requestedUrl.Path, true))
                        {
                            foreach (CustomUrlsMandatoryStringsElement stringToFind in _configUrlsSection.MandatoryStrings)
                            {
                                if (_webContext.Url.Path.IndexOf(stringToFind.Value) > -1)
                                {
                                    bTryCustomUrls = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (data.IsEmpty() && requestedUrl.Path.IndexOf(".") == -1 && bTryCustomUrls)
                    {

                        DateTime lastRun = Convert.ToDateTime(System.Web.HttpContext.Current.Application["customUrlCacheInitialLoopLastRun"]);
                        DateTime lastComplete = System.Web.HttpContext.Current.Application["customUrlCacheInitialLoopComplete"] == null ? DateTime.MinValue : Convert.ToDateTime(System.Web.HttpContext.Current.Application["customUrlCacheInitialLoopLastRun"]);

                        //temp measure - sleep until the initial loop is complete
                        int loops = 0;
                        while (lastComplete < lastRun && loops < 20)
                        {
                            loops++;
                            System.Threading.Thread.Sleep(1000);
                            lastComplete = System.Web.HttpContext.Current.Application["customUrlCacheInitialLoopComplete"] == null ? DateTime.MinValue : Convert.ToDateTime(System.Web.HttpContext.Current.Application["customUrlCacheInitialLoopLastRun"]);
                        }

                        //this code can freak out the 2nd level caching in NHibernate, so clear it if within 5 mins of the last time the cache everything loop was called
                        //TO DO: implement this
                        /*
                        if (DateTime.Now.Subtract(lastRun).TotalMinutes < 5)
                        {
                            NHibernate.Cfg.Configuration Configuration = new NHibernate.Cfg.Configuration();
                            ISessionFactory sessionFactory = Configuration.BuildSessionFactory();

                            sessionFactory.EvictQueries();
                            foreach (var collectionMetadata in sessionFactory.GetAllCollectionMetadata())
                                sessionFactory.EvictCollection(collectionMetadata.Key);
                            foreach (var classMetadata in sessionFactory.GetAllClassMetadata())
                                sessionFactory.EvictEntity(classMetadata.Key);
                        }
                        */

                        //check cache for previously mapped item
                        if (System.Web.HttpContext.Current.Application["customUrlCache_" + _webContext.Url.Path] == null)
                        {
                            //HACK!!  Using the RapidCheck elements in config try to pre-empt this being a known path with the action
                            //This needed to be implemented for performance reasons
                            string fullPath = _webContext.Url.Path;
                            string pathNoAction = "";
                            string action = "";
                            if (fullPath.LastIndexOf("/") > -1)
                            {
                                pathNoAction = fullPath.Substring(0, fullPath.LastIndexOf("/"));
                                action = fullPath.Substring(fullPath.LastIndexOf("/") + 1);
                            }

                            foreach (CustomUrlsRapidCheckElement possibleAction in _configUrlsSection.RapidCheck)
                            {
                                if (possibleAction.Action == action)
                                {
                                    //check for cache
                                    //see whether we have the root item in the cache...
                                    if (System.Web.HttpContext.Current.Application["customUrlCache_" + pathNoAction] != null)
                                    {
                                        //we now have a match without any more calls to the database
										ContentItem ci = ContentItem.Find((ObjectId)System.Web.HttpContext.Current.Application["customUrlCache_" + pathNoAction]);
                                        data = ci.FindPath(action);
                                        System.Web.HttpContext.Current.Application["customUrlCache_" + _webContext.Url.Path] = ci.ID;
                                        System.Web.HttpContext.Current.Application["customUrlCacheAction_" + _webContext.Url.Path] = action;
                                        return data;
                                    }
                                }

                            }

                            // Check for Custom Urls (could be done in a service that subscribes to the IUrlParser.PageNotFound event)...
                            foreach (CustomUrlsIDElement id in _configUrlsSection.ParentIDs)
                            {
                                // First check that the page referenced in web.config actually exists.
								ContentItem customUrlPage = ContentItem.Find(id.ID);
                                if (customUrlPage == null)
                                    continue;

                                //need to check all children of these nodes to see if there's a match
                                ContentItem tryMatch =
                                    Find.EnumerateAccessibleChildren(customUrlPage, id.Depth).SingleOrDefault(
                                        ci => ci.Url.Equals(_webContext.Url.Path, StringComparison.InvariantCultureIgnoreCase));

                                if (tryMatch != null)
                                {
                                    data = tryMatch.FindPath(PathData.DefaultAction);
                                    System.Web.HttpContext.Current.Application["customUrlCache_" + _webContext.Url.Path] = tryMatch.ID;
                                    System.Web.HttpContext.Current.Application["customUrlCacheAction_" + _webContext.Url.Path] = "";
                                    break;
                                }
                                //now need to check for an action...
                                if (fullPath.LastIndexOf("/") > -1)
                                {

                                    //see whether we have the root item in the cache...
                                    if (System.Web.HttpContext.Current.Application["customUrlCache_" + pathNoAction] == null)
                                    {
                                        ContentItem tryMatchAgain =
											Find.EnumerateAccessibleChildren(ContentItem.Find(id.ID), id.Depth).SingleOrDefault(
                                                ci => ci.Url.Equals(pathNoAction, StringComparison.InvariantCultureIgnoreCase));

                                        if (tryMatchAgain != null)
                                        {
                                            data = tryMatchAgain.FindPath(action);
                                            System.Web.HttpContext.Current.Application["customUrlCache_" + _webContext.Url.Path] = tryMatchAgain.ID;
                                            System.Web.HttpContext.Current.Application["customUrlCacheAction_" + _webContext.Url.Path] = action;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        ContentItem ci = ContentItem.Find((ObjectId)System.Web.HttpContext.Current.Application["customUrlCache_" + pathNoAction]);
                                        data = ci.FindPath(action);
                                        System.Web.HttpContext.Current.Application["customUrlCache_" + _webContext.Url.Path] = ci.ID;
                                        System.Web.HttpContext.Current.Application["customUrlCacheAction_" + _webContext.Url.Path] = action;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
							ContentItem ci = ContentItem.Find((ObjectId)System.Web.HttpContext.Current.Application["customUrlCache_" + _webContext.Url.Path]);
                            string act = System.Web.HttpContext.Current.Application["customUrlCacheAction_" + _webContext.Url.Path].ToString();
                            if (string.IsNullOrEmpty(act))
                                return ci.FindPath(PathData.DefaultAction);
                            else
                                return ci.FindPath(act);
                        }
                    }

                    if (data.IsEmpty())
                    {
                        // Allow user code to set path through event
                        if (PageNotFound != null)
                        {
                            PageNotFoundEventArgs args = new PageNotFoundEventArgs(requestedUrl);
                            args.AffectedPath = data;
                            PageNotFound(this, args);

                            if (args.AffectedItem != null)
                                data = args.AffectedItem.FindPath(PathData.DefaultAction);
                            else
                                data = args.AffectedPath;
                        }
                    }
                }

                data.IsRewritable = IsRewritable(_webContext.PhysicalPath);
                return data;
            }
        }

        private bool IsRewritable(string path)
        {
            return _ignoreExistingFiles || (!File.Exists(path) && !Directory.Exists(path));
        }

        #endregion

		void IStartable.Start()
		{
			ContentItem.Observers.Add(this);
		}

		void IStartable.Stop()
		{
			ContentItem.Observers.Remove(this);
		}
    }
}