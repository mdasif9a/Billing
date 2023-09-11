<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillCreate.aspx.cs" Inherits="BillingAspx.BillCreate" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/5.0.0/js/bootstrap.min.js"></script>
    <style>
        .form-control.form-control-sm {
            border-radius: 0;
        }

            .form-control.form-control-sm.readonly {
                background-color: rgba(0,0,0,0.05);
            }
    </style>
</head>
<body>
    <form runat="server">

        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="card my-4">
                        <div class="card-body">
                            <h2 class="mb-4">Invoice</h2>
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="mb-3">
                                        <label for="CustomerName" class="form-label">Customer Name</label>
                                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control form-control-sm" />
                                        <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ControlToValidate="txtCustomerName" ErrorMessage="Customer Name is required" CssClass="text-danger" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="mb-3">
                                        <label for="CustomerEmail" class="form-label">Customer Email</label>
                                        <asp:TextBox ID="txtCustomerEmail" runat="server" CssClass="form-control form-control-sm" />
                                        <asp:RegularExpressionValidator ID="revCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail" ErrorMessage="Invalid Email Address" CssClass="text-danger" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="mb-3">
                                        <label for="CustomerPhone" class="form-label">Customer Phone</label>
                                        <asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control form-control-sm" />
                                    </div>
                                    <div class="mb-3">
                                        <label for="CustomerAddress" class="form-label">Customer Address</label>
                                        <asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="form-control form-control-sm" Rows="4" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="mb-3">
                                        <label for="InvoiceNo" class="form-label">Invoice No</label>
                                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control form-control-sm readonly" ReadOnly="true" />
                                    </div>
                                    <div class="mb-3">
                                        <label for="InvoiceDate" class="form-label">Invoice Date</label>
                                        <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control form-control-sm" />
                                    </div>
                                </div>
                            </div>

                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th>S. No</th>
                                            <th>Products</th>
                                            <th>Price</th>
                                            <th>Quantity</th>
                                            <th>Total</th>
                                            <th>
                                                <button id="addrow" type="button" class="btn btn-sm btn-primary">Add Row</button></th>
                                        </tr>
                                    </thead>
                                    <tbody id="tblItems">
                                        <tr>
                                            <td>1</td>
                                            <td>
                                                <input type="text" name="items[0].Products" class="form-control form-control-sm" required="required" />
                                            </td>
                                            <td>
                                                <input type="number" value="0" name="items[0].Amount" class="form-control form-control-sm" />
                                            </td>
                                            <td>
                                                <input type="number" value="0" name="items[0].Quantity" class="form-control form-control-sm" />
                                            </td>
                                            <td>
                                                <input type="number" name="items[0].Total" value="0" class="form-control form-control-sm readonly" readonly="" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="row justify-content-end">
                                <div class="col-md-4">
                                    <div class="mb-3">
                                        <label for="Tax" class="form-label">Tax (%)</label>
                                        <input type="number" class="form-control form-control-sm" runat="server" value="0" name="Tax" id="Tax" />
                                    </div>
                                    <div class="mb-3">
                                        <label for="TotalAmount" class="form-label">Grand Total</label>
                                        <input type="number" class="form-control form-control-sm readonly" value="0" name="TotalAmount" runat="server" id="TotalAmount" readonly="" />
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <asp:Button Text="Save" runat="server" OnClick="SaveButton_Click" class="btn btn-success float-end" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script>
            $(document).ready(function () {
                let rowNum = $("tbody tr").length;

                $("#addrow").click(function () {
                    let newRow = `<tr><td>${rowNum + 1}</td><td><input type="text" name="items[${rowNum}].Products" class="form-control form-control-sm" required></td><td><input type="number" value="0" name="items[${rowNum}].Amount" class="form-control form-control-sm"></td><td><input type="number" value="0" name="items[${rowNum}].Quantity" id="items[${rowNum}].Quantity" class="form-control form-control-sm"></td><td><input type="number" value="0" name="items[${rowNum}].Total" class="form-control form-control-sm readonly" readonly></td><td><button type="button" class="btn btn-sm btn-danger remove-row">Delete</button></td></tr>`;
                    $("tbody").append(newRow);
                    rowNum++;
                });

                $(document).on("click", ".remove-row", function () {
                    $(this).closest("tr").remove();
                    updateRowNumbers();
                    calculateGrandTotal();
                });

                function updateRowNumbers() {
                    $("tbody tr").each(function (index) {
                        $(this).find("td:first").text(index + 1);
                        $(this).find(`input[name^='items']`).each(function () {
                            let currentName = $(this).attr("name");
                            let newName = currentName.replace(/\d+/, index);
                            $(this).attr("name", newName);
                        });
                    });
                    rowNum--;
                }

                $(document).on("input", "input[name^='items'][name$='Amount']", function () {
                    let rowIndex = parseInt($(this).attr("name").match(/\d+/)[0]);
                    let price = parseFloat($(this).val());
                    let quant = parseInt($(`input[name = 'items[${rowIndex}].Quantity']`).val() || 0)
                    let total = isNaN(price) ? 0 : price * quant;
                    $(`input[name='items[${rowIndex}].Total']`).val(total.toFixed(2));
                    calculateGrandTotal();
                });
                $(document).on("input", "input[name^='items'][name$='Quantity']", function () {
                    let rowIndex = parseInt($(this).attr("name").match(/\d+/)[0]);
                    let price = parseFloat($(`input[name = 'items[${rowIndex}].Amount']`).val() || 0)
                    let quant = parseInt($(this).val());
                    let total = isNaN(price) ? 0 : price * quant;
                    $(`input[name='items[${rowIndex}].Total']`).val(total.toFixed(2));
                    calculateGrandTotal();
                });

                $(document).on("input", "#Tax", function () {
                    calculateGrandTotal();
                });

                function calculateGrandTotal() {
                    let grandTotal = 0;
                    $("input[name^='items'][name$='Total']").each(function () {
                        grandTotal += parseFloat($(this).val());
                    });
                    let tax = parseFloat($("#Tax").val()) || 0;
                    grandTotal += grandTotal * (tax / 100);
                    $("#TotalAmount").val(grandTotal.toFixed(2));
                }
            });
        </script>
    </form>
</body>
</html>
