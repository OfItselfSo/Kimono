-- an insert statement for the PortData_FNDC Table
insert into PortData_FNDC (
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
[Shunt_A_I],
[Shunt_A_AH],
[Shunt_A_kWh],
[Shunt_B_I],
[Shunt_B_AH],
[Shunt_B_kWh],
[Shunt_C_I],
[Shunt_C_AH],
[Shunt_C_kWh],
[SOC],
[Min_SOC],
[Days_since_full],
[CHG_parms_met],
[In_AH_today],
[Out_AH_today],
[In_kWh_today],
[Out_kWh_today],
[Net_CFC_AH],
[Net_CFC_kWh],
[Batt_temp],
[Batt_V],
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
@Shunt_A_I,
@Shunt_A_AH,
@Shunt_A_kWh,
@Shunt_B_I,
@Shunt_B_AH,
@Shunt_B_kWh,
@Shunt_C_I,
@Shunt_C_AH,
@Shunt_C_kWh,
@SOC,
@Min_SOC,
@Days_since_full,
@CHG_parms_met,
@In_AH_today,
@Out_AH_today,
@In_kWh_today,
@Out_kWh_today,
@Net_CFC_AH,
@Net_CFC_kWh,
@Batt_temp,
@Batt_V,
@Aux_mode,
@AUX
)