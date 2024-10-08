﻿// <auto-generated />
using System;
using GalleryInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GalleryInfrastructure.Migrations
{
    [DbContext(typeof(GalleryContext))]
    partial class GalleryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GalleryDomain.Model.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Authors__3214EC0731712C60");

                    b.HasIndex("CountryId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("GalleryDomain.Model.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Countrie__3214EC075245FEBD");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("GalleryDomain.Model.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Link")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Location__3214EC07CB1234CE");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("GalleryDomain.Model.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id")
                        .HasName("PK__Photos__3214EC07A1CC4643");

                    b.HasIndex("AuthorId");

                    b.HasIndex("LocationId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("GalleryDomain.Model.Author", b =>
                {
                    b.HasOne("GalleryDomain.Model.Country", "Country")
                        .WithMany("Authors")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("FK__Authors__Country__398D8EEE");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("GalleryDomain.Model.Photo", b =>
                {
                    b.HasOne("GalleryDomain.Model.Author", "Author")
                        .WithMany("Photos")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Photos__AuthorId__403A8C7D");

                    b.HasOne("GalleryDomain.Model.Location", "Location")
                        .WithMany("Photos")
                        .HasForeignKey("LocationId")
                        .HasConstraintName("FK__Photos__Location__412EB0B6");

                    b.Navigation("Author");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("GalleryDomain.Model.Author", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("GalleryDomain.Model.Country", b =>
                {
                    b.Navigation("Authors");
                });

            modelBuilder.Entity("GalleryDomain.Model.Location", b =>
                {
                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
