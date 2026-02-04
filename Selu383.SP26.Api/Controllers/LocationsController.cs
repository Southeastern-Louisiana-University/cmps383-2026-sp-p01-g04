using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Data;
using Selu383.SP26.Api.Dtos;
using Selu383.SP26.Api.Models;




namespace Selu383.SP26.Api.Controllers
{
	[Route("api/locations")]
	[ApiController]
	public class LocationsController : ControllerBase
	{
		private readonly DataContext _context;

		public LocationsController(DataContext context)
		{
			_context = context;
		}

		// GET: api/locations
		[HttpGet]
		public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
		{
			var locations = await _context.Locations.ToListAsync();
			var dtos = locations.Select(MapToDto).ToList();
			return Ok(dtos);
		}

		// GET: api/locations/5
		[HttpGet("{id}")]
		public async Task<ActionResult<LocationDto>> GetLocation(int id)
		{
			var location = await _context.Locations.FindAsync(id);

			if (location == null)
			{
				return NotFound();
			}

			return Ok(MapToDto(location));
		}

		// POST: api/locations
		[HttpPost]
		public async Task<ActionResult<LocationDto>> PostLocation(LocationDto dto)
		{
			if (!IsValid(dto, out var errorMessage))
			{
				return BadRequest(errorMessage);
			}

			var location = MapToEntity(dto);
			_context.Locations.Add(location);
			await _context.SaveChangesAsync();

			dto.Id = location.Id;
			return CreatedAtAction(nameof(GetLocation), new { id = dto.Id }, dto);
		}

		// PUT: api/locations/5
		[HttpPut("{id}")]
		public async Task<ActionResult<LocationDto>> PutLocation(int id, LocationDto dto)
		{
			if (id != dto.Id)
			{
				return BadRequest("Id mismatch");
			}

			if (!IsValid(dto, out var errorMessage))
			{
				return BadRequest(errorMessage);
			}

			var location = await _context.Locations.FindAsync(id);
			if (location == null)
			{
				return NotFound();
			}

			location.Name = dto.Name;
			location.Address = dto.Address;
			location.TableCount = dto.TableCount;

			_context.Entry(location).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return Ok(dto);
		}

		// DELETE: api/locations/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLocation(int id)
		{
			var location = await _context.Locations.FindAsync(id);
			if (location == null)
			{
				return NotFound();
			}

			_context.Locations.Remove(location);
			await _context.SaveChangesAsync();

			return Ok();
		}

		private static bool IsValid(LocationDto dto, out string errorMessage)
		{
			errorMessage = string.Empty;

			if (string.IsNullOrWhiteSpace(dto.Name))
			{
				errorMessage = "Name is required";
				return false;
			}

			if (dto.Name.Length > 120)
			{
				errorMessage = "Name cannot be longer than 120 characters";
				return false;
			}

			if (string.IsNullOrWhiteSpace(dto.Address))
			{
				errorMessage = "Address is required";
				return false;
			}

			if (dto.TableCount < 1)
			{
				errorMessage = "Must have at least 1 table";
				return false;
			}

			return true;
		}

		private static LocationDto MapToDto(Location location)
		{
			return new LocationDto
			{
				Id = location.Id,
				Name = location.Name,
				Address = location.Address,
				TableCount = location.TableCount
			};
		}

		private static Location MapToEntity(LocationDto dto)
		{
			return new Location
			{
				Name = dto.Name,
				Address = dto.Address,
				TableCount = dto.TableCount
			};
		}
	}
}