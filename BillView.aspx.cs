using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BillingAspx
{
    public partial class BillView : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["BookingSoftDbContext"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idValue = Request.QueryString["id"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"SELECT * FROM Travel2Bills WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@Id", idValue);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        CustomerNameLabel.Text = reader["CustomerName"].ToString();
                        CustomerEmailLabel.Text = reader["CustomerEmail"].ToString();
                        CustomerPhoneLabel.Text = reader["CustomerPhone"].ToString();
                        CustomerAddressLabel.Text = reader["CustomerAddress"].ToString();
                        InvoiceNoLabel.Text = reader["InvoiceNo"].ToString();
                        InvoiceDateLabel.Text = ((DateTime)reader["InvoiceDate"]).ToString("dd-MM-yyyy");
                        TaxLabel.Text = reader["Tax"].ToString();
                        GrandTotalLabel.Text = reader["TotalAmount"].ToString();
                    }
                    reader.Close();
                    string query2 = "Select SUM(Total) as Totalf FROM Bill2Items WHERE InvoiceNo = @InvNo";
                    SqlCommand cmd2 = new SqlCommand(query2, connection);
                    cmd2.Parameters.AddWithValue("@InvNo", InvoiceNoLabel.Text);
                    TotalLabel.Text = cmd2.ExecuteScalar().ToString();
                }
                GetBillItems();
            }

        }

        private void GetBillItems()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = $"SELECT * FROM Bill2Items WHERE InvoiceNo = @InvNo";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@InvNo", InvoiceNoLabel.Text);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    GridViewBillItems.DataSource = reader;
                    GridViewBillItems.DataBind();
                }
                reader.Close();
            }
        }
    }
}