﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Entities;

public partial class MarminaAttendanceContext : DbContext
{
    public MarminaAttendanceContext(DbContextOptions<MarminaAttendanceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Classes> Classes { get; set; }

    public virtual DbSet<ServantWeek> ServantWeek { get; set; }

    public virtual DbSet<Servants> Servants { get; set; }

    public virtual DbSet<Time> Time { get; set; }

    public virtual DbSet<Weeks> Weeks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Classes>(entity =>
        {
            entity.Property(e => e.Intercessor)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Time).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Classes_Time");
        });

        modelBuilder.Entity<ServantWeek>(entity =>
        {
            entity.HasKey(e => new { e.ServantId, e.WeekId });

            entity.Property(e => e.Notes).HasMaxLength(100);

            entity.HasOne(d => d.Servant).WithMany(p => p.ServantWeek)
                .HasForeignKey(d => d.ServantId)
                .HasConstraintName("FK_ServantWeek_Servants");

            entity.HasOne(d => d.Week).WithMany(p => p.ServantWeek)
                .HasForeignKey(d => d.WeekId)
                .HasConstraintName("FK_ServantWeek_Weeks");
        });

        modelBuilder.Entity<Servants>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(80);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(13);
            entity.Property(e => e.Photo).HasMaxLength(60);

            entity.HasOne(d => d.Class).WithMany(p => p.Servants)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servants_Classes");
        });

        modelBuilder.Entity<Time>(entity =>
        {
            entity.Property(e => e.Time1)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Time");
        });

        modelBuilder.Entity<Weeks>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}