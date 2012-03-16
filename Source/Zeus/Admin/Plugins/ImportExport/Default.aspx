﻿<%@ Page Title="Import / Export Items" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.ImportExport.Default" MasterPageFile="../../PreviewFrame.master" %>
<%@ Register TagPrefix="ext" Assembly="Ext.Net" Namespace="Ext.Net" %>
<%@ Register Src="../../AffectedItems.ascx" TagName="AffectedItems" TagPrefix="admin" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		p { margin-bottom: 10px; }
	</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<ext:TabPanel runat="server">
		<Items>
			<ext:Panel Title="Export" ID="tbpExport">
				<Content>
					<p><asp:Button ID="btnExport" runat="server" CssClass="command" OnCommand="btnExport_Command" CausesValidation="false" Text="Export these items" /></p>
					<p><asp:CheckBox ID="chkDefinedDetails" runat="server" Text="Exclude computer generated data" /></p>
			
					<p><b>Exported Items</b></p>
					<div class="affectedItems">
						<admin:AffectedItems id="exportedItems" runat="server" />
					</div>
				</Content>
			</ext:Panel>
			<ext:Panel Title="Import">
				<Content>
					<asp:CustomValidator id="cvImport" runat="server" CssClass="info validator" Display="Dynamic"/>
					<asp:MultiView ID="uploadFlow" runat="server" ActiveViewIndex="0">
						<asp:View ID="uploadView" runat="server">
							<div class="upload">
								<p>
									<asp:FileUpload ID="fuImport" runat="server" />
									<asp:RequiredFieldValidator ID="rfvUpload" ControlToValidate="fuImport" runat="server" ErrorMessage="*" />
								</p>
								<p>
									<asp:Button ID="btnVerify" runat="server" Text="Upload and examine" OnClick="btnVerify_Click" Display="Dynamic" />
									<asp:Button ID="btnUploadImport" runat="server" Text="Import here" OnClick="btnUploadImport_Click" />
							</p>
							</div>
						</asp:View>
						<asp:View ID="preView" runat="server">
							<p><asp:CheckBox ID="chkSkipRoot" runat="server" Text="Skip imported root item" ToolTip="Checking this options cause the first level item not to be imported, and its children to be added to the selected item's children" /></p>
							<asp:Button ID="btnImportUploaded" runat="server" Text="Import" OnClick="btnImportUploaded_Click" />
			    
							<p><b>Imported Items</b></p>
							<div class="affectedItems">
								<admin:AffectedItems id="importedItems" runat="server" />
							</div>
						</asp:View>
					</asp:MultiView>
				</Content>
			</ext:Panel>
		</Items>
	</ext:TabPanel>
</asp:Content>