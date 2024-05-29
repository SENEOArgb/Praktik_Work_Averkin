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

namespace Praktik_Work_2.Views.WindowNew
{
    /// <summary>
    /// Логика взаимодействия для WindowNewDataSoldier.xaml
    /// </summary>
    public partial class WindowNewDataSoldier : Window
    {
        public WindowNewDataSoldier(bool isEditing = false)
        {
            InitializeComponent();

            IsEditing = isEditing;
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public bool IsEditing { get; set; } = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // ...

            if (IsEditing)
            {
                BtSave.Content = "Изменить";
            }

            // ...
        }
    }
}
