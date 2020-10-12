using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UsersManagement.DAL.Models
{
    public partial class UserManagementContext : DbContext
    {
        public UserManagementContext()
        {
        }

        public UserManagementContext(DbContextOptions<UserManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<NlogErrorLog> NlogErrorLog { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=UserManagement;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(250);

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<NlogErrorLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NLogErrorLog");

                entity.Property(e => e.ErrorDatetime).HasColumnType("datetime");

                entity.Property(e => e.ErrorId).ValueGeneratedOnAdd();

                entity.Property(e => e.ErrorLevel)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.ErrorSource).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Salt).HasMaxLength(1000);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
