
GO
/****** Object:  Table [dbo].[tblAddress]    Script Date: 19/05/2019 5:57:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAddress](
	[AddressID] [uniqueidentifier] NOT NULL,
	[ParentID] [uniqueidentifier] NOT NULL,
	[AddressValidated] [bit] NOT NULL,
	[FullAddress] [nvarchar](500) NOT NULL,
	[Latitude] [nvarchar](15) NULL,
	[Longitude] [nvarchar](15) NULL,
	[IsCurrentAddress] [bit] NOT NULL,
	[ParentAddressType] [int] NOT NULL,
	[IsStarterPoint] [bit] NOT NULL,
 CONSTRAINT [PK_tblAddress] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBusiness]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusiness](
	[BusinessID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tblBusiness] PRIMARY KEY CLUSTERED 
(
	[BusinessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCodeVerification]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCodeVerification](
	[CodeID] [uniqueidentifier] NOT NULL,
	[CodeNumber] [int] NOT NULL,
	[PhoneNumber] [bigint] NOT NULL,
	[CountryCode] [int] NOT NULL,
	[Created] [date] NOT NULL,
 CONSTRAINT [PK_tblCodeVerification] PRIMARY KEY CLUSTERED 
(
	[CodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCustomer]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCustomer](
	[CustomerID] [uniqueidentifier] NOT NULL,
	[BusinessID] [uniqueidentifier] NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](300) NOT NULL,
	[DOB] [date] NOT NULL,
	[PhoneNumber] [bigint] NOT NULL,
	[PhoneCountryCode] [int] NOT NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL,
	[Deleted] [bit] NOT NULL,
	[IsGuess] [bit] NOT NULL,
 CONSTRAINT [PK_tblCustomer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDelivery]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDelivery](
	[DeliveryID] [uniqueidentifier] NOT NULL,
	[CustomerSenderID] [uniqueidentifier] NOT NULL,
	[CustomerReceiverID] [uniqueidentifier] NOT NULL,
	[PaymentMethodID] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[TravelDistance] [decimal](18, 3) NOT NULL,
	[EstimatedCost] [decimal](18, 2) NOT NULL,
	[TotalCost] [decimal](18, 2) NOT NULL,
	[DeliveryStatus] [int] NOT NULL,
 CONSTRAINT [PK_tblDelivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDriver]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDriver](
	[DirverLicenceNumber] [nvarchar](50) NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tblDriver] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblGallery]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblGallery](
	[GalleryID] [uniqueidentifier] NOT NULL,
	[ParentID] [uniqueidentifier] NOT NULL,
	[GalleryDate] [date] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_tblGallery] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblLogErrors]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLogErrors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblLogErrors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblParcel]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblParcel](
	[ParcelID] [uniqueidentifier] NOT NULL,
	[ParentID] [uniqueidentifier] NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Length] [int] NOT NULL,
	[Weight] [int] NOT NULL,
	[Worth] [money] NOT NULL,
	[ParentType] [int] NOT NULL,
 CONSTRAINT [PK_tblParcel] PRIMARY KEY CLUSTERED 
(
	[ParcelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblParcelItem]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblParcelItem](
	[ItemID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ParcelID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tblParcelItem] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPaymentMethod]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPaymentMethod](
	[PaymentMethodID] [uniqueidentifier] NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[CardNumber] [nvarchar](30) NOT NULL,
	[CreditCardType] [int] NOT NULL,
	[CurrentCard] [bit] NOT NULL,
 CONSTRAINT [PK_tblPaymentMethod] PRIMARY KEY CLUSTERED 
(
	[PaymentMethodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPicture]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPicture](
	[PictureID] [uniqueidentifier] NOT NULL,
	[ParentID] [uniqueidentifier] NOT NULL,
	[ImageUrl] [nvarchar](500) NOT NULL,
	[ParentType] [int] NOT NULL,
 CONSTRAINT [PK_tblPicture] PRIMARY KEY CLUSTERED 
(
	[PictureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblScript]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblScript](
	[ScriptID] [uniqueidentifier] NOT NULL,
	[ScriptName] [nvarchar](100) NOT NULL,
	[Seq] [int] NOT NULL,
	[ScriptDate] [date] NOT NULL,
 CONSTRAINT [PK_tblScripts] PRIMARY KEY CLUSTERED 
(
	[ScriptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUser](
	[UserID] [uniqueidentifier] NOT NULL,
	[BusinessID] [uniqueidentifier] NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](300) NOT NULL,
	[DOB] [date] NOT NULL,
	[PhoneNumber] [int] NOT NULL,
	[PhoneCountryCode] [int] NOT NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL,
	[FlatNumber] [nvarchar](50) NULL,
	[StreetName] [nvarchar](200) NULL,
	[Suburb] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVehicule]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVehicule](
	[VehiculeID] [uniqueidentifier] NOT NULL,
	[VehiculeTypeID] [uniqueidentifier] NOT NULL,
	[DriverID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tblVehicule] PRIMARY KEY CLUSTERED 
(
	[VehiculeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVehiculeType]    Script Date: 19/05/2019 5:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[tblVehiculeTypeRange]    Script Date: 19/05/2019 5:57:14 PM ******/
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
GO
ALTER TABLE [dbo].[tblAddress] ADD  CONSTRAINT [DF_tblAddress_AddressID]  DEFAULT (newid()) FOR [AddressID]
GO
ALTER TABLE [dbo].[tblAddress] ADD  CONSTRAINT [DF_tblAddress_AddressValidated]  DEFAULT ((0)) FOR [AddressValidated]
GO
ALTER TABLE [dbo].[tblAddress] ADD  CONSTRAINT [DF_tblAddress_IsCurrentAddress]  DEFAULT ((0)) FOR [IsCurrentAddress]
GO
ALTER TABLE [dbo].[tblAddress] ADD  CONSTRAINT [DF_tblAddress_IsStarterPoint]  DEFAULT ((0)) FOR [IsStarterPoint]
GO
ALTER TABLE [dbo].[tblBusiness] ADD  CONSTRAINT [DF_tblBusiness_BusinessID]  DEFAULT (newid()) FOR [BusinessID]
GO
ALTER TABLE [dbo].[tblCodeVerification] ADD  CONSTRAINT [DF_tblCodeVerification_CodeID]  DEFAULT (newid()) FOR [CodeID]
GO
ALTER TABLE [dbo].[tblCodeVerification] ADD  CONSTRAINT [DF_tblCodeVerification_Created]  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[tblCustomer] ADD  CONSTRAINT [DF_tblCustomer_CustomerID]  DEFAULT (newid()) FOR [CustomerID]
GO
ALTER TABLE [dbo].[tblCustomer] ADD  CONSTRAINT [DF_tblCustomer_Email]  DEFAULT (N'guess@email.com') FOR [Email]
GO
ALTER TABLE [dbo].[tblCustomer] ADD  CONSTRAINT [DF_tblCustomer_DOB]  DEFAULT ('01/01/2000') FOR [DOB]
GO
ALTER TABLE [dbo].[tblCustomer] ADD  CONSTRAINT [DF_tblCustomer_PhoneCountryCode]  DEFAULT ((61)) FOR [PhoneCountryCode]
GO
ALTER TABLE [dbo].[tblCustomer] ADD  CONSTRAINT [DF_tblCustomer_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[tblCustomer] ADD  CONSTRAINT [DF_tblCustomer_IsGuess]  DEFAULT ((0)) FOR [IsGuess]
GO
ALTER TABLE [dbo].[tblDelivery] ADD  CONSTRAINT [DF_tblDelivery_DeliveryID]  DEFAULT (newid()) FOR [DeliveryID]
GO
ALTER TABLE [dbo].[tblGallery] ADD  CONSTRAINT [DF_tblGallery_GalleryID]  DEFAULT (newid()) FOR [GalleryID]
GO
ALTER TABLE [dbo].[tblGallery] ADD  CONSTRAINT [DF_tblGallery_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[tblParcel] ADD  CONSTRAINT [DF_tblParcel_ParcelID]  DEFAULT (newid()) FOR [ParcelID]
GO
ALTER TABLE [dbo].[tblParcelItem] ADD  CONSTRAINT [DF_tblParcelItem_ItemID]  DEFAULT (newid()) FOR [ItemID]
GO
ALTER TABLE [dbo].[tblPaymentMethod] ADD  CONSTRAINT [DF_tblPaymentMethod_PaymentMethodID]  DEFAULT (newid()) FOR [PaymentMethodID]
GO
ALTER TABLE [dbo].[tblPaymentMethod] ADD  CONSTRAINT [DF_tblPaymentMethod_CurrentCard]  DEFAULT ((0)) FOR [CurrentCard]
GO
ALTER TABLE [dbo].[tblUser] ADD  CONSTRAINT [DF_tblUser_PhoneCountryCode]  DEFAULT ((61)) FOR [PhoneCountryCode]
GO
ALTER TABLE [dbo].[tblVehicule] ADD  CONSTRAINT [DF_tblVehicule_VehiculeID]  DEFAULT (newid()) FOR [VehiculeID]
GO
ALTER TABLE [dbo].[tblVehiculeType] ADD  CONSTRAINT [DF_tblVehiculeType_VehiculeTypeID]  DEFAULT (newid()) FOR [VehiculeTypeID]
GO
ALTER TABLE [dbo].[tblVehiculeTypeRange] ADD  CONSTRAINT [DF_tblVehiculeTypeRange_VehiculeTypeRangeID]  DEFAULT (newid()) FOR [VehiculeTypeRangeID]
GO
