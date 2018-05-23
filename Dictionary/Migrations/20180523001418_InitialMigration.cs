using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dictionary.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData("Users", new[] { "Id", "Email", "Password", "IsActive", "CreatedOn" },
                new object[] { "0", "SYSTEM", "SYSTEM", false, DateTime.Now });

            migrationBuilder.AddColumn<int>("UserId", "Definitions", nullable: false, defaultValue: 0);
            migrationBuilder.AddForeignKey("FK_Definitions_Users_UserId",
                "Definitions", "UserId", "Users", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.CreateIndex("IX_Definitions_UserId", "Definitions", "UserId");

            // migrationBuilder.CreateTable(
            //     name: "Definitions",
            //     columns: table => new
            //     {
            //         Id = table.Column<long>(nullable: false)
            //             .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //         EnglishWord = table.Column<string>(nullable: true),
            //         PartOfSpeech = table.Column<string>(nullable: true),
            //         MalayalamDefinition = table.Column<string>(nullable: true),
            //         UserId = table.Column<int>(nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Definitions", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Definitions_Users_UserId",
            //             column: x => x.UserId,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Definitions_UserId",
            //     table: "Definitions",
            //     column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "Definitions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
