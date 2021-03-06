USE [master]
GO
/****** Object:  Database [Testing]    Script Date: 12/27/2013 12:57:31 AM ******/
CREATE DATABASE [Testing]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Testing', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Testing.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Testing_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Testing_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Testing] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Testing].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Testing] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Testing] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Testing] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Testing] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Testing] SET ARITHABORT OFF 
GO
ALTER DATABASE [Testing] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Testing] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Testing] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Testing] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Testing] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Testing] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Testing] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Testing] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Testing] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Testing] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Testing] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Testing] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Testing] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Testing] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Testing] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Testing] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Testing] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Testing] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Testing] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Testing] SET  MULTI_USER 
GO
ALTER DATABASE [Testing] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Testing] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Testing] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Testing] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Testing]
GO
/****** Object:  StoredProcedure [dbo].[GetAllDocuments]    Script Date: 12/27/2013 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllDocuments] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Documents;
END

GO
/****** Object:  StoredProcedure [dbo].[InsertExtractionStatus]    Script Date: 12/27/2013 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertExtractionStatus] 
	-- Add the parameters for the stored procedure here
	@DocumentHandle bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into ExtractionStatus (DocumentHandle, Status) values (@DocumentHandle, GETUTCDATE())
END

GO
/****** Object:  StoredProcedure [dbo].[IsExtractionCreated]    Script Date: 12/27/2013 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IsExtractionCreated] 
	@DocumentHandle bigInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if exists ( select 1 from ExtractionStatus where DocumentHandle = @DocumentHandle )
		select IsExtracted = 1
	else
		select IsExtracted = 0
END

GO
/****** Object:  Table [dbo].[Documents]    Script Date: 12/27/2013 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Documents](
	[DocumentHandle] [bigint] NOT NULL,
	[RepositoryType] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[DocumentHandle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExtractionStatus]    Script Date: 12/27/2013 12:57:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtractionStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentHandle] [bigint] NOT NULL,
	[Timestamp] [timestamp] NULL,
	[Status] [datetime] NULL,
 CONSTRAINT [PK_ExtractionStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ExtractionStatus]  WITH CHECK ADD  CONSTRAINT [FK_ExtractionStatus_Documents] FOREIGN KEY([DocumentHandle])
REFERENCES [dbo].[Documents] ([DocumentHandle])
GO
ALTER TABLE [dbo].[ExtractionStatus] CHECK CONSTRAINT [FK_ExtractionStatus_Documents]
GO
ALTER TABLE [dbo].[ExtractionStatus]  WITH CHECK ADD  CONSTRAINT [FK_ExtractionStatus_ExtractionStatus] FOREIGN KEY([Id])
REFERENCES [dbo].[ExtractionStatus] ([Id])
GO
ALTER TABLE [dbo].[ExtractionStatus] CHECK CONSTRAINT [FK_ExtractionStatus_ExtractionStatus]
GO
USE [master]
GO
ALTER DATABASE [Testing] SET  READ_WRITE 
GO
