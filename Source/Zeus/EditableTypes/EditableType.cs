using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Zeus.Editors.Attributes;

namespace Zeus.EditableTypes
{
	public class EditableType
	{
		public IList<EditorContainerAttribute> EditorContainers { get; set; }

		public IList<IEditor> Editors { get; internal set; }

		public IEditorContainer RootContainer { get; set; }

		public IList<IEditorContainer> Containers { get; internal set; }

		public Type ItemType { get; set; }

		public EditableType(Type itemType)
		{
			ItemType = itemType;
		}

		/// <summary>Gets editable attributes available to user.</summary>
		/// <returns>A filtered list of editable fields.</returns>
		public IList<IEditor> GetEditors(IPrincipal user)
		{
			return Editors.Where(e => e.IsAuthorized(user)).ToList();
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
	}
}