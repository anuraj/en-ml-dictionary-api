using System.Collections.Generic;
using Dictionary.Data;
using Dictionary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Dictionary.Services
{
    public class DBDictionaryService : IDictionaryService
    {
        private readonly DictionaryDbContext _dictionaryDbContext;
        public DBDictionaryService(DictionaryDbContext dictionaryDbContext)
        {
            _dictionaryDbContext = dictionaryDbContext;
        }
        public IEnumerable<Definition> FindMeaning(string text)
        {
            return _dictionaryDbContext.Definitions
                .Where(x => x.EnglishWord.Equals(text, StringComparison.OrdinalIgnoreCase));
        }
    }
}