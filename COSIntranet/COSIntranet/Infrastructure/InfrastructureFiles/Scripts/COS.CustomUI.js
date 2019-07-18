function OpenDialog(strPageURL, dialogWidth, dialogHeight) {
    var dialogOptions = SP.UI.$create_DialogOptions();
    dialogOptions.url = strPageURL; // URL of the Page
    dialogOptions.width = dialogWidth; // Width of the Dialog
    dialogOptions.height = dialogHeight; // Height of the Dialog
    dialogOptions.dialogReturnValueCallback = Function.createDelegate(null, CloseCallback); // Function to capture dialog closed event
    SP.UI.ModalDialog.showModalDialog(dialogOptions); // Open the Dialog
    return false;
}

function OpenDialogFromLayout(strPageURL, dialogWidth, dialogHeight) {
    var layoutUrl = SP.Utilities.Utility.getLayoutsPageUrl(strPageURL, dialogWidth, dialogHeight);

    return OpenDialog(layoutUrl);
}

function CloseCallback(strReturnValue, target) {
    if (strReturnValue === SP.UI.DialogResult.OK) // Perform action on Ok.
    {
        if (target && target !== 'null' && target.length > 0) {
            window.location.href = target;
        }
        else {
            window.location.href = window.location.href;
        }

    }
    if (strReturnValue === SP.UI.DialogResult.cancel) // Perform action on Cancel.
    {
        //alert( "User clicked Cancel!");
    }
}

function closeDialog(strMessage) {
    window.frameElement.commonModalDialogClose(0, strMessage);
}