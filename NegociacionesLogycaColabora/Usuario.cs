using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NegociacionesLogycaColabora
{
    public class Usuario
    {

        public int IniciarSesionAdmin(string usuario, string contraseña)
        {
            int res = -1;
            string SQL = "SELECT " +
                            "us_tipo " +
                         "FROM " +
                            "usuarios " +
                         "WHERE " +
                            "us_nombre=@nomb AND " +
                            "us_contraseña=@contra AND " +
                         "us_tipo=1";
            object obj=null;
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nomb", usuario);
                cmd.Parameters.AddWithValue("@contra", contraseña);
                obj=cmd.ExecuteScalar();
                if (!Convert.IsDBNull(obj))
                {
                    res = Convert.ToInt32(obj);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión: " + ex.Message);
            }
            
            return res;
        }

        public string[] IniciarSesion(string usuario, string contraseña)
        {
            string SQL = "SELECT " +
                            "co_gln, " +
                            "co_nombre, " +
                            "us_tipo " +
                        "FROM " +
                            "usuarios " +
                            "INNER JOIN compradores ON co_usuario=us_id " +
                        "WHERE " +
                            "us_nombre=@nomb AND " +
                            "us_contraseña=@contra AND " +
                            "us_tipo IN(2,3,4)";
            string[] res = null;
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nomb", usuario);
                cmd.Parameters.AddWithValue("@contra", contraseña);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    res = new string[3];
                    dr.Read();
                    res[0] = dr.GetString(0);
                    res[1] = dr.GetString(1);
                    res[2] = dr.GetInt32(2).ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión: " + ex.Message);
            }

            return res;
        }

        public void CrearUsuario(string nombre, string contraseña, int tipo)
        {
            string SQL = "BEGIN TRY " +
                        "IF NOT EXISTS(SELECT us_nombre FROM Usuarios WHERE us_nombre=@NOMB) " +
                        "BEGIN " +
                            "INSERT INTO Usuarios (us_nombre, us_contraseña, us_tipo) VALUES(@NOMB, @CONTRA,@TIPO) " +
                        "END " +
                        "ELSE " +
                        "BEGIN " +
                            "RAISERROR('El usuario ya existe.', 16, 10) " +
                        "END " +
                    "END TRY " +
                    "BEGIN CATCH " +
                        "DECLARE @ErrorMessage NVARCHAR(4000); " +
                        "DECLARE @ErrorSeverity INT; " +
                        "DECLARE @ErrorState INT; " +
                        "SELECT @ErrorMessage = ERROR_MESSAGE(), " +
                               "@ErrorSeverity = ERROR_SEVERITY(), " +
                               "@ErrorState = ERROR_STATE(); " +
                               "RAISERROR (@ErrorMessage, " +
                                   "@ErrorSeverity, " +
                                   "@ErrorState " +
                                   "); " +
                    "END CATCH";
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NOMB", nombre);
                cmd.Parameters.AddWithValue("@CONTRA", contraseña);
                cmd.Parameters.AddWithValue("@TIPO", tipo);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario: " + ex.Message);
            }
        }

        public void ActualizarContraseñaUsuario(string nombre, string contraseñaactual, string nuevacontraseña)
        {
            string SQL = "BEGIN TRY " +
                        "UPDATE Usuarios SET us_contraseña=@NUEVA WHERE us_nombre=@NOMB AND us_contraseña=@ACT " +
                    "END TRY " +
                    "BEGIN CATCH " +
                        "DECLARE @ErrorMessage NVARCHAR(4000); " +
                        "DECLARE @ErrorSeverity INT; " +
                        "DECLARE @ErrorState INT; " +
                        "SELECT @ErrorMessage = ERROR_MESSAGE(), " +
                               "@ErrorSeverity = ERROR_SEVERITY(), " +
                               "@ErrorState = ERROR_STATE(); " +
                               "RAISERROR (@ErrorMessage, " +
                                   "@ErrorSeverity, " +
                                   "@ErrorState " +
                                   "); " +
                    "END CATCH";
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NOMB", nombre);
                cmd.Parameters.AddWithValue("@ACT", contraseñaactual);
                cmd.Parameters.AddWithValue("@NUEVA", nuevacontraseña);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la contraseña: " + ex.Message);
            }
        }

        public DataTable ListarUsuarios()
        {
            string SQL = "SELECT us_nombre, us_contraseña, " +
                "CASE us_tipo WHEN 2 THEN 'Comprador' WHEN 3 THEN 'Comprador Universal' END us_tipo FROM Usuarios WHERE us_tipo IN (2,3,4)";
            DataTable dt = null;
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable("Usuarios");
                da.Fill(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado de usuarios: " + ex.Message);
            }
            return dt;
        }
    }
}
