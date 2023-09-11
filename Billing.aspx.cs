using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BillingAspx
{
    public partial class Billing : System.Web.UI.Page
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["BookingSoftDbContext"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        protected void BindGridView()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlQuery = "SELECT * FROM Travel2Bills";
            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                GridViewBills.DataSource = reader;
                GridViewBills.DataBind();
            }
            reader?.Close();
            connection.Close();
        }

        protected void LinkButtonRemove_Click(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            string delid = linkButton.CommandArgument;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql1 = "DELETE FROM Travel2Bills WHERE InvoiceNo ='" + delid + "';";
                SqlCommand cmd1 = new SqlCommand(sql1, connection);
                cmd1.ExecuteNonQuery();
                string sqlQuery = "DELETE FROM Bill2Items WHERE InvoiceNo ='" + delid + "';";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect(Request.RawUrl);
        }
    }

}