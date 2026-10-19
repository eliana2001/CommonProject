
using System;
using System.Collections.Generic;

namespace EduPr.Data;

public partial class PatientAppeal
{
    public int IdAppeal { get; set; }

    public int IdPatient { get; set; }

    public int? IdResponsibleUser { get; set; }

    public string SubjectAppeal { get; set; } = null!;

    public string DescriptionAppeal { get; set; } = null!;

    public string TypeAppeal { get; set; } = null!;

    public string CategoryAppeal { get; set; } = null!;

    public string StatusAppeal { get; set; } = "Новое";

    public DateTime CreatedAppeal { get; set; }

    public DateTime? DueDateAppeal { get; set; }

    public DateTime UpdatedAppeal { get; set; }

    public virtual Patient IdPatientNavigation { get; set; } = null!;

    public virtual User? IdResponsibleUserNavigation { get; set; }

    public virtual ICollection<PatientAppealHistory> History { get; }
        = new List<PatientAppealHistory>();
}
