//using ExcelDataReader;

using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows;
//using ExcelDataReader;

using ClosedXML.Excel;
using System.Windows.Data;
using System.ComponentModel;

namespace Lab2
{
    public  partial class Updater 
    {

        public SortedList<string, (string, string)> differentThreats;
        internal List<string> indices;

        SortedList<string, string> prevThreats = new SortedList<string, string>();
        SortedList<string, string> updThreats = new SortedList<string, string>();
        public Updater(List<Threat> prev, List<Threat> upd)
        {
            indices = new List<string>();
           differentThreats = this.GetDifferentThreats(prev, upd);
        }

        public SortedList<string, (string, string)> GetDifferentThreats(List<Threat> prev, List<Threat> upd)
        {
            SortedList<string, (string, string)> result = new SortedList<string, (string, string)>();      
            foreach (var item in prev)
            {
                prevThreats.Add(item.Id, item.ToString());
            }
            foreach (var item in upd)
            {
                updThreats.Add(item.Id, item.ToString());
            }

            foreach (var newKey in updThreats.Keys)
            {
                if (prevThreats.Keys.Contains(newKey))
                {
                    if (!prevThreats[newKey].Equals(updThreats[newKey]))
                    {

                        result.Add(newKey, (prevThreats[newKey], updThreats[newKey])); this.indices.Add(newKey);
                    }
     
                }
                else
                {
                    result.Add(newKey, ("Этого раньше вообще не было...", updThreats[newKey]));
                    this.indices.Add(newKey);
                }
            }
            foreach (var oldKey in prevThreats.Keys)
            {
                if (!updThreats.Keys.Contains(oldKey))
                {
                    result.Add(oldKey, (prevThreats[oldKey], "В новой версии это стало ненужным..."));
                    this.indices.Add(oldKey);
                    
                }
            }

            return result;
            
        }
       
    }
   
}
