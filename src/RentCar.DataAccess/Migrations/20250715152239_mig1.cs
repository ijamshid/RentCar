using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password_hash",
                value: "lhNexZJpra3Sx2B+cgdzO+mRDm2qjfLIENLuHdJQFk0=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password_hash",
                value: "VgKxWUJru2AunVaMv7+HxScmDjCAVJLLWtvOb6iOkNY=");
        }
    }
}
