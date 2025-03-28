USE [master]
GO
/****** Object:  Database [MarminaAttendance]    Script Date: 3/8/2023 12:46:53 PM ******/
CREATE DATABASE [MarminaAttendance]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MarminaAttendance', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\MarminaAttendance.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MarminaAttendance_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\MarminaAttendance_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MarminaAttendance] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MarminaAttendance].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MarminaAttendance] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MarminaAttendance] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MarminaAttendance] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MarminaAttendance] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MarminaAttendance] SET ARITHABORT OFF 
GO
ALTER DATABASE [MarminaAttendance] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MarminaAttendance] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MarminaAttendance] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MarminaAttendance] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MarminaAttendance] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MarminaAttendance] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MarminaAttendance] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MarminaAttendance] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MarminaAttendance] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MarminaAttendance] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MarminaAttendance] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MarminaAttendance] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MarminaAttendance] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MarminaAttendance] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MarminaAttendance] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MarminaAttendance] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MarminaAttendance] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MarminaAttendance] SET RECOVERY FULL 
GO
ALTER DATABASE [MarminaAttendance] SET  MULTI_USER 
GO
ALTER DATABASE [MarminaAttendance] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MarminaAttendance] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MarminaAttendance] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MarminaAttendance] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MarminaAttendance] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MarminaAttendance] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'MarminaAttendance', N'ON'
GO
ALTER DATABASE [MarminaAttendance] SET QUERY_STORE = ON
GO
ALTER DATABASE [MarminaAttendance] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MarminaAttendance]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[Photo] [nvarchar](80) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](60) NOT NULL,
	[Intercessor] [nvarchar](60) NOT NULL,
	[TimeId] [int] NOT NULL,
 CONSTRAINT [PK_Classes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servants]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Photo] [nvarchar](60) NULL,
	[Phone] [nvarchar](13) NULL,
	[Address] [nvarchar](80) NULL,
 CONSTRAINT [PK_Servants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServantWeek]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServantWeek](
	[ServantId] [int] NOT NULL,
	[WeekId] [int] NOT NULL,
	[Notes] [nvarchar](100) NULL,
 CONSTRAINT [PK_ServantWeek] PRIMARY KEY CLUSTERED 
(
	[ServantId] ASC,
	[WeekId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Time]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Time](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Time] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Time] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Weeks]    Script Date: 3/8/2023 12:46:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Weeks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Date] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Weeks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'389fa307-a1a2-4c3d-8acd-5edd2686c33c', N'120da774-c47f-4049-97bd-2763a0668360', N'Admin', N'ADMIN')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7e5ed5ce-ddd0-43c8-a13f-cef9bbb0a82d', N'389fa307-a1a2-4c3d-8acd-5edd2686c33c')
GO
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [Photo]) VALUES (N'7e5ed5ce-ddd0-43c8-a13f-cef9bbb0a82d', 0, N'd257f8e0-148a-4ed8-8f4b-c97b14e37b70', N'marcosameh678@gmail.com', 0, 1, NULL, N'MARCOSAMEH678@GMAIL.COM', N'MARCO SAMEH', N'AQAAAAIAAYagAAAAEHAEXN7UFOJCuZ+osjP1IU+vm+zr6laRpdZyvcMQ6KkatXILxQkZ4zAAZP/ViMU/Vw==', NULL, 0, N'GLPXVW4L2IV2F3OEGCFNDV2EWLSBB6DW', 0, N'Marco Sameh', N'download.png')
GO
SET IDENTITY_INSERT [dbo].[Classes] ON 

INSERT [dbo].[Classes] ([Id], [Name], [Intercessor], [TimeId]) VALUES (3, N'ملايكة', N'الانبا موسى', 1)
INSERT [dbo].[Classes] ([Id], [Name], [Intercessor], [TimeId]) VALUES (6, N'حضانة', N'مارمينا', 2)
SET IDENTITY_INSERT [dbo].[Classes] OFF
GO
SET IDENTITY_INSERT [dbo].[Servants] ON 

