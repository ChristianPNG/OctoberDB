using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using static System.Collections.Specialized.BitVector32;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : BaseController
    {
        public GradeController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{SchoolID}/{SectionID}/{StudentID}/{GradeTypeCode}/{GradeCodeOccurrence}")]
        public async Task<IActionResult> Delete(int SchoolID, int SectionID, int StudentID, string GradeTypeCode, int GradeCodeOccurrence)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Grades.Where(x => x.SchoolId == SchoolID)
                    .Where(x => x.SectionId == SectionID)
                    .Where(x => x.SchoolId == SchoolID)
                    .Where(x => x.StudentId == StudentID)
                    .Where(x => x.GradeTypeCode == GradeTypeCode)
                    .Where(x => x.GradeCodeOccurrence == GradeCodeOccurrence)
                    .FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Grades.Remove(itm);
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

                var result = await _context.Grades.Select(sp => new GradeDTO
                {
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    NumericGrade = sp.NumericGrade,
                    Comments = sp.Comments,
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
        [Route("Get/{SchoolID}/{SectionID}/{StudentID}/{GradeTypeCode}/{GradeCodeOccurrence}")]
        public async Task<IActionResult> Get(int SchoolID, int SectionID, int StudentID, string GradeTypeCode, int GradeCodeOccurrence)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeDTO? result = await _context
                    .Grades
                    .Where(x => x.SectionId == SectionID)
                    .Where(x => x.SchoolId == SchoolID)
                    .Where(x => x.StudentId == StudentID)
                    .Where(x => x.GradeTypeCode == GradeTypeCode)
                    .Where(x => x.GradeCodeOccurrence == GradeCodeOccurrence)
                     .Select(sp => new GradeDTO
                     {
                         SchoolId = sp.SchoolId,
                         StudentId = sp.StudentId,
                         SectionId = sp.SectionId,
                         GradeTypeCode = sp.GradeTypeCode,
                         GradeCodeOccurrence = sp.GradeCodeOccurrence,
                         NumericGrade = sp.NumericGrade,
                         Comments = sp.Comments,
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
        public async Task<IActionResult> Post([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var itm = await _context.Grades
                    .Where(x => x.SectionId == _GradeDTO.SectionId)
                    .Where(x => x.SchoolId == _GradeDTO.SchoolId)
                    .Where(x => x.StudentId == _GradeDTO.StudentId)
                    .Where(x => x.GradeTypeCode == _GradeDTO.GradeTypeCode)
                    .Where(x => x.GradeCodeOccurrence == _GradeDTO.GradeCodeOccurrence)
                    .FirstOrDefaultAsync();

                if (itm == null)
                {
                    Grade c = new Grade
                    {
                        SchoolId = _GradeDTO.SchoolId,
                        StudentId = _GradeDTO.StudentId,
                        SectionId = _GradeDTO.SectionId,
                        GradeTypeCode = _GradeDTO.GradeTypeCode,
                        GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence,
                        NumericGrade = _GradeDTO.NumericGrade,
                        Comments = _GradeDTO.Comments,
                    };
                    _context.Grades.Add(c);
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
        public async Task<IActionResult> Put([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Grades
                    .Where(x => x.NumericGrade == _GradeDTO.NumericGrade)
                    .Where(x => x.Comments == _GradeDTO.Comments)
                    .FirstOrDefaultAsync();

                itm.NumericGrade = _GradeDTO.NumericGrade;
                itm.Comments = _GradeDTO.Comments;

                _context.Grades.Update(itm);
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
