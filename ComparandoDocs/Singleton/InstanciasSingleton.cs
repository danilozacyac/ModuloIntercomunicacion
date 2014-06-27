using System;
using System.Collections.ObjectModel;
using System.Linq;
using ComparandoDocs.Dao;
using ComparandoDocs.Models;

namespace ComparandoDocs.Singleton
{
    public class InstanciasSingleton
    {
        private static ObservableCollection<Instancias> instancias;

        private InstanciasSingleton() { }

        public static ObservableCollection<Instancias> InstanciasS
        {
            get
            {
                if (instancias == null)
                    instancias = new InstanciasModel().GetInstancias(); 

                return instancias;
            }
        }
    }
}
