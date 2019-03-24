using FASTCapstonePortal.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FASTCapstonePortal
{
    public class CapstoneDBContext : IdentityDbContext<CapstoneUser, IdentityRole<int>, int>
    {

        public CapstoneDBContext(DbContextOptions<CapstoneDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelbuilder.Entity<CapstoneUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

            modelbuilder.Entity<CapstoneUser>()
            .Property(u => u.UserName)
            .IsRequired();

            modelbuilder.Entity<Skill>()
            .HasIndex(s => s.Name)
            .IsUnique();

            modelbuilder.Entity<GroupsProjects>()
            .HasKey(gp => new { gp.GroupId, gp.ProjectId });

            modelbuilder.Entity<Group>()
            .HasOne(g => g.FinalProjectAssigned)
            .WithOne(p => p.AssignedGroup)
            .OnDelete(DeleteBehavior.SetNull);

            modelbuilder.Entity<Project>()
            .HasOne(p => p.AssignedGroup)
            .WithOne(g => g.FinalProjectAssigned)
            .HasForeignKey<Project>("AssignedGroupId")
            .OnDelete(DeleteBehavior.SetNull);

            modelbuilder.Entity<Student>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .OnDelete(DeleteBehavior.SetNull);

            modelbuilder.Entity<Entreaty>()
            .HasIndex(e => new { e.StudentId, e.GroupId}).IsUnique();

            modelbuilder.Entity<Entreaty>()
            .HasOne(r => r.NotificationContext)
            .WithOne()
            .HasForeignKey<NotificationContext>("EntreatyId")
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<Entreaty>()
            .HasOne(e => e.Group)
            .WithMany(g => g.Entreaties)
            .HasForeignKey(e => e.GroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<Entreaty>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Entreaties)
            .HasForeignKey(e => e.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<Notification>()
            .HasOne(n => n.Receiver)
            .WithMany(cu => cu.ReceivedNotifications)
            .HasForeignKey(n => n.ReceiverId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<Notification>()
            .HasOne(n => n.NotificationContext)
            .WithMany(nc => nc.NotificationsSent)
            .HasForeignKey(n => n.NotificationContextId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<GroupsProjects>()
            .HasOne(gp => gp.Group)
            .WithMany(g => g.PreferredProjects)
            .HasForeignKey(gp => gp.GroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<GroupsProjects>()
            .HasOne(gp => gp.Project)
            .WithMany(p => p.PreferredByGroups)
            .HasForeignKey(gp => gp.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<StudentsSkills>()
            .HasKey(ss => new { ss.StudentId, ss.SkillId });

            modelbuilder.Entity<StudentsSkills>()
            .HasOne(ss => ss.Student)
            .WithMany(s => s.Skills)
            .HasForeignKey(ss => ss.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelbuilder.Entity<StudentsSkills>()
            .HasOne(ss => ss.Skill)
            .WithMany(s => s.Students)
            .HasForeignKey(ss => ss.SkillId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelbuilder);
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationContext> NotificationContexts { get; set; }

        public DbSet<Entreaty> Entreaties { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<GroupsProjects> GroupsProjects { get; set; }

        public DbSet<StudentsSkills> StudentsSkills { get; set; }

        public DbSet<Skill> Skills { get; set; }
    }
}
