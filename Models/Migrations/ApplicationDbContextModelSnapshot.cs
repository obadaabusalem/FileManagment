// <auto-generated />
using System;
using FileManagment.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Models.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FileManagment.Model.Files", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("FileData")
                        .IsRequired()
                        .HasColumnType("varbinary(MAX)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("FoldersID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("FoldersID");

                    b.ToTable("TblFiles");
                });

            modelBuilder.Entity("FileManagment.Model.Folders", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FolderPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("TblFolders");
                });

            modelBuilder.Entity("FileManagment.Model.Files", b =>
                {
                    b.HasOne("FileManagment.Model.Folders", null)
                        .WithMany("file")
                        .HasForeignKey("FoldersID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
