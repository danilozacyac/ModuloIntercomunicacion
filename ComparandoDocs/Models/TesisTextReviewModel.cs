using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using ComparandoDocs.Dao;

namespace ComparandoDocs.Models
{
    public class TesisTextReviewModel
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["Modulo"].ConnectionString;

        public ObservableCollection<TesisTextReview> GetTesisList()
        {

            ObservableCollection<TesisTextReview> listaTesis = new ObservableCollection<TesisTextReview>();

            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT * " +
                               "FROM TesisCompara";

            try
            {
                oleConne.Open();

                cmd = new OleDbCommand(sqlCadena, oleConne);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TesisTextReview tesis = new TesisTextReview();
                        tesis.IdTesis = reader["Id"] as int? ?? -1;
                        tesis.DocOriginal = reader["TextoOriginal"].ToString();
                        tesis.DocRevision1 = reader["TextoRevision1"].ToString();
                        tesis.DocRevision2 = reader["TextoRevision2"].ToString();

                        tesis.DocOriginalPlano = reader["TOPlano"].ToString();
                        tesis.DocRevision1Plano = reader["TR1Plano"].ToString();
                        tesis.DocRevision2Plano = reader["TR2Plano"].ToString();
                        tesis.ClaveTesis = reader["ClaveTesis"].ToString();
                        tesis.Oficio = reader["Oficio"].ToString();
                        tesis.DocOriginalPath = reader["DocOriginalPath"].ToString();
                        tesis.DocRevision1Path = reader["DocRevision1Path"].ToString();
                        tesis.DocRevision2Path = reader["DocRevision2Path"].ToString();
                        tesis.Tatj = reader["tatj"] as int? ?? -1;

                        if (reader["FRecepcion"] == System.DBNull.Value)
                        {
                            tesis.FRecepcion = null;
                        }
                        else
                        {
                            tesis.FRecepcion = Convert.ToDateTime(reader["FRecepcion"]);
                        }

                        if (reader["FEnvio"] == DBNull.Value)
                        {
                            tesis.FEnvio = null;
                        }
                        else
                        {
                            tesis.FEnvio = Convert.ToDateTime(reader["FEnvio"]);
                        }
                        tesis.Instancia = reader["instancia"] as int? ?? -1;
                        tesis.Ministro = reader["ministro"] as int? ?? -1;
                        tesis.FRecepcionInt = reader["FRecepcionInt"] as int? ?? -1;
                        tesis.FEnvioInt = reader["FEnvioInt"] as int? ?? -1;
                        //sis.IdTesis = reader["IdOrg"] as int? ?? -1;

                        listaTesis.Add(tesis);
                    }
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                cmd.Dispose();
                reader.Close();
                oleConne.Close();
            }

