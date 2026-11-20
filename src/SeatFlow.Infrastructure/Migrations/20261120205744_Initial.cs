using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeatFlow.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261120205744_Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Venues",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Venues", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Events",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                StartsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                VenueId = table.Column<Guid>(type: "uuid", nullable: false),
                OrganizerId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
                table.ForeignKey(
                    name: "FK_Events_Venues_VenueId",
                    column: x => x.VenueId,
                    principalTable: "Venues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Sections",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                VenueId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sections", x => x.Id);
                table.ForeignKey(
                    name: "FK_Sections_Venues_VenueId",
                    column: x => x.VenueId,
                    principalTable: "Venues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                EventId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                Total = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.ForeignKey(
                    name: "FK_Orders_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "PriceCategories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                EventId = table.Column<Guid>(type: "uuid", nullable: false),
                SectionId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PriceCategories", x => x.Id);
                table.ForeignKey(
                    name: "FK_PriceCategories_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PriceCategories_Sections_SectionId",
                    column: x => x.SectionId,
                    principalTable: "Sections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Seats",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SectionId = table.Column<Guid>(type: "uuid", nullable: false),
                Row = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                Number = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Seats", x => x.Id);
                table.ForeignKey(
                    name: "FK_Seats_Sections_SectionId",
                    column: x => x.SectionId,
                    principalTable: "Sections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                Amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                IdempotencyKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Tickets",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                EventId = table.Column<Guid>(type: "uuid", nullable: false),
                SeatId = table.Column<Guid>(type: "uuid", nullable: false),
                Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CheckedInAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                IsActive = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tickets", x => x.Id);
                table.ForeignKey(
                    name: "FK_Tickets_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Tickets_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Tickets_Seats_SeatId",
                    column: x => x.SeatId,
                    principalTable: "Seats",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Events_VenueId",
            table: "Events",
            column: "VenueId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_EventId",
            table: "Orders",
            column: "EventId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_IdempotencyKey",
            table: "Payments",
            column: "IdempotencyKey",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Payments_OrderId",
            table: "Payments",
            column: "OrderId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceCategories_EventId_SectionId",
            table: "PriceCategories",
            columns: new[] { "EventId", "SectionId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PriceCategories_SectionId",
            table: "PriceCategories",
            column: "SectionId");

        migrationBuilder.CreateIndex(
            name: "IX_Seats_SectionId_Row_Number",
            table: "Seats",
            columns: new[] { "SectionId", "Row", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Sections_VenueId_Name",
            table: "Sections",
            columns: new[] { "VenueId", "Name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_Code",
            table: "Tickets",
            column: "Code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_EventId_SeatId",
            table: "Tickets",
            columns: new[] { "EventId", "SeatId" },
            unique: true,
            filter: "\"IsActive\"");

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_OrderId",
            table: "Tickets",
            column: "OrderId");

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_SeatId",
            table: "Tickets",
            column: "SeatId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Payments");

        migrationBuilder.DropTable(
            name: "PriceCategories");

        migrationBuilder.DropTable(
            name: "Tickets");

        migrationBuilder.DropTable(
            name: "Orders");

        migrationBuilder.DropTable(
            name: "Seats");

        migrationBuilder.DropTable(
            name: "Events");

        migrationBuilder.DropTable(
            name: "Sections");

        migrationBuilder.DropTable(
            name: "Venues");
    }
}
