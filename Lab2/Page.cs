//using ExcelDataReader;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Lab2
{
    public class Page :  INotifyCollectionChanged, INotifyPropertyChanged
    {
        //private static int total = Database.Threats.Count;
        private static int maxPerPage = 15;
        private static int currentPage = 0;
        public static int TotalThreats { get; set; } =  Database.Threats.Count;
        public static int maxPageNumber = (int) Math.Ceiling((double)(TotalThreats / maxPerPage));
        private static string pageInfo = $"УБИ: {MaxPerPage * currentPage + 1} - { Math.Min(MaxPerPage * (currentPage + 1), TotalThreats)} из {TotalThreats}";
        public static ObservableCollection<Threat> page = new ObservableCollection<Threat>();

        public static string PageInfo { get { return pageInfo;  } set { pageInfo = value; StaticPropertyChanged?.Invoke(null, PageInfoPropertyEventArgs); } }
        public static int MaxPerPage { get { return maxPerPage; } set { maxPerPage = value; } }
        public static int CurrentPage { get { return currentPage; }  set { currentPage = value; StaticPropertyChanged?.Invoke(null, PageInfoPropertyEventArgs); } }

        


        private static readonly PropertyChangedEventArgs PageInfoPropertyEventArgs = new PropertyChangedEventArgs(nameof(Page));
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)page).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)page).CollectionChanged -= value;
            }
        }
        public Page() 
        {
            page.Clear();
            int startIndex = Math.Min(MaxPerPage * CurrentPage, Database.Threats.Count);
            TotalThreats = Database.Threats.Count;
            for (int i = startIndex; i < startIndex + maxPerPage; i++)
            {
                if (i >= 0 && i < Database.Threats.Count)
                { page.Add(Database.Threats[i]); }
                else { break; }
            }
            PageInfo = $"УБИ: {MaxPerPage * CurrentPage + 1} - { Math.Min(MaxPerPage * (CurrentPage + 1), TotalThreats)} из {TotalThreats}";


        }
    }


    
}
