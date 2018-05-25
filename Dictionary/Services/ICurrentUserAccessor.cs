using Dictionary.Models;

namespace Dictionary.Services
{
    public interface ICurrentUserAccessor
    {
        User GetCurrentUser { get; }
    }
}