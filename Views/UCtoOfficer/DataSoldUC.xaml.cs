using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Praktik_Work_2.Helper;
using Praktik_Work_2.Models;
using Praktik_Work_2.ViewModels;
using Praktik_Work_2.Views.WindowNew;

namespace Praktik_Work_2.Views.UCtoOfficer
{
    /// <summary>
    /// Логика взаимодействия для DataSoldUC.xaml
    /// </summary>
    public partial class DataSoldUC : UserControl, ExcelEx
    {
        public ObservableCollection<DataSoldier> ListDataSoldier { get; set; } = new ObservableCollection<DataSoldier>();

        DataSoldViewModel vmDataSold = new DataSoldViewModel();

        private SoldiersViewModel vmSoldiers;

        public DataSoldUC()
        {
            InitializeComponent();

            this.DataContext = vmDataSold;

            vmDataSold.LoadData();
        }

        public int MaxIdDataSold()
        {
            int max = 0;
            foreach (var datSoldier in this.vmDataSold.DataSoldiers)
            {
                if (max < datSoldier.DataId)
                {
                    max = datSoldier.DataId;
                };
            }
            return max;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewDataSoldier wnDataSoldier = new WindowNewDataSoldier
            {
                Title = "Новые сведения о солдате"
            };

            //int maxIdDataSoldier = MaxIdDataSold() + 1;
            DataSoldier dataSoldier = new DataSoldier();
            /*{
                SoldierId = maxIdSoldier
            };*/

            wnDataSoldier.DataContext = dataSoldier;

            vmDataSold.soldiers = vmDataSold.GetSoldiers();
            wnDataSoldier.CbSoldId.ItemsSource = vmDataSold.soldiers;

            if (wnDataSoldier.ShowDialog() == true)
            {
                Soldier soldier = (Soldier)wnDataSoldier.CbSoldId.SelectedValue;
                if (soldier != null)
                {
                    dataSoldier.SoldierId = soldier.SoldierId;
                    // добавляем нового солдата в контекст базы данных
                    vmDataSold._context.DataSoldiers.Add(dataSoldier);
                    // сохраняем изменения в базе данных
                    vmDataSold._context.SaveChanges();
                    // обновляем список солдат в DataGrid
                    vmDataSold.DataSoldiers.Add(dataSoldier);
                    OnPropertyChanged(nameof(vmDataSold.DataSoldiers));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_dataSoldiersList.SelectedItem == null)
            {
                MessageBox.Show("Выберите сведения о солдате для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            WindowNewDataSoldier wnDataSold = new WindowNewDataSoldier(isEditing: true)
            {
                Title = "Редактирование сведений о бойце"
            };

            DataSoldier dataSoldier = (DataSoldier)_dataSoldiersList.SelectedItem;
            var tempDataSoldier = dataSoldier.ShallowCopy();
            wnDataSold.DataContext = tempDataSoldier;

            vmSoldiers = new SoldiersViewModel();
            vmDataSold.soldiers = vmSoldiers.ListSolder.ToList();
            wnDataSold.CbSoldId.ItemsSource = vmDataSold.GetSoldiers();
            wnDataSold.CbSoldId.SelectedValue = dataSoldier.SoldierId;

            if (wnDataSold.ShowDialog() == true)
            {
                // сохранение данных в опреративную память
                // перенос данных из временного класса отображения данных
                var dataSoldierId = (Soldier)wnDataSold.CbSoldId.SelectedValue;
                if (dataSoldierId != null)
                {
                    dataSoldier.SoldierId = dataSoldierId.SoldierId;
                    dataSoldier.ParentsAdress = tempDataSoldier.ParentsAdress;
                    dataSoldier.CharacterTraits = tempDataSoldier.CharacterTraits;
                    dataSoldier.AttitudeService = tempDataSoldier.AttitudeService;
                    dataSoldier.PhoneNumber = tempDataSoldier.PhoneNumber;
                    dataSoldier.Education = tempDataSoldier.Education;
                    dataSoldier.CitizenProfession = tempDataSoldier.CitizenProfession;

                    // сохраняем изменения в базе данных
                    vmDataSold._context.Entry(dataSoldier).State = EntityState.Modified;
                    vmDataSold._context.SaveChanges();

                    CollectionViewSource.GetDefaultView(vmDataSold.DataSoldiers).Refresh();
                }
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            DataSoldier dataSoldier = (DataSoldier)_dataSoldiersList.SelectedItem;

            if (dataSoldier == null)
            {
                MessageBox.Show("Выберите сведения о солдате для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Удалить сведения о солдате: \n" + dataSoldier.DataId + " ",
                                              "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
   
            if (result == MessageBoxResult.OK)
            {
                vmDataSold._context.DataSoldiers.Remove(dataSoldier);
                vmDataSold._context.SaveChanges();

                vmDataSold.DataSoldiers.Remove(dataSoldier);

                CollectionViewSource.GetDefaultView(vmDataSold.DataSoldiers).Refresh();
            }
        }

        private void ButCitProfSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = vmDataSold._context.DataSoldiers.AsQueryable();

            if (!string.IsNullOrEmpty(CitProfSearch.Text))
            {
                query = query.Where(b => b.CitizenProfession.Contains(CitProfSearch.Text));
            }

            MessageBoxResult result = MessageBox.Show("Создать отчет по поиску в файле Excel?", "Поиск", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                vmDataSold.DataSoldiers = new ObservableCollection<DataSoldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmDataSold.DataSoldiers).Refresh();

                ExportToExcel();

                Process.Start(new ProcessStartInfo("DataSoldiers.xlsx") { UseShellExecute = true });
            }
            else
            {
                vmDataSold.DataSoldiers = new ObservableCollection<DataSoldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmDataSold.DataSoldiers).Refresh();
            }
        }

        public void ExportToExcel()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("DataSoldiers");

            // заголовки
            worksheet.Cell(1, 1).Value = "Номер данных";
            worksheet.Cell(1, 2).Value = "Солдат";
            worksheet.Cell(1, 3).Value = "Адрес родителей";
            worksheet.Cell(1, 4).Value = "Особенности характера";
            worksheet.Cell(1, 5).Value = "Отношение к службе";
            worksheet.Cell(1, 6).Value = "Номер телефона";
            worksheet.Cell(1, 7).Value = "Образование";
            worksheet.Cell(1, 8).Value = "Гражданская профессия";

            // экспорт из dataGrid в Excel
            int rowIndex = 2;
            foreach (var dataSoldier in _dataSoldiersList.ItemsSource)
            {
                worksheet.Cell(rowIndex, 1).Value = ((DataSoldier)dataSoldier).DataId;
                worksheet.Cell(rowIndex, 2).Value = ((DataSoldier)dataSoldier).SoldierId;
                worksheet.Cell(rowIndex, 3).Value = ((DataSoldier)dataSoldier).ParentsAdress;
                worksheet.Cell(rowIndex, 4).Value = ((DataSoldier)dataSoldier).CharacterTraits;
                worksheet.Cell(rowIndex, 5).Value = ((DataSoldier)dataSoldier).AttitudeService;
                worksheet.Cell(rowIndex, 6).Value = ((DataSoldier)dataSoldier).PhoneNumber;
                worksheet.Cell(rowIndex, 7).Value = ((DataSoldier)dataSoldier).Education;
                worksheet.Cell(rowIndex, 8).Value = ((DataSoldier)dataSoldier).CitizenProfession;

                rowIndex++;
            }

            var range = worksheet.RangeUsed();
            range.Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
            range.Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
            range.Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
            range.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);

            // авто-ширина
            worksheet.Columns().AdjustToContents();

            try
            {
                workbook.SaveAs("DataSoldiers.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Закройте предыдущий отчёт", "Ошибка создания отчёта", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButSubReset_Click(object sender, RoutedEventArgs e)
        {
            CitProfSearch.Clear();
            vmDataSold.LoadData();
            CollectionViewSource.GetDefaultView(vmDataSold.DataSoldiers).Refresh();
        }
    }
}
