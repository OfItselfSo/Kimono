-- an insert statement for the PortData_CC Table
insert into PortData_CC (
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

[Port],
[Dev],
[Type],
[Out_I],
[In_I],
[Batt_V],
[In_V],
[Out_kWh],
[Out_AH],
[CC_mode],
[Aux_mode],
[AUX]
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

@Port,
@Dev,
@Type,
@Out_I,
@In_I,
@Batt_V,
@In_V,
@Out_kWh,
@Out_AH,
@CC_mode,
@Aux_mode,
@AUX
)