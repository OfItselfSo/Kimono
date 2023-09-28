-- this SQL is run in SQLite to create the PortData_MATE3S Table
-- note that it is an EmbeddedResource in the Properties 
-- which allows us to have simple access for run and edit
create table IF NOT EXISTS PortData_MATE3S 
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

[Gateway_Type] varchar(30),
[OutbackDate] date, 
[Sys_Batt_V] double
)

