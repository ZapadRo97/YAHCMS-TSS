
using System.Collections.Generic;
using System.Threading.Tasks;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public interface IArtistRepository : IBaseRepository<Artist>
    {
        Task<List<ArtistType>> GetArtistTypesAsync();
    }
}