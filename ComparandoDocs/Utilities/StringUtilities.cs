using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComparandoDocs.Utilities
{
    public class StringUtilities
    {

        public static char ContainPunctuation(String texto)
        {
            char[] puntuacion = { ',','.','\"','-',';',':' };

            foreach (char charac in puntuacion)
            {
                if (texto.Contains(charac))
                    return charac;
            }

            return ' ';
        }

    }
}
