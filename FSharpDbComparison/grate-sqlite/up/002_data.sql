INSERT INTO Users (Id,Name)
VALUES (1,'John Doe');

INSERT INTO Courses (Id,Title)
VALUES (1,'EF Core Course');

INSERT INTO Chapters (Id,Title)
VALUES (1,'Introduction'),
       (2,'Advanced Topics');

INSERT INTO UserCourses (UserId, CourseId)
VALUES (1,1);

INSERT INTO CourseChapters (CourseId, ChapterId)
VALUES (1,1),(1,2);
