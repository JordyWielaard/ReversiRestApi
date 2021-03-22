USE [ReversiDbRestApi]
GO

/****** Object: Table [dbo].[Cell] Script Date: 3/22/2021 9:57:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cell] (
    [Token] UNIQUEIDENTIFIER NOT NULL,
    [Row]   INT              NOT NULL,
    [Col]   INT              NOT NULL,
    [Kleur] INT              NOT NULL
);


