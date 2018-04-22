using System;

namespace Change.Intranet.Common
{
    /// <summary>
    /// Utilities for content organizer functionality
    /// </summary>
    public static class ContentOrganizerUtilities
    {
        /// <summary>
        /// Event receiver id for ItemDeleting event
        /// </summary>
        public static Guid ItemDeletingERID = new Guid("A49BE3A2-9018-41E6-956A-AAC7CB5D314C");

        /// <summary>
        /// Event receiver id for ItemUpdating event
        /// </summary>
        public static Guid ItemUpdatingERID = new Guid("FB19BDF8-708A-4610-BEC1-A52112034BE9");

        /// <summary>
        /// Event receiver name for ItemDeleting event
        /// </summary>
        public const string ItemDeletingERName = "ChangeFolderDeletingER";

        /// <summary>
        /// Event receiver name for ItemUpdating event
        /// </summary>
        public const string ItemUpdatingERName = "ChangeFolderUpdatingER";

        /// <summary>
        /// Folder Url filed id
        /// </summary>
        public static Guid UrlFieldId = new Guid("{5B0F68F2-8B2E-4C9D-B2B4-157BB8205052}");

        /// <summary>
        /// Error msg shown if deleteing/updating folder is prohibited
        /// </summary>
        public const string PreventFolderDeleteErrorMsg = "Folder used as one of DropOff targets. To delete or update this folder, one should be removed from DropOff targets first. Please contact Site Administrator.";

    }
}
