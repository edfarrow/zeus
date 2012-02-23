using System.Collections.Generic;
using Ext.Net;
using MongoDB.Bson;
using Zeus.Security;
using System;

namespace Zeus.Admin.Plugins.MoveItem
{
	[DirectMethodProxyID(Alias = "Move", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class MoveUserControl : PluginUserControlBase
	{
		[DirectMethod]
		public void MoveNode(ObjectId source, ObjectId destination, int pos)
		{
			// TODO: Work out what this does...
			//int destinationID = Convert.ToInt32(destination);
			//if (destinationID < 0)
			//    destinationID = -1 * (destinationID % 100000);

			ContentItem sourceContentItem = Engine.Persister.Get(source);
            //get abs value of destination - this sorts out placement folders, which have to have the a negative value of their parent node so that sorting, moving etc can work
            ContentItem destinationContentItem = Engine.Persister.Get(destination);

			// Check user has permission to create items under the SelectedItem
			if (!Engine.SecurityManager.IsAuthorized(destinationContentItem, Page.User, Operations.Create))
			{
				ExtNet.MessageBox.Alert("Cannot move item", "You are not authorised to move an item to this location.");
				return;
			}

			// TODO: Uncomment this!
			//sourceContentItem.MoveToIndex(pos);

            //set the updated value on the parent of the item that has been moved (for caching purposes)
            sourceContentItem.Parent.Updated = Utility.CurrentTime();
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