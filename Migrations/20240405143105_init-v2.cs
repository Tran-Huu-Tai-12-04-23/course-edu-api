using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_edu_api.Migrations
{
    /// <inheritdoc />
    public partial class initv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_GroupLessons_CurrentGroupLessonId",
                table: "UserCourse");

            migrationBuilder.DropIndex(
                name: "IX_UserCourse_CurrentGroupLessonId",
                table: "UserCourse");

            migrationBuilder.DropColumn(
                name: "CurrentGroupLessonId",
                table: "UserCourse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrentGroupLessonId",
                table: "UserCourse",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_CurrentGroupLessonId",
                table: "UserCourse",
                column: "CurrentGroupLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_GroupLessons_CurrentGroupLessonId",
                table: "UserCourse",
                column: "CurrentGroupLessonId",
                principalTable: "GroupLessons",
                principalColumn: "Id");
        }
    }
}
