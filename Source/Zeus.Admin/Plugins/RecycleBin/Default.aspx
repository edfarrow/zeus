<%@ Page Title="Recycle Bin" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.Admin.Plugins.RecycleBin.Default" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content runat="server" ContentPlaceHolderID="Content">
	<ext:ResourceManager runat="server" ID="scriptManager" Theme="Gray" />
	
	<script type="text/javascript">
		var renderIcon = function(value, p, record) {
			return String.format('<img src="{0}" alt="{1}" />',
				value, record.data.Title);
		};
	</script>

	<asp:CustomValidator ID="cvRestore" CssClass="validator" ErrorMessage="An item with the same name already exists at the previous location." runat="server" Display="Dynamic" />
	
	<ext:Store ID="exsDataStore" runat="server" OnRefreshData="exsDataStore_RefreshData">
		<Reader>
			<ext:ArrayReader IDProperty="ID">
				<Fields>
					<ext:RecordField Name="ID" Mapping="ID" Type="String" />
					<ext:RecordField Name="Title" Mapping="Title" Type="String" />
					<ext:RecordField Name="DeletedDate" Mapping="DeletedDate" Type="Date" />
					<ext:RecordField Name="PreviousLocation" Mapping="PreviousLocation" Type="String" />
					<ext:RecordField Name="IconUrl" Mapping="IconUrl" Type="String" />
				</Fields>
			</ext:ArrayReader>
		</Reader>
	</ext:Store>
	
	<ext:Viewport runat="server">
		<Content>
			<ext:FitLayout runat="server">
				<Items>
					<ext:GridPanel runat="server" ID="gpaChildren" StoreID="exsDataStore" StripeRows="true" Border="false">
						<CustomConfig>
							<ext:ConfigItem Name="viewConfig" Value="{ emptyText: 'No items in recycle bin.' }" />
						</CustomConfig>
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnEmpty" Text="Empty Recycle Bin" Icon="BinEmpty">
										<DirectEvents>
											<Click OnEvent="btnEmpty_Click">
												<EventMask ShowMask="true" Msg="Emptying... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
								</Items>
							</ext:Toolbar>
						</TopBar>
						<ColumnModel>
							<Columns>
								<ext:Column ColumnID="Icon" Header="" Width="30" DataIndex="IconUrl">
									<Renderer Fn="renderIcon" />
								</ext:Column>
								<ext:Column ColumnID="Title" Header="Title" Width="200" Sortable="true" DataIndex="Title" />
								<ext:Column ColumnID="DeletedDate" Header="Deleted Date" Width="200" Sortable="true" DataIndex="DeletedDate" />
								<ext:Column ColumnID="PreviousLocation" Header="Previous location" Width="200" Sortable="true" DataIndex="PreviousLocation" />
								<ext:CommandColumn>
									<Commands>
										<ext:GridCommand ToolTip-Text="Restore" CommandName="Restore" Icon="ArrowRedo" />
										<ext:GridCommand ToolTip-Text="Permanently delete" CommandName="Delete" Icon="PageDelete" />
									</Commands>
								</ext:CommandColumn>
							</Columns>
						</ColumnModel>
						<LoadMask ShowMask="true" />
						<BottomBar>
							<ext:PagingToolBar runat="server" PageSize="10" StoreID="exsDataStore" />
						</BottomBar>
						<Listeners>
							<Command Handler="this.currentID = record.data.ID; this.currentCommandName = command;" />
						</Listeners>
						<DirectEvents>
							<Command OnEvent="gpaChildren_Command">
								<EventMask ShowMask="true" Msg="Please wait..." Target="Page" />
								<ExtraParams>
									<ext:Parameter Name="ID" Value="this.currentID" Mode="Raw" />
									<ext:Parameter Name="CommandName" Value="this.currentCommandName" Mode="Raw" />
								</ExtraParams>
							</Command>
						</DirectEvents>
					</ext:GridPanel>
				</Items>
			</ext:FitLayout>
		</Content>
	</ext:Viewport>
</asp:Content>
