using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BillingAspx
{
    public partial class BillCreate : System.Web.UI.Page
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["BookingSoftDbContext"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string sqlQuery = "SELECT TOP 1 InvoiceNo FROM Travel2Bills ORDER BY Id DESC";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                string lInvoice = cmd.ExecuteScalar()?.ToString();
                if (String.IsNullOrEmpty(lInvoice))
                {
                    lInvoice = "INV-0000";
                }
                int lastId = int.Parse(lInvoice.Substring(4));
                int newId = lastId + 1;
                string newInvoiceNo = "INV-" + newId.ToString("0000");
                txtInvoiceNo.Text = newInvoiceNo;
                txtInvoiceDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                connection.Close();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            List<InvoiceItem> items = new List<InvoiceItem>();

            for (int i = 0; i < 1000; i++)
            {
                if (Request.Form["items[" + i + "].Products"] == null)
                {
                    break;
                }
                InvoiceItem item = new InvoiceItem();
                item.Products = Request.Form["items[" + i + "].Products"];
                item.Amount = decimal.Parse(Request.Form["items[" + i + "].Amount"]);
                item.Quantity = int.Parse(Request.Form["items[" + i + "].Quantity"]);
                item.Total = decimal.Parse(Request.Form["items[" + i + "].Total"]);
                items.Add(item);
            }
            SaveItemsToDatabase(items);
            Response.Redirect("/Billing.aspx");
        }
        private void SaveItemsToDatabase(List<InvoiceItem> items)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql1 = "INSERT INTO Travel2Bills (InvoiceNo, InvoiceDate, CustomerName, CustomerAddress, CustomerEmail, CustomerPhone, TotalAmount, Tax) VALUES(@InvoiceNo, @InvoiceDate, @CustomerName, @CustomerAddress, @CustomerEmail, @CustomerPhone, @TotalAmount, @Tax)";
                SqlCommand cmd1 = new SqlCommand(sql1, connection);
                cmd1.Parameters.AddWithValue("@InvoiceNo", txtInvoiceNo.Text);
                cmd1.Parameters.AddWithValue("@InvoiceDate", DateTime.ParseExact(txtInvoiceDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture));
                cmd1.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                cmd1.Parameters.AddWithValue("@CustomerAddress", txtCustomerAddress.Text);
                cmd1.Parameters.AddWithValue("@CustomerEmail", txtCustomerEmail.Text);
                cmd1.Parameters.AddWithValue("@CustomerPhone", txtCustomerPhone.Text);
                cmd1.Parameters.AddWithValue("@TotalAmount", decimal.Parse(TotalAmount.Value));
                cmd1.Parameters.AddWithValue("@Tax", decimal.Parse(Tax.Value));
                cmd1.ExecuteNonQuery();
                foreach (var item in items)
                {
                    string sqlQuery = "INSERT INTO Bill2Items (InvoiceNo, Products, Amount, Quantity, Total) VALUES (@InvoiceNo, @Products, @Amount, @Quantity, @Total)";
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@InvoiceNo", txtInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("@Products", item.Products);
                    cmd.Parameters.AddWithValue("@Amount", item.Amount);
                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@Total", item.Total);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        

    }
}