<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.EditItem.Default" ValidateRequest="false" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="admin" Namespace="Zeus.Admin.Web.UI.WebControls" Assembly="Zeus.Admin" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Web.UI.WebControls" Assembly="Zeus" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" %>
<%@ Register TagPrefix="ext" Namespace="Coolite.Ext.UX" Assembly="Coolite.Ext.UX" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
	<ext:ResourcePlaceHolder runat="server" Mode="Script" />
	<ext:ResourcePlaceHolder runat="server" Mode="Style" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ToolbarContainer"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentContainer"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Outside">
	<ext:ResourceManager runat="server" Theme="Gray" AjaxViewStateMode="Enabled" />

	<ext:Viewport runat="server">
		<Content>
			<ext:BorderLayout runat="server">
				<Center>
					<ext:Panel runat="server" Border="false" BodyStyle="padding:5px" AutoScroll="true">
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave" Text="Save" Icon="PageSave" OnClientClick="if (typeof(tinyMCE) !== 'undefined') { tinyMCE.triggerSave(false,true); } return true;">
										<DirectEvents>
											<Click OnEvent="btnSave_Click">
												<EventMask ShowMask="true" Msg="Saving... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
									<ext:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
									<ext:ToolbarFill runat="server" />
								</Items>
							</ext:Toolbar>
						</TopBar>
						<Content>
							<asp:ValidationSummary runat="server" CssClass="info validator" />
							
							<zeus:ItemEditView runat="server" ID="zeusItemEditView" OnItemCreating="zeusItemEditView_ItemCreating"
								OnDefinitionCreating="zeusItemEditView_DefinitionCreating" />
						</Content>
						<BottomBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave2" Text="Save" Icon="PageSave" OnClientClick="if (typeof(tinyMCE) !== 'undefined') { tinyMCE.triggerSave(false,true); } return true;">
										<DirectEvents>
											<Click OnEvent="btnSave_Click">
												<EventMask ShowMask="true" Msg="Saving... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
									<ext:Button runat="server" ID="btnCancel2" Text="Cancel" Icon="Cross" OnClick="btnCancel_Click" AutoPostBack="true" CausesValidation="false" />
								</Items>
							</ext:Toolbar>
						</BottomBar>
					</ext:Panel>
				</Center>
			</ext:BorderLayout>
		</Content>
	</ext:Viewport>
	
	<script type="text/javascript">
		Ext.onReady(function()
		{
			setTimeout(jQuery.zeusKeepAlive.sessionSaver,
				jQuery.zeusKeepAlive.sessionSaverInterval);

			window.top.zeus.setPreviewTitle('<%= Title.Replace("'", "\\'") %>');
		});

		(function($)
		{
			$.zeusKeepAlive =
			{
				sessionSaverUrl: '<%= GetSessionKeepAliveUrl() %>',
				sessionSaverInterval: (60000 * 5),
				sessionSaver: function()
				{
					$.post($.zeusKeepAlive.sessionSaverUrl);
					setTimeout($.zeusKeepAlive.sessionSaver,
						$.zeusKeepAlive.sessionSaverInterval);
				}
			};
		})(jQuery);

   </script>
</asp:Content>