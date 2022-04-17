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

namespace Lab2
{
    public class Database
    {
        
        internal static string path; 
        internal static int total;
        
        public static List<Threat> previousThreats = new List<Threat>();

        private static List<Threat> threats = new List<Threat>();
        public static List<Threat> Threats { get { return threats; } set { threats = value; } }
        public Database()
        {
           total = 0;
           foreach (var item in GetThreats(path))
            {
                total++;
                Threats.Add(item);
            }       
        }

        public static void UpdateDatabase()
        {
            string newPath = Directory.GetCurrentDirectory();
            string fileName = "thrlistToCompare.xlsx";
            newPath = newPath + "/" + fileName;
            DownloadDatabase(newPath);                                                                  
            previousThreats = new List<Threat>(Threats);
            Threats.Clear();
            foreach (var threat in GetThreats(newPath))
            {
                Threats.Add(threat);
            }
            
            if ((Threats.Equals(previousThreats)))
            {
                MessageBox.Show("Статус обновления: ошибка! \nВы просматриваете базу данных последней версии!");
            }
            else { 

                Updater updater = new Updater(previousThreats, Threats);
                Report report = new Report(updater);
                report.UpdateLayout();
            }

        }
        public static void SimpleSave(XLWorkbook wb)
        {
            wb.SaveAs(Database.path);
        }

        public static void SmartSave(XLWorkbook wb) // может, стоит по-дефолту установить директорию, куда скачивается Thrlist?..
        {
            Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
            saveFile.FileName = "Information Threats List";
            saveFile.DefaultExt = ".xlsx"; 
            saveFile.Filter = "Excel documents (.xlsx)|*.xlsx"; 
            bool? result = saveFile.ShowDialog();
            if (result == true)
            {               
                try { wb.SaveAs(saveFile.FileName);
                    wb.Dispose();
                    MessageBox.Show("Файл был успешно сохранён!");
                }
                catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
                {
                    MessageBox.Show("В данный момент файл используется другой программой. Закройте её и попробуйте заново.");
                }
                catch { MessageBox.Show("Ошибка сохранения!"); }
            }
        }
        public static void FormHeader(IXLWorksheet workSheet) 
        {
            
            workSheet.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            workSheet.Style.Border.RightBorder = XLBorderStyleValues.Thick;
            workSheet.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            workSheet.Style.Border.TopBorder = XLBorderStyleValues.Thick;
            workSheet.Cell("A1").Value = "Общая информация";
            workSheet.Cell("A1").Style.Font.Bold = true;
            workSheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workSheet.Range(1, 1, 1, 5).Merge();
            workSheet.Cell("F1").Value = "Последствия";
            workSheet.Cell("F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workSheet.Cell("F1").Style.Font.Bold = true;
            workSheet.Range(1, 6, 1, 8).Merge();

            int colWidth = 30;
            string[] headers = new string[8] {"Идентификатор УБИ", "Наименование УБИ",
                "Описание", "Источник угрозы (характеристика и потенциал нарушителя)",
                "Объект воздействия", "Нарушение конфиденциальности", "Нарушение целостности",
                "Нарушение доступности"};

            for (int r = 1; r <= 8; r++)
            { workSheet.Column(r).Width = colWidth;
                workSheet.Cell(2, r).Value = headers[r - 1];
                workSheet.Column(r).Style.Alignment.WrapText = false;
            }
        }
        public XLWorkbook FormFile()
        {

            var dbToSave = new XLWorkbook();
            var workSheet = dbToSave.AddWorksheet("Sheet");
            FormHeader(workSheet);
            for (int i = 0; i < threats.Count; i++)
                {
                    workSheet.Cell(3 + i, 1).Value = threats[i].Id.Replace("УБИ.", ""); // Должнно ли быть УБИ в начале?
                    workSheet.Cell(3 + i, 2).Value = threats[i].Name; 
                    workSheet.Cell(3 + i, 3).Value = threats[i].Description; 
                    workSheet.Cell(3 + i, 4).Value = threats[i].Source; 
                    workSheet.Cell(3 + i, 5).Value = threats[i].Object; 
                    workSheet.Cell(3 + i, 6).Value = threats[i].IsConfidential ? "0" : "1";
                    workSheet.Cell(3 + i, 7).Value = threats[i].IsUndamaged ? "0" : "1"; 
                    workSheet.Cell(3 + i, 8).Value = threats[i].IsAccessible ? "0" : "1"; 
                }
             return dbToSave;      
        }
        public static void FileCheck()
        {
           string path = Directory.GetCurrentDirectory();   
            string fileName = "thrlist.xlsx";
            path = path + "/" + fileName;
            Database.path = path;
            
           if (!File.Exists(path))
            {
                FirstTimeDownload firstTimeDownload = new FirstTimeDownload() ;
                
                firstTimeDownload.ShowDialog();
                
            }
           else { MessageBox.Show("База данных уже загружена. Программа готова к работе."); }

           

        }
        internal static void DownloadDatabase(string path)
        {
            WebClient client = new WebClient();
            string address = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
            try
            {
                client.DownloadFile(address, path);
                MessageBox.Show("База данных загружена!");
            }
            catch (WebException x) { MessageBox.Show("Что-то пошло не так - Проверьте интернет-соединение!\nОшибка: \n" + x.Message); }
            catch (Exception r) { MessageBox.Show(r.Message); }
        }
        
        private static IEnumerable<Threat> GetThreats(string path)
        {
            using (var database = new XLWorkbook(path))
            {
                IXLWorksheet db = database.Worksheets.Worksheet("Sheet");
                for (int rowN = 3; rowN < db.RowCount(); rowN++)
                {
                    if (db.Cell(rowN, 1).GetValue<string>() == "")    { break; }
                    Threat threat = new Threat
                        (
                        db.Cell(rowN, 1).GetValue<string>(),
                        db.Cell(rowN, 2).GetValue<string>(),
                        db.Cell(rowN, 3).GetValue<string>(),
                        db.Cell(rowN,4).GetValue<string>(),
                        db.Cell(rowN,5).GetValue<string>(),
                        db.Cell(rowN,6).GetValue<string>() == "1", // Проверить логиику соответствия поля булеву значению 
                        db.Cell(rowN,7).GetValue<string>() == "1",
                        db.Cell(rowN, 8).GetValue<string>() == "1"
                        );
                    yield return threat;
                }
            }
        }
       
      
    }
}
