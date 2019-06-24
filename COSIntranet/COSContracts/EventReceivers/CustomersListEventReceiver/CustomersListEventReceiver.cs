using System;
using Change.Contracts.Common;
using Microsoft.SharePoint;

namespace Change.Contracts.EventReceivers.CustomersListEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class CustomersListEventReceiver : SPItemEventReceiver
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
                SPFieldLookupValue geLookupValue = new SPFieldLookupValue(groupEntity);
                listItem[Fields.GroupEntityValueId] = geLookupValue.LookupValue;
                
            }

            string profitCenter = Convert.ToString(listItem[Fields.CustomerProfitCenter]);
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, string.Format("List:{0}, item:{1}, value:{2}", listItem.ParentList.Title, listItem.Title, profitCenter));
            if (!string.IsNullOrEmpty(profitCenter))
            {
                SPFieldLookupValue pcLookupValue = new SPFieldLookupValue(profitCenter);
                listItem[Fields.CustPCValueId] = pcLookupValue.LookupValue;
            }

            EventFiringEnabled = false;
            listItem.SystemUpdate(false);
            EventFiringEnabled = true;
        }
    }
}