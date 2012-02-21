<%@ Page Title="Add New Item" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.NewItem.Default" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="ext" Assembly="Ext.Net" Namespace="Ext.Net" %>
<%@ Import Namespace="Zeus.ContentTypes" %>
<asp:Content runat="server" ContentPlaceHolderID="Toolbar">
	<admin:ToolbarHyperLink runat="server" ID="hlCancel" Text="Cancel" Icon="Cross" CssClass="negative" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<ext:ResourceManager runat="server" Theme="Gray" />
	
	<ext:TabPanel runat="server" ID="tbcTabs" BodyStyle="padding:5px">
		<Items>
			<ext:Panel runat="server" ID="tbpType" Title="Type">
				<Content>
					<asp:ListView runat="server" ID="lsvChildTypes">
						<LayoutTemplate>
							<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
						</LayoutTemplate>
						<ItemTemplate>
							<p>
								<a href="<%# GetEditUrl((ContentType) Container.DataItem) %>"><img runat="server" src='<%# Eval("IconUrl") %>' alt="" /></a>
								<strong><a href="<%# GetEditUrl((ContentType) Container.DataItem) %>"><%# Eval("Title")%></a></strong>
								<%# Eval("ContentTypeAttribute.Description") %>
							</p>
						</ItemTemplate>
						<EmptyDataTemplate>
							<p>You cannot add an item below this location.</p>
						</EmptyDataTemplate>
					</asp:ListView>
				</Content>
			</ext:Panel>
		</Items>
	</ext:TabPanel>
</asp:Content>