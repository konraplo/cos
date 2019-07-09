function OpenDialog(strPageURL) {
    var dialogOptions = SP.UI.$create_DialogOptions();
    dialogOptions.url = strPageURL; // URL of the Page
    dialogOptions.width = 920; // Width of the Dialog
    dialogOptions.height = 500; // Height of the Dialog
    dialogOptions.dialogReturnValueCallback = Function.createDelegate(null, CloseCallback); // Function to capture dialog closed event
    SP.UI.ModalDialog.showModalDialog(dialogOptions); // Open the Dialog
    return false;
}

function OpenDialogFromLayout(strPageURL) {
    var layoutUrl = SP.Utilities.Utility.getLayoutsPageUrl(strPageURL);

    return OpenDialog(layoutUrl);
}

function CloseCallback(strReturnValue, target) {
    if (strReturnValue === SP.UI.DialogResult.OK) // Perform action on Ok.
    {
        window.location.href = window.location.href;
    }
    if (strReturnValue === SP.UI.DialogResult.cancel) // Perform action on Cancel.
    {
        //alert( "User clicked Cancel!");
    }
}

function closeDialog(strMessage) {
    window.frameElement.commonModalDialogClose(0, strMessage);
}