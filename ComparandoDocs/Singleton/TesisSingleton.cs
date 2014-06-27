using System;
using System.Collections.ObjectModel;
using System.Linq;
using ComparandoDocs.Dao;
using ComparandoDocs.Models;

namespace ComparandoDocs.Singleton
{
    public class TesisSingleton
    {
        private static ObservableCollection<TesisTextReview> listadoTesis;

        private TesisSingleton() { }

        public static ObservableCollection<TesisTextReview> Tesis
        {
            get
            {
                if (listadoTesis == null)
                    listadoTesis = new TesisTextReviewModel().GetTesisList(); 

                return listadoTesis;
            }
        }
    }
}
