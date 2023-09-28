-- an insert statement for the PortData_USERDATA Table
insert into PortData_USERDATA (
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

[UDN01],
[UDN02],
[UDN03],
[UDN04],
[UDN05],
[UDN06],
[UDN07],
[UDN08],
[UDN09],
[UDN10],
[UDN11],
[UDN12],
[UDN13],
[UDN14],

[UDD01],
[UDD02],
[UDD03],

[UDT01],
[UDT02],
[UDT03]
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

@UDN01,
@UDN02,
@UDN03,
@UDN04,
@UDN05,
@UDN06,
@UDN07,
@UDN08,
@UDN09,
@UDN10,
@UDN11,
@UDN12,
@UDN13,
@UDN14,

@UDD01,
@UDD02,
@UDD03,

@UDT01,
@UDT02,
@UDT03
)