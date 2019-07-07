using Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.BusinessDev.Project
{
    public partial class ProjectExportUC : UserControl, IFormBaseView
    {

        protected void Page_Load(object sender, EventArgs e)
        {
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
        /// save risk item and close modal dialog
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

            ((DialogLayoutsPageBase)this.Page).EndOperation();
            //EndOperation(1, "");
        }
    }
}
