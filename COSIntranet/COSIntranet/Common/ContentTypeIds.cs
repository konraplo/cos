namespace Change.Intranet.Common
{
    using Microsoft.SharePoint;

    /// <summary>
    /// This class stores the ids of all of the used contenttypes.
    /// </summary>
    public static class ContentTypeIds
    {
        /// <summary>
        /// SPContentTypeId of the Change country ContentType
        /// </summary>
        public static SPContentTypeId Country = new SPContentTypeId("0x010019D973A0C7BC4D118270B2D2994DD979");

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
        public static SPContentTypeId ProjectStoreOpening = new SPContentTypeId("0x01000FE9FB11C1CD4CBDBD91C777F61D0FC7");

        /// <summary>
        /// SPContentTypeId of the Change ProjectMGMT ContentType
        /// </summary>
        public static SPContentTypeId Project = new SPContentTypeId("0x010044B8CD0E2F1142B9B9943EBE2BE14EA6");

        /// <summary>
        /// SPContentTypeId of the Change Project Task ContentType
        /// </summary>
        public static SPContentTypeId ProjectTask = new SPContentTypeId("0x0108000A97F48EEC134ECAA96C4B4BF455953F");

        /// <summary>
        /// SPContentTypeId of the Change Project Template ContentType
        /// </summary>
        public static SPContentTypeId ProjectTemplate = new SPContentTypeId("0x0100FDEA0E9393B04B4F80C728BC49FDED36");
    }
}
