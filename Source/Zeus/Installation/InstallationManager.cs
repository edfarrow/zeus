using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using MongoDB.Bson;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Persistence;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Installation
{
	/// <summary>
	/// Wraps functionality to request database status and generate Zeus's 
	/// database schema on multiple database flavours.
	/// </summary>
	public class InstallationManager
	{
		#region Fields

		private readonly IContentTypeManager _contentTypeManager;
		//private readonly Importer _importer;
		private readonly IFinder _finder;
		private readonly ICredentialService _credentialService;
		private readonly IHost _host;
		private readonly AdminSection _adminConfig;

		#endregion

		#region Constructor

		public InstallationManager(IHost host, IContentTypeManager contentTypeManager, /*Importer importer,*/ 
			IFinder finder, ICredentialService credentialService, AdminSection adminConfig)
		{
			_host = host;
			_contentTypeManager = contentTypeManager;
			//_importer = importer;
			_finder = finder;
			_credentialService = credentialService;
			_adminConfig = adminConfig;
		}

		#endregion

		#region Methods

		public DatabaseStatus GetStatus()
		{
			DatabaseStatus status = new DatabaseStatus();

			UpdateConnection(status);
			UpdateSchema(status);
			UpdateItems(status);

			return status;
		}

		public void CreateAdministratorUser(string username, string password)
		{
			UserCreateStatus createStatus;
			_credentialService.CreateUser(username, password, string.Empty, new[] { _adminConfig.AdministratorRole },
				true, out createStatus);
			if (createStatus != UserCreateStatus.Success)
				throw new ZeusException("Could not create user: " + createStatus);
		}

		public string CreateDatabase(string server, string name)
		{
			Server dbServer = new Server(server);
			Database db = new Database(dbServer, name);
			db.Create();

			return string.Format(@"Server={0};Database={1};Integrated Security=True", server, name);
		}

		private void UpdateItems(DatabaseStatus status)
		{
			try
			{
				status.StartPageID = _host.CurrentSite.StartPageID;
				status.RootItemID = _host.CurrentSite.RootItemID;
				status.StartPage = ContentItem.Find(status.StartPageID);
				status.RootItem = ContentItem.Find(status.RootItemID);
				status.IsInstalled = status.RootItem != null && status.StartPage != null;

				status.HasUsers = _credentialService.GetUser("administrator") != null;
			}
			catch (Exception ex)
			{
				status.IsInstalled = false;
				status.ItemsError = ex.Message;
			}
		}

		private void UpdateSchema(DatabaseStatus status)
		{
			try
			{
				status.Items = _finder.QueryItems().Count();
				//status.Details = _finder.QueryDetails().Count();
				//status.DetailCollections = _finder.QueryDetailCollections().Count();
				//status.AuthorizedRoles = _finder.Query<AuthorizationRule>().Count();
				status.HasSchema = true;
			}
			catch (Exception ex)
			{
				status.HasSchema = false;
				status.SchemaError = ex.Message;
				status.SchemaException = ex;
			}
		}

		private void UpdateConnection(DatabaseStatus status)
		{
			try
			{
				using (IDbConnection conn = GetConnection())
				{
					conn.Open();
					conn.Close();
					status.ConnectionType = conn.GetType().Name;
				}
				status.IsConnected = true;
				status.ConnectionError = null;
			}
			catch (Exception ex)
			{
				status.IsConnected = false;
				status.ConnectionError = ex.Message;
				status.ConnectionException = ex;
			}
		}

		/// <summary>Method that will checks the database. If something goes wrong an exception is thrown.</summary>
		/// <returns>A string with diagnostic information about the database.</returns>
		public string CheckDatabase()
		{
			int itemCount = _finder.QueryItems().Count();
			//int detailCount = _finder.QueryDetails().Count();
			//int detailCollectionCount = _finder.QueryDetailCollections().Count();
			//int authorizationRuleCount = _finder.Query<AuthorizationRule>().Count();

			//return string.Format("Database OK, items: {0}, details: {1}, authorization rules: {2}, detail collections: {3}",
			//                                         itemCount, detailCount, authorizationRuleCount, detailCollectionCount);
			return string.Format("Database OK, items: {0}", itemCount);
		}

		/// <summary>Checks the root node in the database. Throws an exception if there is something really wrong with it.</summary>
		/// <returns>A diagnostic string about the root node.</returns>
		public string CheckRootItem()
		{
			ObjectId rootID = _host.CurrentSite.RootItemID;
			ContentItem rootItem = ContentItem.Find(rootID);
			if (rootItem != null)
				return string.Format("Root node OK, id: {0}, name: {1}, type: {2}, discriminator: {3}, published: {4} - {5}",
					rootItem.ID, rootItem.Name, rootItem.GetType(),
					_contentTypeManager.GetContentType(rootItem.GetType()), rootItem.Published, rootItem.Expires);
			return "No root item found with the id: " + rootID;
		}

		/// <summary>Checks the root node in the database. Throws an exception if there is something really wrong with it.</summary>
		/// <returns>A diagnostic string about the root node.</returns>
		public string CheckStartPage()
		{
			ObjectId startID = _host.CurrentSite.StartPageID;
			ContentItem startPage = ContentItem.Find(startID);
			if (startPage != null)
				return string.Format("Start page OK, id: {0}, name: {1}, type: {2}, discriminator: {3}, published: {4} - {5}",
					startPage.ID, startPage.Name, startPage.GetType(),
					_contentTypeManager.GetContentType(startPage.GetType()), startPage.Published, startPage.Expires);
			return "No start page found with the id: " + startID;
		}

		public ContentItem InsertRootNode(Type type, string name, string title)
		{
			ContentItem item = _contentTypeManager.CreateInstance(type, null);
			item.Name = name;
			item.Title = title;
			item.Save();
			return item;
		}

		public ContentItem InsertStartPage(Type type, ContentItem root, string name, string title)
		{
			ContentItem item = _contentTypeManager.CreateInstance(type, root);
			item.Name = name;
			item.Title = title;
			item.Save();
			return item;
		}

		#region Helper Methods

		public string GetConnectionString()
		{
			return ConfigurationManager.ConnectionStrings[GetConnectionStringName()].ConnectionString;
		}

		public string GetConnectionStringName()
		{
			throw new NotImplementedException();
			//DatabaseSection configSection = ConfigurationManager.GetSection("zeus/database") as DatabaseSection;
			//if (configSection == null)
			//    throw new ZeusException("Missing <zeus/database> configuration section");
			//ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[configSection.ConnectionStringName];
			//if (connectionString == null)
			//    throw new ZeusException("Missing connection string '" + configSection.ConnectionStringName + "'");
			//return configSection.ConnectionStringName;
		}

		public IDbConnection GetConnection()
		{
			return new SqlConnection(GetConnectionString());
		}

		/*public IDbConnection GetConnection()
		{
			IDriver driver = GetDriver();

			IDbConnection conn = driver.CreateConnection();
			if (Cfg.Properties.ContainsKey(Environment.ConnectionString))
				conn.ConnectionString = (string) Cfg.Properties[Environment.ConnectionString];
			else if (Cfg.Properties.ContainsKey(Environment.ConnectionStringName))
				conn.ConnectionString = ConfigurationManager.ConnectionStrings[(string) Cfg.Properties[Environment.ConnectionStringName]].ConnectionString;
			else
				throw new Exception("Didn't find a confgiured connection string or connection string name in the nhibernate configuration.");
			return conn;
		}

		public IDbCommand GenerateCommand(CommandType type, string sqlString)
		{
			IDriver driver = GetDriver();
			return driver.GenerateCommand(type, new NHibernate.SqlCommand.SqlString(sqlString), new SqlType[0]);
		}

		private IDriver GetDriver()
		{
			string driverName = (string) Cfg.Properties[Environment.ConnectionDriver];
			Type driverType = NHibernate.Util.ReflectHelper.ClassForName(driverName);
			return (IDriver) Activator.CreateInstance(driverType);
		}*/

		#endregion

		public ContentItem InsertExportFile(Stream stream, string filename)
		{
			throw new NotImplementedException();
			//IImportRecord record = _importer.Read(stream, filename);
			//_importer.Import(record, null, ImportOptions.AllItems);

			//return record.RootItem;
		}

		#endregion
	}
}
