using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalogue.Api.Migrations
{
    /// <inheritdoc />
    public partial class ImproveDbDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uploaded",
                table: "Assets",
                newName: "UploadedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AssetTags",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "Assets",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Assets",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Assets",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "AssetHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_Barcode",
                table: "Variants",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Variants_VariantCode",
                table: "Variants",
                column: "VariantCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetTags_Name",
                table: "AssetTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ProductId",
                table: "Assets",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_VariantId",
                table: "Assets",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Products_ProductId",
                table: "Assets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Variants_VariantId",
                table: "Assets",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Products_ProductId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Variants_VariantId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Variants_Barcode",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_VariantCode",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_AssetTags_Name",
                table: "AssetTags");

            migrationBuilder.DropIndex(
                name: "IX_Assets_ProductId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_VariantId",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "Assets",
                newName: "Uploaded");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AssetTags",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "Assets",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Assets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Assets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "AssetHistory",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
