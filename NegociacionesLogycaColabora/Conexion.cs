using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
namespace NegociacionesLogycaColabora
{
    public class Conexion
    {
		//public static string RaizArchivos = "\\\\Unoeetweb\\pricat\\";
		public static string RaizArchivos = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\LOGYCA\\";
		public static string CadenaConexionUnoee = ConfigurationManager.ConnectionStrings["unoee"].ConnectionString;
        public static string CadenaConexionLogyca = ConfigurationManager.ConnectionStrings["logyca"].ConnectionString;
        public static string CadenaConexionPortafolio = ConfigurationManager.ConnectionStrings["portafolio"].ConnectionString;
	}
}
