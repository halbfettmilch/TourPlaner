-- Drop the Database

DROP TABLE tours CASCADE;
DROP TABLE logs CASCADE;

--Creating Tables

Create TABLE IF NOT EXISTS tours(
TourID INT,
Name TEXT,
Url TEXT,
CreationTime DATE,
TourLength INT,
Duration INT

);

Create Table IF NOT EXISTS logs(
logID INT,
logText TEXT,
tourItemId INT
);



--Insert statments for testing

Insert into tours(TourID,Name,Url,CreationTime,TourLength,Duration) values ('1','"tour im Prater"','"empty"','2013-06-01','50','33');