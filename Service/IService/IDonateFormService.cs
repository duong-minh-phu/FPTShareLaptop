using DataAccess.DonationFormDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IDonateFormService
    {
        Task<List<DonateFormResModel>> GetAllDonationsAsync();

        Task<DonateFormResModel?> GetDonationByIdAsync(int id);

        Task<DonateFormResModel> CreateDonationAsync(CreateDonateFormReqModel request);

        Task UpdateDonationAsync(int id, UpdateDonateFormReqModel request);

        Task DeleteDonationAsync(int id);
    }
}