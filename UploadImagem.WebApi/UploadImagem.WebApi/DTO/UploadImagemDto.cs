using Microsoft.AspNetCore.Http;
public class UploadImagemDto
{
    public IFormFile Arquivo { get; set; }
    public string Nome { get; set; }
}