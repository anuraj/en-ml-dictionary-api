using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dictionary.Models;
using Dictionary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dictionary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
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
    }
}
