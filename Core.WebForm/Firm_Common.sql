USE [master]
GO
/****** Object:  Database [Firm_Common]    Script Date: 12/11/2015 17:08:15 ******/
CREATE DATABASE [Firm_Common] ON  PRIMARY 
( NAME = N'Firm', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\CarSystem.mdf' , SIZE = 28672KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Firm_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\CarSystem_1.ldf' , SIZE = 291904KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Firm_Common] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Firm_Common].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [Firm_Common] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Firm_Common] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Firm_Common] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Firm_Common] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Firm_Common] SET ARITHABORT OFF
GO
ALTER DATABASE [Firm_Common] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Firm_Common] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Firm_Common] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Firm_Common] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Firm_Common] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Firm_Common] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Firm_Common] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Firm_Common] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Firm_Common] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Firm_Common] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Firm_Common] SET  DISABLE_BROKER
GO
ALTER DATABASE [Firm_Common] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Firm_Common] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Firm_Common] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Firm_Common] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Firm_Common] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Firm_Common] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Firm_Common] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Firm_Common] SET  READ_WRITE
GO
ALTER DATABASE [Firm_Common] SET RECOVERY FULL
GO
ALTER DATABASE [Firm_Common] SET  MULTI_USER
GO
ALTER DATABASE [Firm_Common] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Firm_Common] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'Firm_Common', N'ON'
GO
USE [Firm_Common]
GO
/****** Object:  Table [dbo].[D_Admin_Login_Errors]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Admin_Login_Errors](
	[F_A_E_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_A_E_Name] [nvarchar](50) NOT NULL,
	[F_A_E_Pass] [nvarchar](50) NULL,
	[F_A_E_IP] [nvarchar](50) NOT NULL,
	[F_A_E_UserAgent] [nvarchar](150) NULL,
	[F_A_E_Message] [nvarchar](150) NULL,
	[F_A_E_Create] [datetime] NOT NULL,
 CONSTRAINT [PK_D_Admin_Login_Errors] PRIMARY KEY CLUSTERED 
(
	[F_A_E_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[D_Admin_Login_Errors] ON
INSERT [dbo].[D_Admin_Login_Errors] ([F_A_E_Id], [F_A_E_Name], [F_A_E_Pass], [F_A_E_IP], [F_A_E_UserAgent], [F_A_E_Message], [F_A_E_Create]) VALUES (1, N'panxin1213', N'111111', N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36', N'用户名或密码错误', CAST(0x0000A56700DE0A74 AS DateTime))
INSERT [dbo].[D_Admin_Login_Errors] ([F_A_E_Id], [F_A_E_Name], [F_A_E_Pass], [F_A_E_IP], [F_A_E_UserAgent], [F_A_E_Message], [F_A_E_Create]) VALUES (2, N'panxin1213', N'111111', N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36', N'用户名或密码错误', CAST(0x0000A56801187A29 AS DateTime))
INSERT [dbo].[D_Admin_Login_Errors] ([F_A_E_Id], [F_A_E_Name], [F_A_E_Pass], [F_A_E_IP], [F_A_E_UserAgent], [F_A_E_Message], [F_A_E_Create]) VALUES (3, N'panxin1213', N'111111', N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36', N'用户名或密码错误', CAST(0x0000A56900B04D48 AS DateTime))
INSERT [dbo].[D_Admin_Login_Errors] ([F_A_E_Id], [F_A_E_Name], [F_A_E_Pass], [F_A_E_IP], [F_A_E_UserAgent], [F_A_E_Message], [F_A_E_Create]) VALUES (4, N'zhangkui', N'123456', N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36', N'用户名或密码错误', CAST(0x0000A56A00F6B062 AS DateTime))
SET IDENTITY_INSERT [dbo].[D_Admin_Login_Errors] OFF
/****** Object:  Table [dbo].[D_Admin]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Admin](
	[F_Admin_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_Admin_Name] [nvarchar](50) NOT NULL,
	[F_Admin_Nick] [nvarchar](50) NOT NULL,
	[F_Admin_Password] [nvarchar](32) NOT NULL,
	[F_Admin_Time_Last] [datetime] NOT NULL,
	[F_Admin_Time_Create] [datetime] NOT NULL,
	[F_Admin_IsLock] [bit] NOT NULL,
	[F_Admin_IsSupper] [bit] NOT NULL,
 CONSTRAINT [PK_D_Admin] PRIMARY KEY CLUSTERED 
(
	[F_Admin_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [PK_D_Admin_Name] ON [dbo].[D_Admin] 
(
	[F_Admin_Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[D_Admin] ON
INSERT [dbo].[D_Admin] ([F_Admin_Id], [F_Admin_Name], [F_Admin_Nick], [F_Admin_Password], [F_Admin_Time_Last], [F_Admin_Time_Create], [F_Admin_IsLock], [F_Admin_IsSupper]) VALUES (12, N'panxin1213', N'张奎', N'49ba59abbe56e057', CAST(0x0000A56A01130BBF AS DateTime), CAST(0x0000A56700C1B2BC AS DateTime), 0, 1)
INSERT [dbo].[D_Admin] ([F_Admin_Id], [F_Admin_Name], [F_Admin_Nick], [F_Admin_Password], [F_Admin_Time_Last], [F_Admin_Time_Create], [F_Admin_IsLock], [F_Admin_IsSupper]) VALUES (16, N'admin', N'admin', N'49ba59abbe56e057', CAST(0x0000A56701075F40 AS DateTime), CAST(0x0000A5670107540D AS DateTime), 0, 0)
SET IDENTITY_INSERT [dbo].[D_Admin] OFF
/****** Object:  Table [dbo].[D_Ad_View]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Ad_View](
	[F_View_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_View_Title] [nvarchar](50) NOT NULL,
	[F_View_Address] [nvarchar](500) NOT NULL,
	[F_View_Create] [datetime] NULL,
 CONSTRAINT [PK_D_Ad_View] PRIMARY KEY CLUSTERED 
(
	[F_View_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[D_Ad_View] ON
INSERT [dbo].[D_Ad_View] ([F_View_Id], [F_View_Title], [F_View_Address], [F_View_Create]) VALUES (1, N'网站首页', N'http://www.hnhql.com', NULL)
SET IDENTITY_INSERT [dbo].[D_Ad_View] OFF
/****** Object:  Table [dbo].[D_Admin_Role]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Admin_Role](
	[F_Role_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_Role_Name] [nvarchar](50) NOT NULL,
	[F_Role_Description] [nvarchar](200) NULL,
	[F_Role_Time_Create] [datetime] NOT NULL,
	[F_Role_IsLock] [bit] NOT NULL,
 CONSTRAINT [PK_D_Admin_Role] PRIMARY KEY CLUSTERED 
(
	[F_Role_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[D_Admin_Role] ON
INSERT [dbo].[D_Admin_Role] ([F_Role_Id], [F_Role_Name], [F_Role_Description], [F_Role_Time_Create], [F_Role_IsLock]) VALUES (41, N'角色一', N'角色一', CAST(0x0000A3DD015872E8 AS DateTime), 0)
INSERT [dbo].[D_Admin_Role] ([F_Role_Id], [F_Role_Name], [F_Role_Description], [F_Role_Time_Create], [F_Role_IsLock]) VALUES (42, N'管理员管理', N'', CAST(0x0000A5670107253C AS DateTime), 0)
INSERT [dbo].[D_Admin_Role] ([F_Role_Id], [F_Role_Name], [F_Role_Description], [F_Role_Time_Create], [F_Role_IsLock]) VALUES (43, N'友链管理', N'', CAST(0x0000A56701073928 AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[D_Admin_Role] OFF
/****** Object:  Table [dbo].[D_Ad]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Ad](
	[F_Ad_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_Ad_Title] [nvarchar](50) NOT NULL,
	[F_Ad_Type] [int] NOT NULL,
	[F_Ad_Link] [nvarchar](300) NOT NULL,
	[F_Ad_Image] [nvarchar](500) NULL,
	[F_Ad_Desc] [nvarchar](500) NULL,
	[F_Ad_Html] [nvarchar](max) NULL,
	[F_Ad_IsLock] [bit] NOT NULL,
	[F_Ad_Create] [datetime] NOT NULL,
 CONSTRAINT [PK_D_Ad] PRIMARY KEY CLUSTERED 
(
	[F_Ad_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[D_Ad] ON
INSERT [dbo].[D_Ad] ([F_Ad_Id], [F_Ad_Title], [F_Ad_Type], [F_Ad_Link], [F_Ad_Image], [F_Ad_Desc], [F_Ad_Html], [F_Ad_IsLock], [F_Ad_Create]) VALUES (1, N'红利地板', 0, N'http://honglidiban123.co.chinafloor.cn/', N'', N'红利地板', N'', 0, CAST(0x0000A21100E5ACAE AS DateTime))
INSERT [dbo].[D_Ad] ([F_Ad_Id], [F_Ad_Title], [F_Ad_Type], [F_Ad_Link], [F_Ad_Image], [F_Ad_Desc], [F_Ad_Html], [F_Ad_IsLock], [F_Ad_Create]) VALUES (2, N'圣保罗地板2', 2, N'http://bw00001168.co.chinafloor.cn/', N'/up/u/manage/20130805/m_38a3b460.jpg', N'圣保罗地板', N'', 0, CAST(0x0000A21100F0DFDF AS DateTime))
INSERT [dbo].[D_Ad] ([F_Ad_Id], [F_Ad_Title], [F_Ad_Type], [F_Ad_Link], [F_Ad_Image], [F_Ad_Desc], [F_Ad_Html], [F_Ad_IsLock], [F_Ad_Create]) VALUES (3, N'圣保罗地板1', 3, N'http://bykfloor.co.chinafloor.cn/', N'', N'', N'<script type="text/javascript">alert("a");</script>', 0, CAST(0x0000A21100F65748 AS DateTime))
INSERT [dbo].[D_Ad] ([F_Ad_Id], [F_Ad_Title], [F_Ad_Type], [F_Ad_Link], [F_Ad_Image], [F_Ad_Desc], [F_Ad_Html], [F_Ad_IsLock], [F_Ad_Create]) VALUES (4, N'小游戏合集推荐', 3, N'http://baidu.com', N'', N'小游戏合集推荐', N'<a href="#">双塔奇兵</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a>', 0, CAST(0x0000A39600F9108C AS DateTime))
INSERT [dbo].[D_Ad] ([F_Ad_Id], [F_Ad_Title], [F_Ad_Type], [F_Ad_Link], [F_Ad_Image], [F_Ad_Desc], [F_Ad_Html], [F_Ad_IsLock], [F_Ad_Create]) VALUES (5, N'好玩的小游戏推荐', 3, N'http://baidu.com', N'', N'小游戏精选', N'<a href="#">双塔奇兵</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a><a href="#">双塔奇兵电脑版</a>', 0, CAST(0x0000A39600FB48E8 AS DateTime))
SET IDENTITY_INSERT [dbo].[D_Ad] OFF
/****** Object:  Table [dbo].[D_Image_Record]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Image_Record](
	[F_Image_Record_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_Image_Record_FilePath] [nvarchar](500) NOT NULL,
	[F_Image_Record_Num] [int] NOT NULL,
	[F_Image_Record_InRecord] [nvarchar](max) NULL,
	[F_Image_Record_Md5] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_T_Image_Record] PRIMARY KEY CLUSTERED 
(
	[F_Image_Record_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[D_FriendLink]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_FriendLink](
	[F_FriendLink_Id] [int] IDENTITY(1,1) NOT NULL,
	[F_FriendLink_Title] [nvarchar](50) NOT NULL,
	[F_FriendLink_Url] [nvarchar](500) NOT NULL,
	[F_FriendLink_Type] [tinyint] NOT NULL,
	[F_FriendLink_Image] [nvarchar](500) NULL,
	[F_FriendLink_IsLock] [bit] NOT NULL,
	[F_FriendLink_Create] [datetime] NULL,
	[F_FriendLink_Update] [datetime] NULL,
	[F_FriendLink_Order] [int] NOT NULL,
	[F_FriendLink_ByAddress] [nvarchar](200) NULL,
 CONSTRAINT [PK_D_FriendLink] PRIMARY KEY CLUSTERED 
(
	[F_FriendLink_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 60) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[D_FriendLink] ON
INSERT [dbo].[D_FriendLink] ([F_FriendLink_Id], [F_FriendLink_Title], [F_FriendLink_Url], [F_FriendLink_Type], [F_FriendLink_Image], [F_FriendLink_IsLock], [F_FriendLink_Create], [F_FriendLink_Update], [F_FriendLink_Order], [F_FriendLink_ByAddress]) VALUES (22, N'好看的美剧', N'http://www.36mjw.com/', 1, N'', 0, CAST(0x0000A2F901838514 AS DateTime), CAST(0x0000A2F90183DA98 AS DateTime), 2, N'首页')
INSERT [dbo].[D_FriendLink] ([F_FriendLink_Id], [F_FriendLink_Title], [F_FriendLink_Url], [F_FriendLink_Type], [F_FriendLink_Image], [F_FriendLink_IsLock], [F_FriendLink_Create], [F_FriendLink_Update], [F_FriendLink_Order], [F_FriendLink_ByAddress]) VALUES (23, N'中华橱柜网', N'http://www.chinachugui.com', 1, NULL, 0, CAST(0x0000A5670107A3CC AS DateTime), CAST(0x0000A5670107B5D6 AS DateTime), 10000, N'首页')
SET IDENTITY_INSERT [dbo].[D_FriendLink] OFF
/****** Object:  Table [dbo].[D_Admin_To_Role]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Admin_To_Role](
	[F_Admin_Role_AdminId] [int] NOT NULL,
	[F_Admin_Role_RoleId] [int] NOT NULL,
 CONSTRAINT [PK_D_Admin_To_Role] PRIMARY KEY CLUSTERED 
(
	[F_Admin_Role_AdminId] ASC,
	[F_Admin_Role_RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[D_Admin_To_Role] ([F_Admin_Role_AdminId], [F_Admin_Role_RoleId]) VALUES (16, 42)
INSERT [dbo].[D_Admin_To_Role] ([F_Admin_Role_AdminId], [F_Admin_Role_RoleId]) VALUES (16, 43)
/****** Object:  Table [dbo].[D_Admin_Role_Right]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Admin_Role_Right](
	[F_Admin_Right_RoleId] [int] NOT NULL,
	[NameSpace] [nvarchar](50) NOT NULL,
	[ClassName] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (41, N'Firm.Web.manage.Role', N'select')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (41, N'Firm.Web.manage.Role', N'delete')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (41, N'Firm.Web.manage.Role', N'edit')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (41, N'Firm.Web.manage.Role', N'add')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Admin', N'add')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Admin', N'delete')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Admin', N'edit')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Admin', N'loginlogs')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Admin', N'select')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Role', N'add')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Role', N'delete')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Role', N'edit')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (42, N'Firm.Web.manage.Role', N'select')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (43, N'Firm.Web.manage.FriendLink', N'add')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (43, N'Firm.Web.manage.FriendLink', N'changelock')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (43, N'Firm.Web.manage.FriendLink', N'delete')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (43, N'Firm.Web.manage.FriendLink', N'edit')
INSERT [dbo].[D_Admin_Role_Right] ([F_Admin_Right_RoleId], [NameSpace], [ClassName]) VALUES (43, N'Firm.Web.manage.FriendLink', N'select')
/****** Object:  Table [dbo].[D_Admin_Logs]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Admin_Logs](
	[F_A_Log_Admin_Id] [int] NOT NULL,
	[F_A_Log_Create] [datetime] NOT NULL,
	[F_A_Log_IP] [nvarchar](50) NOT NULL,
	[F_A_Log_UserAgent] [nvarchar](150) NULL,
 CONSTRAINT [PK_D_Admin_Logs] PRIMARY KEY CLUSTERED 
(
	[F_A_Log_Admin_Id] ASC,
	[F_A_Log_Create] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700DD6F0C AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700DD7963 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700DDFE4C AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700DE2C76 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700DFD3C9 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700DFD893 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700E01691 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700E0A423 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700E0B9DD AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700E83EDC AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56700EA3457 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56701064ED3 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A5670106FEB4 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56701072587 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A568011887CA AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56801196A29 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56900B05262 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56900B617CE AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56A00F6B3ED AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56A00F6CAE6 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56A00F7BD52 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56A010BA0C5 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56A0112F825 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (12, CAST(0x0000A56A01130BBF AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
INSERT [dbo].[D_Admin_Logs] ([F_A_Log_Admin_Id], [F_A_Log_Create], [F_A_Log_IP], [F_A_Log_UserAgent]) VALUES (16, CAST(0x0000A56701075F40 AS DateTime), N'127.0.0.1', N'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36')
/****** Object:  Table [dbo].[D_Ad_To_View]    Script Date: 12/11/2015 17:08:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[D_Ad_To_View](
	[F_M_To_V_View_ID] [int] NOT NULL,
	[F_M_To_V_AD_ID] [int] NOT NULL,
	[F_M_To_V_EndTime] [datetime] NOT NULL,
	[F_M_To_V_StartTime] [datetime] NOT NULL,
	[F_M_To_V_Order] [int] NOT NULL,
 CONSTRAINT [PK_D_Ad_To_View] PRIMARY KEY CLUSTERED 
(
	[F_M_To_V_View_ID] ASC,
	[F_M_To_V_AD_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[D_Ad_To_View] ([F_M_To_V_View_ID], [F_M_To_V_AD_ID], [F_M_To_V_EndTime], [F_M_To_V_StartTime], [F_M_To_V_Order]) VALUES (1, 1, CAST(0x0000A22300000000 AS DateTime), CAST(0x0000A21A00000000 AS DateTime), 1)
INSERT [dbo].[D_Ad_To_View] ([F_M_To_V_View_ID], [F_M_To_V_AD_ID], [F_M_To_V_EndTime], [F_M_To_V_StartTime], [F_M_To_V_Order]) VALUES (1, 2, CAST(0x0000A22900000000 AS DateTime), CAST(0x0000A21100000000 AS DateTime), 5)
INSERT [dbo].[D_Ad_To_View] ([F_M_To_V_View_ID], [F_M_To_V_AD_ID], [F_M_To_V_EndTime], [F_M_To_V_StartTime], [F_M_To_V_Order]) VALUES (1, 3, CAST(0x0000A22200000000 AS DateTime), CAST(0x0000A21A00000000 AS DateTime), 11)
/****** Object:  Default [DF_D_FriendLink_F_FriendLink_Type]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_FriendLink] ADD  CONSTRAINT [DF_D_FriendLink_F_FriendLink_Type]  DEFAULT ((0)) FOR [F_FriendLink_Type]
GO
/****** Object:  Default [DF_D_FriendLink_F_FriendLink_IsLock]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_FriendLink] ADD  CONSTRAINT [DF_D_FriendLink_F_FriendLink_IsLock]  DEFAULT ((0)) FOR [F_FriendLink_IsLock]
GO
/****** Object:  Default [DF_D_FriendLink_F_FriendLink_Create]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_FriendLink] ADD  CONSTRAINT [DF_D_FriendLink_F_FriendLink_Create]  DEFAULT (getdate()) FOR [F_FriendLink_Create]
GO
/****** Object:  Default [DF_D_FriendLink_F_FriendLink_Update]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_FriendLink] ADD  CONSTRAINT [DF_D_FriendLink_F_FriendLink_Update]  DEFAULT (getdate()) FOR [F_FriendLink_Update]
GO
/****** Object:  Default [DF_D_FriendLink_F_FriendLink_Order]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_FriendLink] ADD  CONSTRAINT [DF_D_FriendLink_F_FriendLink_Order]  DEFAULT ((0)) FOR [F_FriendLink_Order]
GO
/****** Object:  ForeignKey [FK_D_Admin_To_Role_D_Admin]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_Admin_To_Role]  WITH CHECK ADD  CONSTRAINT [FK_D_Admin_To_Role_D_Admin] FOREIGN KEY([F_Admin_Role_AdminId])
REFERENCES [dbo].[D_Admin] ([F_Admin_Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[D_Admin_To_Role] CHECK CONSTRAINT [FK_D_Admin_To_Role_D_Admin]
GO
/****** Object:  ForeignKey [FK_D_Admin_To_Role_D_Admin_Role]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_Admin_To_Role]  WITH CHECK ADD  CONSTRAINT [FK_D_Admin_To_Role_D_Admin_Role] FOREIGN KEY([F_Admin_Role_RoleId])
REFERENCES [dbo].[D_Admin_Role] ([F_Role_Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[D_Admin_To_Role] CHECK CONSTRAINT [FK_D_Admin_To_Role_D_Admin_Role]
GO
/****** Object:  ForeignKey [FK_D_Admin_Role_Right_D_Admin_Role]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_Admin_Role_Right]  WITH CHECK ADD  CONSTRAINT [FK_D_Admin_Role_Right_D_Admin_Role] FOREIGN KEY([F_Admin_Right_RoleId])
REFERENCES [dbo].[D_Admin_Role] ([F_Role_Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[D_Admin_Role_Right] CHECK CONSTRAINT [FK_D_Admin_Role_Right_D_Admin_Role]
GO
/****** Object:  ForeignKey [FK_D_Admin_Logs_D_Admin]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_Admin_Logs]  WITH CHECK ADD  CONSTRAINT [FK_D_Admin_Logs_D_Admin] FOREIGN KEY([F_A_Log_Admin_Id])
REFERENCES [dbo].[D_Admin] ([F_Admin_Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[D_Admin_Logs] CHECK CONSTRAINT [FK_D_Admin_Logs_D_Admin]
GO
/****** Object:  ForeignKey [FK_D_Ad_To_View_D_Ad]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_Ad_To_View]  WITH CHECK ADD  CONSTRAINT [FK_D_Ad_To_View_D_Ad] FOREIGN KEY([F_M_To_V_AD_ID])
REFERENCES [dbo].[D_Ad] ([F_Ad_Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[D_Ad_To_View] CHECK CONSTRAINT [FK_D_Ad_To_View_D_Ad]
GO
/****** Object:  ForeignKey [FK_D_Ad_To_View_D_Ad_View]    Script Date: 12/11/2015 17:08:16 ******/
ALTER TABLE [dbo].[D_Ad_To_View]  WITH CHECK ADD  CONSTRAINT [FK_D_Ad_To_View_D_Ad_View] FOREIGN KEY([F_M_To_V_View_ID])
REFERENCES [dbo].[D_Ad_View] ([F_View_Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[D_Ad_To_View] CHECK CONSTRAINT [FK_D_Ad_To_View_D_Ad_View]
GO
