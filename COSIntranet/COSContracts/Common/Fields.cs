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
        public const string Title = "Title";

    }
}
