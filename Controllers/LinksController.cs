using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShortLinkApp.Data;
using System;
using System.Threading.Tasks;

namespace ShortLinkApp.Controllers
{
    [ApiController]
    [Route("api/url")]
    public class UrlController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UrlController> _logger;
        private readonly string _baseUrl;

        public UrlController(ApplicationDbContext context, ILogger<UrlController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _baseUrl = configuration["BaseUrl"] ?? "http://localhost:5056";
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlRequest request)
        {
            if (request == null || !Uri.IsWellFormedUriString(request.Url, UriKind.Absolute))
            {
                return BadRequest(new { error = "Invalid URL format" });
            }

            string shortCode = GenerateShortCode();
            string shortUrl = $"{_baseUrl}/api/url/s/{shortCode}";

            var urlEntry = new UrlEntry
            {
                originalUrl = request.Url,
                shortCode = shortCode,
                Created = DateTime.UtcNow,
                Clicks = 0
            };

            _context.UrlEntries.Add(urlEntry);
            await _context.SaveChangesAsync();

            return Ok(new { shortUrl });
        }

        [HttpGet("s/{shortCode}")]
        public async Task<IActionResult> RedirectToUrl(string shortCode)
        {
            var urlEntry = await _context.UrlEntries.FirstOrDefaultAsync(x => x.shortCode == shortCode);

            if (urlEntry == null)
            {
                return NotFound();
            }

            urlEntry.Clicks++;
            await _context.SaveChangesAsync();

            // Вместо простого редиректа, возвращаем JSON с обновлёнными данными
            return Ok(new { originalUrl = urlEntry.originalUrl, clicks = urlEntry.Clicks });
        }


        [HttpGet("stats/{shortCode}")]
        public async Task<IActionResult> GetUrlStats(string shortCode)
        {
            var urlEntry = await _context.UrlEntries.FirstOrDefaultAsync(x => x.shortCode == shortCode);

            if (urlEntry == null)
            {
                return NotFound();
            }

            return Ok(new { clicks = urlEntry.Clicks });
        }

        // Добавьте этот метод для загрузки всех URL
        [HttpGet]
        public async Task<IActionResult> GetAllUrls()
        {
            var urlEntries = await _context.UrlEntries.ToListAsync();
            var result = urlEntries.Select(x => new 
            {
                shortUrl = $"{_baseUrl}/api/url/s/{x.shortCode}",
                originalUrl = x.originalUrl,
                clicks = x.Clicks
            });

            return Ok(result);
        }

        private string GenerateShortCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }
    }
}
