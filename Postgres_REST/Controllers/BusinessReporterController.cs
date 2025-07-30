using Microsoft.AspNetCore.Mvc;
using postgres_rest.Model;

namespace postgres_rest.Controllers;

[ApiController]
[Route("api/v1/business-reporter")]
public class BusinessReporterController : ControllerBase
{
    private readonly ILogger<BusinessReporterController> _logger;
    private readonly BusinessReporterContext _context;

    public BusinessReporterController(ILogger<BusinessReporterController> logger, BusinessReporterContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet("issues")]
    public IEnumerable<Ticket> GetTickets()
    {
        try
        {
            return _context.Tickets;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetTickets");
            return null;
        }
    }


    [HttpGet("tickets")]
    public IEnumerable<Ticket> GetTickets()
    {
        try
        {
            return _context.Tickets;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetTickets");
            return null;
        }
    }
}
