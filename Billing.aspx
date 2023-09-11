<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Billing.aspx.cs" Inherits="BillingAspx.Billing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <a href="/BillCreate" class="btn btn-outline-success mb-3">Add Bill</a>
    <asp:GridView ID="GridViewBills" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="Id" HeaderText="ID" />
        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
        <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MM-yyyy}" />
        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
        <asp:BoundField DataField="CustomerEmail" HeaderText="Customer Email" />
        <asp:BoundField DataField="CustomerPhone" HeaderText="Customer Phone" />
        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:N2}" />
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:HyperLink ID="HyperLinkView" runat="server" NavigateUrl='<%# "BillView.aspx?id=" + Eval("Id") %>' Text="View" />
                <asp:HyperLink ID="HyperLinkEdit" runat="server" NavigateUrl='<%# "BillEdit.aspx?id=" + Eval("Id") %>' Text="Edit" />
                <asp:LinkButton ID="LinkButtonRemove" runat="server" Text="Remove" OnClientClick='return confirm("Are you sure want to delete?");' OnClick="LinkButtonRemove_Click" CommandArgument='<%# Eval("InvoiceNo") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>
