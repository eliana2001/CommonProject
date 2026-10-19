
using Microsoft.EntityFrameworkCore;

namespace EduPr.Data;

public partial class AppDbContext
{
    public virtual DbSet<PatientAppeal> PatientAppeals { get; set; }
        = null!;

    public virtual DbSet<PatientAppealHistory> PatientAppealHistories
    { get; set; } = null!;

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientAppeal>(entity =>
        {
            entity.HasKey(e => e.IdAppeal)
                .HasName("patient_appeal_pkey");

            entity.ToTable("patient_appeal");

            entity.Property(e => e.IdAppeal)
                .HasColumnName("id_appeal")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.IdPatient)
                .HasColumnName("id_patient");

            entity.Property(e => e.IdResponsibleUser)
                .HasColumnName("id_responsible_user");

            entity.Property(e => e.SubjectAppeal)
                .HasMaxLength(200)
                .HasColumnName("subject_appeal");

            entity.Property(e => e.DescriptionAppeal)
                .HasColumnType("text")
                .HasColumnName("description_appeal");

            entity.Property(e => e.TypeAppeal)
                .HasMaxLength(30)
                .HasColumnName("type_appeal");

            entity.Property(e => e.CategoryAppeal)
                .HasMaxLength(100)
                .HasColumnName("category_appeal");

            entity.Property(e => e.StatusAppeal)
                .HasMaxLength(30)
                .HasDefaultValue("Новое")
                .HasColumnName("status_appeal");

            entity.Property(e => e.CreatedAppeal)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_appeal");

            entity.Property(e => e.DueDateAppeal)
                .HasColumnType("date")
                .HasColumnName("due_date_appeal");

            entity.Property(e => e.UpdatedAppeal)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_appeal");

            entity.HasOne(e => e.IdPatientNavigation)
                .WithMany()
                .HasForeignKey(e => e.IdPatient)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("patient_appeal_patient_fk");

            entity.HasOne(e => e.IdResponsibleUserNavigation)
                .WithMany()
                .HasForeignKey(e => e.IdResponsibleUser)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("patient_appeal_responsible_user_fk");

            entity.HasIndex(e => e.IdPatient);
            entity.HasIndex(e => e.StatusAppeal);
            entity.HasIndex(e => e.CategoryAppeal);
            entity.HasIndex(e => e.DueDateAppeal);
            entity.HasIndex(e => e.IdResponsibleUser);
        });

        modelBuilder.Entity<PatientAppealHistory>(entity =>
        {
            entity.HasKey(e => e.IdHistory)
                .HasName("patient_appeal_history_pkey");

            entity.ToTable("patient_appeal_history");

            entity.Property(e => e.IdHistory)
                .HasColumnName("id_history")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.IdAppeal)
                .HasColumnName("id_appeal");

            entity.Property(e => e.IdUser)
                .HasColumnName("id_user");

            entity.Property(e => e.ActionHistory)
                .HasMaxLength(100)
                .HasColumnName("action_history");

            entity.Property(e => e.DetailsHistory)
                .HasColumnType("text")
                .HasColumnName("details_history");

            entity.Property(e => e.CreatedHistory)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_history");

            entity.HasOne(e => e.IdAppealNavigation)
                .WithMany(e => e.History)
                .HasForeignKey(e => e.IdAppeal)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("patient_appeal_history_appeal_fk");

            entity.HasOne(e => e.IdUserNavigation)
                .WithMany()
                .HasForeignKey(e => e.IdUser)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("patient_appeal_history_user_fk");

            entity.HasIndex(e => new { e.IdAppeal, e.CreatedHistory });
        });
    }
}
