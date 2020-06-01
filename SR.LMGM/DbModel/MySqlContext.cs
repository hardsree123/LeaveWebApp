using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SR.LMGM.Models;

namespace SR.LMGM.DbModel
{
    public partial class MySqlContext : DbContext
    {
        public MySqlContext()
        {
        }

        public MySqlContext(DbContextOptions<MySqlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL(Helper.SRConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Designation).HasColumnName("designation");

                entity.Property(e => e.Empcode)
                    .IsRequired()
                    .HasColumnName("empcode")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Empcont)
                    .IsRequired()
                    .HasColumnName("empcont")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Empname)
                    .IsRequired()
                    .HasColumnName("empname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Managedby)
                    .HasColumnName("managedby")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
