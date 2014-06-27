using System;
using System.Linq;

namespace ComparandoDocs.Dao
{
    public class Instancias
    {
        private int idInstancia;
        private string instancia;

        public Instancias(int idInstancia, string instancia)
        {
            this.idInstancia = idInstancia;
            this.instancia = instancia;
        }

        public int IdInstancia
        {
            get
            {
                return this.idInstancia;
            }
            set
            {
                this.idInstancia = value;
            }
        }

        public string Instancia
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
    }
}
