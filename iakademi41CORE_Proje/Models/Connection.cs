using Microsoft.Data.SqlClient;

namespace iakademi41CORE_Proje.Models
{
    public class Connection
    {
        public static SqlConnection ServerConnect
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=LAPTOP-49905JF2\\SQLEXPRESS;Database=iakademi41Core_projeDB;TrustServerCertificate=True;");
                 return sqlcon;
            }
        }
    }
}
