﻿using DOOR.EF.Data;
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
    public class GradeConversionController : BaseController
    {
        public GradeConversionController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetGradeConversion")]
        public async Task<IActionResult> GetGradeConversion()
        {
            List<GradeConversionDTO> lst = await _context.GradeConversions
                .Select(sp => new GradeConversionDTO
                {
                    SchoolId = sp.SchoolId,
                    LetterGrade = sp.LetterGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    GradePoint = sp.GradePoint,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    MaxGrade = sp.MaxGrade,
                    MinGrade = sp.MinGrade
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetGradeConversion/{_SchoolId}")]
        public async Task<IActionResult> GetGradeConversion(int _SchoolId)
        {
            GradeConversionDTO? lst = await _context.GradeConversions
                .Where(x => x.SchoolId == _SchoolId)
                .Select(sp => new GradeConversionDTO
                {
                    SchoolId = sp.SchoolId,
                    LetterGrade = sp.LetterGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    GradePoint = sp.GradePoint,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    MaxGrade = sp.MaxGrade,
                    MinGrade = sp.MinGrade
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostGradeConversion")]
        public async Task<IActionResult> PostGradeConversion([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            try
            {
                GradeConversion c = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId).FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new GradeConversion
                    {
                        GradePoint = _GradeConversionDTO.GradePoint,
                        MaxGrade = _GradeConversionDTO.MaxGrade,
                        MinGrade = _GradeConversionDTO.MinGrade
                    };
                    _context.GradeConversions.Add(c);
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
        [Route("PutGradeConversion")]
        public async Task<IActionResult> PutGradeConversion([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            try
            {
                GradeConversion c = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId).FirstOrDefaultAsync();

                if (c != null)
                {
                    c.GradePoint = _GradeConversionDTO.GradePoint;
                    c.MaxGrade = _GradeConversionDTO.MaxGrade;
                    c.MinGrade = _GradeConversionDTO.MinGrade;

                    _context.GradeConversions.Update(c);
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
        [Route("DeleteGradeConversion/{_SchoolId}")]
        public async Task<IActionResult> DeleteGradeConversion(int _SchoolId)
        {
            try
            {
                GradeConversion c = await _context.GradeConversions.Where(x => x.SchoolId == _SchoolId).FirstOrDefaultAsync();

                if (c != null)
                {
                    _context.GradeConversions.Remove(c);
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