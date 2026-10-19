using Microsoft.EntityFrameworkCore;
namespace EduPr.Data;
public partial class AppDbContext  
{
    public virtual DbSet<Equipment> Equipments { get; set; }
    public virtual DbSet<EquipmentRepair> EquipmentRepairs { get; set; }
}

public partial class Cabinet
{
    public override string ToString() => NumberCabinet;
}
