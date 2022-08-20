USE [Testing]
GO
/****** Object:  Table [dbo].[FileUpload_Client]    Script Date: 21-08-2022 00:13:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_Client](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [varchar](20) NOT NULL,
	[ClientName] [varchar](20) NOT NULL,
	[IsActive] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileUpload_DataStaging]    Script Date: 21-08-2022 00:13:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_DataStaging](
	[RecId] [int] IDENTITY(1,1) NOT NULL,
	[Fld_01] [varchar](600) NULL,
	[Fld_02] [varchar](600) NULL,
	[Fld_03] [varchar](600) NULL,
	[Fld_04] [varchar](600) NULL,
	[Fld_05] [varchar](600) NULL,
	[Fld_06] [varchar](600) NULL,
	[Fld_07] [varchar](600) NULL,
	[Fld_08] [varchar](600) NULL,
	[Fld_09] [varchar](600) NULL,
	[Fld_10] [varchar](600) NULL,
	[Fld_11] [varchar](600) NULL,
	[Fld_12] [varchar](600) NULL,
	[Fld_13] [varchar](600) NULL,
	[Fld_14] [varchar](600) NULL,
	[Fld_15] [varchar](600) NULL,
	[Fld_16] [varchar](600) NULL,
	[Fld_17] [varchar](600) NULL,
	[Fld_18] [varchar](600) NULL,
	[Fld_19] [varchar](600) NULL,
	[Fld_20] [varchar](600) NULL,
	[Fld_21] [varchar](600) NULL,
	[Fld_22] [varchar](600) NULL,
	[Fld_23] [varchar](600) NULL,
	[Fld_24] [varchar](600) NULL,
	[Fld_25] [varchar](600) NULL,
	[Fld_26] [varchar](600) NULL,
	[Fld_27] [varchar](600) NULL,
	[Fld_28] [varchar](600) NULL,
	[Fld_29] [varchar](600) NULL,
	[Fld_30] [varchar](600) NULL,
	[File_Id] [int] NULL,
	[DateTimeStamp] [datetime] NOT NULL,
	[ErrorDesc] [varchar](5000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileUpload_FieldMapping]    Script Date: 21-08-2022 00:13:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_FieldMapping](
	[Field_Id] [int] IDENTITY(1,1) NOT NULL,
	[Field_Name] [varchar](100) NULL,
	[Field_Mapped_Name] [varchar](100) NULL,
	[Field_Order] [int] NOT NULL,
	[Field_Datatype] [varchar](30) NULL,
	[File_Id] [int] NOT NULL,
	[IsActive] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileUpload_FileClientMapping]    Script Date: 21-08-2022 00:13:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_FileClientMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](20) NOT NULL,
	[ClientId] [int] NOT NULL,
	[IsActive] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileUpload_UploadFileDetails]    Script Date: 21-08-2022 00:13:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload_UploadFileDetails](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[Upload_FileName] [varchar](200) NOT NULL,
	[Upload_DateTime] [datetime] NOT NULL,
	[FileClientId] [int] NOT NULL,
	[ErrorDesc] [varchar](5000) NULL,
	[Valid] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FileUpload_Client] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FileUpload_DataStaging] ADD  DEFAULT (getdate()) FOR [DateTimeStamp]
GO
ALTER TABLE [dbo].[FileUpload_FieldMapping] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FileUpload_FileClientMapping] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FileUpload_UploadFileDetails] ADD  DEFAULT (getdate()) FOR [Upload_DateTime]
GO
ALTER TABLE [dbo].[FileUpload_UploadFileDetails] ADD  DEFAULT ((0)) FOR [Valid]
GO
/****** Object:  StoredProcedure [dbo].[GetFileColumn]    Script Date: 21-08-2022 00:13:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFileColumn] 
@FileCategory varchar(25),
@FileName varchar(25),
@ClientId varchar(25),
@FileUploadId INT OUTPUT
AS
BEGIN
DECLARE @FILE_CLIENT_ID INT=0
SET @FILE_CLIENT_ID=(SELECT Id FROM FileUpload_FileClientMapping WHERE ClientId=@ClientId AND FileName=@FileCategory)
INSERT INTO FileUpload_UploadFileDetails(Upload_FileName,FileClientId) VALUES(@FileName,@FILE_CLIENT_ID)  
 SET @FileUploadId=SCOPE_IDENTITY()
 SELECT [Field_Name],[Field_Mapped_Name] from dbo.FileUpload_FieldMapping where [File_Id]=@FILE_CLIENT_ID
END
GO
