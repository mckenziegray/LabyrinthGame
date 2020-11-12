IF EXISTS (SELECT name FROM master.sys.databases WHERE name = N'Labyrinth')
	DROP DATABASE [Labyrinth]

CREATE DATABASE [Labyrinth]
GO

USE [Labyrinth]

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Enemy')
	DROP TABLE [dbo].[Enemy]
	
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Shield')
	DROP TABLE [dbo].[Shield]

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Armor')
	DROP TABLE [dbo].[Armor]

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Weapon')
	DROP TABLE [dbo].[Weapon]

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Item')
	DROP TABLE [dbo].[Item]

CREATE TABLE [dbo].[Item](
	[ItemType] [nvarchar](20) NOT NULL,
	[Value] [int] NOT NULL,
	[Stackable] [bit] NOT NULL,
	[MaxInitialCount] [int] NOT NULL,

	CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
	(
		[ItemType] ASC
	)
)

CREATE TABLE [dbo].[Weapon](
	[WeaponType] [nvarchar](20) NOT NULL,
	[Damage] [int] NOT NULL,
	[Value] [int] NOT NULL,

	CONSTRAINT [PK_Weapon] PRIMARY KEY CLUSTERED 
	(
		[WeaponType] ASC
	)
)

CREATE TABLE [dbo].[Armor](
	[ArmorType] [nvarchar](20) NOT NULL,
	[Defense] [int] NOT NULL,
	[Value] [int] NOT NULL,

	CONSTRAINT [PK_Armor] PRIMARY KEY CLUSTERED 
	(
		[ArmorType] ASC
	)
)

CREATE TABLE [dbo].[Shield](
	[ShieldType] [nvarchar](20) NOT NULL,
	[Defense] [int] NOT NULL,
	[Value] [int] NOT NULL,

	CONSTRAINT [PK_Shield] PRIMARY KEY CLUSTERED 
	(
		[ShieldType] ASC
	)
)

CREATE TABLE [dbo].[Enemy](
	[EnemyType] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[MaxHP] [int] NOT NULL,
	[Power] [int] NOT NULL,
	[XP] [int] NOT NULL,
	[Difficulty] [int] NOT NULL,

	CONSTRAINT [PK_Enemy] PRIMARY KEY CLUSTERED 
	(
		[EnemyType] ASC
	)
)

INSERT INTO [dbo].[Item]	([ItemType],	[Value],	[Stackable],	[MaxInitialCount])
	VALUES					('Weapon',		0,			0,				1),
							('Armor',		0,			0,				1),
							('Shield',		0,			0,				1),
							('Bow',			100,		0,				1),
							('Arrows',		2,			1,				3),
							('Potions',		5,			1,				1),
							('Gold',		1,			1,				5)

INSERT INTO [dbo].[Weapon]	([WeaponType],	[Damage],	[Value])
	VALUES					('Dagger',		5,			15),
							('Sword',		10,			50),
							('Axe',			15,			113)

INSERT INTO [dbo].[Armor]	([ArmorType],	[Defense],	[Value])
	VALUES					('Leather',		3,			5),
							('Chain',		6,			18),
							('Iron',		9,			41)

INSERT INTO [dbo].[Shield]	([ShieldType],	[Defense],	[Value])
	VALUES					('Wood',		4,			8),
							('Kite',		6,			18)

INSERT INTO [dbo].[Enemy]	([EnemyType],	[Description],			[MaxHP],	[Power],	[XP],	[Difficulty])
	VALUES					('Rat',			'A large rat',			3,			1,			9,		0),
							('Spider',		'A giant spider',		3,			2,			12,		1),
							('Wolf',		'A hungry wolf',		4,			3,			17,		3),
							('Ghoul',		'A decrepit ghoul',		5,			4,			21,		6),
							('Goblin',		'A petulent goblin',	6,			6,			30,		10),
							('Vagrant',		'A crazed vagrant',		8,			7,			37,		15),
							('Specter',		'An angry specter',		10,			9,			46,		21),
							('Ogre',		'A menacing ogre',		12,			11,			55,		28),
							('Minotaur',	'The enraged minotaur',	14,			14,			100,	100),
							('Dragon',		'A towering dragon',	17,			17,			200,	100)