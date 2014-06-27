using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using ComparandoDocs.Dao;

namespace ComparandoDocs.Models
{
    public class InstanciasModel
    {


        public ObservableCollection<Instancias> GetInstancias()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Modulo"].ConnectionString;
            ObservableCollection<Instancias> listaTesis = new ObservableCollection<Instancias>();

            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT * " +
                               "FROM Instancias ORDER BY IdInstancia";

            try
            {
                oleConne.Open();

                cmd = new OleDbCommand(sqlCadena, oleConne);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listaTesis.Add(new Instancias(reader["IdInstancia"] as int? ?? -1, reader["Instancia"].ToString()));
                    }
                }
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                cmd.Dispose();
                reader.Close();
                oleConne.Close();
            }

            return listaTesis;
        }
    }
}
