<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="BillView.aspx.cs" Inherits="BillingAspx.BillView" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/5.0.0/js/bootstrap.min.js"></script>
    <style>
        /* General styling */
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 800px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #000;
        }

        /* Styles for the invoice content */
        .invoice-header {
            text-align: center;
            margin-bottom: 20px;
        }

            .invoice-header h1 {
                font-size: 24px;
                font-weight: bold;
            }

        .invoice-details {
            display: flex;
            justify-content: space-between;
            margin-bottom: 20px;
        }

        .customer-details h2,
        .invoice-info h2 {
            font-size: 18px;
            font-weight: bold;
        }

        .invoice-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

            .invoice-table th,
            .invoice-table td {
                border: 1px solid #000;
                padding: 4px;
            }

        .invoice-footer {
            text-align: right;
            margin-top: 50px;
        }

        /* Print-specific styles */
        @media print {
            body {
                background-color: #fff;
                margin: 0;
            }

            .container {
                max-width: 100%;
                padding: 20px;
            }

            /* Hide print button */
            #printButton {
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="invoice-header">
                <h1>COMEANDFLY</h1>
            </div>
            <div class="invoice-details">
                <div class="customer-details">
                    <h2>Customer Details</h2>
                    <asp:Label ID="CustomerNameLabel" runat="server" CssClass="fw-semibold" /><br />
                    <asp:Label ID="CustomerEmailLabel" runat="server" CssClass="fw-semibold" /><br />
                    <asp:Label ID="CustomerPhoneLabel" runat="server" CssClass="fw-semibold" /><br />
                    <asp:Label ID="CustomerAddressLabel" runat="server" CssClass="fw-semibold" /><br />
                </div>
                <div class="invoice-info">
                    <h2>Invoice Details</h2>
                    <asp:Label ID="InvoiceNoLabel" runat="server" CssClass="fw-semibold" /><br />
                    <asp:Label ID="InvoiceDateLabel" runat="server" CssClass="fw-semibold" /><br />
                </div>
            </div>
            <asp:GridView ID="GridViewBillItems" runat="server" AutoGenerateColumns="False" CssClass="table invoice-table">
                <Columns>
                    <asp:TemplateField HeaderText="S. No">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Products" HeaderText="Products" />
                    <asp:BoundField DataField="Amount" HeaderText="Price" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Total" HeaderText="Total" />
                </Columns>
            </asp:GridView>
            <div class="invoice-footer">
                <h6>Total:
                    <asp:Label ID="TotalLabel" runat="server" CssClass="fw-semibold" /></h6>
                <h6>Tax (%):
                    <asp:Label ID="TaxLabel" runat="server" CssClass="fw-semibold" /></h6>
                <h6>Grand Total:
                    <asp:Label ID="GrandTotalLabel" runat="server" CssClass="fw-semibold" /></h6>
            </div>
            <hr />
            <h5><span class="fw-bold">Note :- </span>Nice Products.</h5>
            <asp:Button ID="printButton" runat="server" Text="Print" CssClass="btn btn-dark p-none" OnClientClick="printViewResult(); return false;" />
        </div>
    </form>
    <script>
        function printViewResult() {
            var originalContent = document.body.innerHTML;
            var printContent = document.getElementById("form1").innerHTML;
            document.body.innerHTML = printContent;
            window.print();
            document.body.innerHTML = originalContent;
        }
    </script>
</body>
</html>
