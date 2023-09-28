-- an insert statement for the PortData_INV Table
insert into PortData_INV (
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
[Inv_I_L2],
[Chg_I_L2],
[Buy_I_L2],
[Sell_I_L2],
[VAC1_in_L2],
[VAC2_in_L2],
[VAC_out_L2],
[AC_Input],
[Batt_V],
[AC_mode],
[INV_mode],
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
@Inv_I_L2,
@Chg_I_L2,
@Buy_I_L2,
@Sell_I_L2,
@VAC1_in_L2,
@VAC2_in_L2,
@VAC_out_L2,
@AC_Input,
@Batt_V,
@AC_mode,
@INV_mode,
@AUX
)