using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Praktik_Work_2.Helper;
using Praktik_Work_2.Models;
using Praktik_Work_2.Views.WindowNew;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktik_Work_2.ViewModels
{
    public class SoldiersViewModel : ViewModelBase
    {
        public readonly Com_DataContext _context;
        private ObservableCollection<Soldier> _soldiers;

        public ObservableCollection<Soldier> ListSolder { get; set; } = new ObservableCollection<Soldier>();

        public SubdivViewModel vmSubdiv;
        public List<Subdivision> subdivisions;

        public ObservableCollection<Soldier> Soldiers
        {
            get { return _soldiers; }
            set
            {
                _soldiers = value;
                OnPropertyChanged(nameof(Soldiers));
            }
        }

        public SoldiersViewModel()
        {
            var options = new DbContextOptionsBuilder<Com_DataContext>()
                .UseSqlServer("Server=DESKTOP-U4QAS8P;Database=Com_Data;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=false;")
                .Options;

            _context = new Com_DataContext(options);
            LoadData();
        }

        public void LoadData()
        {
            Soldiers = new ObservableCollection<Soldier>(_context.Soldiers.ToList());
        }

        //private Soldier selectedSoldier;

        //public Soldier SelectedSoldier;

        public List<Subdivision> GetSubdivisions()
        {
            return _context.Subdivisions.ToList();
        }
    }
}
