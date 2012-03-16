<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.Permissions.Default" MasterPageFile="../../PreviewFrame.master" %>
<%@ Register TagPrefix="ext" Namespace="Ext.Net" Assembly="Ext.Net" %>
<asp:Content ContentPlaceHolderID="Content" runat="server">
	<ext:Viewport runat="server">
		<Content>
			<ext:FitLayout runat="server">
				<Items>
					<ext:Panel runat="server">
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave" Text="Save" Icon="Tick">
										<DirectEvents>
											<Click OnEvent="btnSave_Click">
												<EventMask ShowMask="true" Msg="Saving permissions... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
									<ext:Button runat="server" ID="btnSaveRecursive" Text="Save Whole Branch" Icon="Tick">
										<DirectEvents>
											<Click OnEvent="btnSaveRecursive_Click">
												<EventMask ShowMask="true" Msg="Saving permissions... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
								</Items>
							</ext:Toolbar>
						</TopBar>
						<Content>
							<asp:Table runat="server" ID="tblPermissions" CssClass="permissions" />
	
							<br />
							<h3>Add New Role or User</h3>
							<div class="editDetail">
								<asp:Label runat="server" AssociatedControlID="ddlRoles" CssClass="editorLabel">Role</asp:Label>
								<asp:DropDownList runat="server" ID="ddlRoles" Width="200" AppendDataBoundItems="true">
									<asp:ListItem></asp:ListItem>
								</asp:DropDownList>
								<asp:Button runat="server" ID="btnAddRole" Text="Add" OnClick="btnAddRole_Click" /><br />
							</div>
							<div class="editDetail">
								<asp:Label runat="server" AssociatedControlID="ddlUsers" CssClass="editorLabel">User</asp:Label>
								<asp:DropDownList runat="server" ID="ddlUsers" Width="200" AppendDataBoundItems="true">
									<asp:ListItem></asp:ListItem>
								</asp:DropDownList>
								<asp:Button runat="server" ID="btnAddUser" Text="Add" OnClick="btnAddUser_Click" /><br />
							</div>
						</Content>
					</ext:Panel>
				</Items>
			</ext:FitLayout>
		</Content>
	</ext:Viewport>
</asp:Content>