using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.EF.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Accountant> Accountants { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; }
        public DbSet<Inpatient_Admission> Inpatient_Admissions { get; set; }
        public DbSet<Room> Rooms { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           builder.Entity<ApplicationUser>()
          .HasOne(a => a.Doctor)
          .WithOne(d => d.User)
          .HasForeignKey<Doctor>(d => d.UserId);

            builder.Entity<ApplicationUser>()
            .HasOne(a => a.Patient)
            .WithOne(p => p.User)
            .HasForeignKey<Patient>(p => p.UserId);

            builder.Entity<ApplicationUser>()
           .HasOne(a => a.Accountant)
           .WithOne(ac => ac.User)
           .HasForeignKey<Accountant>(ac => ac.UserId);

            builder.Entity<ApplicationUser>()
           .HasOne(a => a.staff)
           .WithOne(s => s.User)
           .HasForeignKey<Staff>(s => s.UserId);

            builder.Entity<Billing>()
                .HasOne(b => b.Accountant)
                .WithMany(ac => ac.Billings)
                .HasForeignKey(b => b.AccountantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Billing>()
                .HasOne(b => b.Patient)
                .WithMany(p => p.Billings)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Doctor>()
                .HasOne(d => d.Department)
                .WithMany(dep => dep.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inpatient_Admission>()
                .HasOne(ia => ia.Patient)
                .WithMany(p => p.Inpatient_Admissions)
                .HasForeignKey(ia => ia.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inpatient_Admission>()
                .HasOne(ia => ia.Department)
                .WithMany(dep => dep.Inpatient_Admissions)
                .HasForeignKey(ia => ia.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inpatient_Admission>()
                .HasOne(ia => ia.Room)
                .WithMany(r => r.Inpatient_Admissions)
                .HasForeignKey(ia => ia.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inpatient_Admission>()
                .HasOne(ia => ia.Appointment)
                .WithMany(a => a.Inpatient_Admissions)
                .HasForeignKey(ia => ia.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inpatient_Admission>()
                .HasOne(ia => ia.Doctor)
                .WithMany(d => d.Inpatient_Admissions)
                .HasForeignKey(ia => ia.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<Laboratory>()
                .HasOne(l => l.Patient)
                .WithMany(p => p.Laboratories)
                .HasForeignKey(l => l.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Laboratory>()
                .HasOne(l => l.Doctor)
                .WithMany(d => d.Laboratories)
                .HasForeignKey(l => l.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Room>()
                .HasOne(r => r.Department)
                .WithMany(dep => dep.Rooms)
                .HasForeignKey(r => r.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Staff>()
                .HasOne(s => s.Department)
                .WithMany(dep => dep.Staff)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
