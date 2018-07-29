using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using virtual_diary.Models;

namespace virtual_diary.Infrastructure
{
    public class AuthContext : IdentityDbContext<UserModel>
    {
        public AuthContext() : base("UserManagmentContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuthContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<Admin>().ToTable("AdminUsers");
            modelBuilder.Entity<Parent>().ToTable("Parents");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<SubjectModel>().ToTable("Subjects");
            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<TeacherSubject>().ToTable("TeachersSubjects");
            modelBuilder.Entity<StudentTeacherSubject>().ToTable("StudentsTeachersSubjects");

        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<TeacherSubject> TeachersSubjects { get; set; }
        public DbSet<StudentTeacherSubject> StudentTeacherSubject { get; set; }


    }
}