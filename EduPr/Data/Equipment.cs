using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EduPr.Data;
[Table("equipment")]
public partial class Equipment
{
    [Key, Column("id_equipment")] public int IdEquipment { get; set; }
    [Column("name_equipment")] public string NameEquipment { get; set; } = null!;
    [Column("inventory_number")] public string InventoryNumber { get; set; } = null!;
    [Column("id_cabinet")] public int? IdCabinet { get; set; }
    [Column("status_equipment")] public string StatusEquipment { get; set; } = "Исправно";
    [Column("last_service_date")] public DateOnly? LastServiceDate { get; set; }
    [Column("next_service_date")] public DateOnly? NextServiceDate { get; set; }
    [ForeignKey(nameof(IdCabinet))] public virtual Cabinet? Cabinet { get; set; }
    public virtual ICollection<EquipmentRepair> Repairs { get; } = new List<EquipmentRepair>();
}
