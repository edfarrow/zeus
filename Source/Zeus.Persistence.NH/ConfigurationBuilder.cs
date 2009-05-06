﻿using System;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using Zeus.ContentTypes;
using System.Text;
using System.Collections.Generic;
using Zeus.Configuration;
using FluentNHibernate.Cfg;
using Zeus.Web;

namespace Zeus.Persistence.NH
{
	public class ConfigurationBuilder : IConfigurationBuilder
	{
		private readonly IContentTypeManager _definitions;
		private readonly NHibernate.Cfg.Configuration _configuration;
		private const string _mappingFormat = @"<?xml version=""1.0"" encoding=""utf-16""?>
<hibernate-mapping xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:nhibernate-mapping-2.2""
	auto-import=""false"">
{0}
</hibernate-mapping>
";
		private const string _classFormat = @"<subclass name=""{0}"" extends=""{1}"" discriminator-value=""{2}"" lazy=""false""/>";

		public NHibernate.Cfg.Configuration Configuration
		{
			get { return _configuration; }
		}

		public ConfigurationBuilder(IContentTypeManager definitions, DatabaseSection databaseSectionConfig)
		{
			_definitions = definitions;

			if (databaseSectionConfig == null)
				databaseSectionConfig = new DatabaseSection();

			var configuration = MsSqlConfiguration.MsSql2005
				//.ConnectionString(c => c.FromConnectionStringWithKey(databaseSectionConfig.ConnectionStringName))
				.Cache(c => c.ProviderClass(databaseSectionConfig.CacheProviderClass));

			if (databaseSectionConfig.CacheEnabled)
				configuration = configuration.Cache(c => c.UseQueryCache());

			IDictionary<string, string> properties = configuration.ToProperties();
			properties["cache.use_second_level_cache"] = databaseSectionConfig.CacheEnabled.ToString();
			properties["connection.connection_string_name"] = databaseSectionConfig.ConnectionStringName;

			ZeusPersistenceModel persistenceModel = new ZeusPersistenceModel();
			persistenceModel.Conventions.DefaultLazyLoad = false;

			_configuration = new NHibernate.Cfg.Configuration().AddProperties(properties);

			AddMapping(_configuration, "Zeus.Persistence.NH.Mappings.Default.hbm.xml, Zeus.Persistence.NH");
			//_configuration.AddAssembly(Assembly.GetExecutingAssembly());

			//persistenceModel.Configure(_configuration);

			//persistenceModel.WriteMappingsTo(@"C:\mappings");

			// For each definition, add a <subclass> element to mapping file.
			StringBuilder mappings = new StringBuilder();
			foreach (Type type in EnumerateDefinedTypes())
				mappings.AppendFormat(_classFormat, GetName(type), GetName(type.BaseType), GetDiscriminator(type));
			_configuration.AddXml(string.Format(_mappingFormat, mappings));
		}

		/// <summary>Adds mappings to the configuration.</summary>
		/// <param name="cfg">The configuration to add the mappings to.</param>
		protected virtual void AddMapping(NHibernate.Cfg.Configuration cfg, string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				string[] pathAssemblyPair = name.Split(',');
				if (pathAssemblyPair.Length != 2) throw new ArgumentException("Expected the property DefaultMapping to be in the format [manifest resource path],[assembly name] but was: " + name);

				Assembly a = Assembly.Load(pathAssemblyPair[1]);
				cfg.AddInputStream(a.GetManifestResourceStream(pathAssemblyPair[0]));
			}
		}

		private IEnumerable<Type> EnumerateDefinedTypes()
		{
			List<Type> types = new List<Type>();
			foreach (ContentType definition in _definitions.GetContentTypes())
					foreach (Type t in EnumerateBaseTypes(definition.ItemType))
					{
						if (t.IsSubclassOf(typeof(ContentItem)) && !types.Contains(t))
						{
							int index = types.IndexOf(t.BaseType);
							types.Insert(index + 1, t);
						}
					}
			return types;
		}

		/// <summary>Enumerates base type chain of the supplied type.</summary>
		/// <param name="t">The type whose base types will be enumerated.</param>
		/// <returns>An enumerator of the supplied item and all it's base types.</returns>
		private static IEnumerable<Type> EnumerateBaseTypes(Type t)
		{
			if (t != null)
			{
				foreach (Type baseType in EnumerateBaseTypes(t.BaseType))
					yield return baseType;
				yield return t;
			}
		}

		private static string GetName(Type t)
		{
			return t.FullName + ", " + t.Assembly.FullName.Split(',')[0];
		}

		private string GetDiscriminator(Type itemType)
		{
			ContentType definition = _definitions[itemType];
			if (definition != null)
				return definition.Discriminator;
			return itemType.Name;
		}
	}
}