-- Drop the Database

DROP TABLE tours CASCADE;
DROP TABLE logs CASCADE;

--Creating Tables

Create TABLE IF NOT EXISTS tours(
tourid INT,
name TEXT,
url TEXT,
creationtime DATE,
tourlength INT,
duration INT

);

Create Table IF NOT EXISTS logs(
logid INT,
logtext TEXT,
touritemid INT
);



--Insert statments for testing

Insert into tours(TourID,Name,Url,CreationTime,TourLength,Duration) values ('1','"tour im Prater"','"empty"','2013-06-01','50','33');