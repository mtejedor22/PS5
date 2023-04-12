using DOOR.EF.Data;
using DOOR.EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using DOOR.Server.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Numerics;
using DOOR.Shared.DTO;
using DOOR.Shared.Utils;
using DOOR.Server.Controllers.Common;


namespace CSBA6.Server.Controllers.app
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionController : BaseController
    {
        public SectionController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetSection")]
        public async Task<IActionResult> GetSection()
        {
            List<SectionDTO> lst = await _context.Sections
                .Select(sp => new SectionDTO
                {
                    SectionId = sp.SectionId,
                    CourseNo = sp.CourseNo,
                    StartDateTime = sp.StartDateTime,
                    Location = sp.Location,
                    InstructorId = sp.InstructorId,
                    Capacity = sp.Capacity,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetSection/{_SectionId}")]
        public async Task<IActionResult> GetSection(int _SectionId)
        {
            SectionDTO? lst = await _context.Sections
                .Where(x => x.SectionId == _SectionId)
                .Select(sp => new SectionDTO
                {
                    SectionId = sp.SectionId,
                    CourseNo = sp.CourseNo,
                    StartDateTime = sp.StartDateTime,
                    Location = sp.Location,
                    InstructorId = sp.InstructorId,
                    Capacity = sp.Capacity,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostSection")]
        public async Task<IActionResult> PostSection([FromBody] SectionDTO _SectionDTO)
        {
            try
            {
                Section c = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId).FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new Section
                    {
                        StartDateTime = _SectionDTO.StartDateTime,
                        Location = _SectionDTO.Location,
                        Capacity = _SectionDTO.Capacity,
                        InstructorId = _SectionDTO.InstructorId
                    };
                    _context.Sections.Add(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }








        [HttpPut]
        [Route("PutSection")]
        public async Task<IActionResult> PutSection([FromBody] SectionDTO _SectionDTO)
        {
            try
            {
                Section c = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId).FirstOrDefaultAsync();

                if (c != null)
                {
                    c.StartDateTime = _SectionDTO.StartDateTime;
                    c.Location = _SectionDTO.Location;
                    c.Capacity = _SectionDTO.Capacity;
                    c.InstructorId = _SectionDTO.InstructorId;

                    _context.Sections.Update(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }


        [HttpDelete]
        [Route("DeleteSection/{_SectionId}")]
        public async Task<IActionResult> DeleteSection(int _SectionId)
        {
            try
            {
                Section c = await _context.Sections.Where(x => x.SectionId == _SectionId).FirstOrDefaultAsync();

                if (c != null)
                {
                    _context.Sections.Remove(c);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }



    }
}