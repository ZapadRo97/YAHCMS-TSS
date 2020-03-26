
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {

        public ArtistRepository(CulturalDbContext context) : base(context) {}

        public async Task<List<ArtistType>> GetArtistTypesAsync()
        {
            return await _context.Set<ArtistType>().ToListAsync();
        }
    }
}