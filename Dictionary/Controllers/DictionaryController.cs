using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dictionary.Data;
using Dictionary.Models;
using Dictionary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dictionary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        private readonly DictionaryDbContext _dictionaryDbContext;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public DictionaryController(IDictionaryService dictionaryService, DictionaryDbContext dictionaryDbContext, ICurrentUserAccessor currentUserAccessor)
        {
            _dictionaryService = dictionaryService;
            _dictionaryDbContext = dictionaryDbContext;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet("Find")]
        public ActionResult<IEnumerable<Definition>> Find([FromQuery, BindRequired]string text)
        {
            var result = _dictionaryService.FindMeaning(text);
            if (result != null && result.Any())
            {
                return Ok(result);
            }

            return NoContent();
        }

        [HttpPost, Authorize]
        public ActionResult<Definition> Post([FromBody]Definition definition)
        {
            definition.User = _currentUserAccessor.GetCurrentUser;
            definition.Id = _dictionaryDbContext.Definitions.Max(x => x.Id) + 1;
            _dictionaryDbContext.Definitions.Add(definition);
            var inserted = _dictionaryDbContext.SaveChanges() == 1;
            if(inserted)
            {
                return Ok();
            }
            
            return BadRequest();
        }
    }
}
