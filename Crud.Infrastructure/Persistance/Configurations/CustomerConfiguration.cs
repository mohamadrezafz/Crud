﻿
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud.Infrastructure.Persistance.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Primary key
        builder.HasKey(c => c.Id);

        // Unique constraint on (FirstName, LastName, DateOfBirth)
        builder.HasAlternateKey(c => new { c.FirstName, c.LastName, c.DateOfBirth });

        //Unique constraint on email
        builder.HasAlternateKey(c => c.Email);

        // Properties
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.DateOfBirth)
            .IsRequired();

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.BankAccountNumber)
            .HasMaxLength(255);
    }
}

