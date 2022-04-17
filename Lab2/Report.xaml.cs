using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
      
        Updater updater;
        public static string GeneralInfo { get; set; } 

       public Report(Updater updater)
        { 
            this.updater = updater;
          
                GeneralInfo =  "Статус завершения операции: "+ (updater.differentThreats.Count == 0 ? "Ошибка" : "Успешно") + "\nИзменено записей: " + updater.differentThreats.Count + (updater.differentThreats.Count == 0 ? "\nВы уже используете последнюю версию базы!":"");
                InitializeComponent();
                GeneralInformation.Text = GeneralInfo; 
                GeneralInformation.UpdateLayout();
                Changes.ItemsSource = updater.indices;
                Changes.Items.Refresh();
                this.ShowDialog();
                UpdateLayout();
           

        }
        private void Changes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ListBox listBox = (System.Windows.Controls.ListBox)sender;
            var selectedItem = listBox.SelectedItem as string;         
            Changes.Items.Refresh();
            if (updater.differentThreats.ContainsKey(selectedItem))
            { OlderThreat.Text = "\t\tБЫЛО:\n" + updater.differentThreats[selectedItem].Item1;
                UpdatedThreat.Text = "\t\tСТАЛО:\n" + updater.differentThreats[selectedItem].Item2;
                OlderThreat.UpdateLayout();
                UpdatedThreat.UpdateLayout(); 
            }
            
            
        }
    }
   
}
