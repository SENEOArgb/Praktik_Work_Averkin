using Microsoft.EntityFrameworkCore;
using Praktik_Work_2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktik_Work_2.ViewModels
{
    public class DataSoldViewModel : ViewModelBase
    {
        public readonly Com_DataContext _context;

        //приватная коллекция для хранения данных после взаимодействия с ними
        private ObservableCollection<DataSoldier> _dataSoldiers;
        //для работы с данными
        public ObservableCollection<DataSoldier> ListDataSolder { get; set; } = new ObservableCollection<DataSoldier>();

        public SoldiersViewModel vmSolder;
        public List<Soldier> soldiers;

        //хранение в списке
        public ObservableCollection<DataSoldier> DataSoldiers
        {
            get { return _dataSoldiers; }
            set
            {
                _dataSoldiers = value;
                OnPropertyChanged(nameof(DataSoldiers));
            }
        }
        
        //конструктор
        public DataSoldViewModel()
        {
            var options = new DbContextOptionsBuilder<Com_DataContext>()
                .UseSqlServer("Server=DESKTOP-U4QAS8P;Database=Com_Data;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=false;")
                .Options;

            _context = new Com_DataContext(options);
            LoadData();
        }

        //загрузка данных из БД
        public void LoadData()
        {
            DataSoldiers = new ObservableCollection<DataSoldier>(_context.DataSoldiers.ToList());
        }

        //private DataSoldier selectedDataSoldier;

        //public DataSoldier SelectedDataSoldier;


        //для внешних ключей солдат

        public List<Soldier> GetSoldiers()
        {
            return _context.Soldiers.ToList();
        }
    }
}
