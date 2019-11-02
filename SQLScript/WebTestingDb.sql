
CREATE DATABASE WebTestingDb;
GO

USE WebTestingDb;
GO


CREATE TABLE Test(
TestId int IDENTITY(1,1) not null PRIMARY KEY,
TestName nvarchar(100) not null,
TestDescription nvarchar(200) not null
);
GO
INSERT INTO Test VALUES('Object orianted programming','Object orianted programming in C# programming language');


CREATE TABLE Question(
QuestionId int IDENTITY(1,1) not null PRIMARY KEY,
TestId int not null FOREIGN KEY REFERENCES Test(TestId),
QuestionText nvarchar(400) not null,
Answer1 nvarchar(400) not null,
Answer2 nvarchar(400) not null,
Answer3 nvarchar(400) not null,
CorrectAnswer int not null,
QuestionNumber int not null
);

INSERT INTO Question VALUES(
1,
'Abstract method :',
'Does not have a body',
'Has body wethout code',
'Has body where we can put our code'
,1,
1);
INSERT INTO Question VALUES(
1,
'One of the differences between the interface and the abstract class is :',
'We can create instance for interface but can not for apstract class',
'There is no diffrence',
'Interface can not have fileds while abstract class can'
,2,
2);
INSERT INTO Question VALUES(
1,
'We can get number of mambers within some generic list by using which line of code :',
'property Length',
'method Count()',
'property Count'
,3,
3);
INSERT INTO Question VALUES(
1,
'Class Account has constructor without parametars and implements interface IAccount. What is the
right line of code?',
'IAccount i = new IAccount()',
'IAccount i = new Account()',
'Account r = new IAccount()'
,2,
4);
INSERT INTO Question VALUES(
1,
'Key of generic dictanory SortedDictionary<int,string> is:',
'variable type int',
'variable type string',
'variable type KeyValuePair<int,string>'
,1,
5);




CREATE TABLE TestResult(
SessionId nvarchar(100) not null PRIMARY KEY,
TestId int not null FOREIGN KEY REFERENCES Test(TestId),
Username nvarchar(50) not null,
FinishTime datetime not null,
WonPoints decimal(9,3) not null
);

CREATE TABLE Testing(
TestingId int IDENTITY(1,1) not null PRIMARY KEY,
SessionId nvarchar(100) not null,
QuestionId int not null FOREIGN KEY REFERENCES Question(QuestionId),
UserAnswer int not null,
AnswerCorrectly bit not null,
CONSTRAINT UQ_Testing UNIQUE(SessionId,QuestionId)
);




