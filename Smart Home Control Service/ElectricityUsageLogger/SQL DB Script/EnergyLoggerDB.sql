USE [master]
GO
/****** Object:  Database [EnergyLogging]    Script Date: 11/04/2016 23:15:19 ******/
CREATE DATABASE [EnergyLogging]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EnergyLogging', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\EnergyLogging.mdf' , SIZE = 7168KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'EnergyLogging_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\EnergyLogging_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [EnergyLogging] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EnergyLogging].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EnergyLogging] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EnergyLogging] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EnergyLogging] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EnergyLogging] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EnergyLogging] SET ARITHABORT OFF 
GO
ALTER DATABASE [EnergyLogging] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EnergyLogging] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EnergyLogging] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EnergyLogging] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EnergyLogging] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EnergyLogging] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EnergyLogging] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EnergyLogging] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EnergyLogging] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EnergyLogging] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EnergyLogging] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EnergyLogging] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EnergyLogging] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EnergyLogging] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EnergyLogging] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EnergyLogging] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EnergyLogging] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EnergyLogging] SET RECOVERY FULL 
GO
ALTER DATABASE [EnergyLogging] SET  MULTI_USER 
GO
ALTER DATABASE [EnergyLogging] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EnergyLogging] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EnergyLogging] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EnergyLogging] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [EnergyLogging] SET DELAYED_DURABILITY = DISABLED 
GO
USE [EnergyLogging]
GO
/****** Object:  User [EnergyLogger]    Script Date: 11/04/2016 23:15:19 ******/
CREATE USER [EnergyLogger] FOR LOGIN [EnergyLogger] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[ElectricMeterLog]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ElectricMeterLog](
	[DateTime] [datetime] NOT NULL,
	[CurrentReading] [int] NOT NULL,
	[DailyTotal] [int] NOT NULL,
 CONSTRAINT [PK_ElectricMeterLog] PRIMARY KEY CLUSTERED 
(
	[DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HeatmiserLog]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HeatmiserLog](
	[DateTime] [datetime] NOT NULL,
	[RateOfChange] [tinyint] NOT NULL,
	[SetRoomTemp] [tinyint] NOT NULL,
	[RunMode] [tinyint] NOT NULL,
	[CurrentAirTemp] [tinyint] NOT NULL,
	[IsHeating] [bit] NOT NULL,
	[IsHotWater] [bit] NOT NULL,
 CONSTRAINT [PK_HeatmiserLog] PRIMARY KEY CLUSTERED 
(
	[DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Holidays]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holidays](
	[HolidayID] [int] IDENTITY(1,1) NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[IsActioned] [bit] NOT NULL,
 CONSTRAINT [PK_Holidays1] PRIMARY KEY CLUSTERED 
(
	[HolidayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WeatherLog]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WeatherLog](
	[DateTime] [datetime] NOT NULL,
	[WindDirection] [varchar](3) NOT NULL,
	[ScreenRelativeHumidity] [decimal](4, 1) NOT NULL,
	[Pressure] [smallint] NOT NULL,
	[WindSpeed] [smallint] NOT NULL,
	[Temperature] [decimal](3, 1) NOT NULL,
	[WeatherType] [smallint] NOT NULL,
	[PressureTendency] [varchar](1) NOT NULL,
	[DewPoint] [decimal](3, 1) NOT NULL,
	[WindGust] [decimal](4, 1) NOT NULL,
 CONSTRAINT [PK_WeatherLog] PRIMARY KEY CLUSTERED 
(
	[DateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteHoliday]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteHoliday]
	-- Add the parameters for the stored procedure here
	@HolidayID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE Holidays WHERE HolidayID = @HolidayID
END

GO
/****** Object:  StoredProcedure [dbo].[GetHeatmiserReadingBasedOnDate]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHeatmiserReadingBasedOnDate]
	-- Add the parameters for the stored procedure here
	@date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [DateTime], CurrentAirTemp, IsHeating
	FROM dbo.HeatmiserLog
	WHERE [DateTime] >= 
	(SELECT TOP 1 [DateTime] FROM dbo.HeatmiserLog WHERE [DateTime] <= @date)
	ORDER BY [DateTime] ASC

END

GO
/****** Object:  StoredProcedure [dbo].[GetPlannedHolidays]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPlannedHolidays]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT HolidayID, FromDate, ToDate, IsActioned FROM dbo.Holidays
	ORDER BY FromDate, ToDate
END

GO
/****** Object:  StoredProcedure [dbo].[PostHeatmiserLog]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PostHeatmiserLog]
	-- Add the parameters for the stored procedure here
	@DateTime datetime, @RateOfChange tinyint, @SetRoomTemp tinyint, @RunMode tinyint, 
	@CurrentAirTemp tinyint, @IsHeating bit, @IsHotWater bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT dbo.HeatmiserLog VALUES 
	(@DateTime, @RateOfChange, @SetRoomTemp, @RunMode, @CurrentAirTemp, @IsHeating, @IsHotWater)
END

GO
/****** Object:  StoredProcedure [dbo].[PostMeterReadings]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PostMeterReadings]
	-- Add the parameters for the stored procedure here
	@DateTime datetime, @CurrentReading int, @DailyTotal int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [DateTime] FROM dbo.ElectricMeterLog WHERE [DateTime] = @DateTime

	IF @@ROWCOUNT = 0
	INSERT dbo.ElectricMeterLog VALUES (@DateTime, @CurrentReading, @DailyTotal)
END

GO
/****** Object:  StoredProcedure [dbo].[PostWeatherLog]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PostWeatherLog]
	-- Add the parameters for the stored procedure here
	@DateTime datetime, @WindDirection varchar(3), @ScreenRelativeHumidity decimal(4,1),
	@Pressure smallint, @WindSpeed smallint, @Temperature decimal(3,1), @WeatherType smallint,
	@PressureTendency varchar(1), @DewPoint decimal(3,1), @WindGust decimal(4,1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE dbo.WeatherLog
	SET WindDirection = @WindDirection, ScreenRelativeHumidity = @ScreenRelativeHumidity,
	Pressure = @Pressure, WindSpeed = @WindSpeed, Temperature = @Temperature, WeatherType = @WeatherType,
	PressureTendency = @PressureTendency, DewPoint = @DewPoint, WindGust = @WindGust
	WHERE [DateTime] = @DateTime


    -- Insert statements for procedure here
	IF @@ROWCOUNT = 0
	INSERT dbo.WeatherLog VALUES
	(@DateTime, @WindDirection, @ScreenRelativeHumidity,@Pressure, @WindSpeed, @Temperature, 
	@WeatherType, @PressureTendency, @DewPoint, @WindGust)
END

GO
/****** Object:  StoredProcedure [dbo].[SetHolidayAsActioned]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SetHolidayAsActioned]
	-- Add the parameters for the stored procedure here
	@HolidayID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.Holidays SET IsActioned = 1 WHERE HolidayID = @HolidayID
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateHoliday]    Script Date: 11/04/2016 23:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateHoliday]
	-- Add the parameters for the stored procedure here
	@HolidayID int, @FromDate datetime, @ToDate datetime, @IsActioned bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@HolidayID = 0) 
	INSERT dbo.Holidays VALUES (@FromDate, @ToDate, @IsActioned)
	ELSE
	UPDATE dbo.Holidays 
	SET FromDate = @FromDate, ToDate = @ToDate, IsActioned = @IsActioned
	WHERE HolidayID = @HolidayID	
END

GO
USE [master]
GO
ALTER DATABASE [EnergyLogging] SET  READ_WRITE 
GO
