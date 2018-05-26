using System.Collections.Generic;
using Dictionary.Data;
using Dictionary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Dictionary.Services
{
    public class DBDictionaryService : IDictionaryService
    {
        private readonly DictionaryDbContext _dictionaryDbContext;
        public DBDictionaryService(DictionaryDbContext dictionaryDbContext)
        {
            _dictionaryDbContext = dictionaryDbContext;
        }
        public async Task<IEnumerable<Definition>> FindMeaning(string text)
        {
            return await _dictionaryDbContext.Definitions.
                Where(x => x.EnglishWord.Equals(text, StringComparison.OrdinalIgnoreCase)).ToArrayAsync();
        }
    }
}