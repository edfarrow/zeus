<%@ Page Title="Move Item" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.CopyItem.Default" %>
<%@ Register Src="../../AffectedItems.ascx" TagName="AffectedItems" TagPrefix="zeus" %>
<%@ Register TagPrefix="ext" Namespace="Ext.Net" Assembly="Ext.Net" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<ext:ResourceManager runat="server" ID="scriptManager" Theme="Gray" />
	
	<ext:Viewport runat="server">
		<Content>
			<ext:FitLayout runat="server">
				<Items>
					<ext:Panel runat="server">
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnCopy" Text="Try Again" Icon="PageCopy">
										<DirectEvents>
											<Click OnEvent="btnCopy_Click">
												<EventMask ShowMask="true" Msg="Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
								</Items>
							</ext:Toolbar>
						</TopBar>
						<Content>
							<asp:CustomValidator id="cvCopy" runat="server" CssClass="validator info" />
							<asp:CustomValidator ID="cvException" runat="server" CssClass="validator info" Display="Dynamic" />

							<asp:Panel runat="server" ID="pnlNewName" CssClass="editDetail" Visible="false">
								<asp:Label runat="server" ID="lblNewName" AssociatedControlID="txtNewName" Text="New name" CssClass="editorLabel" />
								<asp:TextBox ID="txtNewName" runat="server" CssClass="textEditor" />
							</asp:Panel>
							<div class="editDetail">
								<asp:Label ID="lblFrom" runat="server" AssociatedControlID="from" Text="From" CssClass="editorLabel" />
								<asp:Label ID="from" runat="server"/>
							</div>
							<div class="editDetail">
								<asp:Label ID="lblTo" runat="server" AssociatedControlID="to" Text="To" CssClass="editorLabel" />
								<asp:Label ID="to" runat="server"/>
							</div>
							<hr />
							<h3 id="h3" runat="server">Copied items:</h3>
							<div class="affectedItems">
								<zeus:AffectedItems id="itemsToCopy" runat="server" />
							</div>
						</Content>
					</ext:Panel>
				</Items>
			</ext:FitLayout>
		</Content>
	</ext:Viewport>
</asp:Content>