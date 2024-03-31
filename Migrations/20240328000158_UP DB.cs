using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_edu_api.Migrations
{
    /// <inheritdoc />
    public partial class UPDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_QuizzLesson_QuizzId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLesson_Posts_PostId",
                table: "PostLesson");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "QuizzLesson");

            migrationBuilder.DropIndex(
                name: "IX_PostLesson_PostId",
                table: "PostLesson");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_QuizzId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostLesson");

            migrationBuilder.DropColumn(
                name: "QuizzId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "VideoLesson",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PostLessonId",
                table: "SubItemPosts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswerIndex = table.Column<int>(type: "int", nullable: false),
                    Explain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubItemPosts_PostLessonId",
                table: "SubItemPosts",
                column: "PostLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_LessonId",
                table: "Question",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubItemPosts_PostLesson_PostLessonId",
                table: "SubItemPosts",
                column: "PostLessonId",
                principalTable: "PostLesson",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubItemPosts_PostLesson_PostLessonId",
                table: "SubItemPosts");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropIndex(
                name: "IX_SubItemPosts_PostLessonId",
                table: "SubItemPosts");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "VideoLesson");

            migrationBuilder.DropColumn(
                name: "PostLessonId",
                table: "SubItemPosts");

            migrationBuilder.AddColumn<long>(
                name: "PostId",
                table: "PostLesson",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "QuizzId",
                table: "Lessons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "QuizzLesson",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrectAnswerIndex = table.Column<int>(type: "int", nullable: false),
                    Explain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizzLesson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectIndex = table.Column<int>(type: "int", nullable: false),
                    QuizzLessonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_QuizzLesson_QuizzLessonId",
                        column: x => x.QuizzLessonId,
                        principalTable: "QuizzLesson",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostLesson_PostId",
                table: "PostLesson",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_QuizzId",
                table: "Lessons",
                column: "QuizzId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuizzLessonId",
                table: "Answer",
                column: "QuizzLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_QuizzLesson_QuizzId",
                table: "Lessons",
                column: "QuizzId",
                principalTable: "QuizzLesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLesson_Posts_PostId",
                table: "PostLesson",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
