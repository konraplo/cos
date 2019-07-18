namespace Change.Contracts.Common
{
    using System;

    /// <summary>
    /// Ids and names of the most used site columns.
    /// </summary>
    public static class Fields
    {
        public const string ChangeContractsFieldsGroup = "CHANGE Contracts Fields";
        public const string Customer = "ChangeContractCustomer";
        public const string CustomerProfitCenter = "ChangeCustomerProfitCenter";
        public const string GroupEntity = "ChangeContractGroupEntity";
        public const string ContractSubtype = "ChangeContractSubtype";
        public const string Vendor = "ChangeContractVendor";
        public const string ExternalContactVendor = "ChangeContractExtVendorContact";
        public const string ExternalContactCust = "ChangeContractExtCustContact";
        public const string GroupEntityValue = "ChangeContractGroupEntityValue";
        public static Guid GroupEntityValueId = new Guid("{8e8e5164-0db1-4927-91b6-0d3cbe01ee3f}");
        public const string CustPCValue = "ChangeContractCustPCValue";
        public static Guid CustPCValueId = new Guid("{87e52492-eefe-40ee-87e0-a7daec055810}");
        public static Guid ChangeContractWarnDate = new Guid("{2d849d1d-9f93-4514-ab9b-8caea5a85569}");
        public static Guid ChangeContractEndDate = new Guid("{22fbb0b8-11e9-4f69-975d-669294116948}");
        public static Guid ChangeContractContractStatus = new Guid("{8c222fe8-f4a9-4e59-a75c-bf111672c947}");
        public const string Title = "Title";
        public const string StatusActive = "Active";
        public const string StatusExpired = "Expired";

    }
}
