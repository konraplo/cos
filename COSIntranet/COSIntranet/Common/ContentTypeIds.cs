namespace Change.Intranet.Common
{
    using Microsoft.SharePoint;

    /// <summary>
    /// This class stores the ids of all of the used contenttypes.
    /// </summary>
    public static class ContentTypeIds
    {
        /// <summary>
        /// SPContentTypeId of the Change store ContentType
        /// </summary>
        public static SPContentTypeId Store = new SPContentTypeId("0x0100134A06001B184621BDC69D96729FA9B0");

        /// <summary>
        /// SPContentTypeId of the Change Department ContentType
        /// </summary>
        public static SPContentTypeId Department = new SPContentTypeId("0x01000E4FAE189E87422B9AC904625255265B");

        /// <summary>
        /// SPContentTypeId of the Change Project ContentType
        /// </summary>
        public static SPContentTypeId Project = new SPContentTypeId("0x0108001845C9025F79452AA25119FB17AEB359");

        /// <summary>
        /// SPContentTypeId of the Change Project Task ContentType
        /// </summary>
        public static SPContentTypeId ProjectTask = new SPContentTypeId("0x0108000A97F48EEC134ECAA96C4B4BF455953F");
    }
}
