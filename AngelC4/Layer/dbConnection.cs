using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AngelC4
{
    class dbConnection
    {
        SqlConnection conexion;

        public dbConnection()
        {
        // Get HostName
        String hostname = Dns.GetHostName();
        conexion = new SqlConnection("server=" + hostname + "; database=TestDB ; integrated security = true");
        }
       
  
    }
}
