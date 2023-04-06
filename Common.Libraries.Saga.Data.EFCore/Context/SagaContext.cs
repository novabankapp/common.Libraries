
using Common.Libraries.Saga.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common2.Libraries2.Saga2.Data2.EFCore.Context
{
    public class SagaContext : DbContext
    {
        public DbSet<SagaState>? SagaStates { get; set; }
        public DbSet<ConsumedMessage>? ConsumedMessages{ get; set; }
        public SagaContext(DbContextOptions<SagaContext> options)
            : base(options)
        {
            //NpgsqlConnection.GlobalTypeMapper.MapEnum<SagaStatus>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.HasPostgresEnum<SagaStatus>();
            // Addd the Postgres Extension for UUID generation
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<SagaState>(entity =>
            {
                entity.ToTable("sagastate");
                entity.HasKey(x => x.ID);

                entity.HasIndex(x => x.ID)
                    .HasDatabaseName("id")
                    .IsUnique();
                entity.Property(x => x.SagaStatus)
                .HasConversion<string>();

                entity.Property(x => x.ID)
                       .HasColumnName("id")
                       .HasColumnType("uuid")
                       .HasDefaultValueSql("uuid_generate_v4()")  
                       .IsRequired();
                entity.Property(u => u.DateCreated).HasDefaultValueSql("NOW()")
                         .ValueGeneratedOnAdd();

            });

            modelBuilder.Entity<ConsumedMessage>(entity =>
            {
                entity.ToTable("consumedmessages");
                entity.HasKey(x => x.ID);

                entity.HasIndex(x => x.ID)
                    .HasDatabaseName("id")
                    .IsUnique();

                entity.Property(x => x.ID)
                       .HasColumnName("id")
                       .HasColumnType("uuid")
                       .HasDefaultValueSql("uuid_generate_v4()")
                       .IsRequired();
                entity.Property(u => u.DateCreated).HasDefaultValueSql("NOW()")
                         .ValueGeneratedOnAdd();

            });
        }

    }
}
