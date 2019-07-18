// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   ZipUtility constants
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestConsole
{
    /// <summary>
    /// ZipUtility constants
    /// </summary>
    public static class ZipUtilityConst
    {
        /// <summary>
        /// DateTime format used in generated zip file name
        /// </summary>
        public static readonly string GeneratingDateTimeFormat = "{0:yyyyMMdd}_{1:HHmmss}";

        /// <summary>
        /// File name prefix used in generated zip file name
        /// </summary>
        public static readonly string ZipFileNamePrefix = "ProjectExport";

        /// <summary>
        /// File type used in generated zip file name
        /// </summary>
        public static readonly string ZipFileNameType = ".zip";
    }
}