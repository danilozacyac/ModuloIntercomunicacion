using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ComparandoDocs.Dao;
using ComparandoDocs.Reporting;
using ComparandoDocs.Utilities;

namespace ModuloIntercomunicacion
{
    /// <summary>
    /// Lógica de interacción para SeleccionaPeriodoImpresion.xaml
    /// </summary>
    public partial class SeleccionaPeriodoImpresion : Window
    {
        RadioButton selectedRadio = null;
        ObservableCollection<TesisTextReview> listaTesis;


        public SeleccionaPeriodoImpresion(ObservableCollection<TesisTextReview> listaTesis)
        {
            InitializeComponent();
            this.listaTesis = listaTesis;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int year = DateTime.Now.Year;
            while (year <= 2014)
            {
                CbxAnio.Items.Add(year);
                year++;
            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnContinuar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRadio == null)
            {
                MessageBox.Show("Seleccione el periodo del reporte que desea generar");
                return;
            }

            int mes = Convert.ToInt16(selectedRadio.Name.Replace("Rad",""));

            int periodoInicio;
            int periodoFinal;
            if (mes < 13)
            {
                periodoInicio = Convert.ToInt32( CbxAnio.Text + this.GetTwoDigitFormat(mes) + 01);
                periodoFinal = Convert.ToInt32(CbxAnio.Text + this.GetTwoDigitFormat(mes) + 32) ;
            }
            else
            {
                periodoInicio = Convert.ToInt32(CbxAnio.Text + "0101");
                periodoFinal = Convert.ToInt32(CbxAnio.Text + "1231");
            }

            ObservableCollection<TesisTextReview> listaImprimir = (from n in listaTesis
                                                                   where n.FEnvioInt >= periodoInicio && n.FEnvioInt <= periodoFinal
                                                                   select n).ToList().ToObservableCollection();

            TesisRtfWordTable rtf = new TesisRtfWordTable(listaImprimir);
            rtf.GeneraWord();

            this.Close();
        }

        private void Rad_Checked(object sender, RoutedEventArgs e)
        {
            selectedRadio = sender as RadioButton;
        }

        private string GetTwoDigitFormat(int diaMes)
        {
            if (diaMes < 10)
                return "0" + diaMes;
            else
                return diaMes.ToString();
        }
    }
}
