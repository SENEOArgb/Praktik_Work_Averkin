﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Praktik_Work_2.Models;

public partial class Subdivision
{
    public int SubdivisionId { get; set; }

    public string SubdivisionName { get; set; }

    public int? SubdivisionNumber { get; set; }

    public string SubdivisionCommander { get; set; }

    public string SubdivisionLocate { get; set; }

    public DateTime DateAppear { get; set; }

    public virtual ICollection<Soldier> Soldiers { get; set; } = new List<Soldier>();

    public Subdivision ShallowCopy()
    {
        return (Subdivision)this.MemberwiseClone();
    }
}