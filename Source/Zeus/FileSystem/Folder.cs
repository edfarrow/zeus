﻿using Ext.Net;
using Zeus.Editors.Attributes;
using Zeus.Integrity;

namespace Zeus.FileSystem
{
	[ContentType("File Folder", "Folder", "A node that stores files and other folders.", "", 600)]
	[RestrictParents(typeof(IFileSystemContainer), typeof(Folder))]
	public class Folder : FileSystemNode
	{
		public Folder()
		{
			Position = int.MaxValue;
			Visible = false;
		}

		protected override Icon Icon
		{
			get { return Icon.Folder; }
		}

		[TextBoxEditor("Name", 10)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public override string Title
		{
			get { return base.Name; }
			set { base.Title = value; }
		}
	}
}
