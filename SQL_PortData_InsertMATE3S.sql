-- an insert statement for the PortData_MATE3S Table
insert into PortData_MATE3S (
[RecordDate],  -- date on the PC when this record was written
[DeviceAlias],  -- the kimono alias of this 
[KimonoMajorVer],  -- the kimono version number
[KimonoMinorVer],  -- the kimono version number

-- these markers are used to enable the sql query to pick up historical records 
-- in a fast and efficient way
[FiveMinuteMarker],
[TenMinuteMarker],
[FifteenMinuteMarker],
[ThirtyMinuteMarker],
[HourMarker],
[DayMarker],

[OutbackDate],
[Sys_Batt_V],
[Gateway_Type]
)
values
(
@RecordDate,
@DeviceAlias,
@KimonoMajorVer,
@KimonoMinorVer,

@FiveMinuteMarker,
@TenMinuteMarker,
@FifteenMinuteMarker,
@ThirtyMinuteMarker,
@HourMarker,
@DayMarker,

@OutbackDate,
@Sys_Batt_V,
@Gateway_Type
)