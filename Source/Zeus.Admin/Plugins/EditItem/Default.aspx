<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.EditItem.Default" ValidateRequest="false" MasterPageFile="../../PreviewFrame.master" %>
<%@ Register TagPrefix="ext" Assembly="Ext.Net" Namespace="Ext.Net" %>
<%@ Register TagPrefix="zeus" Namespace="Zeus.Editors.Controls" Assembly="Zeus" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
	<ext:ResourcePlaceHolder runat="server" Mode="Script" />
	<ext:ResourcePlaceHolder runat="server" Mode="Style" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<ext:Viewport runat="server">
		<Content>
			<ext:BorderLayout runat="server">
				<Center>
					<ext:Panel runat="server" Border="false" BodyStyle="padding:5px" AutoScroll="true">
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave" Text="Save" Icon="PageSave">
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
							<zeus:ItemEditor runat="server" ID="itemEditor" OnItemCreating="zeusItemEditView_ItemCreating"
								OnDefinitionCreating="zeusItemEditView_DefinitionCreating" />
						</Content>
						<BottomBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSave2" Text="Save" Icon="PageSave">
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