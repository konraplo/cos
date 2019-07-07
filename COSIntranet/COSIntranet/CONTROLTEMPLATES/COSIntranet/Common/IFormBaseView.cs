namespace Change.Intranet.CONTROLTEMPLATES.COSIntranet.Common
{
    public interface IFormBaseView
    {
        /// <summary>
        /// activate specified view panel
        /// </summary>
        /// <param name="pPanel"></param>
        void ActivateView(UIHelper.MainViewPanel pPanel);

        /// <summary>
        /// show error message label
        /// </summary>
        /// <param name="pMessage">error message</param>
        void ShowErrorMessage(string pMessage);
    }
}
