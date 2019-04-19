
GO
/****** Object:  Table [dbo].[tblAddress]    Script Date: 19/04/2019 5:53:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAddress](
	[AddressID] [uniqueidentifier] NOT NULL,
	[FlatNumber] [nvarchar](50) NULL,
	[StreetName] [nvarchar](200) NULL,
	[Suburb] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Postcode] [nvarchar](10) NULL,
	[AddressValidated] [bit] NOT NULL,
	[IsCurrentAddress] [bit] NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tblAddress] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBusiness]    Script Date: 19/04/2019 5:53:59 PM ******/
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
/****** Object:  Table [dbo].[tblCodeVerification]    Script Date: 19/04/2019 5:54:00 PM ******/
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
/****** Object:  Table [dbo].[tblCustomer]    Script Date: 19/04/2019 5:54:00 PM ******/
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
/****** Object:  Table [dbo].[tblDelivery]    Script Date: 19/04/2019 5:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDelivery](
	[DeliveryID] [uniqueidentifier] NOT NULL,
	[PickupAddressID] [uniqueidentifier] NOT NULL,
	[DropoffAddressID] [uniqueidentifier] NOT NULL,
	[SenderID] [uniqueidentifier] NOT NULL,
	[ReceiverID] [uniqueidentifier] NOT NULL,
	[DriverID] [uniqueidentifier] NULL,
	[ParcelID] [uniqueidentifier] NOT NULL,
	[DeliveryDate] [date] NOT NULL,
 CONSTRAINT [PK_tblDelivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDriver]    Script Date: 19/04/2019 5:54:00 PM ******/
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
/****** Object:  Table [dbo].[tblGallery]    Script Date: 19/04/2019 5:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblGallery](
	[GalleryID] [uniqueidentifier] NOT NULL,
	[GalleryDate] [date] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_tblGallery] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblGalleryItem]    Script Date: 19/04/2019 5:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblGalleryItem](
	[GalleryItemID] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](250) NOT NULL,
	[Deleted] [bit] NOT NULL,
	[GalleryID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tblGalleryItem] PRIMARY KEY CLUSTERED 
(
	[GalleryItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblParcel]    Script Date: 19/04/2019 5:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblParcel](
	[ParcelID] [uniqueidentifier] NOT NULL,
	[GalleryID] [uniqueidentifier] NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Length] [int] NOT NULL,
	[Weight] [decimal](10, 1) NOT NULL,
	[Worth] [money] NOT NULL,
 CONSTRAINT [PK_tblParcel] PRIMARY KEY CLUSTERED 
(
	[ParcelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblParcelItem]    Script Date: 19/04/2019 5:54:00 PM ******/
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
ALTER TABLE [dbo].[tblAddress] ADD  CONSTRAINT [DF_tblAddress_AddressID]  DEFAULT (newid()) FOR [AddressID]
GO
ALTER TABLE [dbo].[tblAddress] ADD  CONSTRAINT [DF_tblAddress_AddressValidated]  DEFAULT ((0)) FOR [AddressValidated]
GO
ALTER TABLE [dbo].[tblAddress] ADD  CONSTRAINT [DF_tblAddress_IsCurrentAddress]  DEFAULT ((0)) FOR [IsCurrentAddress]
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
ALTER TABLE [dbo].[tblGalleryItem] ADD  CONSTRAINT [DF_tblGalleryItem_GalleryItemID]  DEFAULT (newid()) FOR [GalleryItemID]
GO
ALTER TABLE [dbo].[tblGalleryItem] ADD  CONSTRAINT [DF_tblGalleryItem_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[tblParcel] ADD  CONSTRAINT [DF_tblParcel_ParcelID]  DEFAULT (newid()) FOR [ParcelID]
GO
ALTER TABLE [dbo].[tblParcelItem] ADD  CONSTRAINT [DF_tblParcelItem_ItemID]  DEFAULT (newid()) FOR [ItemID]
GO
ALTER TABLE [dbo].[tblDelivery]  WITH CHECK ADD  CONSTRAINT [FK_tblDelivery_tblDriver] FOREIGN KEY([DriverID])
REFERENCES [dbo].[tblDriver] ([CustomerID])
GO
ALTER TABLE [dbo].[tblDelivery] CHECK CONSTRAINT [FK_tblDelivery_tblDriver]
GO
ALTER TABLE [dbo].[tblDelivery]  WITH CHECK ADD  CONSTRAINT [FK_tblDelivery_tblParcel] FOREIGN KEY([ParcelID])
REFERENCES [dbo].[tblParcel] ([ParcelID])
GO
ALTER TABLE [dbo].[tblDelivery] CHECK CONSTRAINT [FK_tblDelivery_tblParcel]
GO
ALTER TABLE [dbo].[tblGalleryItem]  WITH CHECK ADD  CONSTRAINT [FK_tblGalleryItem_tblGallery] FOREIGN KEY([GalleryID])
REFERENCES [dbo].[tblGallery] ([GalleryID])
GO
ALTER TABLE [dbo].[tblGalleryItem] CHECK CONSTRAINT [FK_tblGalleryItem_tblGallery]
GO
ALTER TABLE [dbo].[tblParcel]  WITH NOCHECK ADD  CONSTRAINT [FK_tblParcel_tblGallery] FOREIGN KEY([GalleryID])
REFERENCES [dbo].[tblGallery] ([GalleryID])
GO
ALTER TABLE [dbo].[tblParcel] CHECK CONSTRAINT [FK_tblParcel_tblGallery]
GO
ALTER TABLE [dbo].[tblParcelItem]  WITH CHECK ADD  CONSTRAINT [FK_tblParcelItem_tblParcel] FOREIGN KEY([ParcelID])
REFERENCES [dbo].[tblParcel] ([ParcelID])
GO
ALTER TABLE [dbo].[tblParcelItem] CHECK CONSTRAINT [FK_tblParcelItem_tblParcel]
GO
