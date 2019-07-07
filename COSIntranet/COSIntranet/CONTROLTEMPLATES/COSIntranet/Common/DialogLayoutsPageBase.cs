
namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common
{
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Globalization;

    /// <summary>
    /// Modal Dialog-aware Application Page base class.
    /// </summary>
    public class DialogLayoutsPageBase : LayoutsPageBase
    {
        /// <summary>
        /// URL of the page to redirect to when not in Dialog mode.
        /// </summary>
        public string PageToRedirectOnOK { get; set; }

        /// <summary>
        /// Returns true if the Application Page is displayed in Modal Dialog.
        /// </summary>
        public bool IsPopUI
        {
            get
            {
                return !string.IsNullOrEmpty(base.Request.QueryString["IsDlg"]);
            }
        }

        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// Returns the OK response.
        /// </summary>
        public void EndOperation()
        {
            EndOperation(1);
        }

        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// </summary>
        /// <param name="result">Result code to pass to the output. Available results: -1 = invalid; 0 = cancel; 1 = OK</param>
        public void EndOperation(int result)
        {
            EndOperation(result, PageToRedirectOnOK);
        }

        /// <summary>
        /// Call after completing custom logic in the Application Page.
        /// </summary>
        /// <param name="result">Result code to pass to the output. Available results: -1 = invalid; 0 = cancel; 1 = OK</param>
        /// <param name="returnValue">Value to pass to the callback method defined when opening the Modal Dialog.</param>
        public void EndOperation(int result, string returnValue)
        {
            if (IsPopUI)
            {
                Page.Response.Clear();
                Page.Response.Write(string.Format(CultureInfo.InvariantCulture, "<script type=\"text/javascript\">window.frameElement.commonModalDialogClose({0}, {1});</script>", new object[] { result, string.IsNullOrEmpty(returnValue) ? "null" : string.Format("\"{0}\"", returnValue) }));
                Page.Response.End();
            }
            else
            {
                RedirectOnOK();
            }
        }

        /// <summary>
        /// Redirects to the URL specified in the PageToRedirectOnOK property.
        /// </summary>
        public void RedirectOnOK()
        {
            SPUtility.Redirect(PageToRedirectOnOK ?? SPContext.Current.Web.Url, SPRedirectFlags.UseSource, Context);
        }
    }
}
