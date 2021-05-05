﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TalktifAPI.Models
{
    public partial class TalktifContext : DbContext
    {
        public TalktifContext()
        {
        }

        public TalktifContext(DbContextOptions<TalktifContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatRoom> ChatRooms { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserChatRoom> UserChatRooms { get; set; }
        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-TPTMLAN; Initial Catalog=Talktif;User ID=Talktif; Password=vanhuuan89");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.Property(e => e.ChatRoomName).IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(d => d.ChatRoom)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__chatRoo__7F2BE32F");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.Sender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__sender__7E37BEF6");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasOne(d => d.ReporterNavigation)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.Reporter)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__reporter__73BA3083");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Gender).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((1))");

                entity.Property(e => e.Password).IsUnicode(false);
            });

            modelBuilder.Entity<UserChatRoom>(entity =>
            {
                entity.HasKey(e => new { e.User, e.ChatRoomId })
                    .HasName("PK__User_Cha__4372E63A933B1139");

                entity.HasOne(d => d.ChatRoom)
                    .WithMany(p => p.UserChatRooms)
                    .HasForeignKey(d => d.ChatRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Chat__chatR__7B5B524B");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.UserChatRooms)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_ChatR__user__7A672E12");
            });

            modelBuilder.Entity<UserRefreshToken>(entity =>
            {
                entity.Property(e => e.Device).IsUnicode(false);

                entity.Property(e => e.RefreshToken).IsUnicode(false);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.UserRefreshTokens)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Refre__user__628FA481");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}