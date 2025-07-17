using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cars_brands_brand_id",
                table: "cars");

            migrationBuilder.DropForeignKey(
                name: "fk_payments_reservations_reservation_id",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "fk_payments_users_user_id",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "fk_permissions_permission_group_permission_group_id",
                table: "permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_reservations_cars_car_id",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles");

            migrationBuilder.AddForeignKey(
                name: "fk_cars_brands_brand_id",
                table: "cars",
                column: "brand_id",
                principalTable: "brands",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_reservations_reservation_id",
                table: "payments",
                column: "reservation_id",
                principalTable: "reservations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_users_user_id",
                table: "payments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_permissions_permission_group_permission_group_id",
                table: "permissions",
                column: "permission_group_id",
                principalTable: "permission_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_cars_car_id",
                table: "reservations",
                column: "car_id",
                principalTable: "cars",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id",
                principalTable: "permissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cars_brands_brand_id",
                table: "cars");

            migrationBuilder.DropForeignKey(
                name: "fk_payments_reservations_reservation_id",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "fk_payments_users_user_id",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "fk_permissions_permission_group_permission_group_id",
                table: "permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_reservations_cars_car_id",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles");

            migrationBuilder.AddForeignKey(
                name: "fk_cars_brands_brand_id",
                table: "cars",
                column: "brand_id",
                principalTable: "brands",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_reservations_reservation_id",
                table: "payments",
                column: "reservation_id",
                principalTable: "reservations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_payments_users_user_id",
                table: "payments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_permissions_permission_group_permission_group_id",
                table: "permissions",
                column: "permission_group_id",
                principalTable: "permission_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_cars_car_id",
                table: "reservations",
                column: "car_id",
                principalTable: "cars",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id",
                principalTable: "permissions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
