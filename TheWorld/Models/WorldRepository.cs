using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;

        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetUserTripByName(tripName, username);

            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all trips from the database");
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetTripByUserName(string name) {
            return _context.Trips
               .Include(t => t.Stops)
               .Where(x => x.UserName == name).ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(x => x.Name == tripName);
                }

        public async Task<bool> SaveChangesAsync()
        {
            return ( await _context.SaveChangesAsync()) > 0;
        }

        public Trip GetUserTripByName(string tripName, string username)
        {
            return _context.Trips
                 .Include(t => t.Stops)
                 .FirstOrDefault(x => x.Name == tripName && x.UserName == username);
        }
    }
}
