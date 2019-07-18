namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.BusinessDev.Project
{
    using Change.Intranet.Common;
    using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.UI;

    public partial class ProjectExportUC : UserControl, IFormBaseView
    {
        private int projectItemID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string projectId = Request["ProjectId"];
            if (!string.IsNullOrEmpty(projectId))
            {
                projectItemID = Convert.ToInt32(projectId);
            }

            if (!IsPostBack)
            {
                this.ActivateView(UIHelper.MainViewPanel.DataViewPanel);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            string successStatus = Request["success"];
            string zipFileName = Request["packagename"];

            if (!string.IsNullOrEmpty(successStatus))
            {
                if (successStatus.Equals("1"))
                {
                    EndOperationWriteBinaryData(string.IsNullOrEmpty(zipFileName) ? string.Empty : zipFileName, UIHelper.ZipFileSavingPlace.LocalServerTempFolder);
                }
            }

            base.OnPreRender(e);
        }

        public void ActivateView(UIHelper.MainViewPanel pPanel)
        {
            int activeView = (int)pPanel;
            mvwMain.SetActiveView(mvwMain.Views[activeView]);
        }

       
        public void ShowErrorMessage(string pMessage)
        {
            lblErrorMsg.Text = pMessage;
        }

        /// <summary>
        /// export project and close modal dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveBtn_Click(Object sender, EventArgs e)
        {
            this.Page.Validate();
            if (!this.Page.IsValid)
            {
                return;
            }

            try
            {
                using (SPLongOperation longOp = new SPLongOperation(this.Page))
                {
                    string zipPackageName = string.Empty;
                    //longOp.LeadingHTML = SPUtility.GetLocalizedString("$Resources:ChangeExportProjectLongOpTitle", "COSIntranet", SPContext.Current.Web.Language);//"Test1";
                    longOp.LeadingHTML = SPUtility.GetLocalizedString("$Resources:ChangeExportProjectLongOpDesc", "COSIntranet", SPContext.Current.Web.Language);//"Test1";
                    //longOp.TrailingHTML = SPUtility.GetLocalizedString("$Resources:ChangeExportProjectLongOpDesc", "COSIntranet", SPContext.Current.Web.Language); //"Test2";
                    longOp.Begin();

                    string callBackUrl = string.Empty;
                    //--------------------------
                    //code for long running operation is here
                    if (this.projectItemID > 0)
                    {
                        if (cbExportProject.Checked)
                        {
                            // Export project documentation
                            zipPackageName = ProjectHelper.ArchiveProject(SPContext.Current.Web, this.projectItemID, UIHelper.ZipFileSavingPlace.LocalServerTempFolder);
                            callBackUrl = string.Concat(SPContext.Current.Web.Url, Request.Url.PathAndQuery, "&success=1", string.Format("&packagename={0}", zipPackageName));
                        }

                        if (cbRemoveProject.Checked)
                        {
                            // remove project and all project releted stuff
                            ProjectHelper.RemoveProject(SPContext.Current.Web, this.projectItemID);
                        }
                    }


                    //---------------------
                    //((DialogLayoutsPageBase)this.Page).EndOperation(1, string.Concat(Request.Url.ToString(), "&success=1", string.Format("&packagename={0}", zipPackageName)));
                    ((DialogLayoutsPageBase)this.Page).EndOperation(1, callBackUrl);
                    //EndOperationWriteBinaryData(zipPackageName, UIHelper.ZipFileSavingPlace.LocalServerTempFolder);
                    //longOp.End(Request.Url.ToString(), SPRedirectFlags.DoNotEndResponse, HttpContext.Current, "success=1");
                    //longOp.End(@"http://sharcha-p15/_layouts/15/COSIntranet/BusinessDev/ExportProject.aspx?ProjectId=3&success=1", SPRedirectFlags.Default, HttpContext.Current, "success=1");
                    //EndOperationWriteBinaryData();
                }
            }
            catch (ThreadAbortException)
            {
                /* Thrown when redirected */
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, typeof(ProjectExportUC).FullName, ex.Message);
                SPUtility.TransferToErrorPage(ex.ToString());
            }
           
        }

        /// <summary>
        /// Downloads ziped file with archived project 
        /// </summary>
        /// <param name="zipFileName">ziped file name</param>
        /// <param name="fileSavingPlace">Place where the archiv file is saved</param>
        public void EndOperationWriteBinaryData(string zipFileName, UIHelper.ZipFileSavingPlace fileSavingPlace)
        {
            byte[] data = null;
            if (fileSavingPlace == UIHelper.ZipFileSavingPlace.SharePointAssetsList)
            {
                string siteAssetsUrl = SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, "SiteAssets");
                string zipFileUrl = SPUrlUtility.CombineUrl(siteAssetsUrl, string.Concat("Archives", "/", zipFileName));

                SPFile zipFile = SPContext.Current.Web.GetFile(zipFileUrl);
                data = zipFile.OpenBinary(SPOpenBinaryOptions.None);
            }
            else
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string tmpPackagePath = Path.Combine(Path.GetTempPath(), zipFileName);
                    data = File.ReadAllBytes(tmpPackagePath);
                    File.Delete(tmpPackagePath);
                });
            }

            //((DialogLayoutsPageBase)this.Page).EndOperation();
            Response.Clear();
            Response.ClearContent();
            //HttpContext.Current.Response.ClearHeaders();
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + "dupa.txt");
            Response.AppendHeader("Content-Disposition", string.Concat("attachment; ", "filename=", zipFileName));
            //Response.Write(string.Format(CultureInfo.InvariantCulture, "<script type=\"text/javascript\">window.frameElement.commonModalDialogClose({0}, {1});</script>", new object[] { "1", "null" }));
            Response.BinaryWrite(data);
            Response.Flush();
            Response.End();
        }
    }
}
