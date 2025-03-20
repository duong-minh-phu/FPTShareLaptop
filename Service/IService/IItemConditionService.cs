using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ItemConditionDTO;

namespace Service.IService
{
    public interface IItemConditionService
    {
        Task CreateItemCondition(string token, CreateItemConditionReqModel requestModel);
        Task<List<ItemConditionResModel>> GetAllItemConditions();
        Task<ItemConditionResModel?> GetItemConditionById(int conditionId);
        Task UpdateItemCondition(string token, int conditionId, UpdateItemConditionReqModel updateModel);
        Task DeleteItemCondition(string token, int conditionId);
    }
}
