namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.BusinessDev.Project
{
    using Change.Intranet.Common;
    using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using System;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    public partial class CreateProjectTemplateUC : UserControl, IFormBaseView
    {
        private int projectItemID = 0;
        private string templateName = string.Empty;
        private string projectListUrlDir = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string projectId = Request["ProjectId"];
            projectListUrlDir = Request["ListUrlDir"];
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
                    string zipPackageName = string.Empty;
                    longOp.LeadingHTML = SPUtility.GetLocalizedString("$Resources:ChangeExportProjectTemplarelateLongOpDesc", "COSIntranet", SPContext.Current.Web.Language);
                    longOp.Begin();

                    string callBackUrl = string.Empty;
                    //--------------------------
                    //code for long running operation is here
                    if (this.projectItemID > 0)
                    {
                        // save project as template
                        templateName = tbNewEntry.Text;
                        if (string.IsNullOrEmpty(templateName))
                        {
                            SPList list = SPContext.Current.Web.GetList(SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, ListUtilities.Urls.StoreOpenings));
                            SPListItem project = list.GetItemById(this.projectItemID);
                            templateName = project.Title;
                        }

                       
                        if (projectListUrlDir.Contains(ListUtilities.Urls.StoreOpenings))
                        {
                            ProjectHelper.SaveStoreOpeningProjectTemplate(SPContext.Current.Web, this.projectItemID, this.templateName);

                        }
                        else
                        {
                            ProjectHelper.SaveProjectTemplate(SPContext.Current.Web, this.projectItemID, this.templateName);
                        }
                    }

                    //---------------------
                    ((DialogLayoutsPageBase)this.Page).EndOperation(1, callBackUrl);
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
