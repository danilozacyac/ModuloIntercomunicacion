using System;
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
using ComparandoDocs.Utilities;

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
        }

        private void Rev2_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);

            TextRange rangeRevision1 = new TextRange(Rev1.Document.ContentStart, Rev1.Document.ContentEnd);

            List<string> original = range.Text.Split(' ').ToList();

            List<string> revision1 = rangeRevision1.Text.Split(' ').ToList();

            int indexOriginal = 0;
            int startMarkUpIndex = 0;
            int endMarkUpIndex = 0;

            for (int indexRevision = 0; indexRevision < revision1.Count(); indexRevision++)
            {


                if (indexOriginal < original.Count())
                {
                    //Checamos si las palabras son iguales
                    if (revision1[indexRevision].Equals(original[indexOriginal]))
                    {
                        startMarkUpIndex += revision1[indexRevision].Length + 1;
                        indexOriginal++;
                    }
                    else
                    {
                        //startMarkUpIndex += revision1[indexRevision].Length + 1;
                        //Checamos si la palabra es diferente por tener signos de puntuiación al inicio o al final
                        if (revision1[indexRevision].Length == original[indexOriginal].Length + 1 || revision1[indexRevision].Length == original[indexOriginal].Length - 1)
                        {
                            char puntuationChar = StringUtilities.ContainPunctuation(revision1[indexRevision]);

                            if (!puntuationChar.Equals(' ')) ///La diferencia se debe a un signo de puntuación
                            {
                                int puntuationIndex = revision1[indexRevision].IndexOf(puntuationChar);
                                startMarkUpIndex += puntuationIndex;

                                this.HighlightChanges(Rev1.Document.ContentStart, revision1[indexRevision]);
                            }
                            else ///LA diferencia es por un caracter que no es puntuación Ejem. de --- del
                            {
                                if (revision1[indexRevision].Length > original[indexOriginal].Length)
                                {
                                    char[] rev1 = revision1[indexRevision].ToCharArray();
                                    char[] orig = original[indexOriginal].ToCharArray();

                                    int index;
                                    for (index = 0; index < rev1.Count(); index++)
                                    {
                                        if (orig.Count() < index)
                                        {
                                            if (!rev1[index].Equals(orig[index]))
                                                break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    startMarkUpIndex += revision1[indexRevision].Length - index;
                                    this.HighlightChanges(Rev1.Document.ContentStart, revision1[indexRevision]);
                                }
                            }

                            //Convertimos la palabra en un arreglo de caracteres

                            indexOriginal++;
                        }
                        else if (revision1[indexRevision].Length == original[indexOriginal].Length) //Identifica si la palabra es diferente por mayúsculas-minúsculas
                        {
                            char[] revisionPorLetra = revision1[indexRevision].ToCharArray();
                            char[] originalPorLetra = original[indexOriginal].ToCharArray();

                            int difIndex = 0;
                            for (int index = 0; index < revisionPorLetra.Count(); index++)
                            {
                                if (!revisionPorLetra[index].Equals(originalPorLetra[index]))
                                {
                                    difIndex = index;
                                    break;
                                }
                            }

                            this.HighlightChanges(Rev1.Document.ContentStart, revision1[indexRevision]);

                            startMarkUpIndex += revision1[indexRevision].Length + 1;
                            indexOriginal++;
                        }
                        else //Cuando la palabra no coincide y hay que buscar la siguiente coincidencia
                        {

                            ///Buscar hacía adelante, si se encuentra la palabra quiere decir que se elimino alguna frase, por lo que
                            ///no hay que pintar esa palabra
                            ///

                            int indexOriginalActual = indexOriginal;
                            bool foundWord = false;

                            while (indexOriginalActual < (indexOriginal + 5))
                            {
                                if (revision1[indexRevision].Equals(original[indexOriginalActual]))
                                {
                                    foundWord = true;
                                    indexOriginal = indexOriginalActual;
                                    break;
                                }
                                indexOriginalActual++;
                            }

                            if (!foundWord)
                            {
                                this.HighlightChanges(Rev1.Document.ContentStart, revision1[indexRevision]);
                                startMarkUpIndex += revision1[indexRevision].Length + 1;
                            }

                            
                        }
                    }
                }
                else
                {
                    int final = rangeRevision1.Text.Length - startMarkUpIndex;


                    this.HighlightChanges(Rev1.Document.ContentStart, startMarkUpIndex, final);   
                }
            }
        }

        private void HighlightChanges(TextPointer pointer, String regexMark)
        {
            List<int> indices = StringUtilities.IndexOf(pointer.ToString(), regexMark);

            Regex reg = new Regex("("+ regexMark + ")", RegexOptions.Compiled );
            
            bool isFound = false;

            var start = pointer;
            while (start != null && start.CompareTo(Rev1.Document.ContentEnd) < 0)
            {
                if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text && !isFound)
                {
                    var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));



                    var textrange = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
                    

                    //textrange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                    textrange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.LightGreen));
                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    start = textrange.End; // I'm not sure if this is correct or skips ahead too far, try it out!!!
                    //start = start.GetNextContextPosition(LogicalDirection.Forward);
                    //isFound = true; 
                }

                //if (isFound)
                //    start = null;
                //else
                    start = start.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private void HighlightChanges(TextPointer pointer, int iniMarkUp, int endMarkUp)
        {

            var start = pointer;
            //while (start != null && start.CompareTo(Rev1.Document.ContentEnd) < 0)
            //{
            //    if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text && !isFound)
            //    {
                    //var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));

                    var textrange = new TextRange(start.GetPositionAtOffset(iniMarkUp, LogicalDirection.Forward), start.GetPositionAtOffset(iniMarkUp + endMarkUp, LogicalDirection.Backward));
                    //textrange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                    textrange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.LightGreen));
                    textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    start = textrange.End; // I'm not sure if this is correct or skips ahead too far, try it out!!!
                    //start = start.GetNextContextPosition(LogicalDirection.Forward);
                    //isFound = true; 
            //    }

            //    //if (isFound)
            //    //    start = null;
            //    //else
            //    start = start.GetNextContextPosition(LogicalDirection.Forward);
            //}
        }
    }
}