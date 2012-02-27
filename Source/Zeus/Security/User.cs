using System;
using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.Editors.Attributes;
using Zeus.Integrity;
using Zeus.Web.Security.Details;
using Zeus.Web.Security.Items;

namespace Zeus.Security
{
	[ContentType]
	[RestrictParents(typeof(UserContainer))]
	public class User : DataContentItem
	{
		public override string Title
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public string Identifier
		{
			get { return Name; }
		}

		[TextBoxEditor("Username", 20, Required = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public string Username
		{
			get { return Name; }
			set { Name = value; }
		}

		[PasswordEditor("Change Password", 30, Description = "Passwords are encrypted and cannot be viewed, but they can be changed.")]
		public virtual string Password { get; set; }

		[TextBoxEditor("Email", 40)]
		public virtual string Email { get; set; }

		private List<Role> _rolesInternal;

		[RolesEditor(Title = "Roles", SortOrder = 50)]
		public virtual List<Role> RolesInternal
		{
			get { return _rolesInternal; }
			set { _rolesInternal = value; }
		}

		public string[] Roles
		{
			get { return RolesInternal.Select(r => r.Name).ToArray(); }
		}

		[TextBoxEditor("Nonce", 141)]
		public virtual string Nonce { get; set; }

		[CheckBoxEditor("Verified", "", 142)]
		public bool Verified { get; set; }

		private DateTime? _lastLoginDate;

		[DateEditor("Last Login Date", 160)]
		public DateTime? LastLoginDate
		{
			get { return _lastLoginDate ?? Published; }
			set { _lastLoginDate = value; }
		}

		private DateTime? _lastActivityDate;

		[DateEditor("Last Activity Date", 162)]
		public DateTime? LastActivityDate
		{
			get { return _lastActivityDate ?? Published; }
			set { _lastActivityDate = value; }
		}

		private DateTime? _lastPasswordChangedDate;

		[DateEditor("Last Password Changed Date", 164)]
		public DateTime? LastPasswordChangedDate
		{
			get { return _lastPasswordChangedDate ?? Published; }
			set { _lastPasswordChangedDate = value; }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.User); }
		}

        public override string FolderPlacementGroup
        {
            get
            {
                if (Roles.Count() > 1)
                    return "Multiple Roles";
                else if (Roles.Count() ==1)
                    return Roles.First();
                else
                    return "No Roles Defined";

            }
        }

		public User()
		{
			_rolesInternal = new List<Role>();
			Verified = true;
		}
	}
}
