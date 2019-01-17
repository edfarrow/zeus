using NHibernate;
using System.Threading;
using Zeus.Web;

namespace Zeus.Persistence.NH
{
	public class SessionProvider : ISessionProvider
	{
		private const string RequestItemsKey = "SessionProvider.Session";
		private readonly INotifyingInterceptor _interceptor;
		private readonly IWebContext _webContext;

		private NHibernate.Cfg.Configuration _configuration { get; set; }

		private static ISessionFactory _sessionFactory;
		private static readonly object _sessionFactoryLock = new object();

		public SessionProvider(IConfigurationBuilder configurationBuilder, INotifyingInterceptor interceptor, IWebContext webContext)
		{
			//HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

			_interceptor = interceptor;
			_webContext = webContext;

			_configuration = configurationBuilder.Configuration;
		}

		private SessionContext CurrentSession
		{
			get { return _webContext.RequestItems[RequestItemsKey] as SessionContext; }
			set { _webContext.RequestItems[RequestItemsKey] = value; }
		}

		public virtual SessionContext OpenSession
		{
			get
			{
				SessionContext sc = CurrentSession;
				if (sc == null)
				{
					if (_sessionFactory == null)
					{
						lock (_sessionFactoryLock)
						{
							_sessionFactory = _configuration.BuildSessionFactory();
						}
					}
					ISession s = _sessionFactory.OpenSession(_interceptor);
					s.FlushMode = FlushMode.Commit;
					CurrentSession = sc = new SessionContext(this, s);
				}
				return sc;
			}
		}

		public void Dispose()
		{
			SessionContext sc = CurrentSession;

			if (sc != null)
			{
				sc.Session.Dispose();
				CurrentSession = null;
			}
		}
	}
}