            return listaTesis;
        }

        public int SetNewTesisReview(TesisTextReview tesis)
        {

            OleDbConnection connectionEpsOle = new OleDbConnection(connectionString);
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                DateTime today = DateTime.Now;
                string sqlCadena = "SELECT * FROM TesisCompara WHERE Id = 0";
                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionEpsOle);

                dataAdapter.Fill(dataSet, "TesisCompara");

                dr = dataSet.Tables["TesisCompara"].NewRow();
                dr["TextoOriginal"] = tesis.DocOriginal;
                dr["TOPlano"] = tesis.DocOriginalPlano;
                dr["TextoRevision1"] = tesis.DocRevision1;
                dr["TR1Plano"] = tesis.DocRevision1Plano;
                dr["TextoRevision2"] = tesis.DocRevision2;
                dr["TR2Plano"] = tesis.DocRevision2Plano;
                dr["FechaInt"] = today.Year + this.GetTwoDigitFormat(today.Month) + this.GetTwoDigitFormat(today.Day);
                dr["Dia"] = today.Day;
                dr["Mes"] = today.Month;
                dr["Anio"] = today.Year;
                dr["Fecha"] = today.ToShortDateString();
                dr["ClaveTesis"] = tesis.ClaveTesis;
                dr["Oficio"] = tesis.Oficio;

                if (tesis.FRecepcion == null)
                {
                    dr["FRecepcion"] = DBNull.Value;
                    dr["FRecepcionInt"] = "000000";
                }
                else
                {
                    dr["FRecepcion"] = tesis.FRecepcion;
                    dr["FRecepcionInt"] = tesis.FRecepcion.Value.Year.ToString() + this.GetTwoDigitFormat(tesis.FRecepcion.Value.Month) + this.GetTwoDigitFormat(tesis.FRecepcion.Value.Day);
                }

                if (tesis.FEnvio == null)
                {
                    dr["FEnvio"] = DBNull.Value;
                    dr["FEnvioInt"] = "000000";
                }
                else
                {
                    dr["FEnvio"] = tesis.FEnvio;
                    dr["FEnvioInt"] = tesis.FEnvio.Value.Year.ToString() + this.GetTwoDigitFormat(tesis.FEnvio.Value.Month) + this.GetTwoDigitFormat(tesis.FEnvio.Value.Day);
                }

                dr["Instancia"] = tesis.Instancia;
                dr["Ministro"] = tesis.Ministro;
                dr["DocOriginalPath"] = tesis.DocOriginalPath;
                dr["DocRevision1Path"] = tesis.DocRevision1Path;
                dr["DocRevision2Path"] = tesis.DocRevision2Path;
                dr["tatj"] = tesis.Tatj;

                dataSet.Tables["TesisCompara"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionEpsOle.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO TesisCompara (TextoOriginal,TOPlano,TextoRevision1,TR1Plano,TextoRevision2,TR2Plano,FechaInt,Dia,Mes,Anio,Fecha,ClaveTesis,Oficio,FRecepcion,FRecepcionInt,FEnvio,FEnvioInt,Instancia,Ministro,DocOriginalPath,DocRevision1Path,DocRevision2Path,tatj)" +
                                                        " VALUES (@TextoOriginal,@TOPlano,@TextoRevision1,@TR1Plano,@TextoRevision2,@TR2Plano,@FechaInt,@Dia,@Mes,@Anio,@Fecha,@ClaveTesis,@Oficio,@FRecepcion,@FRecepcionInt,@FEnvio,@FEnvioInt,@Instancia,@Ministro,@DocOriginalPath,@DocRevision1Path,@DocRevision2Path,@tatj)";

                dataAdapter.InsertCommand.Parameters.Add("@TextoOriginal", OleDbType.VarChar, 0, "TextoOriginal");
                dataAdapter.InsertCommand.Parameters.Add("@TOPlano", OleDbType.VarChar, 0, "TOPlano");
                dataAdapter.InsertCommand.Parameters.Add("@TextoRevision1", OleDbType.VarChar, 0, "TextoRevision1");
                dataAdapter.InsertCommand.Parameters.Add("@TR1Plano", OleDbType.VarChar, 0, "TR1Plano");
                dataAdapter.InsertCommand.Parameters.Add("@TextoRevision2", OleDbType.VarChar, 0, "TextoRevision2");
                dataAdapter.InsertCommand.Parameters.Add("@TR2Plano", OleDbType.VarChar, 0, "TR2Plano");
                dataAdapter.InsertCommand.Parameters.Add("@FechaInt", OleDbType.Numeric, 0, "FechaInt");
                dataAdapter.InsertCommand.Parameters.Add("@Dia", OleDbType.Numeric, 0, "Dia");
                dataAdapter.InsertCommand.Parameters.Add("@Mes", OleDbType.Numeric, 0, "Mes");
                dataAdapter.InsertCommand.Parameters.Add("@Anio", OleDbType.Numeric, 0, "Anio");
                dataAdapter.InsertCommand.Parameters.Add("@Fecha", OleDbType.Date, 0, "Fecha");
                dataAdapter.InsertCommand.Parameters.Add("@ClaveTesis", OleDbType.VarChar, 0, "ClaveTesis");
                dataAdapter.InsertCommand.Parameters.Add("@Oficio", OleDbType.VarChar, 0, "Oficio");
                dataAdapter.InsertCommand.Parameters.Add("@FRecepcion", OleDbType.Date, 0, "FRecepcion");
                dataAdapter.InsertCommand.Parameters.Add("@FRecepcionInt", OleDbType.Numeric, 0, "FRecepcionInt");
                dataAdapter.InsertCommand.Parameters.Add("@FEnvio", OleDbType.Date, 0, "FEnvio");
                dataAdapter.InsertCommand.Parameters.Add("@FEnvioInt", OleDbType.Numeric, 0, "FEnvioInt");
                dataAdapter.InsertCommand.Parameters.Add("@Instancia", OleDbType.Numeric, 0, "Instancia");
                dataAdapter.InsertCommand.Parameters.Add("@Ministro", OleDbType.Numeric, 0, "Ministro");
                dataAdapter.InsertCommand.Parameters.Add("@DocOriginalPath", OleDbType.VarChar, 0, "DocOriginalPath");
                dataAdapter.InsertCommand.Parameters.Add("@DocRevision1Path", OleDbType.VarChar, 0, "DocRevision1Path");
                dataAdapter.InsertCommand.Parameters.Add("@DocRevision2Path", OleDbType.VarChar, 0, "DocRevision2Path");
                dataAdapter.InsertCommand.Parameters.Add("@tatj", OleDbType.Numeric, 0, "tatj");

                dataAdapter.Update(dataSet, "TesisCompara");

                dataSet.Dispose();
                dataAdapter.Dispose();
                connectionEpsOle.Close();

                tesis.IdTesis = this.GetLatInsertId(tesis.ClaveTesis);
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionEpsOle.Close();
            }
            return tesis.IdTesis;
        }

        public int UpdateTesisReview(TesisTextReview tesis)
        {

            OleDbConnection connectionEpsOle = new OleDbConnection(connectionString);
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                DateTime today = DateTime.Now;
                string sqlCadena = "SELECT * FROM TesisCompara WHERE Id = " + tesis.IdTesis;
                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionEpsOle);

                dataAdapter.Fill(dataSet, "TesisCompara");

                dr = dataSet.Tables["TesisCompara"].Rows[0];
                dr.BeginEdit();
                dr["TextoOriginal"] = tesis.DocOriginal;
                dr["TOPlano"] = tesis.DocOriginalPlano;
                dr["TextoRevision1"] = tesis.DocRevision1;
                dr["TR1Plano"] = tesis.DocRevision1Plano;
                dr["TextoRevision2"] = tesis.DocRevision2;
                dr["TR2Plano"] = tesis.DocRevision2Plano;
                dr["FechaInt"] = today.Year + this.GetTwoDigitFormat(today.Month) + this.GetTwoDigitFormat(today.Day);
                dr["Dia"] = today.Day;
                dr["Mes"] = today.Month;
                dr["Anio"] = today.Year;
                dr["Fecha"] = today.ToShortDateString();
                dr["ClaveTesis"] = tesis.ClaveTesis;
                dr["Oficio"] = tesis.Oficio;
                if (tesis.FRecepcion == null)
                {
                    dr["FRecepcion"] = DBNull.Value;
                    dr["FRecepcionInt"] = "000000";
                }
                else
                {
                    dr["FRecepcion"] = tesis.FRecepcion;
                    dr["FRecepcionInt"] = tesis.FRecepcion.Value.Year.ToString() + this.GetTwoDigitFormat(tesis.FRecepcion.Value.Month) + this.GetTwoDigitFormat(tesis.FRecepcion.Value.Day);
                }

                if (tesis.FEnvio == null)
                {
                    dr["FEnvio"] = DBNull.Value;
                    dr["FEnvioInt"] = "000000";
                }
                else
                {
                    dr["FEnvio"] = tesis.FEnvio;
                    dr["FEnvioInt"] = tesis.FEnvio.Value.Year.ToString() + this.GetTwoDigitFormat(tesis.FEnvio.Value.Month) + this.GetTwoDigitFormat(tesis.FEnvio.Value.Day);
                } 
                
                dr["Instancia"] = tesis.Instancia;
                dr["Ministro"] = tesis.Ministro;
                dr["DocOriginalPath"] = tesis.DocOriginalPath;
                dr["DocRevision1Path"] = tesis.DocRevision1Path;
                dr["DocRevision2Path"] = tesis.DocRevision2Path;
                dr["tatj"] = tesis.Tatj;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionEpsOle.CreateCommand();
                dataAdapter.UpdateCommand.CommandText = "UPDATE TesisCompara SET TextoOriginal = @TextoOrigina,TOPlano = @TOPlano,TextoRevision1 = @TextoRevision1," + 
                                                        "TR1Plano = @TR1Plano,TextoRevision2 = @TextoRevision2,TR2Plano = @TR2Plano,FechaInt = @FechaInt,Dia = @Dia," + 
                                                        "Mes = @Mes,Anio = @Anio,Fecha = @Fecha,ClaveTesis = @ClaveTesis,Oficio = @Oficio,FRecepcion = @FRecepcion," + 
                                                        "FRecepcionInt = @FRecepcionInt,FEnvio = @FEnvio,FEnvioInt = @FEnvioInt,Instancia = @Instancia,Ministro = @Ministro," +
                                                        "DocOriginalPath = @DocOriginalPath, DocRevision1Path = @DocRevision1Path, DocRevision2Path = @DocRevision2Path, tatj = @tatj " +
                                                        " WHERE Id = @Id";

                dataAdapter.UpdateCommand.Parameters.Add("@TextoOriginal", OleDbType.VarChar, 0, "TextoOriginal");
                dataAdapter.UpdateCommand.Parameters.Add("@TOPlano", OleDbType.VarChar, 0, "TOPlano");
                dataAdapter.UpdateCommand.Parameters.Add("@TextoRevision1", OleDbType.VarChar, 0, "TextoRevision1");
                dataAdapter.UpdateCommand.Parameters.Add("@TR1Plano", OleDbType.VarChar, 0, "TR1Plano");
                dataAdapter.UpdateCommand.Parameters.Add("@TextoRevision2", OleDbType.VarChar, 0, "TextoRevision2");
                dataAdapter.UpdateCommand.Parameters.Add("@TR2Plano", OleDbType.VarChar, 0, "TR2Plano");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaInt", OleDbType.Numeric, 0, "FechaInt");
                dataAdapter.UpdateCommand.Parameters.Add("@Dia", OleDbType.Numeric, 0, "Dia");
                dataAdapter.UpdateCommand.Parameters.Add("@Mes", OleDbType.Numeric, 0, "Mes");
                dataAdapter.UpdateCommand.Parameters.Add("@Anio", OleDbType.Numeric, 0, "Anio");
                dataAdapter.UpdateCommand.Parameters.Add("@Fecha", OleDbType.Date, 0, "Fecha");
                dataAdapter.UpdateCommand.Parameters.Add("@ClaveTesis", OleDbType.VarChar, 0, "ClaveTesis");
                dataAdapter.UpdateCommand.Parameters.Add("@Oficio", OleDbType.VarChar, 0, "Oficio");
                dataAdapter.UpdateCommand.Parameters.Add("@FRecepcion", OleDbType.Date, 0, "FRecepcion");
                dataAdapter.UpdateCommand.Parameters.Add("@FRecepcionInt", OleDbType.Numeric, 0, "FRecepcionInt");
                dataAdapter.UpdateCommand.Parameters.Add("@FEnvio", OleDbType.Date, 0, "FEnvio");
                dataAdapter.UpdateCommand.Parameters.Add("@FEnvioInt", OleDbType.Numeric, 0, "FEnvioInt");
                dataAdapter.UpdateCommand.Parameters.Add("@Instancia", OleDbType.Numeric, 0, "Instancia");
                dataAdapter.UpdateCommand.Parameters.Add("@Ministro", OleDbType.Numeric, 0, "Ministro");
                dataAdapter.UpdateCommand.Parameters.Add("@DocOriginalPath", OleDbType.VarChar, 0, "DocOriginalPath");
                dataAdapter.UpdateCommand.Parameters.Add("@DocRevision1Path", OleDbType.VarChar, 0, "DocRevision1Path");
                dataAdapter.UpdateCommand.Parameters.Add("@DocRevision2Path", OleDbType.VarChar, 0, "DocRevision2Path");
                dataAdapter.UpdateCommand.Parameters.Add("@tatj", OleDbType.Numeric, 0, "tatj");
                dataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                

                dataAdapter.Update(dataSet, "TesisCompara");

                dataSet.Dispose();
                dataAdapter.Dispose();
                connectionEpsOle.Close();

            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connectionEpsOle.Close();
            }
            return tesis.IdTesis;
        }

        public void DeleteTesisReview(TesisTextReview tesis)
        {

            OleDbConnection connection = new OleDbConnection(connectionString);

            try
            {
                OleDbCommand sqlComm = new OleDbCommand();
                sqlComm = connection.CreateCommand();
                sqlComm.CommandText = @"DELETE FROM TEsisCompara WHERE id = @IdTesis";
                sqlComm.Parameters.Add("@IdTesis", OleDbType.Numeric);
                sqlComm.Parameters["@IdTesis"].Value = tesis.IdTesis;
                connection.Open();
                sqlComm.ExecuteNonQuery();
                connection.Close();
                
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connection.Close();
            }
        }

        private string GetTwoDigitFormat(int diaMes)
        {
            if (diaMes < 10)
                return "0" + diaMes;
            else
                return diaMes.ToString();
        }

        public int GetLatInsertId(string claveTesis)
        {
            int id = 0;

            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            String sqlCadena = "SELECT Id FROM TesisCompara WHERE ClaveTesis = '" + claveTesis + "'";

            try
            {
                oleConne.Open();

                cmd = new OleDbCommand(sqlCadena, oleConne);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["Id"]);
                    }
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                cmd.Dispose();
                reader.Close();
                oleConne.Close();
            }

            return id;
        }
    }
}