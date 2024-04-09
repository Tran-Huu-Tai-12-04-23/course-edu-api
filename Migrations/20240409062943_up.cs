using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_edu_api.Migrations
{
    /// <inheritdoc />
    public partial class up : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NextLessonId",
                table: "UserCourse",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrevLessonId",
                table: "UserCourse",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_NextLessonId",
                table: "UserCourse",
                column: "NextLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_PrevLessonId",
                table: "UserCourse",
                column: "PrevLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_Lessons_NextLessonId",
                table: "UserCourse",
                column: "NextLessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_Lessons_PrevLessonId",
                table: "UserCourse",
                column: "PrevLessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_Lessons_NextLessonId",
                table: "UserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_Lessons_PrevLessonId",
                table: "UserCourse");

            migrationBuilder.DropIndex(
                name: "IX_UserCourse_NextLessonId",
                table: "UserCourse");

            migrationBuilder.DropIndex(
                name: "IX_UserCourse_PrevLessonId",
                table: "UserCourse");

            migrationBuilder.DropColumn(
                name: "NextLessonId",
                table: "UserCourse");

            migrationBuilder.DropColumn(
                name: "PrevLessonId",
                table: "UserCourse");
        }
    }
}
