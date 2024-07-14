
	create table Employees(
		EmployeeID char(10) NOT NULL PRIMARY KEY,
		[Name] NVARCHAR(128) NOT NULL,
		[Email] NVARCHAR(200) NOT NULL,
		Mobile char(10) not null CHECK (Mobile LIKE '[6-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
		Gender NVARCHAR(6) NOT NULL CHECK (Gender IN ('Male', 'Female', 'Other')),
		Age INT NOT NULL CHECK (Age >= 18 AND Age <= 60),
		PhoneNumber char(10)
	)

	alter table Employees
	add check(LEN([Name]) >= 2 and LEN(Name)<=128)

	alter table Employees
	ADD CONSTRAINT EmailValid CHECK (Email like '%_@__%.__%')


	INSERT INTO [dbo].[Employees]
           ([EmployeeId]
           ,[Name]
           ,[Email]
           ,[Mobile]
           ,[Gender]
           ,[Age])
     VALUES
           ('Employee01'
           , 'Riya'
           , 'riya23@gmail.com'
           , '7945201230'
           , 'Female'
           , 20)
	GO

	SELECT * FROM Employees
	--drop table employees
--Employee01