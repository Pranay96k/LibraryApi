using LibraryApi.Models;

namespace LibraryApi.Services
{
    public interface ITokenService
    {
        string CreateToken(Member member);

    }
}
