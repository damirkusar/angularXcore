﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ResourceTemplate.Data;
using System;

namespace ResourceTemplate.Data.Migrations
{
    [DbContext(typeof(ResourceTemplateDbContext))]
    [Migration("20171229191251_InitResourceDbContext")]
    partial class InitResourceDbContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ResourceTemplate.Data.Models.Resource", b =>
                {
                    b.Property<Guid>("ResourceId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new Guid("db6fe248-5896-49b0-b121-b77e42a1e94f"));

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("ResourceId");

                    b.ToTable("Resources");
                });
#pragma warning restore 612, 618
        }
    }
}
