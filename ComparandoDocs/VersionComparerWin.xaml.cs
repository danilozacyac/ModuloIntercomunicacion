using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using ComparandoDocs.Dao;
using ComparandoDocs.Models;
using ComparandoDocs.Singleton;
using Microsoft.Win32;
using OfficeDocMangement;

namespace ComparandoDocs
{
    /// <summary>
    /// Lógica de interacción para VersionComparerWin.xaml
    /// </summary>
    public partial class VersionComparerWin : Window
    {
        private TesisTextReview tesis;
        private bool isUpdating = false;

        public VersionComparerWin()
        {
            InitializeComponent();
            tesis = new TesisTextReview();
        }

        public VersionComparerWin(TesisTextReview tesisUpdate)
        {
            InitializeComponent();
            this.tesis = tesisUpdate;
            this.isUpdating = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CbxMinistro.DataContext = MinistrosSingleton.MinistrosS;
            CbxInstancia.DataContext = from n in InstanciasSingleton.InstanciasS
                                       where n.IdInstancia > 0
                                       select n;

            if (isUpdating)
                SetDataOnUpdate();
        }

        private void BtnPathOrigen_Click(object sender, RoutedEventArgs e)
        {
            String convertedFilePath = "";

            String pathFile = this.GetFilePath("Original");
            TxtPathOrigen.Text = pathFile;

            if (!String.IsNullOrEmpty(pathFile) && !String.IsNullOrWhiteSpace(pathFile))
            {
                if (!pathFile.EndsWith(".rtf"))
                {
                    convertedFilePath = Path.GetTempFileName();

                    DocConversion.SetNewDocFormat(pathFile, convertedFilePath, "RTF");

                    pathFile = convertedFilePath + ".rtf";
                }

                TextRange range;

                System.IO.FileStream fStream;

                if (System.IO.File.Exists(pathFile))
                {
                    range = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);

                    fStream = new System.IO.FileStream(pathFile, System.IO.FileMode.OpenOrCreate);

                    range.Load(fStream, System.Windows.DataFormats.Rtf);

                    fStream.Close();
                }

                //OriginalText.Font
                var text = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);
                text.ApplyPropertyValue(TextElement.FontSizeProperty, 10.0);
                OriginalText.Document.TextAlignment = TextAlignment.Justify;
            }
        }

        private String GetFilePath(string texto)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Office Documents|*.doc;*.docx| RichTextFiles |*.rtf";

            dialog.InitialDirectory = @"C:\Users\" + Environment.UserName + @"\Documents";
            dialog.Title = "Selecciona el archivo " + texto;
            dialog.ShowDialog();

            return dialog.FileName;
        }

        private string comparer1Path = "";

        private void BtnPathRev1_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TxtPathOrigen.Text) && !String.IsNullOrWhiteSpace(TxtPathOrigen.Text))
            {
                String pathFile = this.GetFilePath(" a Revisar");
                TxtPathRev1.Text = pathFile;

                if (!String.IsNullOrEmpty(pathFile) && !String.IsNullOrWhiteSpace(pathFile))
                {
                    string xmlResult = Path.GetTempFileName() + ".xml";

                    WordDocComparer comparer = new WordDocComparer(TxtPathOrigen.Text, TxtPathRev1.Text, xmlResult);
                    comparer.Compare();

                    LoadDocumentRevision(xmlResult, Rev1, 1);
                    comparer1Path = xmlResult;
                }
            }
            else
            {
                MessageBox.Show("Primero debes seleccionar el archivo original");
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            tesis.ClaveTesis = TxtClave.Text;
            tesis.Oficio = TxtOficio.Text;
            tesis.FRecepcion = DtLlegada.SelectedDate;
            tesis.FEnvio = DtEnvio.SelectedDate;
            tesis.DocOriginalPath = TxtPathOrigen.Text;
            tesis.DocRevision1Path = TxtPathRev1.Text;
            tesis.DocRevision2Path = TxtPathAprobada.Text;

            tesis.Instancia = Convert.ToInt32(CbxInstancia.SelectedValue);
            tesis.Ministro = Convert.ToInt32(CbxMinistro.SelectedValue);

            TextRange range = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);
            tesis.DocOriginalPlano = range.Text;

            range = new TextRange(Rev1.Document.ContentStart, Rev1.Document.ContentEnd);
            tesis.DocRevision1Plano = range.Text;

            range = new TextRange(Rev2.Document.ContentStart, Rev2.Document.ContentEnd);
            tesis.DocRevision2Plano = range.Text;

            tesis.DocOriginal = this.GetRtfString(OriginalText);

            //this.GeneraRtfDetalle(@"C:\Users\lavega\Desktop\pp001.rtf", tesis.DocOriginal);

            tesis.DocRevision1 = this.GetRtfString(Rev1);
            //this.GeneraRtfDetalle(@"C:\Users\lavega\Desktop\pp002.rtf", tesis.DocRevision1);

            tesis.DocRevision2 = this.GetRtfString(Rev2);

            TesisTextReviewModel review = new TesisTextReviewModel();

            if (!isUpdating)
            {
                tesis.IdTesis = review.SetNewTesisReview(tesis);
                TesisSingleton.Tesis.Add(tesis);
            }
            else
                review.UpdateTesisReview(tesis);

            tesis.Tatj = (RadJuris.IsChecked == true) ? 1 : 0;

            

            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private string GetRtfString(RichTextBox rich)
        {
            var doc = rich.Document;
            var range = new TextRange(doc.ContentStart, doc.ContentEnd);
            var ms = new MemoryStream();
            range.Save(ms, DataFormats.Rtf);
            ms.Seek(0, SeekOrigin.Begin);

            var rtfString = new StreamReader(ms).ReadToEnd();

            return rtfString;
        }

        public void GeneraRtfDetalle(string docName, string rtfString)
        {
            System.IO.StreamWriter writer = new StreamWriter(docName, false, System.Text.Encoding.Default);
            writer.WriteLine(rtfString);
            writer.Close();
        }

        private void SetDataOnUpdate()
        {
            TxtClave.Text = tesis.ClaveTesis;
            TxtOficio.Text = tesis.Oficio;
            DtLlegada.SelectedDate = tesis.FRecepcion;
            DtEnvio.SelectedDate = tesis.FEnvio;

            CbxInstancia.SelectedValue = tesis.Instancia;
            CbxMinistro.SelectedValue = tesis.Ministro;

            TxtPathOrigen.Text = tesis.DocOriginalPath;
            TxtPathRev1.Text = tesis.DocRevision1Path;
            TxtPathAprobada.Text = tesis.DocRevision2Path ;

            this.CargaTextoConFormato(tesis.DocOriginal, OriginalText);
            this.CargaTextoConFormato(tesis.DocRevision1, Rev1);
            this.CargaTextoConFormato(tesis.DocRevision2, Rev2);

            if (tesis.Tatj == 1)
                RadJuris.IsChecked = true;
            else
                RadAislada.IsChecked = true;
        }

        private void CargaTextoConFormato(string texto, RichTextBox rich)
        {
            var doc = rich.Document;
            var range = new TextRange(doc.ContentStart, doc.ContentEnd);
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(texto);
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            range.Load(ms, DataFormats.Rtf);
        }
        
        private void BtnPathAprobada_Click(object sender, RoutedEventArgs e)
        {
            string revision1 = this.GetRtfString(Rev1);
            string docFile = Path.GetTempFileName() + ".doc";
            System.IO.StreamWriter writer = new StreamWriter(docFile, false, System.Text.Encoding.Default);
            writer.WriteLine(revision1);
            writer.Close();

            //string temp = comparer1Path.Replace(".xml", ".doc");
            //System.IO.File.Move(comparer1Path, temp);
            //comparer1Path = comparer1Path.Replace(".xml", ".doc");
            String pathFile = this.GetFilePath(" de la tesis aprobada");
            TxtPathAprobada.Text = pathFile;

            string xmlResult = Path.GetTempFileName() + ".xml";

            WordDocComparer comparer = new WordDocComparer(docFile, TxtPathAprobada.Text, xmlResult);
            comparer.Compare();

            LoadDocumentRevision(xmlResult, Rev2, 2);
        }

        /// <summary>
        /// Marca los cambios realizados entre diferentes versiones de documentos
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="targetRichTextBox"></param>
        /// <param name="tipoMarcado">1. Marcatextos  --- 2. Subrayado</param>
        private void LoadDocumentRevision(string filePath, RichTextBox targetRichTextBox, int tipoMarcado)
        {
            FlowDocument mcFlowDoc = new FlowDocument();

            XmlDocument xDoc = new XmlDocument();

            //La ruta del documento XML permite rutas relativas 
            //respecto del ejecutable!

            xDoc.Load(filePath);

            XmlNodeList docBody = xDoc.GetElementsByTagName("w:body");

            foreach (XmlElement seccionesDoc in docBody)
            {
                foreach (XmlElement nodoHijo in seccionesDoc)
                {
                    XmlNodeList hijos = nodoHijo.ChildNodes;

                    foreach (XmlElement subHijos in hijos)
                    {
                        Paragraph para = new Paragraph();
                        para.FontFamily = new FontFamily("Arial");
                        para.FontSize = 10;
                        para.TextAlignment = TextAlignment.Justify;
                        
                        foreach (XmlElement texto in subHijos.ChildNodes)
                        {

                            switch (texto.LocalName)
                            {
                                case "sub-section":
                                    foreach (XmlElement textoSubSeccion in texto.ChildNodes)
                                    {
                                       para =  this.SetFormat(textoSubSeccion, tipoMarcado);
                                       mcFlowDoc.Blocks.Add(para);
                                    }
                                    break;
                                case "r":
                                    if (tipoMarcado == 1)
                                    {
                                        string text = texto.InnerText;

                                        XmlNodeList elem = texto.GetElementsByTagName("w:b");

                                        if (elem.Count > 0)
                                            para.Inlines.Add(new Bold(new Run(text)));
                                        else
                                            para.Inlines.Add(new Run(text));
                                        //Aqui es donde lo agrego al RichTextBox}
                                    }
                                    else if (tipoMarcado == 2)
                                    {
                                        XmlNodeList elem = texto.GetElementsByTagName("w:highlight");
                                        XmlNodeList bold = texto.GetElementsByTagName("w:b");

                                        if (elem.Count > 0)
                                        {
                                            Run run = new Run(texto.InnerText);
                                            run.Background = new SolidColorBrush(Colors.SpringGreen);

                                            if (bold.Count > 0)
                                                para.Inlines.Add(new Bold(run));
                                            else
                                                para.Inlines.Add(run);
                                        }
                                        else if (bold.Count > 0)
                                        {
                                            para.Inlines.Add(new Bold(new Run(texto.InnerText)));
                                        }
                                        else
                                            para.Inlines.Add(new Run(texto.InnerText));
                                    }
                                    break;
                                case "annotation":
                                    XmlAttributeCollection attrib = texto.Attributes;
                                    string atribute = attrib["w:type"].Value;

                                    if (atribute.Equals("Word.Insertion"))
                                    {
                                        Run run;

                                        if (tipoMarcado == 1)
                                        {
                                            run = new Run(texto.InnerText);
                                            run.Background = new SolidColorBrush(Colors.SpringGreen);
                                            para.Inlines.Add(run);
                                        }
                                        else
                                        {
                                            XmlNodeList high = texto.GetElementsByTagName("w:highlight");

                                            if (high.Count > 0)
                                            {
                                                run = new Run(texto.InnerText);
                                                run.Background = new SolidColorBrush(Colors.SpringGreen);
                                                para.Inlines.Add(run);
                                            }
                                            else
                                                para.Inlines.Add(new Underline(new Run(texto.InnerText)));
                                        }
                                    }

                                    break;
                            }

                            Console.WriteLine(texto.LocalName);
                        }
                        mcFlowDoc.Blocks.Add(para);
                    }
                }
                
                targetRichTextBox.Document = mcFlowDoc;
            }
        }


        private Paragraph SetFormat(XmlElement texto,int tipoMarcado)
        {
            Paragraph para = new Paragraph();
            para.FontFamily = new FontFamily("Arial");
            para.FontSize = 10;
            para.TextAlignment = TextAlignment.Justify;

            switch (texto.LocalName)
            {
                case "r":
                case "p":
                    if (tipoMarcado == 1)
                    {
                        string text = texto.InnerText;

                        XmlNodeList elem = texto.GetElementsByTagName("w:b");

                        if (elem.Count > 0)
                            para.Inlines.Add(new Bold(new Run(text)));
                        else
                            para.Inlines.Add(new Run(text));
                        //Aqui es donde lo agrego al RichTextBox}
                    }
                    else if (tipoMarcado == 2)
                    {
                        XmlNodeList elem = texto.GetElementsByTagName("w:highlight");
                        XmlNodeList bold = texto.GetElementsByTagName("w:b");

                        if (elem.Count > 0)
                        {
                            Run run = new Run(texto.InnerText);
                            run.Background = new SolidColorBrush(Colors.SpringGreen);

                            if (bold.Count > 0)
                                para.Inlines.Add(new Bold(run));
                            else
                                para.Inlines.Add(run);
                        }
                        else if (bold.Count > 0)
                        {
                            para.Inlines.Add(new Bold(new Run(texto.InnerText)));
                        }
                        else
                            para.Inlines.Add(new Run(texto.InnerText));
                    }
                    break;
                case "annotation":
                    XmlAttributeCollection attrib = texto.Attributes;
                    string atribute = attrib["w:type"].Value;

                    if (atribute.Equals("Word.Insertion"))
                    {
                        Run run;

                        if (tipoMarcado == 1)
                        {
                            run = new Run(texto.InnerText);
                            run.Background = new SolidColorBrush(Colors.SpringGreen);
                            para.Inlines.Add(run);
                        }
                        else
                        {
                            XmlNodeList high = texto.GetElementsByTagName("w:highlight");

                            if (high.Count > 0)
                            {
                                run = new Run(texto.InnerText);
                                run.Background = new SolidColorBrush(Colors.SpringGreen);
                                para.Inlines.Add(run);
                            }
                            else
                                para.Inlines.Add(new Underline(new Run(texto.InnerText)));
                        }
                    }

                    break;
            }

            return para;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double sizeValue = SlFontSize.Value;

            var range = new TextRange(OriginalText.Document.ContentStart, OriginalText.Document.ContentEnd);
            range.ApplyPropertyValue(TextElement.FontSizeProperty, sizeValue);

            range = new TextRange(Rev1.Document.ContentStart, Rev1.Document.ContentEnd);
            range.ApplyPropertyValue(TextElement.FontSizeProperty, sizeValue);

            range = new TextRange(Rev2.Document.ContentStart, Rev2.Document.ContentEnd);
            range.ApplyPropertyValue(TextElement.FontSizeProperty, sizeValue);
        }
    }
}