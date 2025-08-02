﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UploadImagem.WebApi.Models;

namespace UploadImagem.WebApi.DbContextImagem;

public partial class UploadImagemDbContext : DbContext
{
    public UploadImagemDbContext()
    {
    }

    public UploadImagemDbContext(DbContextOptions<UploadImagemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Imagen> Imagens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=G15-MVINICIUS;Database=UploadImagemDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Imagens__3214EC0753A04898");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
