EpiHotel, the most bugged hotel in the world.

Queries per SQL Server Management Studio

In fondo c'è anche una query per popolare la tabella Dipendenti, è necessario perchè l'app starta sulla pagina di login. C'èanche una stored procedure usata per mostrare il dettaglio della prenotazione.

CREATE DATABASE EpiHotel;

USE [EpiHotel]
GO

-----TABELLA CAMERE-----

/****** Object:  Table [dbo].[Camere]    Script Date: 09/03/2024 13:10:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Camere](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumeroCamera] [int] NULL,
	[Descrizione] [nvarchar](255) NULL,
	[Tipologia] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [EpiHotel]
GO

----TABELLA CLIENTI----

/****** Object:  Table [dbo].[Clienti]    Script Date: 09/03/2024 14:57:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Clienti](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](100) NULL,
	[Cognome] [nvarchar](100) NULL,
	[Citta] [nvarchar](100) NULL,
	[Provincia] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NULL,
	[CodiceFiscale] [nvarchar](16) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [EpiHotel]
GO


----TABELLA DIPENDENTI----

/****** Object:  Table [dbo].[Dipendenti]    Script Date: 09/03/2024 14:58:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Dipendenti](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](100) NULL,
	[Cognome] [nvarchar](100) NULL,
	[DataNascita] [date] NULL,
	[LuogoNascita] [nvarchar](100) NULL,
	[Indirizzo] [nvarchar](255) NULL,
	[CodiceFiscale] [nvarchar](16) NULL,
	[Matricola] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[PasswordDipendente] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [EpiHotel]
GO

----TABELLA PRENOTAZIONI----

/****** Object:  Table [dbo].[Prenotazioni]    Script Date: 09/03/2024 14:58:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Prenotazioni](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NULL,
	[IdCamera] [int] NULL,
	[DataPrenotazione] [date] NULL,
	[NumeroProgressivoAnno] [int] NULL,
	[Anno] [int] NULL,
	[PeriodoDal] [date] NULL,
	[PeriodoAl] [date] NULL,
	[CaparraConfirmatoria] [decimal](10, 2) NULL,
	[TariffaApplicata] [decimal](10, 2) NULL,
	[PernottamentoConColazione] [bit] NULL,
	[MezzaPensione] [bit] NULL,
	[PensioneCompleta] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Prenotazioni]  WITH CHECK ADD FOREIGN KEY([IdCamera])
REFERENCES [dbo].[Camere] ([Id])
GO

ALTER TABLE [dbo].[Prenotazioni]  WITH CHECK ADD FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clienti] ([Id])
GO

USE [EpiHotel]
GO

----TABELLA SERVIZI----

/****** Object:  Table [dbo].[Servizi]    Script Date: 09/03/2024 14:59:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Servizi](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPrenotazione] [int] NULL,
	[TipoServizio] [nvarchar](50) NULL,
	[Data] [date] NULL,
	[Quantita] [int] NULL,
	[Prezzo] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Servizi]  WITH CHECK ADD FOREIGN KEY([IdPrenotazione])
REFERENCES [dbo].[Prenotazioni] ([Id])
GO


----STORED PROCEDURE MOSTRADETTAGLIOPRENOTAZIONE----

USE [EpiHotel]
GO

/****** Object:  StoredProcedure [dbo].[MostraDettaglioPrenotazione]    Script Date: 09/03/2024 15:02:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[MostraDettaglioPrenotazione]
    @IdPrenotazione INT
AS
BEGIN
PRINT 'Inizio esecuzione stored procedure MostraDettaglioPrenotazione';
PRINT 'IdPrenotazione: ' + CAST(@IdPrenotazione AS NVARCHAR(10));
    SELECT 
        Camere.NumeroCamera,
        Prenotazioni.PeriodoDal,
        Prenotazioni.PeriodoAl,
        Prenotazioni.TariffaApplicata,
        Clienti.Nome AS ClienteNome,
        Clienti.Cognome AS ClienteCognome,
        Clienti.Email AS ClienteEmail,
        Clienti.Telefono AS ClienteTelefono,
        Prenotazioni.CaparraConfirmatoria,
        SUM(ISNULL(Servizi.Prezzo, 0)) AS SommaServizi,
        (Prenotazioni.TariffaApplicata - Prenotazioni.CaparraConfirmatoria + SUM(ISNULL(Servizi.Prezzo, 0))) AS ImportoDaSaldare,
        STRING_AGG(ISNULL(Servizi.TipoServizio, ''), ', ') AS ServiziAggiuntivi
    FROM Prenotazioni
    JOIN Camere ON Prenotazioni.IdCamera = Camere.Id
    JOIN Clienti ON Prenotazioni.IdCliente = Clienti.Id
    LEFT JOIN Servizi ON Prenotazioni.Id = Servizi.IdPrenotazione
    WHERE Prenotazioni.Id = @IdPrenotazione
    GROUP BY 
        Camere.NumeroCamera, 
        Prenotazioni.PeriodoDal, 
        Prenotazioni.PeriodoAl, 
        Prenotazioni.TariffaApplicata, 
        Clienti.Nome, 
        Clienti.Cognome, 
        Clienti.Email, 
        Clienti.Telefono,
        Prenotazioni.CaparraConfirmatoria;
		PRINT 'Fine esecuzione stored procedure MostraDettaglioPrenotazione';
END
GO

----QUERY PER POPOLARE TABELLA DIPENDENTI----
INSERT INTO Dipendenti (Nome, Cognome, DataNascita, LuogoNascita, Indirizzo, CodiceFiscale, Matricola, Email, PasswordDipendente)
VALUES 
('Mario', 'Rossi', '1990-05-15', 'Roma', 'Via Roma 123', 'RSSMRA90M15H501A', 'M12345', 'mario.rossi@email.com', 'password123'),
('Luca', 'Bianchi', '1985-08-22', 'Milano', 'Via Milano 456', 'BNCLCU85M22H501B', 'L67890', 'luca.bianchi@email.com', 'securepwd'),
('Laura', 'Verdi', '1993-03-10', 'Napoli', 'Via Napoli 789', 'VRDLAU93M10H501C', 'L09876', 'laura.verdi@email.com', 'mypassword');
