
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Configuration
{
    public class ArtistTypeConfiguration : IEntityTypeConfiguration<ArtistType>
    {
        public void Configure(EntityTypeBuilder<ArtistType> builder)
        {
            builder.HasData
            (
                new ArtistType
                {
                    ID = -1,
                    Name = "Poet",
                    Description = "Handy with words"
                },

                new ArtistType
                {
                    ID = -2,
                    Name = "Painter",
                    Description = "Handy with a brush"
                },
                new ArtistType
                {
                    ID = -3,
                    Name = "Sculptor",
                    Description = "An artist who makes sculptures"
                }
            );
        }
    }
}