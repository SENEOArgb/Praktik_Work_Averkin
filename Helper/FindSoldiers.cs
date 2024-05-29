using Praktik_Work_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Praktik_Work_2.Helper
{
    public class FindSoldiers
    {
        int Id;
        public FindSoldiers(int Id)
        {
            this.Id = Id;
        }

        public bool SoldiersPredicate(Soldier soldiers)
        {
            return soldiers.SoldierId == Id;
        }
    }
}
