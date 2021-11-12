// <copyright file="BaseEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dapper.Contrib.Extensions;

    public class BaseEntity
    {
        [Column(Order = 0)]
        [ExplicitKey]
        [Required(ErrorMessage = "{0} is required")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(Order = 96)]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Column(Order = 97)]
        public Guid CreatedBy { get; set; }

        [Column(Order = 98)]
        public DateTime ModificationDate { get; set; } = DateTime.UtcNow;

        [Column(Order = 99)]
        public Guid ModifiedBy { get; set; }
    }
}
