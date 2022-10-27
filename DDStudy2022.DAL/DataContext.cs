﻿using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.DAL;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users => Set<User>();
}