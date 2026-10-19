
using System;

namespace EduPr.Data;

public partial class PatientAppealHistory
{
    public int IdHistory { get; set; }

    public int IdAppeal { get; set; }

    public int? IdUser { get; set; }

    public string ActionHistory { get; set; } = null!;

    public string? DetailsHistory { get; set; }

    public DateTime CreatedHistory { get; set; }

    public virtual PatientAppeal IdAppealNavigation { get; set; } = null!;

    public virtual User? IdUserNavigation { get; set; }
}
