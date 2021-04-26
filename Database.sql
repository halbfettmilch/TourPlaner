-- Drop the Database

DROP TABLE tours CASCADE;

--Creating Tables

Create TABLE IF NOT EXISTS tours(
TourID INT,
TourDate DATE,
TourLength INT,
DurationTIME,
TourOwner TEXT,
TourPic INT
);



--Insert statments for testing

Insert into users(userID,username,userpassword) values(2,'kienboeck','password');
Insert into tours(tourID,TourDate,TourLength,Duration,TourOwner,TourPic) values ('1','2013-06-01','50','00:00:00','2','123321');