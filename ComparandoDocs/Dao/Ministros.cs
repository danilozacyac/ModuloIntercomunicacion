using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparandoDocs.Dao
{
    public class Ministros
    {
        private readonly int idMinistro;
        private readonly string ministro;
        
        public Ministros(int idMinistro, string ministro)
        {
            this.idMinistro = idMinistro;
            this.ministro = ministro;
        }
        
        
        public string Ministro
        {
            get
            {
                return this.ministro;
            }
        }

        public int IdMinistro
        {
            get
            {
                return this.idMinistro;
            }
        }
    }
}
