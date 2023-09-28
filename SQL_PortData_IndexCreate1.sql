﻿-- this creates an index on the RecordData, DeviceAlias columns of the PortData_%TABLE_ID% table
create UNIQUE index IF NOT EXISTS IDXPortData%TABLE_ID% on PortData_%TABLE_ID%([RecordDate], [DeviceAlias])