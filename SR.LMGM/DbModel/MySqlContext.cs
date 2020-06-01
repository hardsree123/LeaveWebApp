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

        public virtual DbSet<Enumvals> Enumvals { get; set; }
        public virtual DbSet<Leaverecords> Leaverecords { get; set; }
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
            modelBuilder.Entity<Enumvals>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");

                entity.ToTable("enumvals");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Desc)
                    .IsRequired()
                    .HasColumnName("desc")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Typedesc)
                    .HasColumnName("typedesc")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Leaverecords>(entity =>
            {
                entity.ToTable("leaverecords");

                entity.HasIndex(e => e.Reqid)
                    .HasName("reqid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Assignedto)
                    .IsRequired()
                    .HasColumnName("assignedto")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Attachmentpath)
                    .HasColumnName("attachmentpath")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Empcode)
                    .IsRequired()
                    .HasColumnName("empcode")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Lastapprovaltime).HasColumnName("lastapprovaltime");

                entity.Property(e => e.Lastapprover)
                    .HasColumnName("lastapprover")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Leavefrom).HasColumnName("leavefrom");

                entity.Property(e => e.Leaveto).HasColumnName("leaveto");

                entity.Property(e => e.Leavetype).HasColumnName("leavetype");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasColumnName("reason")
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.Rejectiondesc)
                    .HasColumnName("rejectiondesc")
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.Reqid)
                    .IsRequired()
                    .HasColumnName("reqid")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'200001'");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Designation).HasColumnName("designation");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

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

                entity.Property(e => e.Teamname)
                    .HasColumnName("teamname")
                    .HasMaxLength(50)
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
