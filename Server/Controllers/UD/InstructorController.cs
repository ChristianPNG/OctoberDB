using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using static Duende.IdentityServer.Models.IdentityResources;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : BaseController
    {
        public InstructorController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{SchoolID}/{InstructorID}")]
        public async Task<IActionResult> Delete(int SchoolID, int InstructorID)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Instructors.Where(x => x.SchoolId == SchoolID).Where(x => x.InstructorId == InstructorID).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Instructors.Remove(itm);
                }
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Instructors.Select(sp => new InstructorDTO
                {
                    SchoolId = sp.SchoolId,
                    InstructorId = sp.InstructorId,
                    Salutation = sp.Salutation,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    StreetAddress = sp.StreetAddress,
                    Zip = sp.Zip,
                    Phone = sp.Phone,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                })
                .ToListAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpGet]
        [Route("Get/{SchoolID}/{InstructorID}")]
        public async Task<IActionResult> Get(int SchoolID, int InstructorID)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                InstructorDTO? result = await _context
                    .Instructors
                    .Where(x => x.InstructorId == InstructorID)
                    .Where(x => x.SchoolId == SchoolID)
                     .Select(sp => new InstructorDTO
                     {
                         SchoolId = sp.SchoolId,
                         InstructorId = sp.InstructorId,
                         Salutation = sp.Salutation,
                         FirstName = sp.FirstName,
                         LastName = sp.LastName,
                         StreetAddress = sp.StreetAddress,
                         Zip = sp.Zip,
                         Phone = sp.Phone,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate,
                     })
                .SingleOrDefaultAsync();

                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] InstructorDTO _InstructorDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Instructors.Where(x => x.SchoolId == _InstructorDTO.SchoolId).Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    Instructor c = new Instructor
                    {
                        SchoolId = _InstructorDTO.SchoolId,
                        InstructorId = _InstructorDTO.InstructorId,
                        Salutation = _InstructorDTO.Salutation,
                        FirstName = _InstructorDTO.FirstName,
                        LastName = _InstructorDTO.LastName,
                        StreetAddress = _InstructorDTO.StreetAddress,
                        Zip = _InstructorDTO.Zip,
                        Phone = _InstructorDTO.Phone,
                    };
                    _context.Instructors.Add(c);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] InstructorDTO _InstructorDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Instructors.Where(x => x.SchoolId == _InstructorDTO.SchoolId).Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();

                itm.Salutation = _InstructorDTO.Salutation;
                itm.FirstName = _InstructorDTO.FirstName;
                itm.LastName = _InstructorDTO.LastName;
                itm.StreetAddress = _InstructorDTO.StreetAddress;
                itm.Zip = _InstructorDTO.Zip;
                itm.Phone = _InstructorDTO.Phone;

                _context.Instructors.Update(itm);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
    }
}
