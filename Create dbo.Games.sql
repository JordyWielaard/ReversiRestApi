USE [ReversiDbRestApi]
GO

/****** Object: Table [dbo].[Games] Script Date: 3/22/2021 9:58:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Games] (
    [ID]           INT              IDENTITY (1, 1) NOT NULL,
    [Token]        UNIQUEIDENTIFIER NULL,
    [Speler1Token] UNIQUEIDENTIFIER NULL,
    [Speler2Token] UNIQUEIDENTIFIER NULL,
    [Omschrijving] VARCHAR (255)    NULL,
    [AandeBeurt]   INT              NULL,
    [Winnaar]      INT              NULL,
    [Afgelopen]    BIT              NULL
);


