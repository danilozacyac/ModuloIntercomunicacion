﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace ComparandoDocs
{
    /// <summary>
    /// Interaction logic for VersionComparer.xaml
    /// </summary>
    public partial class VersionComparer : UserControl
    {
        public static string textoOriginal;
        public static string textoRevision1;
        public static string textoRevision2;


        public VersionComparer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OriginalText_Loaded(object sender, RoutedEventArgs e)
        {

            
            TextRange range = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);
            range.Text = @"Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas Letraset, las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.";
            Regex reg = new Regex("(aldus|Lorem i|end)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var start = OriginalText.Document.ContentStart;
            while (start != null && start.CompareTo(OriginalText.Document.ContentEnd) < 0)
            {
                if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));

                    var textrange = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
                    textrange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                    textrange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.LightGreen));
                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    start = textrange.End; // I'm not sure if this is correct or skips ahead too far, try it out!!!
                }
                start = start.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private void Rev1_Loaded(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);

            TextRange rangeRevision1 = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);

            List<string> original = range.Text.Split(' ').ToList();

            List<string> revision1 = rangeRevision1.Text.Split(' ').ToList();

            int indexOriginal = 0;
            for (int indexRevision = 0; indexRevision < revision1.Count(); indexRevision++)
            {
                //Checamos si las palabras son iguales
                if (revision1[indexRevision].Equals(original[indexOriginal]))
                {
                }
                else
                {
                    //Checamos si la 
                    if (revision1[indexRevision].Length == original[indexOriginal].Length + 1 || revision1[indexRevision].Length == original[indexOriginal].Length - 1)
                    {
                        //Convertimos la palabra en un arreglo de caracteres
                        char[] rev1 = revision1[indexRevision].ToCharArray();
                        char[] orig = original[indexOriginal].ToCharArray();



                    }
                }
            }

        }

        private void Rev2_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
