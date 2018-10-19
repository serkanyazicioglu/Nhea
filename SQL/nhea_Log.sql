GO

/****** Object:  Table [dbo].[nhea_Log]    Script Date: 19.10.2018 22:06:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[nhea_Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Source] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[Level] [nvarchar](50) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Exception] [varchar](max) NULL,
	[ExceptionFile] [nvarchar](max) NULL,
	[ExceptionData] [nvarchar](max) NULL,
 CONSTRAINT [PK_nhea_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[nhea_Log] ADD  CONSTRAINT [DF_nhea_Log_Date]  DEFAULT (getdate()) FOR [Date]
GO