using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Zeus.ContentProperties;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.ContentTypes
{
	public class ContentType : IComparable<ContentType>, ITypeDefinition
	{
		#region Fields

		private readonly IList<ContentType> _allowedChildren = new List<ContentType>();

		#endregion

		#region Properties

		/// <summary>Gets or sets additional child types allowed below this item.</summary>
		public IList<ContentType> AllowedChildren
		{
			get { return _allowedChildren; }
		}

		public string Discriminator
		{
			get { return ContentTypeAttribute.Name ?? ItemType.Name; }
		}

		public int SortOrder
		{
			get { return ContentTypeAttribute.SortOrder; }
		}

		public IList<EditorContainerAttribute> EditorContainers { get; set; }

		public Type ItemType { get; set; }

		public string IconUrl
		{
			get { return ((ContentItem) Activator.CreateInstance(ItemType)).IconUrl; }
		}

		public ContentTypeAttribute ContentTypeAttribute { get; set; }

		/// <summary>Definitions which are not enabled are not available when creating new items.</summary>
		public bool Enabled { get; set; }

        public bool IsPage { get; set; }

        public bool IgnoreSEOAssets
        {
            get
            {
                if (Activator.CreateInstance(ItemType) is PageContentItem)
                    return ((PageContentItem)Activator.CreateInstance(ItemType)).UseProgrammableSEOAssets;
                else
                    return false;
            }
        }
        public string IgnoreSEOExplanation {
            get
            {
                if (Activator.CreateInstance(ItemType) is PageContentItem)
                    return ((PageContentItem)Activator.CreateInstance(ItemType)).UseProgrammableSEOAssetsExplanation;
                else
                    return string.Empty;
            }
        }

		/// <summary>Gets or sets whether this content type has been defined. Weirdly enough a content type
		/// may exist without being defined. To define a content type the class must implement 
		/// the <see cref="ContentType"/> attribute.</summary>
		public bool IsDefined { get; internal set; }

		/// <summary>Gets roles or users allowed to edit items defined by this content type.</summary>
		public IList<string> AuthorizedRoles { get; internal set; }

		public IEnumerable<IContentProperty> Properties { get; internal set; }

		public IList<IEditor> Editors { get; internal set; }

		public IEnumerable<IDisplayer> Displayers { get; internal set; }

		public IEditorContainer RootContainer { get; set; }

		public IList<IEditorContainer> Containers { get; internal set; }

		/// <summary>Gets the name used when presenting this item class to editors.</summary>
		public string Title
		{
			get { return ContentTypeAttribute.Title; }
		}

		public bool Translatable { get; set; }

		public AdminSiteTreeVisibility Visibility { get; set; }

		#endregion

		#region Constructor

		public ContentType(Type itemType)
		{
			Enabled = true;
			ItemType = itemType;
			ContentTypeAttribute = new ContentTypeAttribute { Title = itemType.Name, Name = itemType.Name };
			Visibility = AdminSiteTreeVisibility.Visible;
		}

		#endregion

		#region Methods

		public void AddProperty(IContentProperty property)
		{
			var properties = Properties.ToList();
			properties.Add(property);
			Properties = properties;
		}

		public IContentProperty GetProperty(string name)
		{
			return Properties.SingleOrDefault(p => p.Name == name);
		}

		public IContentProperty GetProperty(string name, object value)
		{
			IContentProperty property = GetProperty(name);
			if (property == null)
			{
				// Create a property, based on the type of value.
				if (value == null)
					throw new ArgumentNullException("value", "Cannot create a property for name '" + name + "' if 'value' is null");
				return Context.Current.Resolve<IContentPropertyManager>().CreateProperty(name, value.GetType());
			}
			return property;
		}

		public IDisplayer GetDisplayer(string propertyName)
		{
			IDisplayer displayer = Displayers.SingleOrDefault(d => d.Name == propertyName);
			if (displayer != null)
				return displayer;

			IContentProperty property = Properties.SingleOrDefault(p => p.Name == propertyName);
			if (property != null)
				return property.GetDefaultDisplayer();

			return null;
		}

		/// <summary>Gets editable attributes available to user.</summary>
		/// <returns>A filtered list of editable fields.</returns>
		public IList<IEditor> GetEditors(IPrincipal user)
		{
			return Editors.Where(e => e.IsAuthorized(user)).ToList();
		}

		/// <summary>Adds an allowed child definition to the list of allowed definitions.</summary>
		/// <param name="definition">The allowed child definition to add.</param>
		public void AddAllowedChild(ContentType definition)
		{
			if (!AllowedChildren.Contains(definition))
				AllowedChildren.Add(definition);
		}

		public bool IsAuthorized(IPrincipal user)
		{
			if (user == null || AuthorizedRoles == null)
				return true;
			foreach (string role in AuthorizedRoles)
				if (string.Equals(user.Identity.Name, role, StringComparison.OrdinalIgnoreCase) || user.IsInRole(role))
					return true;
			return false;
		}

		/// <summary>Find out if this item allows sub-items of a certain type.</summary>
		/// <param name="child">The item that should be checked whether it is allowed below this item.</param>
		/// <returns>True if the specified child item is allowed below this item.</returns>
		public bool IsChildAllowed(ContentType child)
		{
			return AllowedChildren.Contains(child);
		}

		/// <summary>
		/// Adds an containable editor or container to existing editors and to a container.
		/// </summary>
		/// <param name="containable">The editable to add.</param>
		public void Add(IContainable containable)
		{
			if (string.IsNullOrEmpty(containable.ContainerName))
			{
				RootContainer.AddContained(containable);
				AddToCollection(containable);
			}
			else
			{
				foreach (IEditorContainer container in Containers)
				{
					if (container.Name == containable.ContainerName)
					{
						container.AddContained(containable);
						AddToCollection(containable);
						return;
					}
				}
				throw new ZeusException(
					"The editor '{0}' references a container '{1}' which doesn't seem to be defined on '{2}'. Either add a container with this name or remove the reference to that container.",
					containable.Name, containable.ContainerName, ItemType);
			}
		}

		private void AddToCollection(IContainable containable)
		{
			if (containable is IEditor)
				Editors.Add(containable as IEditor);
			else if (containable is IEditorContainer)
				Containers.Add(containable as IEditorContainer);
		}

		public void ReplaceEditor(string name, IEditor newEditor)
		{
			IEditor editor = Editors.SingleOrDefault(e => e.Name == name);
			if (editor == null)
				return;

			newEditor.Name = editor.Name;
			newEditor.SortOrder = editor.SortOrder;

			// TODO: Remove this fudge
			newEditor.PropertyType = editor.PropertyType;

			List<IEditor> newEditors = new List<IEditor>(Editors);
			newEditors.Remove(editor);
			newEditors.Add(newEditor);
			newEditors.Sort();
			Editors = newEditors;

			IEditorContainer container = Containers.SingleOrDefault(c => c.Contained.Contains(editor)) ?? RootContainer;
			container.Contained.Remove(editor);
			container.Contained.Add(newEditor);
			container.Contained.Sort();
		}

		/// <summary>Removes an allowed child definition from the list of allowed definitions if not already removed.</summary>
		/// <param name="definition">The definition to remove.</param>
		public void RemoveAllowedChild(ContentType definition)
		{
			if (AllowedChildren.Contains(definition))
				AllowedChildren.Remove(definition);
		}

		#region IComparable

		int IComparable<ContentType>.CompareTo(ContentType other)
		{
			return SortOrder - other.SortOrder;
		}

		#endregion

		#endregion
	}
}