
GO
CREATE TABLE [dbo].[tblVehiculeType](
	[VehiculeTypeID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[StartingFee] [money] NOT NULL,
	[CostPerkilometre] [money] NOT NULL,
	[MaximumDistance] [bigint] NOT NULL,
	[MaximumWeight] [int] NOT NULL,
	[MaximumWidth] [int] NOT NULL,
	[MaximumHeight] [int] NOT NULL,
	[MaximumLength] [int] NOT NULL,
 CONSTRAINT [PK_tblVehiculeType] PRIMARY KEY CLUSTERED 
(
	[VehiculeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVehiculeTypeRange]    Script Date: 17/05/2019 3:57:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVehiculeTypeRange](
	[VehiculeTypeRangeID] [uniqueidentifier] NOT NULL,
	[VehiculeTypeID] [uniqueidentifier] NOT NULL,
	[RangeType] [int] NOT NULL,
	[MinimumValue] [decimal](18, 0) NOT NULL,
	[MaximumValue] [decimal](18, 0) NOT NULL,
	[CostPerExtraUnit] [money] NOT NULL,
	[Unit] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_tblVehiculeTypeRange] PRIMARY KEY CLUSTERED 
(
	[VehiculeTypeRangeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[tblVehiculeType] ADD  CONSTRAINT [DF_tblVehiculeType_VehiculeTypeID]  DEFAULT (newid()) FOR [VehiculeTypeID]
GO
ALTER TABLE [dbo].[tblVehiculeTypeRange] ADD  CONSTRAINT [DF_tblVehiculeTypeRange_VehiculeTypeRangeID]  DEFAULT (newid()) FOR [VehiculeTypeRangeID]

