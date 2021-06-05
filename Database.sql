-- Drop the Database

DROP TABLE tours CASCADE;
DROP TABLE logs CASCADE;

--Creating Tables

Create TABLE IF NOT EXISTS tours(
tourid INT PRIMARY KEY,
name TEXT,
fromstart TEXT,
too TEXT,
creationtime DATE,
tourlength INT,
duration INT,
description TEXT

);

Create Table IF NOT EXISTS logs(
logid INT PRIMARY KEY,
date DATE,
maxvelocity INT,
minvelocity INT,
avvelocity INT,
caloriesburnt INT,
duration INT,
author TEXT,
commentt TEXT,
touritemid INT
);

ALTER TABLE logs ADD FOREIGN KEY ("touritemid")
REFERENCES tours ("tourid") ON DELETE CASCADE;

--Insert statments for testing

-- Drop the Database

DROP TABLE tours CASCADE;
DROP TABLE logs CASCADE;

--Creating Tables

Create TABLE IF NOT EXISTS tours(
tourid INT PRIMARY KEY,
name TEXT,
fromstart TEXT,
to TEXT,
creationtime DATE,
tourlength INT,
duration INT,
description TEXT

);

Create Table IF NOT EXISTS logs(
logid INT PRIMARY KEY,
date DATE,
maxvelocity INT,
minvelocity INT,
avvelocity INT,
caloriesburnt INT,
duration INT,
author TEXT,
commentt TEXT,
touritemid INT
);

ALTER TABLE logs ADD FOREIGN KEY ("touritemid")
REFERENCES tours ("tourid") ON DELETE CASCADE;

--Insert statments for testing

Insert into tours(TourID,Name,Fromstart,To,CreationTime,TourLength,Duration,Description) values ('1','"tour im Prater"','"wien"','"bratislava"','2013-06-01','50','13','"super"');
Insert into tours(TourID,Name,Fromstart,To,CreationTime,TourLength,Duration,Description) values ('2','"Tulln-Wien"','"wien"','"münchen"','2013-07-01','50','32','"super"');
Insert into tours(TourID,Name,Fromstart,To,CreationTime,TourLength,Duration,Description) values ('3','"Ybbs-Gmunden"','"wien"','"saalbach"','2013-07-22','50','44','"super"');
Insert into tours(TourID,Name,Fromstart,To,CreationTime,TourLength,Duration,Description) values ('4','"Gmunden-Strobl"','"porto"','"lissabon"','2018-05-01','50','5','"super"');
Insert into tours(TourID,Name,Fromstart,To,CreationTime,TourLength,Duration,Description) values ('5','"lacknergasse-Gsoellnergasse"','"lacknergasse"','"gsoellnergasse"','2002-02-02','50','69','"super"');
Insert into tours(TourID,Name,Fromstart,To,CreationTime,TourLength,Duration,Description) values ('6','"Wien-Porto"','"korneuburg"','"lacknergasse"','2001-08-02','50','55','"super"');

Insert into logs(logid,date,maxvelocity,minvelocity,avvelocity,caloriesburnt,duration,touritemid,author,commentt) values ('5','2013-07-01','2','3','4','5','6','1','"Andreas"','"Super Tour"');

Insert into logs(logid,date,maxvelocity,minvelocity,avvelocity,caloriesburnt,duration,touritemid,author,commentt) values ('5','2013-07-01','2','3','4','5','6','1','"Andreas"','"Super Tour"');