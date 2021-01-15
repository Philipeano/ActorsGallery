using ActorsGallery.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.Data.MySqlClient;

namespace ActorsGallery.Data.MySqlDataService
{
    public class ActorsGalleryContext : DbContext
    {
        public ActorsGalleryContext(DbContextOptions<ActorsGalleryContext> options)
            : base(options)
        {

        }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Episode> Episodes { get; set; }

        public DbSet<EpisodeCharacter> EpisodeCharacters { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Location> Locations { get; set; }


        // Configure Entity Relationships

        // A Character can have only one Location
        // A Location can have many Characters
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.HasOne(character => character.Location)
                .WithMany(location => location.Characters)
                .HasForeignKey(character => character.LocationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        
        // An Episode can feature many Characters
        // A Character can feature in many Episodes
        public void Configure(EntityTypeBuilder<EpisodeCharacter> builder)
        {
            builder.HasKey(role => new { role.EpisodeId, role.CharacterId });

            builder.HasOne(role => role.Episode)
                .WithMany(episode => episode.EpisodeCharacters)
                .HasForeignKey(role => role.EpisodeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(role => role.Character)
                .WithMany(character => character.EpisodesFeaturedIn)
                .HasForeignKey(role => role.CharacterId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        // A Comment must target one Episode
        // An Episode can have many Comments
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(comment => comment.Episode)
                .WithMany(episode => episode.EpisodeComments)
                .HasForeignKey(comment => comment.EpisodeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
