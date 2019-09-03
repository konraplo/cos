﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateProjectTemplateUC.ascx.cs" Inherits="Change.Intranet.CONTROLTEMPLATES.COSIntranet.BusinessDev.Project.CreateProjectTemplateUC" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/15/InputFormControl.ascx" %>

<table>
    <tr>
        <td>        
            <asp:MultiView ID="mvwMain" runat="server" ActiveViewIndex="-1">
                <asp:View ID="mmErrorMessages" runat="server">
                    <asp:Label runat="server" ID="lblErrorMsg" Text="" ForeColor="Red"></asp:Label>
                </asp:View> 
                <asp:View ID="mmView" runat="server">
                    <table>                                              
                        <tr>
                            <td colspan="2">
                            <wssuc:InputFormSection runat="server" ID="exportSection" onKeyDown="onKeyPress()" title="Export all project tasks and save as template " Description="">
				                <Template_InputFormControls>
					                <wssuc:InputFormControl ID="exportSectionFormControl" runat="server">
						                <Template_Control>
                                            <asp:Label ID="lblName" runat="server" Text="Template name" ></asp:Label>
                                            <asp:TextBox ID="tbNewEntry" runat="server" Width="150px" ></asp:TextBox>
						                </Template_Control>
					                </wssuc:InputFormControl>
				                </Template_InputFormControls>
			                </wssuc:InputFormSection>
                           </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="SaveBtn_Click"></asp:Button>
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClientClick="closeDialog('')"></asp:Button>
                            </td>
                        </tr>                                  
                    </table>
                   
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>