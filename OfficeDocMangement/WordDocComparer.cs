using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace OfficeDocMangement
{
    public class WordDocComparer
    {
        #region Variables
        
        //private Word.Application WordApp;
        //private Word.Document aDoc;
        private readonly string docOriginal;
        private readonly string docRevision;
        private readonly string xmlResult;
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// This is the constructor for WordDocComparer
        /// </summary>
        public WordDocComparer(string docOriginal, string docRevision, string xmlResult)
        {
            this.docOriginal = docOriginal;
            this.docRevision = docRevision;
            this.xmlResult = xmlResult;

        }
        
        #endregion
        
        #region Public Methods
        
        public void Compare()
        {
            object missing = System.Reflection.Missing.Value;
            
            object readonlyobj = false;
            object filename = docOriginal;
            
            //create a word application object for processing the word file.
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            
            //create a word document object and open the above file..
            Word.Document doc = app.Documents.Open(ref filename, ref missing, ref readonlyobj, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            
            doc.TrackRevisions = true;
            doc.ShowRevisions = true;
            doc.PrintRevisions = true;
            doc.Compare(docRevision, missing, Word.WdCompareTarget.wdCompareTargetCurrent, true, false, false, false, false);

            object saveToFormat = Word.WdSaveFormat.wdFormatXML;
            doc.SaveAs(xmlResult, ref saveToFormat, ref missing, ref missing, ref missing, ref missing, 
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            doc.Close(ref missing, ref missing, ref missing);
            app.Quit(ref missing, ref missing, ref missing);
            //MessageBox.Show("Process complete");
        }
        
        #endregion
        
        #region Private Method
        
        #endregion
        
    }
}