<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOrder.aspx.cs" Inherits="Zeus.AddIns.ECommerce.Plugins.ViewOrder" Debug="true" %>
<%@ Import Namespace="Zeus.AddIns.ECommerce.ContentTypes.Data"%>
<%@ Import Namespace="Zeus.BaseLibrary.ExtensionMethods"%>
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
									<ext:Button runat="server" ID="btnProcess" Text="Process" Icon="BasketGo">
										<DirectEvents>
											<Click OnEvent="btnProcess_Click">
												<EventMask ShowMask="true" Msg="Processing... Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
									<ext:Button runat="server" ID="btnCancel" Text="Cancel Order" Icon="Cross">
										<DirectEvents>
											<Click OnEvent="btnCancel_Click">
												<EventMask ShowMask="true" Msg="Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
									<ext:Button runat="server" ID="btnBack" Text="Back to Manage Orders" Icon="ArrowLeft">
										<DirectEvents>
											<Click OnEvent="btnBack_Click">
												<EventMask ShowMask="true" Msg="Please wait." Target="Page" />
											</Click>
										</DirectEvents>
									</ext:Button>
								</Items>
							</ext:Toolbar>
						</TopBar>
						<Content>
							<table class="tb" id="adminTable">
								<tr class="titles">
									<th>Customer</th>
									<th>Billing Address</th>
									<th>Shipping Address</th>
								</tr>
								<tr>
									<td>
										Email: <%= SelectedOrder.EmailAddress %><br />
										Telephone: <%= SelectedOrder.TelephoneNumber %><br />
										Mobile: <%= SelectedOrder.MobileTelephoneNumber %>
									</td>
									<% if (SelectedOrder.BillingAddress != null) { %>
									<td>
										<%= SelectedOrder.BillingAddress.PersonTitle %> <%= SelectedOrder.BillingAddress.FirstName %> <%= SelectedOrder.BillingAddress.Surname %><br />
										<%= SelectedOrder.BillingAddress.AddressLine1 %><br />
										<%= SelectedOrder.BillingAddress.AddressLine2 %><br />
										<%= SelectedOrder.BillingAddress.TownCity %><br />
										<%= SelectedOrder.BillingAddress.Postcode %><br />
									</td>
									<% } else { %>
									<td>No Billing Address Recorded</td>
									<% } %>
									<td>
										<%= SelectedOrder.ShippingAddress.PersonTitle %> <%= SelectedOrder.ShippingAddress.FirstName %> <%= SelectedOrder.ShippingAddress.Surname %><br />
										<%= SelectedOrder.ShippingAddress.AddressLine1 %><br />
										<%= SelectedOrder.ShippingAddress.AddressLine2 %><br />
										<%= SelectedOrder.ShippingAddress.TownCity %><br />
										<%= SelectedOrder.ShippingAddress.Postcode %><br />
									</td>
								</tr>
							</table>
							<div style="clear:both" />
							<table class="tb" id="adminTable">
								<tr class="titles">
									<th>Status</th>
									<th>Delivery Method</th>
									<th>Order Date</th>
									<th>Order Number</th>
									<th>Payment Method</th>
								</tr>
								<tr>
									<td><%= SelectedOrder.Status.GetDescription() %></td>
									<td><%= SelectedOrder.DeliveryMethod == null ? "N/A" : SelectedOrder.DeliveryMethod.Title%></td>
									<td><%= SelectedOrder.Created %></td>
									<td><%= SelectedOrder.ID %></td>
									<td><%= SelectedOrder.PaymentMethod.GetDescription() %></td>
								</tr>
							</table>
							<div style="clear:both" />
	
							<table class="tb" id="adminTable">
								<tr class="titles">
									<th>Product</th>
			
									<th>Quantity</th>
									<th>Price Per Unit</th>
									<th>Line Total</th>
								</tr>
								<% foreach (OrderItem orderItem in SelectedOrder.Items) { %>
								<tr>
									<td><%= orderItem.DisplayTitle %></td>
			
									<td><%= orderItem.Quantity %></td>
									<td><%= orderItem.Price.ToString("C2")%></td>
									<td><%= orderItem.LineTotal.ToString("C2")%></td>
								</tr>
								<% } %>
		
								<tr>
									<td colspan="3">TOTAL</td>
									<td><%= SelectedOrder.TotalPrice.ToString("C2") %></td>
								</tr>
							</table>
						</Content>
					</ext:Panel>
				</Items>
			</ext:FitLayout>
		</Content>
	</ext:Viewport>
</asp:Content>