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
    /// Логика взаимодействия для WindowNewSoldier.xaml
    /// </summary>
    public partial class WindowNewSoldier : Window
    {
        public WindowNewSoldier(bool isEditing = false)
        {
            InitializeComponent();

            IsEditing = isEditing;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // ...

            if (IsEditing)
            {
                BtSave.Content = "Изменить";
            }

            // ...
        }

        public bool IsEditing { get; set; } = false;

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
