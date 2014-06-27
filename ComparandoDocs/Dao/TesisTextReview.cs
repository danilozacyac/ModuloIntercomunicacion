using System;
using System.Linq;

namespace ComparandoDocs.Dao
{
    public class TesisTextReview
    {
        private int idTesis;
        private string docOriginal;
        private string docOriginalPlano;
        private string docOriginalPath;
        private string docRevision1;
        private string docRevision1Plano;
        private string docRevision1Path;
        private string docRevision2;
        private string docRevision2Plano;
        private string docRevision2Path;
        private int fechaInt;
        private DateTime fecha;
        private string claveTesis;
        private string oficio;
        private DateTime? fRecepcion;
        private int fRecepcionInt;
        private DateTime? fEnvio;
        private int fEnvioInt;
        private int instancia;
        private int ministro;
        private int tatj;
        
        

        

        public int IdTesis
        {
            get
            {
                return this.idTesis;
            }
            set
            {
                this.idTesis = value;
            }
        }

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

        public string DocOriginalPlano
        {
            get
            {
                return this.docOriginalPlano;
            }
            set
            {
                this.docOriginalPlano = value;
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

        public string DocRevision1Plano
        {
            get
            {
                return this.docRevision1Plano;
            }
            set
            {
                this.docRevision1Plano = value;
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

        public string DocRevision2Plano
        {
            get
            {
                return this.docRevision2Plano;
            }
            set
            {
                this.docRevision2Plano = value;
            }
        }

        public int FechaInt
        {
            get
            {
                return this.fechaInt;
            }
            set
            {
                this.fechaInt = value;
            }
        }

        public DateTime Fecha
        {
            get
            {
                return this.fecha;
            }
            set
            {
                this.fecha = value;
            }
        }

        public string ClaveTesis
        {
            get
            {
                return this.claveTesis;
            }
            set
            {
                this.claveTesis = value;
            }
        }

        public string Oficio
        {
            get
            {
                return this.oficio;
            }
            set
            {
                this.oficio = value;
            }
        }

        public DateTime? FRecepcion
        {
            get
            {
                return this.fRecepcion;
            }
            set
            {
                this.fRecepcion = value;
            }
        }

        public int FRecepcionInt
        {
            get
            {
                return this.fRecepcionInt;
            }
            set
            {
                this.fRecepcionInt = value;
            }
        }

        public DateTime? FEnvio
        {
            get
            {
                return this.fEnvio;
            }
            set
            {
                this.fEnvio = value;
            }
        }

        public int FEnvioInt
        {
            get
            {
                return this.fEnvioInt;
            }
            set
            {
                this.fEnvioInt = value;
            }
        }

        public int Instancia
        {
            get
            {
                return this.instancia;
            }
            set
            {
                this.instancia = value;
            }
        }

        public int Ministro
        {
            get
            {
                return this.ministro;
            }
            set
            {
                this.ministro = value;
            }
        }

        public string DocRevision2Path
        {
            get
            {
                return this.docRevision2Path;
            }
            set
            {
                this.docRevision2Path = value;
            }
        }

        public string DocRevision1Path
        {
            get
            {
                return this.docRevision1Path;
            }
            set
            {
                this.docRevision1Path = value;
            }
        }

        public string DocOriginalPath
        {
            get
            {
                return this.docOriginalPath;
            }
            set
            {
                this.docOriginalPath = value;
            }
        }

        public int Tatj
        {
            get
            {
                return this.tatj;
            }
            set
            {
                this.tatj = value;
            }
        }
    }
}
