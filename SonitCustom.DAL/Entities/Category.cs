﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SonitCustom.DAL.Entities;

public partial class Category
{
    public int CateId { get; set; }

    public string CateName { get; set; }

    public string Prefix { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}