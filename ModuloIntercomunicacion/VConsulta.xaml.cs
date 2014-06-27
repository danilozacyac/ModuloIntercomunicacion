using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ComparandoDocs;
using ComparandoDocs.Dao;
using ComparandoDocs.Models;
using ComparandoDocs.Singleton;

namespace ModuloIntercomunicacion
{
    /// <summary>
    /// Lógica de interacción para VConsulta.xaml
    /// </summary>
    public partial class VConsulta : Window
    {
        private TesisTextReview selectedTesis;


        public VConsulta()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GridTesisReview.DataContext = TesisSingleton.Tesis;

            CbxInstancia.DataContext = InstanciasSingleton.InstanciasS;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            VersionComparerWin win = new VersionComparerWin();
            win.ShowDialog();
        }

        private void GridTesisReview_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedTesis = GridTesisReview.SelectedItem as TesisTextReview;
        }

        private void BtnGeneraWord_Click(object sender, RoutedEventArgs e)
        {
            SeleccionaPeriodoImpresion periodo = new SeleccionaPeriodoImpresion(TesisSingleton.Tesis);
            periodo.ShowDialog();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTesis != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Estas seguro de querer eliminar este registro", "ATENCIÓN:", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    TesisTextReviewModel model = new TesisTextReviewModel();
                    model.DeleteTesisReview(selectedTesis);

                    TesisSingleton.Tesis.Remove(selectedTesis);
                }
            }
            else
            {
                MessageBox.Show("Seleccione la tesis que desea eliminar");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTesis == null)
            {
                MessageBox.Show("Seleccione la tesis que desea actualizar");
            }
            else
            {
                VersionComparerWin win = new VersionComparerWin(selectedTesis);
                win.ShowDialog();
            }
        }

        private void CbxInstancia_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Instancias inst = CbxInstancia.SelectedItem as Instancias;

            if (inst.IdInstancia != 5)
            {
                GridTesisReview.DataContext = (from n in TesisSingleton.Tesis
                                               where n.Instancia == inst.IdInstancia
                                               select n);
            }
            else
                GridTesisReview.DataContext = TesisSingleton.Tesis;
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();

            List<TesisTextReview> temp = (from n in TesisSingleton.Tesis
                                             where n.DocOriginalPlano.ToUpper().Contains(tempString)
                                             select n).ToList();

            GridTesisReview.DataContext = temp;
        }
    }
}