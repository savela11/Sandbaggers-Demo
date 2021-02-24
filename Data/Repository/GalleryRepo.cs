using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace Data.Repository
{
    public class GalleryRepo : Repository<Gallery>,  IGalleryRepo
    {
        private readonly AppDbContext _context;

        public GalleryRepo(AppDbContext db) : base(db)
        {
            _context = db;
        }


    }
}
