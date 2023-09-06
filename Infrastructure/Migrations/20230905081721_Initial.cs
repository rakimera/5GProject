using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Antennas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    VerticalSizeDiameter = table.Column<decimal>(type: "numeric", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Antennas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CounterAgents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    BIN = table.Column<string>(type: "text", nullable: false),
                    DirectorName = table.Column<string>(type: "text", nullable: false),
                    DirectorSurname = table.Column<string>(type: "text", nullable: false),
                    DirectorPatronymic = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterAgents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistrictName = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExecutiveCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    LicenseNumber = table.Column<string>(type: "text", nullable: true),
                    LicenseDateOfIssue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BIN = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    TownName = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutiveCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanPinDocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    DateOfIssue = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPinDocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslatorType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslatorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslatorsSpecs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Frequency = table.Column<decimal>(type: "numeric", nullable: false),
                    Power = table.Column<decimal>(type: "numeric", nullable: false),
                    Gain = table.Column<decimal>(type: "numeric", nullable: false),
                    AntennaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslatorsSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslatorsSpecs_Antennas_AntennaId",
                        column: x => x.AntennaId,
                        principalTable: "Antennas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TownName = table.Column<string>(type: "text", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Towns_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ExecutiveCompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ExecutiveCompanies_ExecutiveCompanyId",
                        column: x => x.ExecutiveCompanyId,
                        principalTable: "ExecutiveCompanies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RadiationZones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Degree = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    DirectionType = table.Column<int>(type: "integer", nullable: false),
                    TranslatorSpecsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiationZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadiationZones_TranslatorsSpecs_TranslatorSpecsId",
                        column: x => x.TranslatorSpecsId,
                        principalTable: "TranslatorsSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AntennaTranslators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TranslatorSpecsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Power = table.Column<decimal>(type: "numeric", nullable: false),
                    TransmitLossFactor = table.Column<decimal>(type: "numeric", nullable: false),
                    TranslatorTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Gain = table.Column<decimal>(type: "numeric", nullable: false),
                    ProjectAntennaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntennaTranslators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntennaTranslators_TranslatorType_TranslatorTypeId",
                        column: x => x.TranslatorTypeId,
                        principalTable: "TranslatorType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AntennaTranslators_TranslatorsSpecs_TranslatorSpecsId",
                        column: x => x.TranslatorSpecsId,
                        principalTable: "TranslatorsSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BiohazardRadii",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Degree = table.Column<int>(type: "integer", nullable: false),
                    MaximumBiohazardRadius = table.Column<decimal>(type: "numeric", nullable: false),
                    BiohazardRadiusZ = table.Column<decimal>(type: "numeric", nullable: false),
                    BiohazardRadiusX = table.Column<decimal>(type: "numeric", nullable: false),
                    DirectionType = table.Column<int>(type: "integer", nullable: false),
                    AntennaTranslatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiohazardRadii", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiohazardRadii_AntennaTranslators_AntennaTranslatorId",
                        column: x => x.AntennaTranslatorId,
                        principalTable: "AntennaTranslators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnergyResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Distance = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    AntennaTranslatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergyResults_AntennaTranslators_AntennaTranslatorId",
                        column: x => x.AntennaTranslatorId,
                        principalTable: "AntennaTranslators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummaryBiohazardRadii",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Degree = table.Column<int>(type: "integer", nullable: false),
                    MaximumBiohazardRadius = table.Column<decimal>(type: "numeric", nullable: false),
                    BiohazardRadiusZ = table.Column<decimal>(type: "numeric", nullable: false),
                    BiohazardRadiusX = table.Column<decimal>(type: "numeric", nullable: false),
                    DirectionType = table.Column<int>(type: "integer", nullable: false),
                    AntennaTranslatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummaryBiohazardRadii", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummaryBiohazardRadii_AntennaTranslators_AntennaTranslatorId",
                        column: x => x.AntennaTranslatorId,
                        principalTable: "AntennaTranslators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectNumber = table.Column<string>(type: "text", nullable: false),
                    ContrAgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutiveCompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    SanPinDockId = table.Column<Guid>(type: "uuid", nullable: true),
                    YearOfInitial = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ProjectStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    SummaryBiohazardRadiusId = table.Column<Guid>(type: "uuid", nullable: true),
                    DistrictName = table.Column<string>(type: "text", nullable: false),
                    TownName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_CounterAgents_ContrAgentId",
                        column: x => x.ContrAgentId,
                        principalTable: "CounterAgents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_ExecutiveCompanies_ExecutiveCompanyId",
                        column: x => x.ExecutiveCompanyId,
                        principalTable: "ExecutiveCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_ProjectsStatuses_ProjectStatusId",
                        column: x => x.ProjectStatusId,
                        principalTable: "ProjectsStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_SanPinDocks_SanPinDockId",
                        column: x => x.SanPinDockId,
                        principalTable: "SanPinDocks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_SummaryBiohazardRadii_SummaryBiohazardRadiusId",
                        column: x => x.SummaryBiohazardRadiusId,
                        principalTable: "SummaryBiohazardRadii",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_Users_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsAntennae",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Azimuth = table.Column<decimal>(type: "numeric", nullable: false),
                    Height = table.Column<decimal>(type: "numeric", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Tilt = table.Column<decimal>(type: "numeric", nullable: false),
                    AntennaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsAntennae", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectsAntennae_Antennas_AntennaId",
                        column: x => x.AntennaId,
                        principalTable: "Antennas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsAntennae_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFluxDensities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Distance = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFluxDensities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFluxDensities_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntennaTranslators_ProjectAntennaId",
                table: "AntennaTranslators",
                column: "ProjectAntennaId");

            migrationBuilder.CreateIndex(
                name: "IX_AntennaTranslators_TranslatorSpecsId",
                table: "AntennaTranslators",
                column: "TranslatorSpecsId");

            migrationBuilder.CreateIndex(
                name: "IX_AntennaTranslators_TranslatorTypeId",
                table: "AntennaTranslators",
                column: "TranslatorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BiohazardRadii_AntennaTranslatorId",
                table: "BiohazardRadii",
                column: "AntennaTranslatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EnergyResults_AntennaTranslatorId",
                table: "EnergyResults",
                column: "AntennaTranslatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ContrAgentId",
                table: "Projects",
                column: "ContrAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ExecutiveCompanyId",
                table: "Projects",
                column: "ExecutiveCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ExecutorId",
                table: "Projects",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SanPinDockId",
                table: "Projects",
                column: "SanPinDockId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SummaryBiohazardRadiusId",
                table: "Projects",
                column: "SummaryBiohazardRadiusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsAntennae_AntennaId",
                table: "ProjectsAntennae",
                column: "AntennaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsAntennae_ProjectId",
                table: "ProjectsAntennae",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiationZones_TranslatorSpecsId",
                table: "RadiationZones",
                column: "TranslatorSpecsId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SummaryBiohazardRadii_AntennaTranslatorId",
                table: "SummaryBiohazardRadii",
                column: "AntennaTranslatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalFluxDensities_ProjectId",
                table: "TotalFluxDensities",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Towns_DistrictId",
                table: "Towns",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslatorsSpecs_AntennaId",
                table: "TranslatorsSpecs",
                column: "AntennaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExecutiveCompanyId",
                table: "Users",
                column: "ExecutiveCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AntennaTranslators_ProjectsAntennae_ProjectAntennaId",
                table: "AntennaTranslators",
                column: "ProjectAntennaId",
                principalTable: "ProjectsAntennae",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntennaTranslators_ProjectsAntennae_ProjectAntennaId",
                table: "AntennaTranslators");

            migrationBuilder.DropTable(
                name: "BiohazardRadii");

            migrationBuilder.DropTable(
                name: "EnergyResults");

            migrationBuilder.DropTable(
                name: "RadiationZones");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TotalFluxDensities");

            migrationBuilder.DropTable(
                name: "Towns");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ProjectsAntennae");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "CounterAgents");

            migrationBuilder.DropTable(
                name: "ProjectsStatuses");

            migrationBuilder.DropTable(
                name: "SanPinDocks");

            migrationBuilder.DropTable(
                name: "SummaryBiohazardRadii");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AntennaTranslators");

            migrationBuilder.DropTable(
                name: "ExecutiveCompanies");

            migrationBuilder.DropTable(
                name: "TranslatorType");

            migrationBuilder.DropTable(
                name: "TranslatorsSpecs");

            migrationBuilder.DropTable(
                name: "Antennas");
        }
    }
}
