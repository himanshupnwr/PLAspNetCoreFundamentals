
using Microsoft.EntityFrameworkCore;
using System;

namespace PLAspNetCoreFundamentals.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly PieShopDbContext _context;

        public PieRepository(PieShopDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Pie> AllPies => _context.Pies.Include(c=>c.Category).ToList();

        public IEnumerable<Pie> PiesOfTheWeek => _context.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);

        public Pie? GetPieById(int pieId)
        {
            return _context.Pies.FirstOrDefault(p => p.PieId == pieId);
        }

        public IEnumerable<Pie> SearchPies(string searchQuery)
        {
            return _context.Pies.Where(p=>p.Name.Contains(searchQuery));
        }
    }
}
