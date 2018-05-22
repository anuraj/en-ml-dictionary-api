using System.Collections.Generic;
using Dictionary.Models;

namespace Dictionary.Services
{
    public interface IDictionaryService
    {
        IEnumerable<Definition> FindMeaning(string text);
    }
}