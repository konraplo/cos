using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change.Intranet.EventReceivers.BussinesDev
{
    using Change.Intranet.Common;
    using Microsoft.SharePoint;

    /// <summary>
    /// Event receivers for store list
    /// </summary>
    public class StoreListEventReceiver : SPItemEventReceiver
    {
       
        /// <summary>
        /// Ein Element wurde hinzugefügt..
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemAdded");
        }

        /// <summary>
        /// Ein Element wurde aktualisiert..
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "ItemUpdated");
        }

        private void SetStoreId(SPListItem storeItem)
        { 
            Logger.WriteLog(Logger.Category.Information, this.GetType().Name, "Set Store Id");
        }
    }
}
