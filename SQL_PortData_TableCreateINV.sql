-- this SQL is run in SQLite to create the PortData_INV Table
-- note that it is an EmbeddedResource in the Properties 
-- which allows us to have simple access for run and edit
create table IF NOT EXISTS PortData_INV_v0104
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

-- properties specific to FXR Dev ports
[Port] integer,
[Dev] varchar(30),
[Type] varchar(30),
[Inv_I_L2] double,
[Chg_I_L2] double,
[Buy_I_L2] double,
[Sell_I_L2] double,
[VAC1_in_L2] double,
[VAC2_in_L2] double,
[VAC_out_L2] double,
[AC_Input] varchar(30),
[Batt_V] double,
[AC_mode] varchar(30),
[INV_mode] varchar(30),
--[Warn] varchar(30),   Not stored
--[Error varchar(30),   Not stored
[AUX] varchar(30),
-- properties specific to Radian INV ports Kimono v01.04
[Inv_I_L1] double,
[Chg_I_L1] double,
[Buy_I_L1] double,
[Sell_I_L1] double,
[VAC1_in_L1] double,
[VAC2_in_L1] double,
[VAC_out_L1] double,
[RELAY] varchar(30)

)

