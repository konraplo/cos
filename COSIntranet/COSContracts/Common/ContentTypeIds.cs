namespace Change.Contracts.Common
{
    using Microsoft.SharePoint;

    /// <summary>
    /// This class stores the ids of all of the used contenttypes.
    /// </summary>
    public static class ContentTypeIds
    {
        /// <summary>
        /// SPContentTypeId of the Change GroupEntity ContentType
        /// </summary>
        public static SPContentTypeId GroupEntity = new SPContentTypeId("0x0100E344B5D6D69640888293940AF91C134B");

        /// <summary>
        /// SPContentTypeId of the Change ContractSubtype ContentType
        /// </summary>
        public static SPContentTypeId ContractSubtype = new SPContentTypeId("0x0100825961710DCF4339A21FDD203EA7A34B");

        /// <summary>
        /// SPContentTypeId of the Change Contract
        /// </summary>
        public static SPContentTypeId Contract = new SPContentTypeId("0x0120D52000212A0B99C39645CDBE3E278A5246E5DF");

        /// <summary>
        /// SPContentTypeId of the Change Contract Document
        /// </summary>
        public static SPContentTypeId ContractDocument = new SPContentTypeId("0x010100A6B0F4956C484C4CB1BB5B8F0618F5F3");

        /// <summary>
        /// SPContentTypeId of the Change ExternalContacts ContentType
        /// </summary>
        public static SPContentTypeId ExternalContact = new SPContentTypeId("0x010600116954ED24DA48F1929A22BC298FABA3");

        /// <summary>
        /// SPContentTypeId of the Change Customer Profit Center ContentType
        /// </summary>
        public static SPContentTypeId CustomerProfitCenter = new SPContentTypeId("0x0100A2318F2B3595446C9BA1ACCBD57655C2");

        /// <summary>
        /// SPContentTypeId of the Change Customer ContentType
        /// </summary>
        public static SPContentTypeId Customer = new SPContentTypeId("0x010046659AEEFDA14270AF680421AD589C7B");

        /// <summary>
        /// SPContentTypeId of the Change Vendor ContentType
        /// </summary>
        public static SPContentTypeId Vendor = new SPContentTypeId("0x0100515EA817AED3414FAD8893653F1F6B88");
    }
}
