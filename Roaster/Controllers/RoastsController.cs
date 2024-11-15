using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roaster.Controllers.Abstractions;
using Roaster.Infrastructure.Persistence;
using Roaster.Infrastructure.Persistence.Models;
using Roaster.Requests;
using Roaster.Responses;

namespace Roaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoastsController : CustomApiController
    {
        private readonly ApplicationDbContext _context;

        public RoastsController(IDataProtectionProvider provider, ApplicationDbContext context) : base(provider)
        {
            _context = context;
        }

        [HttpPost("/", Name = "CreateRoast")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateRoastRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var roast = Roast.Create(request.Name);
            await _context.AddAsync(roast, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var protectedId = ProtectId(roast.Id);

            return Created($"/api/roast/{protectedId}", null);
        }

        [HttpGet("/id", Name = "GetRoastById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] string id, CancellationToken cancellationToken = default)
        {
            var unprotectedKey = UnprotectId(id);
            var roast = await _context.FindAsync<Roast>(unprotectedKey);

            return roast is null ? NotFound() : Ok(new GetRoastByIdResponse(ProtectId(roast.Id), roast.Name));
        }

        [HttpGet("/", Name = "GetAllRoasts")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var roasts = await _context.Roasts.ToListAsync(cancellationToken);

            return !roasts.Any() ? NoContent() : Ok(new GetAllRoastsResponse(roasts.Select(r => new SingleRoast(ProtectId(r.Id), r.Name))));
        }
    }
}
