using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Models;

public partial class TaskManagementContext : DbContext
{
    public TaskManagementContext()
    {
    }

    public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GnrlNote> GnrlNotes { get; set; }
 
    public virtual DbSet<GnrlSetting> GnrlSettings { get; set; }

    public virtual DbSet<MstrCustomer> MstrCustomers { get; set; }

    public virtual DbSet<MstrNotification> MstrNotifications { get; set; }

    public virtual DbSet<MstrOrganization> MstrOrganizations { get; set; }

    public virtual DbSet<MstrProject> MstrProjects { get; set; }

    public virtual DbSet<MstrRole> MstrRoles { get; set; }

    public virtual DbSet<MstrTask> MstrTasks { get; set; }

    public virtual DbSet<MstrTimeSheet> MstrTimeSheets { get; set; }

    public virtual DbSet<MstrUser> MstrUsers { get; set; }

    public virtual DbSet<MstrUserStory> MstrUserStories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-O5CF6TDH\\MSSQLSERVER01;Database=TaskManagement;TrustServerCertificate=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GnrlNote>(entity =>
        {
            entity.HasKey(e => e.NotesId).HasName("PK__GNRL_Not__35AB5B4AFF183F9B");

            entity.ToTable("GNRL_Notes");

            entity.Property(e => e.NotesId)
                .ValueGeneratedNever()
                .HasColumnName("NotesID");
            entity.Property(e => e.FileName).HasMaxLength(100);
            entity.Property(e => e.LastModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ObjectId).HasColumnName("ObjectID");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.GnrlNotes)
                .HasForeignKey(d => d.LastModifiedBy)
                .HasConstraintName("FK__GNRL_Note__LastM__37A5467C");
        });

        modelBuilder.Entity<GnrlSetting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__GNRL_Set__54372AFD084175A9");

            entity.ToTable("GNRL_Settings");

            entity.Property(e => e.SettingId)
                .ValueGeneratedNever()
                .HasColumnName("SettingID");
            entity.Property(e => e.TaskPrefix).HasMaxLength(50);
            entity.Property(e => e.UserStoryPrefix).HasMaxLength(50);
        });

        modelBuilder.Entity<MstrCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__MSTR_Cus__A4AE64B8930897F7");

            entity.ToTable("MSTR_Customer");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CustomerName).HasMaxLength(100);
        });

        modelBuilder.Entity<MstrNotification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__MSTR_Not__20CF2E32A1B03046");

            entity.ToTable("MSTR_Notification");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("NotificationID");
            entity.Property(e => e.NotificationDate).HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.MstrNotifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_MSTR_Notification_MSTR_User");
        });

        modelBuilder.Entity<MstrOrganization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId);

            entity.ToTable("MSTR_Organization");

            entity.HasIndex(e => e.Email, "UQ__MSTR_Org__A9D10534575A375A").IsUnique();

            entity.Property(e => e.OrganizationId)
                .ValueGeneratedNever()
                .HasColumnName("OrganizationID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Contact).HasMaxLength(15);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LogoUrl).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<MstrProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__MSTR_Pro__761ABED0F53D8E3C");

            entity.ToTable("MSTR_Project");

            entity.Property(e => e.ProjectId)
                .ValueGeneratedNever()
                .HasColumnName("ProjectID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.ProjectTitle).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.MstrProjects)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__MSTR_Proj__Custo__2C3393D0");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.MstrProjectLastModifiedByNavigations)
                .HasForeignKey(d => d.LastModifiedBy)
                .HasConstraintName("FK__MSTR_Proj__LastM__2D27B809");

            entity.HasOne(d => d.Manager).WithMany(p => p.MstrProjectManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__MSTR_Proj__Manag__2B3F6F97");
        });

        modelBuilder.Entity<MstrRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("MSTR_Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<MstrTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__MSTR_Tas__7C6949D13CB5C557");

            entity.ToTable("MSTR_Task");

            entity.Property(e => e.TaskId)
                .ValueGeneratedNever()
                .HasColumnName("TaskID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ParentTaskId).HasColumnName("ParentTaskID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaskNo).HasMaxLength(50);
            entity.Property(e => e.UserStoryId).HasColumnName("UserStoryID");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.MstrTasks)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK__MSTR_Task__Assig__4AB81AF0");

            entity.HasOne(d => d.ParentTask).WithMany(p => p.InverseParentTask)
                .HasForeignKey(d => d.ParentTaskId)
                .HasConstraintName("FK_MSTR_Task_MSTR_Task");

            entity.HasOne(d => d.Project).WithMany(p => p.MstrTasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__MSTR_Task__Proje__4BAC3F29");

            entity.HasOne(d => d.UserStory).WithMany(p => p.MstrTasks)
                .HasForeignKey(d => d.UserStoryId)
                .HasConstraintName("FK_MSTR_Task_MSTR_UserStory");
        });




        modelBuilder.Entity<MstrTimeSheet>(entity =>
        {
            entity.HasKey(e => e.TimeSheetId);

            entity.ToTable("MSTR_TimeSheet");

            entity.Property(e => e.TimeSheetId)
                .ValueGeneratedNever()
                .HasColumnName("TimeSheetID");
            entity.Property(e => e.AddedOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Hours).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.MstrTimeSheetApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK_MSTR_TimeSheet_MSTR_User1");

            entity.HasOne(d => d.Project).WithMany(p => p.MstrTimeSheets)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_MSTR_TimeSheet_MSTR_Project");

            entity.HasOne(d => d.Task).WithMany(p => p.MstrTimeSheets)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_MSTR_TimeSheet_MSTR_Task");

            entity.HasOne(d => d.User).WithMany(p => p.MstrTimeSheetUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_MSTR_TimeSheet_MSTR_User");
        });

        modelBuilder.Entity<MstrUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__MSTR_Use__1788CCAC707B6D62");

            entity.ToTable("MSTR_User");

            entity.HasIndex(e => e.Email, "UQ__MSTR_Use__A9D10534105539B0").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNo).HasMaxLength(15);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.MstrUsers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_MSTR_User_MSTR_Role");
        });

        modelBuilder.Entity<MstrUserStory>(entity =>
        {
            entity.HasKey(e => e.UserStoryId).HasName("PK__MSTR_Use__EE23D125C8137798");

            entity.ToTable("MSTR_UserStory");

            entity.Property(e => e.UserStoryId)
                .ValueGeneratedNever()
                .HasColumnName("UserStoryID");
            entity.Property(e => e.LastModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.MstrUserStoryLastModifiedByNavigations)
                .HasForeignKey(d => d.LastModifiedBy)
                .HasConstraintName("FK__MSTR_User__LastM__403A8C7D");

            entity.HasOne(d => d.Owner).WithMany(p => p.MstrUserStoryOwners)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__MSTR_User__Owner__412EB0B6");

            entity.HasOne(d => d.Project).WithMany(p => p.MstrUserStories)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__MSTR_User__Proje__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
