BEGIN TRANSACTION;

INSERT INTO Users (Name)
VALUES ('John Doe'),('Jane Doe');

INSERT INTO Courses (Title)
VALUES ('EF Core Course'),('F# Course');

INSERT INTO Chapters (Title)
VALUES ('Introduction'),('Advanced Topics');

INSERT INTO UserCourses (UserId, CourseId)
VALUES (1,1);

INSERT INTO CourseChapters (CourseId, ChapterId)
VALUES (1,1),(1,2);

COMMIT;
