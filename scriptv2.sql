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

CREATE TABLE [PostLesson] (
    [Id] bigint NOT NULL IDENTITY,
    CONSTRAINT [PK_PostLesson] PRIMARY KEY ([Id])
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
    [VideoId] nvarchar(max) NOT NULL,
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
    [Status] int NOT NULL,
    [CategoryCourseId] bigint NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_CategoriesCourse_CategoryCourseId] FOREIGN KEY ([CategoryCourseId]) REFERENCES [CategoriesCourse] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Role] int NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [FullName] nvarchar(max) NULL,
    [Bio] nvarchar(max) NULL,
    [Avatar] nvarchar(max) NULL,
    [TokenVerify] nvarchar(max) NULL,
    [VerifyAt] datetime2 NULL,
    [UserSettingId] bigint NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_UserSettings_UserSettingId] FOREIGN KEY ([UserSettingId]) REFERENCES [UserSettings] ([Id])
);
GO

CREATE TABLE [GroupLessons] (
    [Id] bigint NOT NULL IDENTITY,
    [Index] bigint NOT NULL,
    [Title] nvarchar(max) NULL,
    [CourseId] bigint NULL,
    CONSTRAINT [PK_GroupLessons] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupLessons_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
);
GO

CREATE TABLE [Ratings] (
    [Id] bigint NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [RateAt] datetime2 NOT NULL,
    [Star] int NOT NULL,
    [CourseId] bigint NULL,
    CONSTRAINT [PK_Ratings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Ratings_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
);
GO

CREATE TABLE [PaymentHistories] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] bigint NOT NULL,
    [PaymentType] int NOT NULL,
    [PaymentAt] datetime2 NOT NULL,
    [Amount] float NULL,
    [IsPayment] bit NOT NULL,
    CONSTRAINT [PK_PaymentHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PaymentHistories_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Posts] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [Thumbnail] nvarchar(max) NULL,
    [IsPin] bit NOT NULL,
    [IsApproved] bit NOT NULL,
    [createAt] datetime2 NOT NULL,
    [ApproveDate] datetime2 NOT NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Posts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Comments] (
    [Id] bigint NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [CommentAt] datetime2 NOT NULL,
    [PostId] bigint NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id])
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
    [PostLessonId] bigint NULL,
    CONSTRAINT [PK_SubItemPosts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubItemPosts_PostLesson_PostLessonId] FOREIGN KEY ([PostLessonId]) REFERENCES [PostLesson] ([Id]),
    CONSTRAINT [FK_SubItemPosts_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id])
);
GO

CREATE TABLE [Lessons] (
    [Id] bigint NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Index] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [VideoId] bigint NULL,
    [PostId] bigint NULL,
    [GroupLessonId] bigint NULL,
    [UserCourseId] bigint NULL,
    CONSTRAINT [PK_Lessons] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Lessons_GroupLessons_GroupLessonId] FOREIGN KEY ([GroupLessonId]) REFERENCES [GroupLessons] ([Id]),
    CONSTRAINT [FK_Lessons_PostLesson_PostId] FOREIGN KEY ([PostId]) REFERENCES [PostLesson] ([Id]),
    CONSTRAINT [FK_Lessons_VideoLesson_VideoId] FOREIGN KEY ([VideoId]) REFERENCES [VideoLesson] ([Id])
);
GO

CREATE TABLE [Question] (
    [Id] bigint NOT NULL IDENTITY,
    [Index] int NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Answers] nvarchar(max) NOT NULL,
    [CorrectAnswerIndex] int NOT NULL,
    [Explain] nvarchar(max) NOT NULL,
    [ImgURL] nvarchar(max) NOT NULL,
    [LessonId] bigint NULL,
    CONSTRAINT [PK_Question] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Question_Lessons_LessonId] FOREIGN KEY ([LessonId]) REFERENCES [Lessons] ([Id])
);
GO

CREATE TABLE [UserCourse] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] bigint NOT NULL,
    [CourseId] bigint NOT NULL,
    [RegisterAt] datetime2 NOT NULL,
    [IsPayment] bit NOT NULL,
    [PaymentHistoryId] bigint NULL,
    [CurrentLessonId] bigint NULL,
    CONSTRAINT [PK_UserCourse] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserCourse_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserCourse_Lessons_CurrentLessonId] FOREIGN KEY ([CurrentLessonId]) REFERENCES [Lessons] ([Id]),
    CONSTRAINT [FK_UserCourse_PaymentHistories_PaymentHistoryId] FOREIGN KEY ([PaymentHistoryId]) REFERENCES [PaymentHistories] ([Id]),
    CONSTRAINT [FK_UserCourse_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [NoteLessons] (
    [Id] bigint NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [LessonId] bigint NOT NULL,
    [UserId] bigint NOT NULL,
    [NoteAt] datetime2 NOT NULL,
    [UserCourseId] bigint NULL,
    CONSTRAINT [PK_NoteLessons] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NoteLessons_UserCourse_UserCourseId] FOREIGN KEY ([UserCourseId]) REFERENCES [UserCourse] ([Id])
);
GO

CREATE INDEX [IX_Comments_PostId] ON [Comments] ([PostId]);
GO

CREATE INDEX [IX_Courses_CategoryCourseId] ON [Courses] ([CategoryCourseId]);
GO

CREATE INDEX [IX_GroupLessons_CourseId] ON [GroupLessons] ([CourseId]);
GO

CREATE INDEX [IX_Lessons_GroupLessonId] ON [Lessons] ([GroupLessonId]);
GO

CREATE INDEX [IX_Lessons_PostId] ON [Lessons] ([PostId]);
GO

CREATE INDEX [IX_Lessons_UserCourseId] ON [Lessons] ([UserCourseId]);
GO

CREATE INDEX [IX_Lessons_VideoId] ON [Lessons] ([VideoId]);
GO

CREATE INDEX [IX_NoteLessons_UserCourseId] ON [NoteLessons] ([UserCourseId]);
GO

CREATE INDEX [IX_PaymentHistories_UserId] ON [PaymentHistories] ([UserId]);
GO

CREATE INDEX [IX_Posts_UserId] ON [Posts] ([UserId]);
GO

CREATE INDEX [IX_Question_LessonId] ON [Question] ([LessonId]);
GO

CREATE INDEX [IX_Ratings_CourseId] ON [Ratings] ([CourseId]);
GO

CREATE INDEX [IX_SubItemPosts_PostId] ON [SubItemPosts] ([PostId]);
GO

CREATE INDEX [IX_SubItemPosts_PostLessonId] ON [SubItemPosts] ([PostLessonId]);
GO

CREATE INDEX [IX_UserCourse_CourseId] ON [UserCourse] ([CourseId]);
GO

CREATE INDEX [IX_UserCourse_CurrentLessonId] ON [UserCourse] ([CurrentLessonId]);
GO

CREATE INDEX [IX_UserCourse_PaymentHistoryId] ON [UserCourse] ([PaymentHistoryId]);
GO

CREATE INDEX [IX_UserCourse_UserId] ON [UserCourse] ([UserId]);
GO

CREATE INDEX [IX_Users_UserSettingId] ON [Users] ([UserSettingId]);
GO

ALTER TABLE [Lessons] ADD CONSTRAINT [FK_Lessons_UserCourse_UserCourseId] FOREIGN KEY ([UserCourseId]) REFERENCES [UserCourse] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240408052814_Initial', N'8.0.2');
GO

COMMIT;
GO

