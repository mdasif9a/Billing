using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BillingAspx
{
    public partial class BillEdit : System.Web.UI.Page
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["BookingSoftDbContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int invoiceId;
                    if (int.TryParse(Request.QueryString["id"], out invoiceId))
                    {
                        LoadInvoiceForEditing(invoiceId);
                    }
                }
            }
        }

        private void LoadInvoiceForEditing(int invoiceId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM Travel2Bills WHERE Id = @InvoiceId";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtInvoiceNo.Text = reader["InvoiceNo"].ToString();
                    txtInvoiceDate.Text = ((DateTime)reader["InvoiceDate"]).ToString("dd-MM-yyyy");
                    txtCustomerName.Text = reader["CustomerName"].ToString();
                    txtCustomerAddress.Text = reader["CustomerAddress"].ToString();
                    txtCustomerEmail.Text = reader["CustomerEmail"].ToString();
                    txtCustomerPhone.Text = reader["CustomerPhone"].ToString();
                    Tax.Value = reader["Tax"].ToString();
                    TotalAmount.Value = reader["TotalAmount"].ToString();
                }
            }
            GenerateHtmlTable(txtInvoiceNo.Text);
        }

        private void UpdateInvoice(int invoiceId, List<InvoiceItem> items)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "UPDATE Travel2Bills SET InvoiceDate = @InvoiceDate, CustomerName = @CustomerName, CustomerAddress = @CustomerAddress, CustomerEmail = @CustomerEmail, CustomerPhone = @CustomerPhone, Tax = @Tax, TotalAmount = @TotalAmount WHERE Id = @InvoiceId";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.ParseExact(txtInvoiceDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                cmd.Parameters.AddWithValue("@CustomerAddress", txtCustomerAddress.Text);
                cmd.Parameters.AddWithValue("@CustomerEmail", txtCustomerEmail.Text);
                cmd.Parameters.AddWithValue("@CustomerPhone", txtCustomerPhone.Text);
                cmd.Parameters.AddWithValue("@Tax", decimal.Parse(Tax.Value));
                cmd.Parameters.AddWithValue("@TotalAmount", decimal.Parse(TotalAmount.Value));
                cmd.ExecuteNonQuery();
                foreach (var item in items)
                {
                    string sql2;
                    if (item.Id == 0)
                    {
                        sql2 = "INSERT INTO Bill2Items (InvoiceNo, Products, Amount, Quantity, Total) VALUES (@InvoiceNo, @Products, @Amount, @Quantity, @Total)";
                    }
                    else
                    {
                        sql2 = "UPDATE Bill2Items SET InvoiceNo = @InvoiceNo, Products = @Products, Amount = @Amount, Quantity = @Quantity, Total = @Total WHERE ItemID = @Id";
                    }
                    SqlCommand cmd2 = new SqlCommand(sql2, connection);
                    cmd2.Parameters.AddWithValue("@InvoiceNo", txtInvoiceNo.Text);
                    cmd2.Parameters.AddWithValue("@Products", item.Products);
                    cmd2.Parameters.AddWithValue("@Amount", item.Amount);
                    cmd2.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd2.Parameters.AddWithValue("@Total", item.Total);
                    if (item.Id != 0)
                    {
                        cmd2.Parameters.AddWithValue("@Id", item.Id);
                    }
                    cmd2.ExecuteNonQuery();
                }
            }

        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            List<InvoiceItem> items = new List<InvoiceItem>();

            for (int i = 0; i < 1000; i++)
            {
                if (Request.Form["items[" + i + "].Products"] == null)
                {
                    break;
                }
                InvoiceItem item = new InvoiceItem();
                item.Id = int.Parse(Request.Form["items[" + i + "].Id"]);
                item.Products = Request.Form["items[" + i + "].Products"];
                item.Amount = decimal.Parse(Request.Form["items[" + i + "].Amount"]);
                item.Quantity = int.Parse(Request.Form["items[" + i + "].Quantity"]);
                item.Total = decimal.Parse(Request.Form["items[" + i + "].Total"]);
                items.Add(item);
            }
            int invoiceId = int.Parse(Request.QueryString["id"]);
            UpdateInvoice(invoiceId, items);
            Response.Redirect("/Billing.aspx");
        }

        private void GenerateHtmlTable(string InvNo)
        {
            System.Text.StringBuilder tableHtml = new System.Text.StringBuilder();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM Bill2Items WHERE InvoiceNo = @InvoiceNo";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@InvoiceNo", InvNo);
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    tableHtml.Append("<tr>");
                    tableHtml.Append("<td>" + (i + 1) + "</td>");
                    tableHtml.Append("<td><input type='hidden' name='items[" + i + "].Id' value='" + reader["ItemID"].ToString() + "' />");
                    tableHtml.Append("<input type='text' name='items[" + i + "].Products' class='form-control form-control-sm' required='required' value='" + reader["Products"].ToString() + "' /></td>");
                    tableHtml.Append("<td><input type='number' name='items[" + i + "].Amount' class='form-control form-control-sm' value='" + reader["Amount"].ToString() + "' /></td>");
                    tableHtml.Append("<td><input type='number' name='items[" + i + "].Quantity' class='form-control form-control-sm' value='" + reader["Quantity"].ToString() + "' /></td>");
                    tableHtml.Append("<td><input type='number' name='items[" + i + "].Total' class='form-control form-control-sm readonly' readonly='' value='" + reader["Total"].ToString() + "' /></td>");
                    tableHtml.Append("<td></td>");
                    tableHtml.Append("</tr>");
                    i++;
                }
            }
            litTable.Text = tableHtml.ToString();
        }
    }
}