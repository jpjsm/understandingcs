using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LogsReceiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OPAController : ControllerBase
    {
        private const string default_opa_status_path = "/var/logs/OPA/status";
        private const string default_opa_decision_path = "/var/logs/OPA/decision";
        private const string valid_name_pattern = "^[A-Za-z][A-Za-z0-9_.-]{5,}[A-Za-z]$";
        private readonly ILogger<OPAController> _logger;

        public OPAController(ILogger<OPAController> logger)
        {
            _logger = logger;
        }

        [Route("status/{id}")]
        [HttpPost]
        public async Task<IActionResult> UploadOpaStatus(string id, IFormFile status_file)
        {
            if (string.IsNullOrWhiteSpace(id) || !Regex.IsMatch(id, valid_name_pattern))
                return BadRequest("Invalid file name.");

            try
            {
                string filepath = Path.Combine(default_opa_status_path, $"S{DateTime.UtcNow.Ticks}.{id}");
                using (var stream = System.IO.File.Create(filepath))
                {
                    await status_file.CopyToAsync(stream);
                }

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("decision/{id}")]
        [HttpPost]
        public async Task<IActionResult> UploadOpaDecision(string id, IFormFile decision_file)
        {
            if (string.IsNullOrWhiteSpace(id) || !Regex.IsMatch(id, valid_name_pattern))
                return BadRequest("Invalid file name.");

            try
            {
                string filepath = Path.Combine(default_opa_decision_path, $"S{DateTime.UtcNow.Ticks}.{id}");
                using (var stream = System.IO.File.Create(filepath))
                {
                    await decision_file.CopyToAsync(stream);
                }

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}