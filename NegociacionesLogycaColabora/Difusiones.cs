using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegociacionesLogycaColabora
{
	public class Difusiones
	{
		public DataTable ListarDifusionesES()
		{
			string SQL = @"select 
	                            f08_observacion f08_descripción,
	                            f08_codigo,
                                f08_cod_cluster
                            from 
	                            t08_difusiones
                            where 
                                f08_activo=1"
			;


			DataTable res = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionPortafolio);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new DataTable();
					res.Load(dr);
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar difusiones: " + ex.Message);
			}
			return res;
		}

		public DataTable ListarPortafoliosDifusion(int difusion)
		{
			string SQL = @"select 
	                            f09_cod_portafolio,
                                f09_co
                            from 
	                            t08_difusiones
	                            inner join t09_co_difusiones on f09_cod_difusion=f08_codigo
                            where
	                            f08_codigo=@cod";

			DataTable res = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionPortafolio);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@cod", difusion);
				// cmd.Parameters.AddWithValue("@cluster", cluster);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new DataTable();
					res.Load(dr);
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar centros operación difusión: " + ex.Message);
			}
			return res;
		}

		public object ObtenerValorConfiguracion(int id)
		{
			string SQL = @"select
	                            f13_valor
                            from
	                            t13_configuracion
                            where
	                            f13_id=@id";
			object res = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionPortafolio);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@id", id);
				res = cmd.ExecuteScalar();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener valor de configuración: " + ex.Message);
			}
			return res;
		}
	}
}
