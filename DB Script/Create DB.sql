USE [master]
GO
/****** Object:  Database [Bitcoin]    Script Date: 9/16/2018 19:49:13 ******/
CREATE DATABASE [Bitcoin]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Bitcoin', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Bitcoin.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Bitcoin_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Bitcoin_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Bitcoin] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Bitcoin].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Bitcoin] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Bitcoin] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Bitcoin] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Bitcoin] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Bitcoin] SET ARITHABORT OFF 
GO
ALTER DATABASE [Bitcoin] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Bitcoin] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Bitcoin] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Bitcoin] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Bitcoin] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Bitcoin] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Bitcoin] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Bitcoin] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Bitcoin] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Bitcoin] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Bitcoin] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Bitcoin] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Bitcoin] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Bitcoin] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Bitcoin] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Bitcoin] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Bitcoin] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Bitcoin] SET RECOVERY FULL 
GO
ALTER DATABASE [Bitcoin] SET  MULTI_USER 
GO
ALTER DATABASE [Bitcoin] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Bitcoin] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Bitcoin] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Bitcoin] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Bitcoin] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Bitcoin', N'ON'
GO
ALTER DATABASE [Bitcoin] SET QUERY_STORE = OFF
GO
USE [Bitcoin]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [Bitcoin]
GO
/****** Object:  Table [dbo].[Block]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Block](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Index] [int] NOT NULL,
	[Hash] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BlockId] [int] NOT NULL,
	[Hash] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VIn]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VIn](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[TransactionHash] [varchar](100) NULL,
	[Address] [varchar](50) NULL,
	[Value] [money] NULL,
 CONSTRAINT [PK_VIn_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VOut]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VOut](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[TransactionHash] [varchar](100) NULL,
	[Address] [varchar](50) NULL,
	[Value] [money] NULL,
 CONSTRAINT [PK_VOut_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Block] FOREIGN KEY([BlockId])
REFERENCES [dbo].[Block] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Block]
GO
ALTER TABLE [dbo].[VIn]  WITH CHECK ADD  CONSTRAINT [FK_VIn_Transaction] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([Id])
GO
ALTER TABLE [dbo].[VIn] CHECK CONSTRAINT [FK_VIn_Transaction]
GO
ALTER TABLE [dbo].[VOut]  WITH CHECK ADD  CONSTRAINT [FK_VOut_Transaction] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([Id])
GO
ALTER TABLE [dbo].[VOut] CHECK CONSTRAINT [FK_VOut_Transaction]
GO
/****** Object:  StoredProcedure [dbo].[GetMaxInTransactionDetails]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetMaxInTransactionDetails]
AS
BEGIN

	SET NOCOUNT ON;

	select top 100 V.Address, B.[Index], T.Hash, V.Value from VIn V 
	INNER JOIN [Transaction] T ON V.TransactionId = T.Id
	INNER JOIN [Block] B ON T.BlockId = B.Id
	WHERE V.Address IS NOT NULL
	GROUP BY V.[Address], B.[Index], T.[Hash], V.[Value]
	ORDER BY V.[Value] DESC

END
GO
/****** Object:  StoredProcedure [dbo].[GetMaxOutTransactionDetails]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetMaxOutTransactionDetails]
AS
BEGIN

	SET NOCOUNT ON;

	select top 100 V.Address, B.[Index], T.Hash, V.Value from VOut V 
	INNER JOIN [Transaction] T ON V.TransactionId = T.Id
	INNER JOIN [Block] B ON T.BlockId = B.Id
	WHERE V.Address IS NOT NULL
	GROUP BY V.[Address], B.[Index], T.[Hash], V.[Value]
	ORDER BY V.[Value] DESC

END
GO
/****** Object:  StoredProcedure [dbo].[GetMaxOverallIncomeAddress]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetMaxOverallIncomeAddress]
AS
BEGIN

	SET NOCOUNT ON;

	select top 100 V.Address, SUM(V.[Value]) from VIn V 
	WHERE V.Address IS NOT NULL
	GROUP BY V.[Address]
	ORDER BY SUM(V.[Value]) DESC


END
GO
/****** Object:  StoredProcedure [dbo].[GetMaxOverallSpentAddress]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetMaxOverallSpentAddress]
AS
BEGIN

	SET NOCOUNT ON;

	select top 100 V.Address, SUM(V.[Value]) from VOut V 
	WHERE V.Address IS NOT NULL
	GROUP BY V.[Address]
	ORDER BY SUM(V.[Value]) DESC


END
GO
/****** Object:  StoredProcedure [dbo].[GetMostUsedInAddresses]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetMostUsedInAddresses]
AS
BEGIN

	SET NOCOUNT ON;

	select top 100 [Address], COUNT([Address]) from dbo.VIn
	WHERE [Address] IS NOT NULL
	GROUP BY [Address]
	ORDER BY COUNT([Address]) DESC

END
GO
/****** Object:  StoredProcedure [dbo].[GetMostUsedOutAddresses]    Script Date: 9/16/2018 19:49:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetMostUsedOutAddresses]
AS
BEGIN

	SET NOCOUNT ON;

	select top 100 [Address], COUNT([Address]) from dbo.VOut
	WHERE [Address] IS NOT NULL
	GROUP BY [Address]
	ORDER BY COUNT([Address]) DESC

END
GO
USE [master]
GO
ALTER DATABASE [Bitcoin] SET  READ_WRITE 
GO
