using EduPr.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EduPr.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
    public virtual DbSet<Cabinet> Cabinets { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<DoctorSchedule> DoctorSchedules { get; set; }
    public virtual DbSet<ExaminationResult> ExaminationResults { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<LabResearch> LabResearches { get; set; }
    public virtual DbSet<MedicalCard> MedicalCards { get; set; }
    public virtual DbSet<Medication> Medications { get; set; }
    public virtual DbSet<MedicationPrescription> MedicationPrescriptions { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }
    public virtual DbSet<PaymentType> PaymentTypes { get; set; }
    public virtual DbSet<ResearchReferral> ResearchReferrals { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Service> Services { get; set; }
    public virtual DbSet<ServiceType> ServiceTypes { get; set; }
    public virtual DbSet<SpecialistSchedule> SpecialistSchedules { get; set; }
    public virtual DbSet<Specialization> Specializations { get; set; }
    public virtual DbSet<User> Users { get; set; }


    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=EducPractice;Username=postgres;Password=75300067");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.IdAppointment).HasName("appointment_pkey");
            entity.ToTable("appointment");
            entity.Property(e => e.IdAppointment).HasColumnName("id_appointment");
            entity.Property(e => e.ComplaintsAppointment).HasColumnName("complaints_appointment");
            entity.Property(e => e.CreatedAppointment)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_appointment");
            entity.Property(e => e.DateAppointment).HasColumnName("date_appointment");
            entity.Property(e => e.DiagnosisAppointment).HasColumnName("diagnosis_appointment");
            entity.Property(e => e.IdCabinet).HasColumnName("id_cabinet");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.IdPatient).HasColumnName("id_patient");
            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.RecommendationsAppointment).HasColumnName("recommendations_appointment");
            entity.Property(e => e.TimeAppointment).HasColumnName("time_appointment");

            entity.HasOne(d => d.IdCabinetNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdCabinet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_id_cabinet_fkey");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_id_doctor_fkey");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_id_patient_fkey");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_id_service_fkey");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_id_status_fkey");
        });

        modelBuilder.Entity<AppointmentStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("appointment_status_pkey");
            entity.ToTable("appointment_status");
            entity.HasIndex(e => e.NameStatus, "appointment_status_name_status_key").IsUnique();
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.NameStatus)
                .HasMaxLength(100)
                .HasColumnName("name_status");
        });

        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(e => e.IdCabinet).HasName("cabinet_pkey");
            entity.ToTable("cabinet");
            entity.Property(e => e.IdCabinet).HasColumnName("id_cabinet");
            entity.Property(e => e.IdDepartment).HasColumnName("id_department");
            entity.Property(e => e.NumberCabinet)
                .HasMaxLength(50)
                .HasColumnName("number_cabinet");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Cabinets)
                .HasForeignKey(d => d.IdDepartment)
                .HasConstraintName("cabinet_id_department_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.IdDepartment).HasName("department_pkey");
            entity.ToTable("department");
            entity.Property(e => e.IdDepartment).HasColumnName("id_department");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.NameDepartment)
                .HasMaxLength(200)
                .HasColumnName("name_department");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("doctor_pkey");
            entity.ToTable("doctor");
            entity.HasIndex(e => e.IdUser, "doctor_id_user_key").IsUnique();
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.FirstNameDoctor)
                .HasMaxLength(100)
                .HasColumnName("first_name_doctor");
            entity.Property(e => e.IdDepartment).HasColumnName("id_department");
            entity.Property(e => e.IdSpecialization).HasColumnName("id_specialization");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.LastNameDoctor)
                .HasMaxLength(100)
                .HasColumnName("last_name_doctor");
            entity.Property(e => e.MiddleNameDoctor)
                .HasMaxLength(100)
                .HasColumnName("middle_name_doctor");
            entity.Property(e => e.PhoneDoctor)
                .HasMaxLength(20)
                .HasColumnName("phone_doctor");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.IdDepartment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doctor_id_department_fkey");

            entity.HasOne(d => d.IdSpecializationNavigation).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.IdSpecialization)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doctor_id_specialization_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.IdUser)
                .HasConstraintName("doctor_id_user_fkey");
        });

        modelBuilder.Entity<DoctorSchedule>(entity =>
        {
            entity.HasKey(e => e.IdDoctorSchedule).HasName("doctor_schedule_pkey");
            entity.ToTable("doctor_schedule");
            entity.Property(e => e.IdDoctorSchedule).HasColumnName("id_doctor_schedule");
            entity.Property(e => e.DateSchedule).HasColumnName("date_schedule");
            entity.Property(e => e.EndTimeSchedule).HasColumnName("end_time_schedule");
            entity.Property(e => e.IdCabinet).HasColumnName("id_cabinet");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.IsAvailableSchedule)
                .HasDefaultValueSql("true")
                .HasColumnName("is_available_schedule");
            entity.Property(e => e.StartTimeSchedule).HasColumnName("start_time_schedule");

            entity.HasOne(d => d.IdCabinetNavigation).WithMany(p => p.DoctorSchedules)
                .HasForeignKey(d => d.IdCabinet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doctor_schedule_id_cabinet_fkey");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.DoctorSchedules)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doctor_schedule_id_doctor_fkey");
        });

        modelBuilder.Entity<ExaminationResult>(entity =>
        {
            entity.HasKey(e => e.IdExaminationResult).HasName("examination_result_pkey");
            entity.ToTable("examination_result");
            entity.Property(e => e.IdExaminationResult).HasColumnName("id_examination_result");
            entity.Property(e => e.ConclusionExam).HasColumnName("conclusion_exam");
            entity.Property(e => e.IdAppointment).HasColumnName("id_appointment");
            entity.Property(e => e.IdLabResearch).HasColumnName("id_lab_research");
            entity.Property(e => e.ResultDateExam)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("result_date_exam");
            entity.Property(e => e.ResultsExam).HasColumnName("results_exam");

            entity.HasOne(d => d.IdAppointmentNavigation).WithMany(p => p.ExaminationResults)
                .HasForeignKey(d => d.IdAppointment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("examination_result_id_appointment_fkey");

            entity.HasOne(d => d.IdLabResearchNavigation).WithMany(p => p.ExaminationResults)
                .HasForeignKey(d => d.IdLabResearch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("examination_result_id_lab_research_fkey");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("invoice_pkey");
            entity.ToTable("invoice");
            entity.Property(e => e.IdInvoice).HasColumnName("id_invoice");
            entity.Property(e => e.DiscountInvoice)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("discount_invoice");
            entity.Property(e => e.FinalAmountInvoice)
                .HasPrecision(10, 2)
                .HasColumnName("final_amount_invoice");
            entity.Property(e => e.IdAppointment).HasColumnName("id_appointment");
            entity.Property(e => e.IdPatient).HasColumnName("id_patient");
            entity.Property(e => e.IdPaymentStatus).HasColumnName("id_payment_status");
            entity.Property(e => e.IssueDateInvoice)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_date_invoice");
            entity.Property(e => e.TotalAmountInvoice)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount_invoice");

            entity.HasOne(d => d.IdAppointmentNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdAppointment)
                .HasConstraintName("invoice_id_appointment_fkey");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_id_patient_fkey");

            entity.HasOne(d => d.IdPaymentStatusNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdPaymentStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_id_payment_status_fkey");
        });

        modelBuilder.Entity<LabResearch>(entity =>
        {
            entity.HasKey(e => e.IdLabResearch).HasName("lab_research_pkey");
            entity.ToTable("lab_research");
            entity.Property(e => e.IdLabResearch).HasColumnName("id_lab_research");
            entity.Property(e => e.CostLabResearch)
                .HasPrecision(10, 2)
                .HasColumnName("cost_lab_research");
            entity.Property(e => e.NameLabResearch)
                .HasMaxLength(200)
                .HasColumnName("name_lab_research");
        });

        modelBuilder.Entity<MedicalCard>(entity =>
        {
            entity.HasKey(e => e.IdMedicalCard).HasName("medical_card_pkey");
            entity.ToTable("medical_card");
            entity.HasIndex(e => e.IdPatient, "medical_card_id_patient_key").IsUnique();
            entity.Property(e => e.IdMedicalCard).HasColumnName("id_medical_card");
            entity.Property(e => e.CreatedCard)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_card");
            entity.Property(e => e.IdPatient).HasColumnName("id_patient");

            entity.HasOne(d => d.IdPatientNavigation).WithOne(p => p.MedicalCard)
                .HasForeignKey<MedicalCard>(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medical_card_id_patient_fkey");
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.IdMedication).HasName("medication_pkey");
            entity.ToTable("medication");
            entity.Property(e => e.IdMedication).HasColumnName("id_medication");
            entity.Property(e => e.NameMedication)
                .HasMaxLength(200)
                .HasColumnName("name_medication");
        });

        modelBuilder.Entity<MedicationPrescription>(entity =>
        {
            entity.HasKey(e => e.IdPrescription).HasName("medication_prescription_pkey");
            entity.ToTable("medication_prescription");
            entity.Property(e => e.IdPrescription).HasColumnName("id_prescription");
            entity.Property(e => e.DosagePrescription)
                .HasMaxLength(200)
                .HasColumnName("dosage_prescription");
            entity.Property(e => e.DurationDaysPrescription).HasColumnName("duration_days_prescription");
            entity.Property(e => e.IdAppointment).HasColumnName("id_appointment");
            entity.Property(e => e.IdMedication).HasColumnName("id_medication");
            entity.Property(e => e.InstructionsPrescription).HasColumnName("instructions_prescription");

            entity.HasOne(d => d.IdAppointmentNavigation).WithMany(p => p.MedicationPrescriptions)
                .HasForeignKey(d => d.IdAppointment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medication_prescription_id_appointment_fkey");

            entity.HasOne(d => d.IdMedicationNavigation).WithMany(p => p.MedicationPrescriptions)
                .HasForeignKey(d => d.IdMedication)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medication_prescription_id_medication_fkey");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient).HasName("patient_pkey");
            entity.ToTable("patient");
            entity.Property(e => e.IdPatient).HasColumnName("id_patient");
            entity.Property(e => e.BirthDatePatient).HasColumnName("birth_date_patient");
            entity.Property(e => e.CreatedPatient)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_patient");
            entity.Property(e => e.FirstNamePatient)
                .HasMaxLength(100)
                .HasColumnName("first_name_patient");
            entity.Property(e => e.LastNamePatient)
                .HasMaxLength(100)
                .HasColumnName("last_name_patient");
            entity.Property(e => e.MiddleNamePatient)
                .HasMaxLength(100)
                .HasColumnName("middle_name_patient");
            entity.Property(e => e.PhonePatient)
                .HasMaxLength(20)
                .HasColumnName("phone_patient");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.IdPaymentStatus).HasName("payment_status_pkey");
            entity.ToTable("payment_status");
            entity.HasIndex(e => e.NamePaymentStatus, "payment_status_name_payment_status_key").IsUnique();
            entity.Property(e => e.IdPaymentStatus).HasColumnName("id_payment_status");
            entity.Property(e => e.NamePaymentStatus)
                .HasMaxLength(100)
                .HasColumnName("name_payment_status");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.IdPaymentType).HasName("payment_type_pkey");
            entity.ToTable("payment_type");
            entity.HasIndex(e => e.NamePaymentType, "payment_type_name_payment_type_key").IsUnique();
            entity.Property(e => e.IdPaymentType).HasColumnName("id_payment_type");
            entity.Property(e => e.NamePaymentType)
                .HasMaxLength(100)
                .HasColumnName("name_payment_type");
        });

        modelBuilder.Entity<ResearchReferral>(entity =>
        {
            entity.HasKey(e => e.IdReferral).HasName("research_referral_pkey");
            entity.ToTable("research_referral");
            entity.Property(e => e.IdReferral).HasColumnName("id_referral");
            entity.Property(e => e.IdAppointment).HasColumnName("id_appointment");
            entity.Property(e => e.IdLabResearch).HasColumnName("id_lab_research");
            entity.Property(e => e.ReferralDateReferral)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("referral_date_referral");

            entity.HasOne(d => d.IdAppointmentNavigation).WithMany(p => p.ResearchReferrals)
                .HasForeignKey(d => d.IdAppointment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("research_referral_id_appointment_fkey");

            entity.HasOne(d => d.IdLabResearchNavigation).WithMany(p => p.ResearchReferrals)
                .HasForeignKey(d => d.IdLabResearch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("research_referral_id_lab_research_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("review_pkey");
            entity.ToTable("review");
            entity.Property(e => e.IdReview).HasColumnName("id_review");
            entity.Property(e => e.CommentReview).HasColumnName("comment_review");
            entity.Property(e => e.CreatedReview)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_review");
            entity.Property(e => e.IdAppointment).HasColumnName("id_appointment");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.IdPatient).HasColumnName("id_patient");
            entity.Property(e => e.RatingReview).HasColumnName("rating_review");

            entity.HasOne(d => d.IdAppointmentNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdAppointment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_id_appointment_fkey");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_id_doctor_fkey");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_id_patient_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("role_pkey");
            entity.ToTable("role");
            entity.HasIndex(e => e.NameRole, "role_name_role_key").IsUnique();
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.NameRole)
                .HasMaxLength(100)
                .HasColumnName("name_role");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("service_pkey");
            entity.ToTable("service");
            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.CostService)
                .HasPrecision(10, 2)
                .HasColumnName("cost_service");
            entity.Property(e => e.DurationMinutesService).HasColumnName("duration_minutes_service");
            entity.Property(e => e.IdServiceType).HasColumnName("id_service_type");
            entity.Property(e => e.NameService)
                .HasMaxLength(200)
                .HasColumnName("name_service");
            entity.Property(e => e.RequirementsService).HasColumnName("requirements_service");

            entity.HasOne(d => d.IdServiceTypeNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdServiceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_id_service_type_fkey");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.IdServiceType).HasName("service_type_pkey");
            entity.ToTable("service_type");
            entity.HasIndex(e => e.NameServiceType, "service_type_name_service_type_key").IsUnique();
            entity.Property(e => e.IdServiceType).HasColumnName("id_service_type");
            entity.Property(e => e.NameServiceType)
                .HasMaxLength(200)
                .HasColumnName("name_service_type");
        });

        modelBuilder.Entity<SpecialistSchedule>(entity =>
        {
            entity.HasKey(e => e.IdSpecialistSchedule).HasName("specialist_schedule_pkey");
            entity.ToTable("specialist_schedule");
            entity.Property(e => e.IdSpecialistSchedule).HasColumnName("id_specialist_schedule");
            entity.Property(e => e.DayOfWeekSchedule).HasColumnName("day_of_week_schedule");
            entity.Property(e => e.EndTimeSchedule).HasColumnName("end_time_schedule");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.StartTimeSchedule).HasColumnName("start_time_schedule");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.SpecialistSchedules)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialist_schedule_id_doctor_fkey");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.IdSpecialization).HasName("specialization_pkey");
            entity.ToTable("specialization");
            entity.HasIndex(e => e.NameSpecialization, "specialization_name_specialization_key").IsUnique();
            entity.Property(e => e.IdSpecialization).HasColumnName("id_specialization");
            entity.Property(e => e.NameSpecialization)
                .HasMaxLength(200)
                .HasColumnName("name_specialization");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_pkey");
            entity.ToTable("user");
            entity.HasIndex(e => e.LoginUser, "user_login_user_key").IsUnique();
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.LoginUser)
                .HasMaxLength(100)
                .HasColumnName("login_user");
            entity.Property(e => e.PasswordUser)
                .HasMaxLength(255)
                .HasColumnName("password_user");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_id_role_fkey");
        });

        // ==========================================================
        // НАСТРОЙКА НАШЕГО МОДУЛЯ ПОСТАВЩИКОВ
        // ==========================================================
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("supplier_pkey");
            entity.ToTable("supplier");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.OrganizationName).HasMaxLength(200).HasColumnName("organization_name");
            entity.Property(e => e.ContactPerson).HasMaxLength(100).HasColumnName("contact_person");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
            entity.Property(e => e.Address).HasMaxLength(300).HasColumnName("address");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}