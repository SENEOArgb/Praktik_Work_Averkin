using Praktik_Work_2.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Praktik_Work_2.Views.UCtoCommander
{
    /// <summary>
    /// Логика взаимодействия для DataSoldiersUc.xaml
    /// </summary>
    public partial class DataSoldiersUc : UserControl
    {
        public DataSoldiersUc()
        {
            InitializeComponent();
            this.DataContext = new DataSoldViewModel();
        }
    }
}
