GO
/****** Object:  Table [dbo].[nhea_MailProvider]    Script Date: 27.05.2019 10:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nhea_MailProvider](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[PackageSize] [int] NOT NULL,
	[StandbyPeriod] [int] NOT NULL,
 CONSTRAINT [PK_MailProvider] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[nhea_MailProvider] ([Id], [Name], [Url], [Description], [PackageSize], [StandbyPeriod]) VALUES (1, N'Gmail', N'gmail.com', N'Google Mail Service', 10, 1)
GO
INSERT [dbo].[nhea_MailProvider] ([Id], [Name], [Url], [Description], [PackageSize], [StandbyPeriod]) VALUES (2, N'Hotmail', N'hotmail.com, msn.com, outlook.com', N'Microsoft Mail Service', 15, 4)
GO
INSERT [dbo].[nhea_MailProvider] ([Id], [Name], [Url], [Description], [PackageSize], [StandbyPeriod]) VALUES (3, N'Yahoo', N'yahoo.com, yahoo.co.uk', N'Yahoo Mail Service', 10, 1)
GO
INSERT [dbo].[nhea_MailProvider] ([Id], [Name], [Url], [Description], [PackageSize], [StandbyPeriod]) VALUES (4, N'Mynet', N'mynet.com', N'Mynet Mail Service', 5, 1)
GO
ALTER TABLE [dbo].[nhea_MailProvider] ADD  CONSTRAINT [DF_MailProvider_PackageSize]  DEFAULT ((1)) FOR [PackageSize]
GO



GO
/****** Object:  Table [dbo].[nhea_MailQueue]    Script Date: 27.05.2019 10:22:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nhea_MailQueue](
	[Id] [uniqueidentifier] NOT NULL,
	[From] [nvarchar](256) NOT NULL,
	[To] [nvarchar](max) NOT NULL,
	[Cc] [nvarchar](max) NOT NULL,
	[Bcc] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[MailProviderId] [int] NULL,
	[Priority] [datetime] NOT NULL,
	[IsReadyToSend] [bit] NOT NULL,
	[HasAttachment] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_nhea_MailQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[nhea_MailQueueAttachment]    Script Date: 27.05.2019 10:22:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nhea_MailQueueAttachment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MailQueueId] [uniqueidentifier] NOT NULL,
	[AttachmentName] [nvarchar](500) NOT NULL,
	[AttachmentData] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_nhea_MailQueueAttachment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[nhea_MailQueueHistory]    Script Date: 27.05.2019 10:22:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nhea_MailQueueHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[From] [nvarchar](256) NOT NULL,
	[To] [nvarchar](max) NOT NULL,
	[Cc] [nvarchar](max) NOT NULL,
	[Bcc] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[MailProviderId] [int] NULL,
	[Priority] [datetime] NOT NULL,
	[HasAttachment] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[HistoryCreateDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_nhea_MailQueueHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[nhea_MailQueue] ADD  CONSTRAINT [DF_nhea_MailQueue_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[nhea_MailQueue] ADD  CONSTRAINT [DF_MailQueue_Priority]  DEFAULT (getdate()) FOR [Priority]
GO
ALTER TABLE [dbo].[nhea_MailQueue] ADD  CONSTRAINT [DF_nhea_MailQueue_IsReadyToSent]  DEFAULT ((0)) FOR [IsReadyToSend]
GO
ALTER TABLE [dbo].[nhea_MailQueue] ADD  CONSTRAINT [DF_nhea_MailQueue_HasAttachment]  DEFAULT ((0)) FOR [HasAttachment]
GO
ALTER TABLE [dbo].[nhea_MailQueue] ADD  CONSTRAINT [DF_nhea_MailQueue_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[nhea_MailQueueHistory] ADD  CONSTRAINT [DF_MailQueueHistory_Priority]  DEFAULT (getdate()) FOR [Priority]
GO
ALTER TABLE [dbo].[nhea_MailQueueHistory] ADD  CONSTRAINT [DF_nhea_MailQueueHistory_HasAttachment]  DEFAULT ((0)) FOR [HasAttachment]
GO
ALTER TABLE [dbo].[nhea_MailQueueHistory] ADD  CONSTRAINT [DF_nhea_MailQueueHistory_HistoryCreateDate]  DEFAULT (getdate()) FOR [HistoryCreateDate]
GO
ALTER TABLE [dbo].[nhea_MailQueue]  WITH CHECK ADD  CONSTRAINT [FK_MailQueue_MailProvider] FOREIGN KEY([MailProviderId])
REFERENCES [dbo].[nhea_MailProvider] ([Id])
GO
ALTER TABLE [dbo].[nhea_MailQueue] CHECK CONSTRAINT [FK_MailQueue_MailProvider]
GO
ALTER TABLE [dbo].[nhea_MailQueueHistory]  WITH CHECK ADD  CONSTRAINT [FK_MailQueueHistory_MailProvider] FOREIGN KEY([MailProviderId])
REFERENCES [dbo].[nhea_MailProvider] ([Id])
GO
ALTER TABLE [dbo].[nhea_MailQueueHistory] CHECK CONSTRAINT [FK_MailQueueHistory_MailProvider]
GO
