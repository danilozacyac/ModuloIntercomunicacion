using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComparandoDocs
{
    public class Documentos
    {
        string docOriginal;
        string docRevision1;
        string docRevision2;

        public string DocOriginal
        {
            get
            {
                return this.docOriginal;
            }
            set
            {
                this.docOriginal = value;
            }
        }

        public string DocRevision1
        {
            get
            {
                return this.docRevision1;
            }
            set
            {
                this.docRevision1 = value;
            }
        }

        public string DocRevision2
        {
            get
            {
                return this.docRevision2;
            }
            set
            {
                this.docRevision2 = value;
            }
        }
    }
}
