// <copyright file="UserMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Providers.Mappers
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MyAccess.Domains.Entities;

    public class UserMapper : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(e => e.Id)
                   .IsRequired();

            builder
                .HasIndex(u => u.EmailAddress)
                .IsUnique();
        }
    }
}
