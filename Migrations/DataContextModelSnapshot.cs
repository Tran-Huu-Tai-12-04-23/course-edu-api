﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using course_edu_api.Data;

#nullable disable

namespace course_edu_api.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("course_edu_api.Entities.Banner", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ActionLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BtnTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subtitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Thumbnails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Banners");
                });

            modelBuilder.Entity("course_edu_api.Entities.CategoryCourse", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLock")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("CategoriesCourse");
                });

            modelBuilder.Entity("course_edu_api.Entities.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CommentAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("PostId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("course_edu_api.Entities.Course", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AdviseVideo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CategoryCourseId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("RequireSkill")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("SubTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryCourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("course_edu_api.Entities.GroupLesson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("CourseId")
                        .HasColumnType("bigint");

                    b.Property<long>("Index")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("GroupLessons");
                });

            modelBuilder.Entity("course_edu_api.Entities.Lesson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("GroupLessonId")
                        .HasColumnType("bigint");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<long?>("PostId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<long?>("UserCourseId")
                        .HasColumnType("bigint");

                    b.Property<long?>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GroupLessonId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserCourseId");

                    b.HasIndex("VideoId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("course_edu_api.Entities.NoteLesson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("LessonId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("NoteAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UserCourseId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserCourseId");

                    b.ToTable("NoteLessons");
                });

            modelBuilder.Entity("course_edu_api.Entities.PaymentHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<double?>("Amount")
                        .HasColumnType("float");

                    b.Property<bool>("IsPayment")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PaymentAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentHistories");
                });

            modelBuilder.Entity("course_edu_api.Entities.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("ApproveDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPin")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("course_edu_api.Entities.PostLesson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.HasKey("Id");

                    b.ToTable("PostLesson");
                });

            modelBuilder.Entity("course_edu_api.Entities.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Answers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CorrectAnswerIndex")
                        .HasColumnType("int");

                    b.Property<string>("Explain")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<long?>("LessonId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("course_edu_api.Entities.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("PostId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("course_edu_api.Entities.SubItemPost", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Alt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImgURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("PostId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PostLessonId")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("PostLessonId");

                    b.ToTable("SubItemPosts");
                });

            modelBuilder.Entity("course_edu_api.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("TokenVerify")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserSettingId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("VerifyAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserSettingId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("course_edu_api.Entities.UserCourse", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CurrentLessonId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsPayment")
                        .HasColumnType("bit");

                    b.Property<long?>("PaymentHistoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RegisterAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("CurrentLessonId");

                    b.HasIndex("PaymentHistoryId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCourse");
                });

            modelBuilder.Entity("course_edu_api.Entities.UserSetting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("FacebookLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GithubLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailForNewCourse")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNotificationForCmtOfYourBlog")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNotificationForNewCourse")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNotificationForPinInDiscuss")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNotificationForReplyCmt")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("course_edu_api.Entities.VideoLesson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("VideoId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VideoLesson");
                });

            modelBuilder.Entity("course_edu_api.Entities.Comment", b =>
                {
                    b.HasOne("course_edu_api.Entities.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("course_edu_api.Entities.Course", b =>
                {
                    b.HasOne("course_edu_api.Entities.CategoryCourse", "CategoryCourse")
                        .WithMany()
                        .HasForeignKey("CategoryCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryCourse");
                });

            modelBuilder.Entity("course_edu_api.Entities.GroupLesson", b =>
                {
                    b.HasOne("course_edu_api.Entities.Course", null)
                        .WithMany("GroupLessons")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("course_edu_api.Entities.Lesson", b =>
                {
                    b.HasOne("course_edu_api.Entities.GroupLesson", null)
                        .WithMany("Lessons")
                        .HasForeignKey("GroupLessonId");

                    b.HasOne("course_edu_api.Entities.PostLesson", "Post")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.HasOne("course_edu_api.Entities.UserCourse", null)
                        .WithMany("LessonPassed")
                        .HasForeignKey("UserCourseId");

                    b.HasOne("course_edu_api.Entities.VideoLesson", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId");

                    b.Navigation("Post");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("course_edu_api.Entities.NoteLesson", b =>
                {
                    b.HasOne("course_edu_api.Entities.UserCourse", null)
                        .WithMany("Notes")
                        .HasForeignKey("UserCourseId");
                });

            modelBuilder.Entity("course_edu_api.Entities.PaymentHistory", b =>
                {
                    b.HasOne("course_edu_api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("course_edu_api.Entities.Post", b =>
                {
                    b.HasOne("course_edu_api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("course_edu_api.Entities.Question", b =>
                {
                    b.HasOne("course_edu_api.Entities.Lesson", null)
                        .WithMany("Quiz")
                        .HasForeignKey("LessonId");
                });

            modelBuilder.Entity("course_edu_api.Entities.Rating", b =>
                {
                    b.HasOne("course_edu_api.Entities.Post", null)
                        .WithMany("Ratings")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("course_edu_api.Entities.SubItemPost", b =>
                {
                    b.HasOne("course_edu_api.Entities.Post", null)
                        .WithMany("Items")
                        .HasForeignKey("PostId");

                    b.HasOne("course_edu_api.Entities.PostLesson", null)
                        .WithMany("items")
                        .HasForeignKey("PostLessonId");
                });

            modelBuilder.Entity("course_edu_api.Entities.User", b =>
                {
                    b.HasOne("course_edu_api.Entities.UserSetting", "UserSetting")
                        .WithMany()
                        .HasForeignKey("UserSettingId");

                    b.Navigation("UserSetting");
                });

            modelBuilder.Entity("course_edu_api.Entities.UserCourse", b =>
                {
                    b.HasOne("course_edu_api.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("course_edu_api.Entities.Lesson", "CurrentLesson")
                        .WithMany()
                        .HasForeignKey("CurrentLessonId");

                    b.HasOne("course_edu_api.Entities.PaymentHistory", "PaymentHistory")
                        .WithMany()
                        .HasForeignKey("PaymentHistoryId");

                    b.HasOne("course_edu_api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("CurrentLesson");

                    b.Navigation("PaymentHistory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("course_edu_api.Entities.Course", b =>
                {
                    b.Navigation("GroupLessons");
                });

            modelBuilder.Entity("course_edu_api.Entities.GroupLesson", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("course_edu_api.Entities.Lesson", b =>
                {
                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("course_edu_api.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Items");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("course_edu_api.Entities.PostLesson", b =>
                {
                    b.Navigation("items");
                });

            modelBuilder.Entity("course_edu_api.Entities.UserCourse", b =>
                {
                    b.Navigation("LessonPassed");

                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
