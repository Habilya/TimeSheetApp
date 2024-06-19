USE [master];
GO

IF DB_ID('TimesheetApp') IS NOT NULL
  set noexec on               -- prevent creation when already exists

CREATE DATABASE [TimesheetApp]
GO
USE [TimesheetApp];
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Inactivable_id] [uniqueidentifier] NOT NULL,
	[SearchableSummary_Title] [nvarchar](max) NULL,
	[SearchableSummary_Details] [nvarchar](max) NULL,
	[SearchableSummary_ExtendedDetails] [nvarchar](max) NULL,
	[SearchableSummary_Search1] [nvarchar](max) NULL,
	[SearchableSummary_Search2] [nvarchar](max) NULL,
	[SearchableSummary_Search3] [nvarchar](max) NULL,
	[SearchableSummary_Search4] [nvarchar](max) NULL,
	[SearchableSummary_Search5] [nvarchar](max) NULL,
	[SearchableSummary_Search6] [nvarchar](max) NULL,
	[SearchableSummary_Search7] [nvarchar](max) NULL,
	[SearchableSummary_SearchSoundex1] [nvarchar](max) NULL,
	[SearchableSummary_SearchSoundex2] [nvarchar](max) NULL,
	[SearchableSummary_Search8] [nvarchar](max) NULL,
	[SearchableSummary_Search9] [nvarchar](max) NULL,
	[SearchableSummary_Search10] [nvarchar](max) NULL,
	[SearchableSummary_Search11] [nvarchar](max) NULL,
	[SearchableSummary_Search12] [nvarchar](max) NULL,
	[SearchableSummary_Search13] [nvarchar](max) NULL,
	[Considerations] [nvarchar](max) NULL,
	[ShortName] [nvarchar](max) NULL,
	[IsPatient] [bit] NOT NULL,
	[LoginId] [uniqueidentifier] NULL,
	[ProfilePictureId] [uniqueidentifier] NULL,
	[FullNameConcatenated]  AS (([SearchableSummary_Search1]+' ')+[SearchableSummary_Search2]) PERSISTED,
	[FullNameConcatenatedReveresed]  AS (([SearchableSummary_Search2]+' ')+[SearchableSummary_Search1]) PERSISTED,
PRIMARY KEY CLUSTERED 
(
	[Inactivable_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndividualMessage](
	[Id] [uniqueidentifier] NOT NULL,
	[Version] [int] NOT NULL,
	[CreationDate] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[LastUpdateDate] [datetime] NULL,
	[LastUpdatedBy] [uniqueidentifier] NULL,
	[DeletionDate] [datetime] NULL,
	[DeletedBy] [uniqueidentifier] NULL,
	[ArchivalDate] [datetime] NULL,
	[ArchivedBy] [uniqueidentifier] NULL,
	[Subject] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[SendDate] [datetime] NOT NULL,
	[IsTask] [bit] NOT NULL,
	[StartDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[IsDraft] [bit] NOT NULL,
	[IsGroupTask] [bit] NOT NULL,
	[DocumentPatientId] [uniqueidentifier] NULL,
	[Filename] [nvarchar](max) NULL,
	[TypeTaskLookupId] [uniqueidentifier] NULL,
	[PriorityLookupId] [uniqueidentifier] NULL,
	[FromContactId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lookup](
	[PrimaryKeyID] [uniqueidentifier] NOT NULL,
	[LookupTypeName] [nvarchar](max) NOT NULL,
	[LookupTypeCode] [nvarchar](max) NOT NULL,
	[LookupCode] [nvarchar](max) NOT NULL,
	[LookupPosition] [int] NULL,
	[LookupID] [uniqueidentifier] NOT NULL,
	[en] [nvarchar](max) NULL,
	[fr] [nvarchar](max) NULL,
	[enValueID] [uniqueidentifier] NULL,
	[frValueID] [uniqueidentifier] NULL,
	[enLocalizedValueID] [uniqueidentifier] NULL,
	[frLocalizedValueID] [uniqueidentifier] NULL,
	[LookupTypeID] [uniqueidentifier] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [uniqueidentifier] NOT NULL,
	[username] [nvarchar](50) NULL,
	[first_name] [nvarchar](50) NULL,
	[last_name] [nvarchar](50) NULL,
	[email] [nvarchar](max) NULL,
	[date_of_birth] [datetime2](7) NOT NULL,
	[date_created] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Users] ([id], [username], [first_name], [last_name], [email], [date_of_birth], [date_created]) VALUES (N'844e71eb-684b-466e-af4d-1a7b86d4f5b6', N'Dick', N'Dick', N'Richard', N'rd@email.com', CAST(N'1998-01-01T00:00:00.0000000' AS DateTime2), CAST(N'1968-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [username], [first_name], [last_name], [email], [date_of_birth], [date_created]) VALUES (N'59efd189-8306-41d0-a3fc-381d6fb0adfe', N'Dick', N'Rocky', N'Balboa', N'rb@email.com', CAST(N'1997-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2016-03-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [username], [first_name], [last_name], [email], [date_of_birth], [date_created]) VALUES (N'5293847f-24b7-47f9-8ca0-596dbe1777d6', N'Dick', N'Johnny', N'Cash', N'jc@email.com', CAST(N'1968-01-01T00:00:00.0000000' AS DateTime2), CAST(N'1989-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [username], [first_name], [last_name], [email], [date_of_birth], [date_created]) VALUES (N'682131e0-bfcd-4d7f-99a8-9408a64ce825', N'Dick', N'Georges', N'Washington', N'gw@email.com', CAST(N'1796-01-01T00:00:00.0000000' AS DateTime2), CAST(N'1796-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [username], [first_name], [last_name], [email], [date_of_birth], [date_created]) VALUES (N'94c74f99-427a-43d1-a822-cf0fe25d6834', N'Dick', N'Arnold', N'Schwarzenegger', N'as@email.com', CAST(N'2016-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2016-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [username], [first_name], [last_name], [email], [date_of_birth], [date_created]) VALUES (N'10caa521-18ef-425a-ba3a-d133651f6c3b', N'Dick', N'Donald', N'Trump', N'dt@email.com', CAST(N'2016-02-01T00:00:00.0000000' AS DateTime2), CAST(N'2016-02-01T00:00:00.0000000' AS DateTime2))
GO