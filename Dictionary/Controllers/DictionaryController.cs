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

        [HttpGet("{text}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Definition>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Definition>>> Find([FromRoute, BindRequired]string text)
        {
            var result = await _dictionaryService.FindMeaning(text);
            if (result != null && result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Definition), 200)]
        [ProducesResponseType(400)]
        public ActionResult<Definition> Post([FromBody]Definition definition)
        {
            definition.User = _currentUserAccessor.GetCurrentUser;
            definition.Id = _dictionaryDbContext.Definitions.Max(x => x.Id) + 1;
            _dictionaryDbContext.Definitions.Add(definition);
            var inserted = _dictionaryDbContext.SaveChanges() == 1;
            if (inserted)
            {
                return Ok(definition);
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Definition), 200)]
        [ProducesResponseType(400)]
        public ActionResult<Definition> Update([FromBody]Definition definition)
        {
            definition.User = _currentUserAccessor.GetCurrentUser;
            _dictionaryDbContext.Update(definition);
            var updated = _dictionaryDbContext.SaveChanges() == 1;
            if (updated)
            {
                return Ok(definition);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public ActionResult Delete(int id)
        {
            var definition = _dictionaryDbContext.Definitions
                .FirstOrDefault(x => x.Id == id);
            if (definition == null)
            {
                return NotFound();
            }

            if (definition.User != _currentUserAccessor.GetCurrentUser)
            {
                return Unauthorized();
            }

            _dictionaryDbContext.Definitions.Remove(definition);
            var deleted = _dictionaryDbContext.SaveChanges() == 1;
            if (deleted)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
