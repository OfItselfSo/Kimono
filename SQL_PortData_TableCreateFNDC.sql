-- this SQL is run in SQLite to create the PortData_FNDC Table
-- note that it is an EmbeddedResource in the Properties 
-- which allows us to have simple access for run and edit
create table IF NOT EXISTS PortData_FNDC
(
[id] integer PRIMARY KEY AUTOINCREMENT,
[RecordDate] date,  -- date on the PC when this record was written
[DeviceAlias] varchar2(10),  -- the name used internally by Kimono for this record
[KimonoMajorVer] integer,  -- the major version number of Kimono used to create this record
[KimonoMinorVer] integer,  -- the minor version number of Kimono used to create this record

-- these markers are used to enable the sql query to pick up intervals of historical records 
-- in a fast and efficient way

[FiveMinuteMarker] bool, -- if true it indicates is the definitive record for a 5 minute interval
[TenMinuteMarker] bool, -- if true it indicates is the definitive record for a 10 minute interval
[FifteenMinuteMarker] bool, -- if true it indicates is the definitive record for a 15 minute interval
[ThirtyMinuteMarker] bool, -- if true it indicates is the definitive record for a 30 minute interval
[HourMarker] bool, -- if true it indicates is the definitive record for a 60 minute interval
[DayMarker] bool, -- if true it indicates is the definitive record for a day interval

[Port] integer,
[Dev] varchar(30),
-- [Enabled] varchar(30),  not stored
[Shunt_A_I] double,
[Shunt_A_AH] double,
[Shunt_A_kWh] double,
[Shunt_B_I] double,
[Shunt_B_AH] double,
[Shunt_B_kWh] double,
[Shunt_C_I] double,
[Shunt_C_AH] double,
[Shunt_C_kWh] double,
[SOC] double,
[Min_SOC] double,
[Days_since_full] double,
[CHG_parms_met] bool,
[In_AH_today] double,
[Out_AH_today] double,
[In_kWh_today] double,
[Out_kWh_today] double,
[Net_CFC_AH] double,
[Net_CFC_kWh] double,
[Batt_V] double,
[Batt_temp] varchar(30),
[Aux_mode] varchar(30),
[AUX] varchar(30)
)

