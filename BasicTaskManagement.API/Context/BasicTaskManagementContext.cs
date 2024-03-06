using BasicTaskManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasicTaskManagement.API.Context;

public partial class BasicTaskManagementContext : DbContext
{
    public BasicTaskManagementContext() { }

    public BasicTaskManagementContext(DbContextOptions<BasicTaskManagementContext> options)
        : base(options) { }

    public virtual DbSet<TaskGroup> TaskGroups { get; set; }

    public virtual DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskGroup>(entity =>
        {
            entity.HasIndex(e => e.Name, "UC_TaskGroupName").IsUnique();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.HasOne(d => d.TaskGroup).WithMany(p => p.TaskItems)
                .HasForeignKey(d => d.TaskGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskItems_TaskGroups");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
