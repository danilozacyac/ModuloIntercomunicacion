using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace OfficeDocMangement
{
    public class DocConversion
    {
        /// <summary>
        /// Convierte un documento de formato .Doc o .Docx a cualquiera de los formatos compatibles
        /// con Microsoft Word
        /// </summary>
        /// <param name="filePath">Ruta del archivo a convertir</param>
        /// <param name="newformatPath">Ruta del archivo generado</param>
        /// <param name="format">Tipo de Archivo Requerido (PDF, XML, RTf, ...)</param>
        public static void SetNewDocFormat(String filePath, String newformatPath, String format)
        {
            try
            {
                Word.Application WordApp = new Word.Application();
                Word.Document WordDoc = new Word.Document();

                object DocNoParam = Type.Missing;
                object DocReadOnly = false;
                object DocVisible = false;
                object SaveToFormat = "";

                WordDoc = WordApp.Documents.Open(filePath,
                    ref DocNoParam,
                    ref DocReadOnly,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocVisible,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam,
                    ref DocNoParam);
                WordDoc.Activate();

                switch (format)
                {
                    case "PDF":
                        SaveToFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                        WordDoc.SaveAs( newformatPath + ".pdf", ref SaveToFormat, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam);
                        break;
                    case "HTML":
                        SaveToFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
                        WordDoc.SaveAs(newformatPath + ".html", ref SaveToFormat, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam);
                        break;
                    case "RTF":
                        SaveToFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatRTF;
                        WordDoc.SaveAs(newformatPath + ".rtf", ref SaveToFormat, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam);
                        break;
                    case "TXT":
                        SaveToFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatText;
                        WordDoc.SaveAs(newformatPath + ".txt", ref SaveToFormat, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam);
                        break;
                    case "XML":
                        SaveToFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXML;
                        WordDoc.SaveAs(newformatPath + ".xml", ref SaveToFormat, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam);
                        break;
                    case "XPS":
                        SaveToFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXPS;
                        WordDoc.SaveAs(newformatPath + ".xps", ref SaveToFormat, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam, ref DocNoParam);
                        break;
                
                }

                WordDoc.Close();
                WordApp.Application.Quit(ref DocNoParam, ref DocNoParam, ref DocNoParam);
                WordDoc = null;
                WordApp = null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}