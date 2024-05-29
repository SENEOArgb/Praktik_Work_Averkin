using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Praktik_Work_2.Helper;
using Praktik_Work_2.Models;
using Praktik_Work_2.ViewModels;
using Praktik_Work_2.Views.UCtoCommander;
using Praktik_Work_2.Views.WindowNew;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Diagnostics;
using System.Windows.Controls;

namespace Praktik_Work_2.Views.UCtoOfficer
{
    /// <summary>
    /// Логика взаимодействия для Soldrs.xaml
    /// </summary>
    public partial class Soldrs : UserControl, ExcelEx
    {
        public ObservableCollection<Soldier> ListSoldier { get; set; } = new ObservableCollection<Soldier>();

        SoldiersViewModel vmSoldiers = new SoldiersViewModel();

        private SubdivViewModel vmSubdivision;

        public Soldrs()
        {
            InitializeComponent();

            this.DataContext = vmSoldiers;

            // загружаем данные из базы данных
            vmSoldiers.LoadData();
        }


        public int MaxIdSold()
        {
            int max = 0;
            foreach (var soldier in this.vmSoldiers.Soldiers)
            {
                if (max < soldier.SoldierId)
                {
                    max = soldier.SoldierId;
                };
            }
            return max;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewSoldier wnSoldier = new WindowNewSoldier
            {
                Title = "Новый боец"
            };

            int maxIdSoldier = MaxIdSold() + 1;
            Soldier soldier = new Soldier();
            /*{
                SoldierId = maxIdSoldier
            };*/

            wnSoldier.DataContext = soldier;

            vmSoldiers.subdivisions = vmSoldiers.GetSubdivisions();
            wnSoldier.CbSubId.ItemsSource = vmSoldiers.subdivisions;

            if (wnSoldier.ShowDialog() == true)
            {
                Subdivision subdivision = (Subdivision)wnSoldier.CbSubId.SelectedValue;
                if (subdivision != null)
                {
                    soldier.SubdivisionId = subdivision.SubdivisionId;
                    // добавляем нового солдата в контекст базы данных
                    vmSoldiers._context.Soldiers.Add(soldier);
                    // сохраняем изменения в базе данных
                    vmSoldiers._context.SaveChanges();
                    // обновляем список солдат в DataGrid
                    vmSoldiers.Soldiers.Add(soldier);
                    OnPropertyChanged(nameof(vmSoldiers.Soldiers));
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
            if (_soldiersList.SelectedItem == null)
            {
                MessageBox.Show("Выберите солдата для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            WindowNewSoldier wnSold = new WindowNewSoldier(isEditing: true)
            {
                Title = "Редактирование данных бойца"
            };
            Soldier soldier = (Soldier)_soldiersList.SelectedItem;
            var tempSoldier = soldier.ShallowCopy();
            wnSold.DataContext = tempSoldier;

            vmSubdivision = new SubdivViewModel();
            vmSoldiers.subdivisions = vmSubdivision.ListSubd.ToList();
            wnSold.CbSubId.ItemsSource = vmSoldiers.GetSubdivisions();
            wnSold.CbSubId.SelectedValue = soldier.SubdivisionId;

            if (wnSold.ShowDialog() == true)
            {
                // сохранение данных в опреративную память
                // перенос данных из временного класса отображения данных
                var subdivision = (Subdivision)wnSold.CbSubId.SelectedValue;
                if (subdivision != null)
                {
                    soldier.SubdivisionId = subdivision.SubdivisionId;
                    soldier.SoldierName = tempSoldier.SoldierName;
                    soldier.SoldierSurname = tempSoldier.SoldierSurname;
                    soldier.DateBirth = tempSoldier.DateBirth;
                    soldier.Growth = tempSoldier.Growth;
                    soldier.Weight = tempSoldier.Weight;
                    soldier.Rank = tempSoldier.Rank;
                    soldier.DateGiveRank = tempSoldier.DateGiveRank;
                    soldier.Post = tempSoldier.Post;
                    soldier.DutyForm = tempSoldier.DutyForm;
                    soldier.DateMobilization = tempSoldier.DateMobilization;

                    // сохраняем изменения в базе данных
                    vmSoldiers._context.Entry(soldier).State = EntityState.Modified;
                    vmSoldiers._context.SaveChanges();

                    CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();
                }
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            Soldier soldier = (Soldier)_soldiersList.SelectedItem;

            if (soldier == null)
            {
                MessageBox.Show("Выберите солдата для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Удалить данные по солдату: \n" + soldier.SoldierId + " ",
                                              "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                vmSoldiers._context.Soldiers.Remove(soldier);
                vmSoldiers._context.SaveChanges();

                vmSoldiers.Soldiers.Remove(soldier);

                CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();
            }
        }

        private void ButSubSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = vmSoldiers._context.Soldiers.AsQueryable();

            if (!string.IsNullOrEmpty(SubdSearch.Text) && int.TryParse(SubdSearch.Text, out int subdId))
            {
                query = query.Where(b => b.SubdivisionId == subdId);
            }

            MessageBoxResult result = MessageBox.Show("Создать отчет по поиску в файле Excel?", "Поиск", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                vmSoldiers.Soldiers = new ObservableCollection<Soldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();

                ExportToExcel();

                Process.Start(new ProcessStartInfo("Soldiers.xlsx") { UseShellExecute = true });
            }
            else
            {
                vmSoldiers.Soldiers = new ObservableCollection<Soldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();
            }
        }

        public void ExportToExcel()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Soldiers");

            worksheet.Cell(1, 1).Value = "Номер солдата";
            worksheet.Cell(1, 2).Value = "Подразделение";
            worksheet.Cell(1, 3).Value = "Имя солдата";
            worksheet.Cell(1, 4).Value = "Фамилия солдата";
            worksheet.Cell(1, 5).Value = "Дата рождения";
            worksheet.Cell(1, 6).Value = "Рост, см";
            worksheet.Cell(1, 7).Value = "Вес, кг";
            worksheet.Cell(1, 8).Value = "Звание";
            worksheet.Cell(1, 9).Value = "Дата получения звания";
            worksheet.Cell(1, 10).Value = "Должность";
            worksheet.Cell(1, 11).Value = "Форма службы";
            worksheet.Cell(1, 12).Value = "Дата мобилизации";
            
            int rowIndex = 2;
            foreach (var soldier in _soldiersList.ItemsSource)
            {
                worksheet.Cell(rowIndex, 1).Value = ((Soldier)soldier).SoldierId;
                worksheet.Cell(rowIndex, 2).Value = ((Soldier)soldier).SubdivisionId;
                worksheet.Cell(rowIndex, 3).Value = ((Soldier)soldier).SoldierName;
                worksheet.Cell(rowIndex, 4).Value = ((Soldier)soldier).SoldierSurname;
                worksheet.Cell(rowIndex, 5).Value = ((Soldier)soldier).DateBirth.ToString("dd.MM.yyyy");
                worksheet.Cell(rowIndex, 6).Value = ((Soldier)soldier).Growth;
                worksheet.Cell(rowIndex, 7).Value = ((Soldier)soldier).Weight;
                worksheet.Cell(rowIndex, 8).Value = ((Soldier)soldier).Rank;
                worksheet.Cell(rowIndex, 9).Value = ((Soldier)soldier).DateGiveRank.ToString("dd.MM.yyyy");
                worksheet.Cell(rowIndex, 10).Value = ((Soldier)soldier).Post;
                worksheet.Cell(rowIndex, 11).Value = ((Soldier)soldier).DutyForm;
                worksheet.Cell(rowIndex, 12).Value = ((Soldier)soldier).DateMobilization.ToString("dd.MM.yyyy");

                rowIndex++;
            }

            var range = worksheet.RangeUsed();
            range.Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
            range.Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
            range.Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
            range.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);

            worksheet.Columns().AdjustToContents();

            try
            {
                workbook.SaveAs("Soldiers.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Закройте предыдущий отчёт", "Ошибка создания отчёта", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButSubReset_Click(object sender, RoutedEventArgs e)
        {
            SubdSearch.Clear();
            DutyFormSearch.Clear();
            RankSearch.Clear();
            vmSoldiers.LoadData();
            CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();
        }

        private void ButDutyFormSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = vmSoldiers._context.Soldiers.AsQueryable();

            if (!string.IsNullOrEmpty(DutyFormSearch.Text))
            {
                query = query.Where(b => b.DutyForm.Contains(DutyFormSearch.Text));
            }

            MessageBoxResult result = MessageBox.Show("Создать отчет по поиску в файле Excel?", "Поиск", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                vmSoldiers.Soldiers = new ObservableCollection<Soldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();

                ExportToExcel();

                Process.Start(new ProcessStartInfo("Soldiers.xlsx") { UseShellExecute = true });
            }
            else
            {
                vmSoldiers.Soldiers = new ObservableCollection<Soldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();
            }
        }

        private void ButRankSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = vmSoldiers._context.Soldiers.AsQueryable();

            if (!string.IsNullOrEmpty(RankSearch.Text))
            {
                query = query.Where(b => b.Rank.Contains(RankSearch.Text));
            }

            MessageBoxResult result = MessageBox.Show("Создать отчет по поиску в файле Excel?", "Поиск", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                vmSoldiers.Soldiers = new ObservableCollection<Soldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();

                ExportToExcel();

                Process.Start(new ProcessStartInfo("Soldiers.xlsx") { UseShellExecute = true });
            }
            else
            {
                vmSoldiers.Soldiers = new ObservableCollection<Soldier>(query.ToList());
                CollectionViewSource.GetDefaultView(vmSoldiers.Soldiers).Refresh();
            }
        }
    }
}