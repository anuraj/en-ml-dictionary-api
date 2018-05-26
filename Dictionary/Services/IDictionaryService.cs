using System.Collections.Generic;
using System.Threading.Tasks;
using Dictionary.Models;

namespace Dictionary.Services
{
    public interface IDictionaryService
    {
        Task<IEnumerable<Definition>> FindMeaning(string text);
    }
}