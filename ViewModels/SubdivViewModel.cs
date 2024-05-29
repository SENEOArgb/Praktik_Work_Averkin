using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Praktik_Work_2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktik_Work_2.ViewModels
{
    public class SubdivViewModel :ViewModelBase, INotifyPropertyChanged
    {
        public readonly Com_DataContext _context;
        private ObservableCollection<Subdivision> _subdivision;
        public ObservableCollection<Subdivision> ListSubd { get; set; } = new ObservableCollection<Subdivision>();

        public ObservableCollection<Subdivision> Subdivision
        {
            get { return _subdivision; }
            set
            {
                _subdivision = value;
                OnPropertyChanged(nameof(Subdivision));
            }
        }

        public SubdivViewModel()
        {
            var options = new DbContextOptionsBuilder<Com_DataContext>()
                .UseSqlServer("Server=DESKTOP-U4QAS8P;Database=Com_Data;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=false;")
                .Options;

            _context = new Com_DataContext(options);
            LoadData();
        }

        public void LoadData()
        {
            Subdivision = new ObservableCollection<Subdivision>(_context.Subdivisions.ToList());
        }

        private Subdivision _selectedSubdivision;
        public Subdivision SelectedSubdivision
        {
            get { return _selectedSubdivision; }
            set
            {
                _selectedSubdivision = value;
                OnPropertyChanged(nameof(SelectedSubdivision));
            }
        }
    }
}
