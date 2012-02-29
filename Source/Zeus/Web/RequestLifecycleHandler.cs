using System;
using System.Diagnostics;
using System.Web;
using Ninject;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using Zeus.Engine;
using Zeus.Installation;
using Zeus.Security;

namespace Zeus.Web
{
	/// <summary>
	/// Handles the request life cycle for Zeus by invoking url rewriting, 
	/// authorizing and closing NHibernate session.
	/// </summary>
	public class RequestLifecycleHandler : IRequestLifecycleHandler, IStartable
	{
		readonly IWebContext webContext;
		readonly EventBroker broker;
		readonly InstallationManager installer;
		readonly IRequestDispatcher dispatcher;

		protected bool initialized = false;
		protected bool checkInstallation = false;
		protected string installerUrl = "~/admin/install/default.aspx";
		private readonly AdminSection _adminConfig;
		private readonly ISecurityEnforcer _securityEnforcer;

		/// <summary>Creates a new instance of the RequestLifeCycleHandler class.</summary>
		/// <param name="webContext">The web context wrapper.</param>
		public RequestLifecycleHandler(IWebContext webContext, EventBroker broker, InstallationManager installer, IRequestDispatcher dispatcher, AdminSection editConfig, ISecurityEnforcer securityEnforcer)
		{
			this.webContext = webContext;
			this.broker = broker;
			this.installer = installer;
			this.dispatcher = dispatcher;
			checkInstallation = editConfig.Installer.CheckInstallationStatus;
			//installerUrl = editConfig.Installer.InstallUrl;
			_adminConfig = editConfig;
			_securityEnforcer = securityEnforcer;
		}

		/// <summary>Subscribes to applications events.</summary>
		/// <param name="broker">The application.</param>
		public void Init(EventBroker broker)
		{
			Debug.WriteLine("RequestLifeCycleHandler.Init");

			broker.BeginRequest += Application_BeginRequest;
			broker.AuthorizeRequest += Application_AuthorizeRequest;
			broker.EndRequest += Application_EndRequest;
		}

		protected virtual void Application_BeginRequest(object sender, EventArgs e)
		{
			if (!initialized)
			{
				// we need to have reached begin request before we can do certain 
				// things in IIS7. concurrency isn't crucial here.
				initialized = true;
				if (webContext.IsWeb)
				{
					if (Url.ServerUrl == null)
						Url.ServerUrl = webContext.Url.HostUrl;
					if (checkInstallation)
						CheckInstallation();
				}
			}

			webContext.CurrentPath = dispatcher.ResolvePath();
		}

		private void CheckInstallation()
		{
			bool isEditing = webContext.ToAppRelative(webContext.Url.Path).StartsWith("~/" + _adminConfig.Path, StringComparison.InvariantCultureIgnoreCase);
			if (!isEditing && !installer.GetStatus().IsInstalled)
			{
				webContext.Response.Redirect(installerUrl);
			}
		}

		protected virtual void Application_AuthorizeRequest(object sender, EventArgs e)
		{
			_securityEnforcer.AuthoriseRequest();
		}

		protected virtual void Application_EndRequest(object sender, EventArgs e)
		{
			webContext.Close();
		}

		#region IStartable Members

		public void Start()
		{
			broker.BeginRequest += Application_BeginRequest;
			broker.AuthorizeRequest += Application_AuthorizeRequest;
			broker.EndRequest += Application_EndRequest;
		}

		public void Stop()
		{
			broker.BeginRequest -= Application_BeginRequest;
			broker.AuthorizeRequest -= Application_AuthorizeRequest;
			broker.EndRequest -= Application_EndRequest;
		}

		#endregion
	}
}