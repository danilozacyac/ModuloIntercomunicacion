using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ComparandoDocs.Dao;
using ComparandoDocs.Models;

namespace ComparandoDocs.Singleton
{
    public class MinistrosSingleton
    {
        private static ObservableCollection<Ministros> ministros;

        private MinistrosSingleton() { }

        public static ObservableCollection<Ministros> MinistrosS
        {
            get
            {
                if (ministros == null)
                    ministros = new MinistrosModel().GetMinistros(); //.GetVolumenes(DbConnDac.GetConnectionMantesisSql());

                return ministros;
            }
        }
    }
}
