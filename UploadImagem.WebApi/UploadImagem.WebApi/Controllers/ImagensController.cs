using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadImagem.WebApi.DTO;
using UploadImagem.WebApi.Interfaces;
using UploadImagem.WebApi.Models;

namespace UploadImagem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagensController : ControllerBase
    {
        private readonly IImagemRepository _repository;
        private readonly IWebHostEnvironment _env;

        public ImagensController(IImagemRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImagemPorId(int id)
        {
            var imagem = await _repository.GetByIdAsync(id);
            if (imagem == null)
                return NotFound();

            return Ok(imagem);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasImagens()
        {
            var imagens = await _repository.GetAllAsync();
            return Ok(imagens);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadImagem([FromForm] UploadImagemDto dto)
        {
            if (dto.Arquivo == null || dto.Arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            var extensao = Path.GetExtension(dto.Arquivo.FileName);
            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";

            var pastaRelativa = "wwwroot/imagens";
            var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa);

            // Garante que a pasta exista
            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await dto.Arquivo.CopyToAsync(stream);
            }

            var imagem = new Imagen
            {
                Nome = dto.Nome,
                Caminho = Path.Combine(pastaRelativa, nomeArquivo).Replace("\\", "/")
            };

            await _repository.CreateAsync(imagem);

            return CreatedAtAction(nameof(GetImagemPorId), new { id = imagem.Id }, imagem);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarNomeImagem(int id, [FromBody] PutImagemDTO imagemAtualizada)
        {
            if (string.IsNullOrWhiteSpace(imagemAtualizada.novoNome))
                return BadRequest("O novo nome não pode ser vazio.");

            var imagem = await _repository.GetByIdAsync(id);
            if (imagem == null)
                return NotFound("Imagem não encontrada.");

            imagem.Nome = imagemAtualizada.novoNome;

            var atualizado = await _repository.UpdateAsync(imagem);
            if (!atualizado)
                return StatusCode(500, "Erro ao atualizar a imagem.");

            return Ok(imagem);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarImagem(int id)
        {
            var imagem = await _repository.GetByIdAsync(id);
            if (imagem == null)
                return NotFound("Imagem não encontrada.");

            // Caminho absoluto do arquivo
            var caminhoFisico = Path.Combine(Directory.GetCurrentDirectory(), imagem.Caminho.Replace("/", Path.DirectorySeparatorChar.ToString()));


            // Deleta o arquivo do disco, se existir
            if (System.IO.File.Exists(caminhoFisico))
            {
                try
                {
                    System.IO.File.Delete(caminhoFisico);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro ao excluir o arquivo: {ex.Message}");
                }
            }

            // Remove do banco
            var deletado = await _repository.DeleteAsync(id);
            if (!deletado)
                return StatusCode(500, "Erro ao excluir a imagem do banco.");

            return NoContent();
        }


    }
}
