using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using System.Diagnostics;
using Telerik.Blazor.Components;
using Telerik.DataSource.Extensions;
using Telerik.SvgIcons;
using static System.Collections.Specialized.BitVector32;
using Section = OCTOBER.EF.Models.Section;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : BaseController
    {

        public SectionController(OCTOBEROracleContext context,
                               IHttpContextAccessor httpContextAccessor,
                               IMemoryCache memoryCache)
               : base(context, httpContextAccessor) { }


        [HttpDelete]
        [Route("Delete/{SectionID}")]
        public async Task<IActionResult> Delete(int SectionID)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == SectionID).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Sections.Remove(itm);
                }
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
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

                var result = await _context.Sections.Select(sp => new SectionDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Capacity = sp.Capacity,
                    InstructorId = sp.InstructorId,
                    Location = sp.Location,
                    StartDateTime = sp.StartDateTime,
                    SectionNo = sp.SectionNo,
                    CourseNo = sp.CourseNo,
                    SectionId = sp.SectionId
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
            [Route("Get/{SchoolID}/{SectionID}")]
            public async Task<IActionResult> Get(int SchoolID, int SectionID)
            {
                try
                {
                    await _context.Database.BeginTransactionAsync();

                    SectionDTO? result = await _context
                        .Sections
                        .Where(x => x.SectionId == SectionID)
                        .Where(x => x.SchoolId == SchoolID)
                         .Select(sp => new SectionDTO
                         {
                             CreatedBy = sp.CreatedBy,
                             CreatedDate = sp.CreatedDate,
                             ModifiedBy = sp.ModifiedBy,
                             ModifiedDate = sp.ModifiedDate,
                             Capacity = sp.Capacity,
                             InstructorId = sp.InstructorId,
                             Location = sp.Location,
                             StartDateTime = sp.StartDateTime,
                             SectionNo = sp.SectionNo,
                             CourseNo = sp.CourseNo,
                             SectionId = sp.SectionId
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
            public async Task<IActionResult> Post([FromBody] SectionDTO _T)
            {
                try
                {
                    await _context.Database.BeginTransactionAsync();

                    var itm = await _context.Sections.Where(x => x.SectionId == _T.SectionId).FirstOrDefaultAsync();

                    if (itm == null)
                    {
                        Section c = new Section
                        {
                            CreatedBy = _T.CreatedBy,
                            CreatedDate = _T.CreatedDate,
                            ModifiedBy = _T.ModifiedBy,
                            ModifiedDate = _T.ModifiedDate,
                            Capacity = _T.Capacity,
                            InstructorId = _T.InstructorId,
                            Location = _T.Location,
                            StartDateTime = _T.StartDateTime,
                            SectionNo = _T.SectionNo,
                            CourseNo = _T.CourseNo,
                            SectionId = _T.SectionId
                        };
                        _context.Sections.Add(c);
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

            public async Task<IActionResult> Put([FromBody] SectionDTO _T)
            {
                try
                {
                    await _context.Database.BeginTransactionAsync();

                    var itm = await _context.Sections.Where(x => x.SectionId == _T.SectionId).FirstOrDefaultAsync();

                    itm.Capacity = _T.Capacity;
                    itm.InstructorId = _T.InstructorId;
                    itm.Location = _T.Location;
                    itm.StartDateTime = _T.StartDateTime;

                    _context.Sections.Update(itm);
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
