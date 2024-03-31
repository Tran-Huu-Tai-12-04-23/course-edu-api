IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Banners] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Subtitle] nvarchar(max) NOT NULL,
    [Thumbnails] nvarchar(max) NOT NULL,
    [ActionLink] nvarchar(max) NOT NULL,
    [BtnTitle] nvarchar(max) NOT NULL,
    [StartColor] nvarchar(max) NOT NULL,
    [EndColor] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Banners] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CategoriesCourse] (
    [Id] bigint NOT NULL IDENTITY,
    [CategoryName] nvarchar(max) NOT NULL,
    [IsLock] bit NOT NULL,
    CONSTRAINT [PK_CategoriesCourse] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [NoteLessons] (
    [Id] bigint NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [TimeSecond] bigint NOT NULL,
    [LessonId] bigint NOT NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_NoteLessons] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [QuizzLesson] (
    [Id] bigint NOT NULL IDENTITY,
    [Question] nvarchar(max) NOT NULL,
    [CorrectAnswerIndex] int NOT NULL,
    [Explain] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_QuizzLesson] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserSettings] (
    [Id] bigint NOT NULL IDENTITY,
    [FacebookLink] nvarchar(max) NOT NULL,
    [GithubLink] nvarchar(max) NOT NULL,
    [IsEmailForNewCourse] bit NOT NULL,
    [IsNotificationForNewCourse] bit NOT NULL,
    [IsNotificationForReplyCmt] bit NOT NULL,
    [IsNotificationForCmtOfYourBlog] bit NOT NULL,
    [IsNotificationForPinInDiscuss] bit NOT NULL,
    CONSTRAINT [PK_UserSettings] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [VideoLesson] (
    [Id] bigint NOT NULL IDENTITY,
    [VideoURL] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_VideoLesson] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Courses] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Price] float NOT NULL,
    [SubTitle] nvarchar(max) NOT NULL,
    [Target] nvarchar(max) NOT NULL,
    [RequireSkill] nvarchar(max) NOT NULL,
    [Thumbnail] nvarchar(max) NOT NULL,
    [AdviseVideo] nvarchar(max) NOT NULL,
    [CategoryCourseId] bigint NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_CategoriesCourse_CategoryCourseId] FOREIGN KEY ([CategoryCourseId]) REFERENCES [CategoriesCourse] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Answer] (
    [Id] bigint NOT NULL IDENTITY,
    [CorrectIndex] int NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [QuizzLessonId] bigint NULL,
    CONSTRAINT [PK_Answer] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Answer_QuizzLesson_QuizzLessonId] FOREIGN KEY ([QuizzLessonId]) REFERENCES [QuizzLesson] ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Role] int NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [FullName] nvarchar(max) NOT NULL,
    [Bio] nvarchar(max) NOT NULL,
    [Avatar] nvarchar(max) NOT NULL,
    [UserSettingId] bigint NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_UserSettings_UserSettingId] FOREIGN KEY ([UserSettingId]) REFERENCES [UserSettings] ([Id])
);
GO

CREATE TABLE [GroupLessons] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [CourseId] bigint NULL,
    CONSTRAINT [PK_GroupLessons] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupLessons_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
);
GO

CREATE TABLE [Posts] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [StatePost] nvarchar(max) NULL,
    [Thumbnail] nvarchar(max) NULL,
    [IsPin] bit NOT NULL,
    [isApproved] bit NOT NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Posts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PostLesson] (
    [Id] bigint NOT NULL IDENTITY,
    [PostId] bigint NOT NULL,
    CONSTRAINT [PK_PostLesson] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PostLesson_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [SubItemPosts] (
    [Id] bigint NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Index] int NOT NULL,
    [Content] text NOT NULL,
    [Alt] nvarchar(max) NOT NULL,
    [ImgURL] nvarchar(max) NOT NULL,
    [Link] nvarchar(max) NOT NULL,
    [PostId] bigint NULL,
    CONSTRAINT [PK_SubItemPosts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubItemPosts_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id])
);
GO

CREATE TABLE [Lessons] (
    [Id] bigint NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [QuizzId] bigint NOT NULL,
    [VideoId] bigint NOT NULL,
    [PostId] bigint NOT NULL,
    CONSTRAINT [PK_Lessons] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Lessons_PostLesson_PostId] FOREIGN KEY ([PostId]) REFERENCES [PostLesson] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Lessons_QuizzLesson_QuizzId] FOREIGN KEY ([QuizzId]) REFERENCES [QuizzLesson] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Lessons_VideoLesson_VideoId] FOREIGN KEY ([VideoId]) REFERENCES [VideoLesson] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Answer_QuizzLessonId] ON [Answer] ([QuizzLessonId]);
GO

CREATE INDEX [IX_Courses_CategoryCourseId] ON [Courses] ([CategoryCourseId]);
GO

CREATE INDEX [IX_GroupLessons_CourseId] ON [GroupLessons] ([CourseId]);
GO

CREATE INDEX [IX_Lessons_PostId] ON [Lessons] ([PostId]);
GO

CREATE INDEX [IX_Lessons_QuizzId] ON [Lessons] ([QuizzId]);
GO

CREATE INDEX [IX_Lessons_VideoId] ON [Lessons] ([VideoId]);
GO

CREATE INDEX [IX_PostLesson_PostId] ON [PostLesson] ([PostId]);
GO

CREATE INDEX [IX_Posts_UserId] ON [Posts] ([UserId]);
GO

CREATE INDEX [IX_SubItemPosts_PostId] ON [SubItemPosts] ([PostId]);
GO

CREATE INDEX [IX_Users_UserSettingId] ON [Users] ([UserSettingId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240324065438_Initial', N'8.0.2');
GO

COMMIT;
GO

