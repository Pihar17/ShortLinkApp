using Microsoft.AspNetCore.Mvc;

namespace ShortLinkApp.Controllers
{
    [Route("")]
    [ApiController]  // Добавлено для лучшей интеграции с API
    public class HomeController : ControllerBase // Изменено на ControllerBase, так как он лучше подходит для API
    {
        /// <summary>
        /// Возвращает приветственное сообщение для корневого маршрута.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the Short Link App! Use the API to shorten your links.");
        }
    }
}

