CREATE TABLE IF NOT EXISTS equipment (
  id_equipment serial PRIMARY KEY,
  name_equipment varchar(200) NOT NULL,
  inventory_number varchar(50) UNIQUE NOT NULL,
  id_cabinet int REFERENCES cabinet(id_cabinet),
  status_equipment varchar(50) NOT NULL DEFAULT 'Исправно',
  last_service_date date, next_service_date date
);

CREATE TABLE IF NOT EXISTS equipment_repair (
  id_repair serial PRIMARY KEY,
  id_equipment int NOT NULL REFERENCES equipment(id_equipment) ON DELETE CASCADE,
  repair_date date NOT NULL,
  description_repair text
);