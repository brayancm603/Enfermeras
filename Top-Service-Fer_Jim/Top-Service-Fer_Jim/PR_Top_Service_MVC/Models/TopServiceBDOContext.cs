using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Models
{
    public partial class TopServiceBDOContext : DbContext
    {
        public TopServiceBDOContext()
        {
        }

        public TopServiceBDOContext(DbContextOptions<TopServiceBDOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<AreaProfesional> AreaProfesionals { get; set; } = null!;
        public virtual DbSet<Costumer> Costumers { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Postulation> Postulations { get; set; } = null!;
        public virtual DbSet<Profesional> Profesionals { get; set; } = null!;
        public virtual DbSet<Quotation> Quotations { get; set; } = null!;
        public virtual DbSet<Receipt> Receipts { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("workstation id=TopServiceDB.mssql.somee.com;packet size=4096;user id=TopServiceDB;pwd=Univalle;data source=TopServiceDB.mssql.somee.com;persist security info=False;initial catalog=TopServiceDB;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.IdAdmin);

                entity.ToTable("Admin");

                entity.Property(e => e.IdAdmin)
                    .ValueGeneratedNever()
                    .HasColumnName("Id_Admin");

                entity.Property(e => e.Birthdate)
                    .HasColumnType("date")
                    .HasColumnName("birthdate");

                entity.Property(e => e.CelphoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Celphone_Number");

                entity.HasOne(d => d.IdAdminNavigation)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.IdAdmin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_Person");
            });
            modelBuilder.Entity<AreaProfesional>(entity =>
            {
                entity.HasKey(e => e.IdProfesional);
                entity.ToTable("AreaProfesional");
                entity.Property(e => e.IdProfesional).HasColumnName("Id_Profesional");

                entity.Property(e => e.idArea).HasColumnName("id_Area");




                entity.HasOne(d => d.IdProfesionalNavigation)
                    .WithMany(p => p.AreaProfesionals)
                    .HasForeignKey(d => d.IdProfesional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Postulation_Profesional");

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.AreaProfesionals)
                    .HasForeignKey(d => d.idArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotation_JobArea");
            });

            modelBuilder.Entity<Costumer>(entity =>
            {
                entity.HasKey(e => e.IdCostumer);

                entity.ToTable("Costumer");

                entity.Property(e => e.IdCostumer)
                    .ValueGeneratedNever()
                    .HasColumnName("Id_Costumer");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Ci)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCostumerNavigation)
                    .WithOne(p => p.Costumer)
                    .HasForeignKey<Costumer>(d => d.IdCostumer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Costumer_Person");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.IdDepartment);

                entity.ToTable("Department");

                entity.Property(e => e.IdDepartment)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Id_Department");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<JobArea>(entity =>
            {
                entity.HasKey(e => e.idArea);

                entity.ToTable("JobArea");

                entity.Property(e => e.idArea)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_Area");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.IdPerson);

                entity.ToTable("Person");

                entity.Property(e => e.IdPerson).HasColumnName("Id_Person");

                entity.Property(e => e.IdDepartment).HasColumnName("Id_Department");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SecondLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();


                entity.HasOne(d => d.IdDepartmentNavigation)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.IdDepartment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_Department");
            });

            

            modelBuilder.Entity<Postulation>(entity =>
            {
                entity.HasKey(e => e.IdPostulation);

                entity.ToTable("Postulation");

                entity.Property(e => e.IdPostulation).HasColumnName("Id_Postulation");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.ProfessionalTitles)
                     .HasMaxLength(150)
                     .IsUnicode(false);

                entity.Property(e => e.Certification)
                  .HasMaxLength(150)
                  .IsUnicode(false);

                entity.Property(e => e.WorkExperience)
                  .HasMaxLength(150)
                  .IsUnicode(false);

                entity.Property(e => e.idArea).HasColumnName("id_Area");
                entity.Property(e => e.IdProfessional).HasColumnName("Id_Professional");

                entity.HasOne(d => d.IdAreaNavigation)
                  .WithMany(p => p.Postulations)
                  .HasForeignKey(d => d.idArea)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Quotation_JobArea");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdProfessionalNavigation)
                    .WithMany(p => p.Postulations)
                    .HasForeignKey(d => d.IdProfessional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Postulation_Profesional");
            });

            modelBuilder.Entity<Profesional>(entity =>
            {
                entity.HasKey(e => e.IdProfesional);

                entity.ToTable("Profesional");

                entity.Property(e => e.IdProfesional)
                    .ValueGeneratedNever()
                    .HasColumnName("Id_Profesional");
             

                entity.Property(e => e.Birthdate).HasColumnType("date");

             
                

                entity.HasOne(d => d.IdProfesionalNavigation)
                    .WithOne(p => p.Profesional)
                    .HasForeignKey<Profesional>(d => d.IdProfesional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profesional_Person");

                
            });

            modelBuilder.Entity<Quotation>(entity =>
            {
                entity.HasKey(e => e.IdQuotation);

                entity.ToTable("Quotation");

                entity.Property(e => e.IdQuotation).HasColumnName("Id_Quotation");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IdCostumer).HasColumnName("IdCostumer");
                entity.Property(e => e.IdProfesional).HasColumnName("IdProfesional");

             

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();


                entity.HasOne(d => d.IdCostumerNavigation)
               .WithMany(p => p.Quotations)
               .HasForeignKey(d => d.IdCostumer)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Costumer_Person");


                entity.HasOne(d => d.IdProfesionalNavigation)
                .WithMany(p => p.Quotations)
                .HasForeignKey(d => d.IdProfesional)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quotation_Profesional");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.IdReceipt);

                entity.ToTable("Receipt");

                entity.Property(e => e.IdReceipt)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Id_Receipt");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.IdReceiptNavigation)
                    .WithOne(p => p.Receipt)
                    .HasForeignKey<Receipt>(d => d.IdReceipt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Receipt_Service");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.IdService);

                entity.ToTable("Service");

                entity.Property(e => e.IdService).HasColumnName("Id_Service");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IdAdmin).HasColumnName("Id_Admin");

                entity.Property(e => e.IdProfessional).HasColumnName("Id_Professional");
                entity.Property(e => e.IdCostumer).HasColumnName("Id_Costumer");
                
               

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.deleted)
                   .HasMaxLength(1)
                   .IsUnicode(false)
                   .IsFixedLength();

                entity.HasOne(d => d.IdAdminNavigation)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.IdAdmin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Admin");

                entity.HasOne(d => d.IdProfessionalNavigation)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.IdProfessional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Profesional");

                entity.HasOne(d => d.IdCostumerNavigation)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.IdCostumer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Costumer");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.Property(e => e.IdUser)
                    .ValueGeneratedNever()
                    .HasColumnName("Id_User");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Person1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<PR_Top_Service_MVC.Models.PersonAdmin>? PersonAdmin { get; set; }

        public DbSet<PR_Top_Service_MVC.Models.ServiceReceipt>? ServiceReceipt { get; set; }

        public DbSet<PR_Top_Service_MVC.Models.CustomerUser>? CustomerUser { get; set; }

        public DbSet<PR_Top_Service_MVC.Models.JobArea>? JobArea { get; set; }
        public DbSet<PR_Top_Service_MVC.Models.AreaProfesional>? AreaProfesional { get; set; }
        public DbSet<PR_Top_Service_MVC.Models.ProfesionalUser>? ProfesionalUser { get; set; }
    }
}
