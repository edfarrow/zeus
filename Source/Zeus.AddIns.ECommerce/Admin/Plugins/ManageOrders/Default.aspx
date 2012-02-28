<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders.Default" %>
<%@ Register TagPrefix="ext" Namespace="Ext.Net" Assembly="Ext.Net" %>

<asp:Content runat="server" ContentPlaceHolderID="Content">
	<ext:Store ID="exsDataStore" runat="server" OnRefreshData="exsDataStore_RefreshData">
		<Reader>
			<ext:ArrayReader IDProperty="ID">
				<Fields>
					<ext:RecordField Name="ID" Mapping="ID" Type="String" />
					<ext:RecordField Name="Created" Mapping="Created" Type="Date" />
					<ext:RecordField Name="CustomerName" Mapping="CustomerName" Type="String" />
					<ext:RecordField Name="TotalItemCount" Mapping="TotalItemCount" Type="Int" />
					<ext:RecordField Name="TotalPrice" Mapping="TotalPrice" Type="Float" />
					<ext:RecordField Name="Status" Mapping="Status" Type="String" />
					<ext:RecordField Name="PaymentMethod" Mapping="PaymentMethod" Type="String" />
					<ext:RecordField Name="Path" Mapping="Path" Type="String" />
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
							<ext:ConfigItem Name="viewConfig" Value="{ emptyText: 'No orders matching the selected filter.' }" />
						</CustomConfig>
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button runat="server" ID="btnSeeAll" Text="See All Orders" Icon="BasketGo">
										<DirectEvents>
											<Click OnEvent="btnSeeAll_Click">
												<EventMask ShowMask="true" Msg="Updating... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
									<ext:Button runat="server" ID="btnSeeOnlyUnprocessed" Text="See Only Unprocessed Orders" Icon="Basket" Disabled="true">
										<DirectEvents>
											<Click OnEvent="btnSeeOnlyUnprocessed_Click">
												<EventMask ShowMask="true" Msg="Updating... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
									<ext:Button runat="server" ID="btnSearch" Text="Search for an Order" Icon="Magnifier">
										<DirectEvents>
											<Click OnEvent="btnSearch_Click">
												<EventMask ShowMask="true" Msg="Updating... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
								</Items>
							</ext:Toolbar>
						</TopBar>
						<ColumnModel>
							<Columns>
								<ext:Column ColumnID="ID" Header="Order Number" Sortable="true" DataIndex="ID" />
								<ext:Column ColumnID="Created" Header="Date Placed" Sortable="true" DataIndex="Created">
									<Renderer Format="Date" />
								</ext:Column>
								<ext:Column ColumnID="CustomerName" Header="Customer" Sortable="true" DataIndex="CustomerName" />
								<ext:Column ColumnID="TotalItemCount" Header="# Items" Sortable="true" DataIndex="TotalItemCount" />
								<ext:Column ColumnID="TotalPrice" Header="Total Price" Sortable="true" DataIndex="TotalPrice" />
								<ext:Column ColumnID="Status" Header="Status" Sortable="true" DataIndex="Status" />
								<ext:Column ColumnID="PaymentMethod" Header="Method" Sortable="true" DataIndex="PaymentMethod" />
								<ext:CommandColumn>
									<Commands>
										<ext:GridCommand ToolTip-Text="Details" CommandName="Details" Icon="NoteEdit" />
									</Commands>
								</ext:CommandColumn>
							</Columns>
						</ColumnModel>
						<LoadMask ShowMask="true" />
						<Plugins>
							<ext:GridFilters runat="server">
								<Filters>
									<ext:StringFilter DataIndex="ID" />
									<ext:StringFilter DataIndex="CustomerFirstName" />
									<ext:StringFilter DataIndex="CustomerLastName" />
									<ext:StringFilter DataIndex="CustomerEmail" />
								</Filters>
							</ext:GridFilters>
						</Plugins>
						<BottomBar>
							<ext:PagingToolBar runat="server" PageSize="30" StoreID="exsDataStore" />
						</BottomBar>
						<Listeners>
							<Command Handler="window.location.href = 'vieworder.aspx?selected=' + record.data.Path;" />
						</Listeners>
					</ext:GridPanel>
				</Items>
			</ext:FitLayout>
		</Content>
	</ext:Viewport>
</asp:Content>