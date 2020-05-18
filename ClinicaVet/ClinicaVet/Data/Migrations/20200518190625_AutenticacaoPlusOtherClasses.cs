using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicaVet.Data.Migrations
{
    public partial class AutenticacaoPlusOtherClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Sexo = table.Column<string>(nullable: true),
                    NIF = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Veterinarios",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(maxLength: 40, nullable: false),
                    NumCedulaProf = table.Column<string>(maxLength: 10, nullable: false),
                    Foto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinarios", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Animais",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Especie = table.Column<string>(nullable: true),
                    Peso = table.Column<float>(nullable: false),
                    Raca = table.Column<string>(nullable: true),
                    Foto = table.Column<string>(nullable: true),
                    DonoFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animais", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Animais_Donos_DonoFK",
                        column: x => x.DonoFK,
                        principalTable: "Donos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(nullable: false),
                    Observacoes = table.Column<string>(nullable: true),
                    VeterinarioFK = table.Column<int>(nullable: false),
                    AnimalFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Consultas_Animais_AnimalFK",
                        column: x => x.AnimalFK,
                        principalTable: "Animais",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultas_Veterinarios_VeterinarioFK",
                        column: x => x.VeterinarioFK,
                        principalTable: "Veterinarios",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Donos",
                columns: new[] { "ID", "NIF", "Nome", "Sexo" },
                values: new object[,]
                {
                    { 1, "813635582", "Luís Freitas", "M" },
                    { 2, "854613462", "Andreia Gomes", "F" },
                    { 3, "265368715", "Cristina Sousa", "F" },
                    { 4, "835623190", "Sónia Rosa", "F" },
                    { 5, "751512205", "António Santos", "M" },
                    { 6, "728663835", "Gustavo Alves", "M" },
                    { 7, "644388118", "Rosa Vieira", "F" },
                    { 8, "262618487", "Daniel Dias", "M" },
                    { 9, "842615197", "Tânia Gomes", "F" },
                    { 10, "635139506", "Andreia Correia", "F" }
                });

            migrationBuilder.InsertData(
                table: "Veterinarios",
                columns: new[] { "ID", "Foto", "Nome", "NumCedulaProf" },
                values: new object[,]
                {
                    { 1, "Maria.jpg", "Maria Pinto", "vet-034589" },
                    { 2, "Ricardo.jpg", "Ricardo Ribeiro", "vet-034590" },
                    { 3, "Jose.jpg", "José Soares", "vet-056732" }
                });

            migrationBuilder.InsertData(
                table: "Animais",
                columns: new[] { "ID", "DonoFK", "Especie", "Foto", "Nome", "Peso", "Raca" },
                values: new object[,]
                {
                    { 1, 1, "Cão", "animal1.jpg", "Bubi", 24f, "Pastor Alemão" },
                    { 10, 1, "Gato", "animal10.jpg", "Saltitão", 2f, "Persa" },
                    { 3, 2, "Cão", "animal3.jpg", "Tripé", 4f, "Serra Estrela" },
                    { 2, 3, "Cão", "animal2.jpg", "Pastor", 50f, "Serra Estrela" },
                    { 4, 3, "Cavalo", "animal4.jpg", "Saltador", 580f, "Lusitana" },
                    { 5, 3, "Gato", "animal5.jpg", "Tareco", 1f, "siamês" },
                    { 8, 7, "Cão", "animal8.jpg", "Forte", 20f, "Rottweiler" },
                    { 9, 8, "Vaca", "animal9.jpg", "Castanho", 652f, "Mirandesa" },
                    { 6, 9, "Cão", "animal6.jpg", "Cusca", 45f, "Labrador" },
                    { 7, 10, "Cão", "animal7.jpg", "Morde Tudo", 39f, "Dobermann" }
                });

            migrationBuilder.InsertData(
                table: "Consultas",
                columns: new[] { "ID", "AnimalFK", "Data", "Observacoes", "VeterinarioFK" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2020, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 16, 6, new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 7, 6, new DateTime(2020, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 },
                    { 17, 9, new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2 },
                    { 10, 9, new DateTime(2020, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2 },
                    { 5, 9, new DateTime(2020, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2 },
                    { 9, 8, new DateTime(2020, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 },
                    { 20, 5, new DateTime(2020, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 15, 5, new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 6, 5, new DateTime(2020, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 },
                    { 4, 4, new DateTime(2020, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2 },
                    { 14, 2, new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 3, 2, new DateTime(2020, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 2, 3, new DateTime(2020, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 19, 10, new DateTime(2020, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 11, 10, new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 18, 1, new DateTime(2020, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 },
                    { 13, 1, new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 },
                    { 8, 7, new DateTime(2020, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 },
                    { 12, 7, new DateTime(2020, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animais_DonoFK",
                table: "Animais",
                column: "DonoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_AnimalFK",
                table: "Consultas",
                column: "AnimalFK");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_VeterinarioFK",
                table: "Consultas",
                column: "VeterinarioFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Animais");

            migrationBuilder.DropTable(
                name: "Veterinarios");

            migrationBuilder.DropTable(
                name: "Donos");
        }
    }
}
