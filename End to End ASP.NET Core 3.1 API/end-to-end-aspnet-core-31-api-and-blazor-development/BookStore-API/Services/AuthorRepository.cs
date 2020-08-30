using BookStore_API.Contracts;
using BookStore_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        readonly ApplicationDbContext _context;
        public AuthorRepository(ApplicationDbContext db)
        {
            _context = db;
        }
        public async Task<bool> Create(Author entity)
        {
            await _context.Authors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {

            _context.Authors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Author>> FindAll()
        {
            var authors = await _context.Authors.Include(e => e.Books).ToListAsync();
            return authors;
        }

        public async Task<Author> FindById(int id)
        {

            var author = await _context.Authors.Include(e => e.Books).FirstOrDefaultAsync(e => e.Id == id);
            return author;
        }

        public async Task<bool> IsExists(int id)
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Author entity)
        {

            _context.Authors.Update(entity);
            return await Save();
        }
    }
}
