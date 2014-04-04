using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComparandoDocs.Utilities
{
    public class StringUtilities
    {

        public static String[] ArticulosyConjArray = { "EL", "LA", "LOS", "LAS", "EN", "UN","UNA","DE","DEL" };

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


        public static bool isAnArticle(String palabra)
        {
            return ArticulosyConjArray.Contains(palabra);
        }


        /// <summary>
        /// Devuelve una lista con las posiciones encontradas para la palabra buscada
        /// </summary>
        /// <param name="texto">Texto completo donde se buscara</param>
        /// <param name="searchFor">Palabra o frase buscada</param>
        /// <returns></returns>
        public static List<int> IndexOf(String texto, string searchFor)
        {
            List<int> indices = new List<int>();

            while (texto.Length > 0)
            {
                int index = texto.IndexOf(searchFor);

                if (index != -1)
                {
                    indices.Add(index);
                    texto = texto.Substring(index + searchFor.Length);
                }
                else
                {
                    break;
                }
            }

            return indices;
        }
    }
}
