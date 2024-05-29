using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Praktik_Work_2.Helper;
using Praktik_Work_2.Models;
using Praktik_Work_2.ViewModels;
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

namespace Praktik_Work_2.Views.UCtoOfficer
{
    /// <summary>
    /// Логика взаимодействия для Subdiv.xaml
    /// </summary>
    public partial class Subdiv : UserControl
    {

        public ObservableCollection<Subdivision> ListSubdivision { get; set; } = new ObservableCollection<Subdivision>();

        SubdivViewModel vmSubdivision = new SubdivViewModel();

        public Subdiv()
        {
            InitializeComponent();
            this.DataContext = vmSubdivision;

            vmSubdivision.LoadData();
        }
        public int MaxIdSub()
        {
            int max = 0;
            foreach (var r in this.vmSubdivision.Subdivision)
            {
                if (max < r.SubdivisionId)
                {
                    max = r.SubdivisionId;
                };
            }
            return max;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowNewSubdivision wnSubd = new WindowNewSubdivision
            {
                Title = "Новое подразделение"
            };

            Subdivision subdivision = new Subdivision();

            wnSubd.DataContext = subdivision;

            if (wnSubd.ShowDialog() == true)
            {
                // добавляем новое подразделение в контекст базы данных
                vmSubdivision._context.Subdivisions.Add(subdivision);
                // сохраняем изменения в базе данных
                vmSubdivision._context.SaveChanges();
                // обновляем список подразделений в DataGrid
                vmSubdivision.Subdivision.Add(subdivision);
                OnPropertyChanged(nameof(vmSubdivision.Subdivision));
                vmSubdivision.SelectedSubdivision = subdivision;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_subdivList.SelectedItem == null)
            {
                MessageBox.Show("Выберите подразделение для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            WindowNewSubdivision wnSubd = new WindowNewSubdivision
            {
                Title = "Редактирование подразделения"
            };

            Subdivision subdivision = (Subdivision)_subdivList.SelectedItem;
            Subdivision tempSubdivision = new Subdivision();

            tempSubdivision = subdivision.ShallowCopy();
            wnSubd.DataContext = tempSubdivision;

            if (wnSubd.ShowDialog() == true)
            {
                // сохранение данных в оперативной памяти
                subdivision.SubdivisionName = tempSubdivision.SubdivisionName;
                subdivision.SubdivisionNumber = tempSubdivision.SubdivisionNumber;
                subdivision.SubdivisionCommander = tempSubdivision.SubdivisionCommander;
                subdivision.SubdivisionLocate = tempSubdivision.SubdivisionLocate;
                subdivision.DateAppear = tempSubdivision.DateAppear;

                // сохраняем изменения в базе данных
                vmSubdivision._context.Entry(subdivision).State = EntityState.Modified;
                vmSubdivision._context.SaveChanges();

                // обновляем представление данных
                CollectionViewSource.GetDefaultView(vmSubdivision.Subdivision).Refresh();
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            Subdivision subdivision = (Subdivision)_subdivList.SelectedItem;

            if (subdivision == null)
            {
                MessageBox.Show("Выберите подразделение для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Удалить данные по подразделению: " + subdivision.SubdivisionId, "Предупреждение",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                vmSubdivision._context.Subdivisions.Remove(subdivision);
                vmSubdivision._context.SaveChanges();

                vmSubdivision.Subdivision.Remove(subdivision);

                CollectionViewSource.GetDefaultView(vmSubdivision.Subdivision).Refresh();
            }
        }
    }
}
