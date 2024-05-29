﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Praktik_Work_2.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Praktik_Work_2.Models;

public partial class Soldier
{
    public int SoldierId { get; set; }

    public int SubdivisionId { get; set; }

    public string SoldierName { get; set; }

    public string SoldierSurname { get; set; }

    public DateTime DateBirth { get; set; }

    public int? Growth { get; set; }

    public int? Weight { get; set; }

    public string Rank { get; set; }

    public DateTime DateGiveRank { get; set; }

    public string Post { get; set; }

    public string DutyForm { get; set; }

    public DateTime DateMobilization { get; set; }


    public virtual ICollection<DataSoldier> DataSoldiers { get; set; } = new List<DataSoldier>();

    public virtual Subdivision Subdivision { get; set; }

    public Soldier ShallowCopy()
    {
        return (Soldier)this.MemberwiseClone();
    }

    public Soldier CopyFromSoldier(Soldier soldier)
    {
        SubdivViewModel vmSubd = new SubdivViewModel();
        int subdId = 0;
        foreach (var subdivision in vmSubd.ListSubd)
        {
            if (subdivision.SubdivisionId == soldier.SubdivisionId)
            {
                subdId = soldier.SubdivisionId;
                break;
            }
        }
        if (subdId != 0)
        {
            this.SoldierId = soldier.SoldierId;
            this.SubdivisionId = subdId;
            this.SoldierName = soldier.SoldierName;
            this.SoldierSurname = soldier.SoldierSurname;
            this.DateBirth = soldier.DateBirth;
            this.Growth = soldier.Growth;
            this.Weight = soldier.Weight;
            this.Rank = soldier.Rank;
            this.DateGiveRank = soldier.DateGiveRank;
            this.Post = soldier.Post;
            this.DutyForm = soldier.DutyForm;
            this.DateMobilization = soldier.DateMobilization;
        }
        return this;
    }
}