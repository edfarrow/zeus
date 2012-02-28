using System;
using Ext.Net;
using MongoDB.Bson;
using Zeus.Security;

namespace Zeus.Admin.Plugins.MoveItem
{
	[DirectMethodProxyID(Alias = "Move", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class MoveUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void MoveNode(string source, string destination, int pos)
		{
			// TODO: Work out what this does...
			//int destinationID = Convert.ToInt32(destination);
			//if (destinationID < 0)
			//    destinationID = -1 * (destinationID % 100000);

			ContentItem sourceContentItem = ContentItem.Find(ObjectId.Parse(source));
            //get abs value of destination - this sorts out placement folders, which have to have the a negative value of their parent node so that sorting, moving etc can work
			ContentItem destinationContentItem = ContentItem.Find(ObjectId.Parse(destination));

			// Check user has permission to create items under the SelectedItem
			if (!Engine.SecurityManager.IsAuthorized(destinationContentItem, Page.User, Operations.Create))
			{
				ExtNet.MessageBox.Alert("Cannot move item", "You are not authorised to move an item to this location.");
				return;
			}

			sourceContentItem.MoveToPosition(pos);

            //set the updated value on the parent of the item that has been moved (for caching purposes)
            sourceContentItem.Parent.Updated = DateTime.Now;
            sourceContentItem.Parent.Save();

            ContentItem theParent = sourceContentItem.Parent.Parent;
            while (theParent.Parent != null)
            {
                //go up the tree updating - if a child has been changed, so effectively has the parent
                theParent.Save();
                theParent = theParent.Parent;
            }
		}
	}
}