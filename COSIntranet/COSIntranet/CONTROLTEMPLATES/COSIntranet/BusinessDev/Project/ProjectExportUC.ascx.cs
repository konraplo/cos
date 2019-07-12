namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.BusinessDev.Project
{
    using Change.Intranet.Common;
    using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Globalization;
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
            if (!string.IsNullOrEmpty(successStatus))
            {
                if (successStatus.Equals("1"))
                {
                    EndOperationWriteBinaryData();
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
                    //longOp.LeadingHTML = SPUtility.GetLocalizedString("$Resources:ChangeExportProjectLongOpTitle", "COSIntranet", SPContext.Current.Web.Language);//"Test1";
                    longOp.LeadingHTML = SPUtility.GetLocalizedString("$Resources:ChangeExportProjectLongOpDesc", "COSIntranet", SPContext.Current.Web.Language);//"Test1";
                    //longOp.TrailingHTML = SPUtility.GetLocalizedString("$Resources:ChangeExportProjectLongOpDesc", "COSIntranet", SPContext.Current.Web.Language); //"Test2";
                    longOp.Begin();

                    //--------------------------
                    //code for long running operation is here
                    if (this.projectItemID > 0)
                    {
                        if (cbExportProject.Checked)
                        {
                            // exprot project docu
                        }

                        if (cbRemoveProject.Checked)
                        {
                            // remove project and all project releted stuff
                            //ProjectHelper.RemoveProject(SPContext.Current.Web, this.projectItemID);
                        }
                        Thread.Sleep(5000);
                    }


                    //---------------------
                    ((DialogLayoutsPageBase)this.Page).EndOperation(1, string.Concat(Request.Url.ToString(), "&success=1"));
                    //EndOperationWriteBinaryData();
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

        public void EndOperationWriteBinaryData()
        {
            string test = "test";
            byte[] data = Encoding.ASCII.GetBytes(test); ;

            //((DialogLayoutsPageBase)this.Page).EndOperation();
            Response.Clear();
            Response.ClearContent();
            //HttpContext.Current.Response.ClearHeaders();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "dupa.txt");
            //Response.Write(string.Format(CultureInfo.InvariantCulture, "<script type=\"text/javascript\">window.frameElement.commonModalDialogClose({0}, {1});</script>", new object[] { "1", "null" }));
            Response.BinaryWrite(data);
            Response.Flush();
            Response.End();
        }
    }
}
