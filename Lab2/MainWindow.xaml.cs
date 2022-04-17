using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
//using System.Windows.Forms;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static bool IsReasonToStop =false;
        private static Database DB;

        public MainWindow()
        {
            
            Database.FileCheck();
            if (IsReasonToStop) { this.Close(); }
            else
            {
                InitializeComponent();
                DB = new Database();
                Page.TotalThreats = Database.Threats.Count;
                new Page();
                ThreatsDataBase.ItemsSource = Page.page;
            }
            

        }
         
        private void Next_Page_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (!Page.page[Page.page.Count - 1].Equals(Database.Threats[Database.Threats.Count - 1]))
                {
                    ++Page.CurrentPage;
                    Page.PageInfo = $"УБИ: {Page.MaxPerPage * Page.CurrentPage + 1} - { Math.Min(Page.MaxPerPage * (Page.CurrentPage + 1), Database.Threats.Count)} из {Database.Threats.Count}";
                    new Page();
                    CurrentView.Text = Page.PageInfo;
                    CurrentView.UpdateLayout();
                    ThreatsDataBase.Items.Refresh();

                }


            }
            catch (ArgumentOutOfRangeException) 
            {
                Page.CurrentPage = (int) Math.Floor( (double) Database.Threats.Count / (Page.MaxPerPage) ) ; 
                Previous_Page_Button_Click(sender, e);
            }
        }

        private void Previous_Page_Button_Click(object sender, RoutedEventArgs e)
        {          
                if (Page.CurrentPage > 0)
                {

                    --Page.CurrentPage;
                    new Page();

                    ThreatsDataBase.Items.Refresh();
                    CurrentView.Text = Page.PageInfo;
                    CurrentView.UpdateLayout();

                }
            
           
            
        }

 
        private void ItemsPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;
            string selectedItem = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
            Page.MaxPerPage = int.Parse(selectedItem);
            //new Page();
            new Page();
            ThreatsDataBase.Items.Refresh();
             Previous_Page_Button_Click(sender, e); Next_Page_Button_Click(sender, e); 
            
        }
 
        private void SelectedRow_Button_Click(object sender, SelectionChangedEventArgs e)
        {            
                try
                {
                    int cellRowNum = this.ThreatsDataBase.SelectedIndex;

                    System.Windows.MessageBox.Show(Page.page[cellRowNum].ToString(), "УГРОЗА БЕЗОПАСНОСТИ ИНФОРМАЦИИ");
                }
                catch (Exception r) { }
            
           
        }

        private void SaveDatabase_Click(object sender, RoutedEventArgs e)
        {
            Database.SmartSave(DB.FormFile());
            
        }

        private void UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            Database.UpdateDatabase();
        }

        private void SimpleSave_Button_Click(object sender, RoutedEventArgs e)
        {
            Database.SimpleSave(DB.FormFile());
        }

   
    }
}
