using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class initialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InwardReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CHANNEL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SESSION_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRANSACTION_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RESPONSE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRANSACTION_TIME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ORIGINATOR_INSTITUTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ORIGINATOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_INSTITUTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_ACCOUNT_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_ACCOUNT_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NARRATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAYMENT_REFERENCE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LAST_12_DIGITS_OF_SESSION_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCredited = table.Column<bool>(type: "bit", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoreBankingReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionExist = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InwardReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NipInwardBatches",
                columns: table => new
                {
                    BatchId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NipInwardBatches", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "NipOutwardBatches",
                columns: table => new
                {
                    BatchId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NipOutwardBatches", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "OutwardReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CHANNEL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SESSION_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRANSACTION_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RESPONSE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRANSACTION_TIME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ORIGINATOR_INSTITUTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ORIGINATOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_INSTITUTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_ACCOUNT_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_ACCOUNT_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NARRATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAYMENT_REFERENCE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LAST_12_DIGITS_OF_SESSION_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDebited = table.Column<bool>(type: "bit", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoreBankingReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionExist = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutwardReports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InwardReports");

            migrationBuilder.DropTable(
                name: "NipInwardBatches");

            migrationBuilder.DropTable(
                name: "NipOutwardBatches");

            migrationBuilder.DropTable(
                name: "OutwardReports");
        }
    }
}
