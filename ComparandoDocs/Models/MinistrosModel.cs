using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using ComparandoDocs.Dao;

namespace ComparandoDocs.Models
{
    public class MinistrosModel
    {

        public ObservableCollection<Ministros> GetMinistros()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Modulo"].ConnectionString;
            ObservableCollection<Ministros> listaTesis = new ObservableCollection<Ministros>();

            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT * " +
                               "FROM Ministros";

            try
            {
                oleConne.Open();

                cmd = new OleDbCommand(sqlCadena, oleConne);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listaTesis.Add(new Ministros(reader["IdMinistro"] as int? ?? -1,reader["Ministro"].ToString()));
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
