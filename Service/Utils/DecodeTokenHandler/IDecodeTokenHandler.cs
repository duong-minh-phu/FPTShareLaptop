using DataAccess.TokenDTO;

namespace Service.Utils.DecodeTokenHandler
{
    public interface IDecodeTokenHandler
    {
        TokenModel decode(string token);

    }
}