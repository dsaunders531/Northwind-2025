// <copyright file="Region.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Context.Models
{
    [Table("Region")]
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        [Key]
        [Column("RegionID")]
        public int RegionId { get; set; }

        [StringLength(50)]
        public string RegionDescription { get; set; } = null!;

        [InverseProperty("Region")]
        public virtual ICollection<Territory> Territories { get; set; }
    }
}
