using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace video_service.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "channelModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChannelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channelModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_channelModels_userModels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "userModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "videoModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<double>(type: "float", nullable: false),
                    ChannelModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videoModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_videoModels_channelModels_ChannelModelId",
                        column: x => x.ChannelModelId,
                        principalTable: "channelModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_videoModels_userModels_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "userModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    VideoModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentModel_userModels_UserId",
                        column: x => x.UserId,
                        principalTable: "userModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentModel_videoModels_VideoModelId",
                        column: x => x.VideoModelId,
                        principalTable: "videoModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QualityVotesModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoModelId = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<double>(type: "float", nullable: false),
                    VideoModelId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityVotesModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityVotesModel_videoModels_VideoModelId1",
                        column: x => x.VideoModelId1,
                        principalTable: "videoModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReplyModel",
                columns: table => new
                {
                    ReplyModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    CommentModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplyModel", x => x.ReplyModelId);
                    table.ForeignKey(
                        name: "FK_ReplyModel_CommentModel_CommentModelId",
                        column: x => x.CommentModelId,
                        principalTable: "CommentModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReplyModel_userModels_UserId",
                        column: x => x.UserId,
                        principalTable: "userModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikeModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VideoModelID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReplyModelId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikeModel_CommentModel_CommentModelId",
                        column: x => x.CommentModelId,
                        principalTable: "CommentModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeModel_ReplyModel_ReplyModelId",
                        column: x => x.ReplyModelId,
                        principalTable: "ReplyModel",
                        principalColumn: "ReplyModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeModel_userModels_UserId",
                        column: x => x.UserId,
                        principalTable: "userModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikeModel_videoModels_VideoModelID",
                        column: x => x.VideoModelID,
                        principalTable: "videoModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_channelModels_ChannelId",
                table: "channelModels",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_UserId",
                table: "CommentModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_VideoModelId",
                table: "CommentModel",
                column: "VideoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeModel_CommentModelId",
                table: "LikeModel",
                column: "CommentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeModel_ReplyModelId",
                table: "LikeModel",
                column: "ReplyModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeModel_UserId",
                table: "LikeModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeModel_VideoModelID",
                table: "LikeModel",
                column: "VideoModelID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityVotesModel_VideoModelId1",
                table: "QualityVotesModel",
                column: "VideoModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyModel_CommentModelId",
                table: "ReplyModel",
                column: "CommentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyModel_UserId",
                table: "ReplyModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_videoModels_AuthorId",
                table: "videoModels",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_videoModels_ChannelModelId",
                table: "videoModels",
                column: "ChannelModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeModel");

            migrationBuilder.DropTable(
                name: "QualityVotesModel");

            migrationBuilder.DropTable(
                name: "ReplyModel");

            migrationBuilder.DropTable(
                name: "CommentModel");

            migrationBuilder.DropTable(
                name: "videoModels");

            migrationBuilder.DropTable(
                name: "channelModels");

            migrationBuilder.DropTable(
                name: "userModels");
        }
    }
}
