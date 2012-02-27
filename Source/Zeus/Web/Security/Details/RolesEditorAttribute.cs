using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;
using Zeus.Editors.Attributes;

namespace Zeus.Web.Security.Details
{
	public class RolesEditorAttribute : AbstractEditorAttribute
	{
		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			CheckBoxList cbl = editor as CheckBoxList;
			List<Items.Role> roles = new List<Items.Role>();
			foreach (ListItem li in cbl.Items)
				if (li.Selected)
				{
					Items.Role role = Context.Current.Resolve<IWebSecurityManager>().GetRole(li.Value);
					roles.Add(role);
				}
			item[Name] = roles;

			return true;
		}

		protected override void UpdateEditorInternal(ContentItem item, Control editor)
		{
			CheckBoxList cbl = editor as CheckBoxList;
			var dc = item[Name] as List<Items.Role>;
			if (dc != null)
			{
				foreach (Items.Role role in dc)
				{
					ListItem li = cbl.Items.FindByValue(role.Name);
					if (li != null)
					{
						li.Selected = true;
					}
					else
					{
						li = new ListItem(role.Name);
						li.Selected = true;
						li.Attributes["style"] = "color:silver";
						cbl.Items.Add(li);
					}
				}
			}
		}

		protected override Control AddEditor(Control container)
		{
			CheckBoxList cbl = new CheckBoxList();
			foreach (Items.Role role in Context.Current.Resolve<IWebSecurityManager>().GetRoles(container.Page.User))
				cbl.Items.Add(role.Name);
			container.Controls.Add(cbl);
			return cbl;
		}
	}
}
