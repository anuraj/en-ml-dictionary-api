using System.Linq;
using Dictionary.Data;
using Dictionary.Models;
using Microsoft.AspNetCore.Http;

namespace Dictionary.Services
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DictionaryDbContext _dictionaryDbContext;
        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, DictionaryDbContext dictionaryDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dictionaryDbContext = dictionaryDbContext;
        }
        public User GetCurrentUser
        {
            get
            {
                return _dictionaryDbContext.Users.First(u => u.Email == _httpContextAccessor.HttpContext.User.Identity.Name);
            }
        }
    }
}