INSERT [dbo].[Servants] ([Id], [ClassId], [Name], [Photo], [Phone], [Address]) VALUES (3, 3, N'فادى ماركو', N'user.jpg', N'01225115538', N'alayai')
INSERT [dbo].[Servants] ([Id], [ClassId], [Name], [Photo], [Phone], [Address]) VALUES (4, 3, N'سامح بقطر', N'user.jpg', N'01255558', N'الكوثر')
INSERT [dbo].[Servants] ([Id], [ClassId], [Name], [Photo], [Phone], [Address]) VALUES (5, 3, N'هانى ميخائيل', N'user.jpg', N'01255558', N'الكوثر')
INSERT [dbo].[Servants] ([Id], [ClassId], [Name], [Photo], [Phone], [Address]) VALUES (6, 3, N'ابرهيم هانى', N'user.jpg', N'010152556', N'الحفر')
INSERT [dbo].[Servants] ([Id], [ClassId], [Name], [Photo], [Phone], [Address]) VALUES (7, 3, N'بولا سمير', N'user.jpg', N'012555633', N'التقوى')
INSERT [dbo].[Servants] ([Id], [ClassId], [Name], [Photo], [Phone], [Address]) VALUES (8, 3, N'ماركو', N'user.jpg', N'01255558', N'الهلال')
SET IDENTITY_INSERT [dbo].[Servants] OFF
GO
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (3, 6, NULL)
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (3, 7, NULL)
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (4, 5, NULL)
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (4, 6, NULL)
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (4, 7, NULL)
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (5, 7, NULL)
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (6, 3, NULL)
INSERT [dbo].[ServantWeek] ([ServantId], [WeekId], [Notes]) VALUES (6, 5, NULL)
GO
SET IDENTITY_INSERT [dbo].[Time] ON 

INSERT [dbo].[Time] ([Id], [Time]) VALUES (1, N'الخميس')
INSERT [dbo].[Time] ([Id], [Time]) VALUES (2, N'الجمعة صباحا')
INSERT [dbo].[Time] ([Id], [Time]) VALUES (3, N'الجمعة مساءً')
INSERT [dbo].[Time] ([Id], [Time]) VALUES (4, N'السبت')
SET IDENTITY_INSERT [dbo].[Time] OFF
GO
SET IDENTITY_INSERT [dbo].[Weeks] ON 

INSERT [dbo].[Weeks] ([Id], [Name], [Date]) VALUES (3, N'الاسبوع الاول من يناير', CAST(N'2023-01-05T00:00:00' AS SmallDateTime))
INSERT [dbo].[Weeks] ([Id], [Name], [Date]) VALUES (5, N'الاسبوع الثانى من يناير', CAST(N'2023-01-12T00:00:00' AS SmallDateTime))
INSERT [dbo].[Weeks] ([Id], [Name], [Date]) VALUES (6, N'الاسبوع الثالت من يناير', CAST(N'2023-01-19T00:00:00' AS SmallDateTime))
INSERT [dbo].[Weeks] ([Id], [Name], [Date]) VALUES (7, N'الاسبوع الرابع من يناير ', CAST(N'2023-01-26T00:00:00' AS SmallDateTime))
SET IDENTITY_INSERT [dbo].[Weeks] OFF
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Classes]  WITH CHECK ADD  CONSTRAINT [FK_Classes_Time] FOREIGN KEY([TimeId])
REFERENCES [dbo].[Time] ([Id])
GO
ALTER TABLE [dbo].[Classes] CHECK CONSTRAINT [FK_Classes_Time]
GO
ALTER TABLE [dbo].[Servants]  WITH CHECK ADD  CONSTRAINT [FK_Servants_Classes] FOREIGN KEY([ClassId])
REFERENCES [dbo].[Classes] ([Id])
GO
ALTER TABLE [dbo].[Servants] CHECK CONSTRAINT [FK_Servants_Classes]
GO
ALTER TABLE [dbo].[ServantWeek]  WITH CHECK ADD  CONSTRAINT [FK_ServantWeek_Servants] FOREIGN KEY([ServantId])
REFERENCES [dbo].[Servants] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServantWeek] CHECK CONSTRAINT [FK_ServantWeek_Servants]
GO
ALTER TABLE [dbo].[ServantWeek]  WITH CHECK ADD  CONSTRAINT [FK_ServantWeek_Weeks] FOREIGN KEY([WeekId])
REFERENCES [dbo].[Weeks] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServantWeek] CHECK CONSTRAINT [FK_ServantWeek_Weeks]
GO
USE [master]
GO
ALTER DATABASE [MarminaAttendance] SET  READ_WRITE 
GO
