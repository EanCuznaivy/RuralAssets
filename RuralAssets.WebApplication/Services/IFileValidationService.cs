using System.Threading.Tasks;

namespace RuralAssets.WebApplication
{
    public interface IFileValidationService
    {
        Task ReceiveFileAsync();
        Task<bool> ValidateFileAsync();
    }
}