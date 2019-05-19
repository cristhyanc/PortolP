CREATE TABLE [dbo].[tblCodeVerification](
	[CodeID] [uniqueidentifier] NOT NULL,
	[CodeNumber] [int] NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[CountryCode] [nvarchar](5) NOT NULL,
	[Created] [date] NOT NULL,
 CONSTRAINT [PK_tblCodeVerification] PRIMARY KEY CLUSTERED 
(
	[CodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblCodeVerification] ADD  CONSTRAINT [DF_tblCodeVerification_CodeID]  DEFAULT (newid()) FOR [CodeID]
GO

ALTER TABLE [dbo].[tblCodeVerification] ADD  CONSTRAINT [DF_tblCodeVerification_Created]  DEFAULT (getdate()) FOR [Created]
GO


