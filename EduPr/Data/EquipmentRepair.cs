using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EduPr.Data;
[Table("equipment_repair")]
public partial class EquipmentRepair
{
    [Key, Column("id_repair")] public int IdRepair { get; set; }
    [Column("id_equipment")] public int IdEquipment { get; set; }
    [Column("repair_date")] public DateOnly RepairDate { get; set; }
    [Column("description_repair")] public string? DescriptionRepair { get; set; }
    public virtual Equipment? Equipment { get; set; }
}
