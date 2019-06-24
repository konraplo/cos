using System;
using System.Security.Permissions;
using Change.Contracts.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Change.Contracts.EventReceivers.VendorsListEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class VendorsListEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            UpdateItem(properties.ListItem);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            UpdateItem(properties.ListItem);
        }

        private void UpdateItem(SPListItem listItem)
        {
            string groupEntity = Convert.ToString(listItem[Fields.GroupEntity]);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("List:{0}, item:{1}, value:{2}", listItem.ParentList.Title, listItem.Title, groupEntity));
            if (!string.IsNullOrEmpty(groupEntity))
            {
                EventFiringEnabled = false;
                SPFieldLookupValue geLookupValue = new SPFieldLookupValue(groupEntity);
                listItem[Fields.GroupEntityValueId] = geLookupValue.LookupValue;
                listItem.SystemUpdate(false);
                EventFiringEnabled = true;
            }
        }
    }
}