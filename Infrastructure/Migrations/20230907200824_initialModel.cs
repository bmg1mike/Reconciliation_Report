using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BatchId = table.Column<string>(type: "text", nullable: true),
                    CHANNEL = table.Column<string>(type: "text", nullable: true),
                    SESSION_ID = table.Column<string>(type: "text", nullable: true),
                    TRANSACTION_TYPE = table.Column<string>(type: "text", nullable: true),
                    RESPONSE = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<string>(type: "text", nullable: true),
                    TRANSACTION_TIME = table.Column<string>(type: "text", nullable: true),
                    ORIGINATOR_INSTITUTION = table.Column<string>(type: "text", nullable: true),
                    ORIGINATOR = table.Column<string>(type: "text", nullable: true),
                    DESTINATION_INSTITUTION = table.Column<string>(type: "text", nullable: true),
                    DESTINATION_ACCOUNT_NAME = table.Column<string>(type: "text", nullable: true),
                    DESTINATION_ACCOUNT_NO = table.Column<string>(type: "text", nullable: true),
                    NARRATION = table.Column<string>(type: "text", nullable: true),
                    PAYMENT_REFERENCE = table.Column<string>(type: "text", nullable: true),
                    LAST_12_DIGITS_OF_SESSION_ID = table.Column<string>(type: "text", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsCredited = table.Column<bool>(type: "boolean", nullable: true),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoreBankingReference = table.Column<string>(type: "text", nullable: true),
                    TransactionExist = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InwardReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NipInwardBatches",
                columns: table => new
                {
                    BatchId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NipInwardBatches", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "NipOutwardBatches",
                columns: table => new
                {
                    BatchId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BatchId = table.Column<string>(type: "text", nullable: true),
                    CHANNEL = table.Column<string>(type: "text", nullable: true),
                    SESSION_ID = table.Column<string>(type: "text", nullable: true),
                    TRANSACTION_TYPE = table.Column<string>(type: "text", nullable: true),
                    RESPONSE = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<string>(type: "text", nullable: true),
                    TRANSACTION_TIME = table.Column<string>(type: "text", nullable: true),
                    ORIGINATOR_INSTITUTION = table.Column<string>(type: "text", nullable: true),
                    ORIGINATOR = table.Column<string>(type: "text", nullable: true),
                    DESTINATION_INSTITUTION = table.Column<string>(type: "text", nullable: true),
                    DESTINATION_ACCOUNT_NAME = table.Column<string>(type: "text", nullable: true),
                    DESTINATION_ACCOUNT_NO = table.Column<string>(type: "text", nullable: true),
                    NARRATION = table.Column<string>(type: "text", nullable: true),
                    PAYMENT_REFERENCE = table.Column<string>(type: "text", nullable: true),
                    LAST_12_DIGITS_OF_SESSION_ID = table.Column<string>(type: "text", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDebited = table.Column<bool>(type: "boolean", nullable: true),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoreBankingReference = table.Column<string>(type: "text", nullable: true),
                    TransactionExist = table.Column<bool>(type: "boolean", nullable: true)
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
