
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(CulturalDbContext context) : base(context) {}
    }
}