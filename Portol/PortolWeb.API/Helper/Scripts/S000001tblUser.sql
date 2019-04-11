CREATE TABLE [dbo].[tblUser](
	[UserID] [uniqueidentifier] NOT NULL,
	[BusinessID] [uniqueidentifier] NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](300) NOT NULL,
	[DOB] [date] NOT NULL,
	[PhoneNumber] BIGINT NOT NULL,
	[PhoneCountryCode] [int] NOT NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL,
	[FlatNumber] [nvarchar](50) NULL,
	[StreetName] [nvarchar](200) NULL,
	[Suburb] [nvarchar](50) NULL,	
	[State] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblUser] ADD  CONSTRAINT [DF_tblUser_PhoneCountryCode]  DEFAULT ((61)) FOR [PhoneCountryCode]
GO

ALTER TABLE [dbo].[tblUser] ADD  CONSTRAINT [DF_tblUser_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO