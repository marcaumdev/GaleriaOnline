using Microsoft.EntityFrameworkCore;
using UploadImagem.WebApi.DbContextImagem;
using UploadImagem.WebApi.Interfaces;
using UploadImagem.WebApi.Models;

namespace UploadImagem.WebApi.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private readonly UploadImagemDbContext _context;

        public ImagemRepository(UploadImagemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Imagen>> GetAllAsync()
        {
            return await _context.Imagens.ToListAsync();
        }

        public async Task<Imagen?> GetByIdAsync(int id)
        {
            return await _context.Imagens.FindAsync(id);
        }

        public async Task<Imagen> CreateAsync(Imagen imagem)
        {
            _context.Imagens.Add(imagem);
            await _context.SaveChangesAsync();
            return imagem;
        }

        public async Task<bool> UpdateAsync(Imagen imagem)
        {
            _context.Imagens.Update(imagem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var imagem = await _context.Imagens.FindAsync(id);
            if (imagem == null)
                return false;

            _context.Imagens.Remove(imagem);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
