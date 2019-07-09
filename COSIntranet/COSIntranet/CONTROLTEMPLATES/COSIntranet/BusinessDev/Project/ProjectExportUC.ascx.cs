namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.BusinessDev.Project
{
    using Change.Intranet.Common;
    using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Threading;
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
                            // remove all project releted stuff
                        }
                        Thread.Sleep(5000);
                    }


                    //---------------------
                    ((DialogLayoutsPageBase)this.Page).EndOperation();
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
    }
}
