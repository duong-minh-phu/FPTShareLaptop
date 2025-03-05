using DataAccess.DonationFormDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IDonationFormService
    {
        Task<IEnumerable<DonationFormResModel>> GetAllDonationsAsync();

        Task<DonationFormResModel?> GetDonationByIdAsync(int id);

        Task CreateDonationAsync(CreateDonationFormReqModel request);

        Task UpdateDonationAsync(int id, UpdateDonationFormReqModel request);

        Task SoftDeleteDonationAsync(int id);
    }
}