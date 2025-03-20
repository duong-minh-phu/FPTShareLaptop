using System.Net;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.ItemConditionDTO;
using Service.IService;
using Service.Utils.CustomException;

public class ItemConditionService : IItemConditionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTService _jwtService;

    public ItemConditionService(IUnitOfWork unitOfWork, IJWTService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    // Retrieve all item conditions
    public async Task<List<ItemConditionResModel>> GetAllItemConditions()
    {
        var conditions = await _unitOfWork.ItemCondition.GetAllAsync(includeProperties: c => c.Item);
        return conditions.Select(c => new ItemConditionResModel
        {
            ConditionId = c.ConditionId,
            ItemId = c.ItemId,
            ItemName = c.Item.ItemName,
            ConditionType = c.ConditionType,
            Description = c.Description,
            Status = c.Status,
            ImageUrl = c.ImageUrl,
            CheckedBy = c.CheckedBy,
            CheckedDate = c.CheckedDate
        }).ToList();
    }

    // Retrieve item condition by ID
    public async Task<ItemConditionResModel> GetItemConditionById(int conditionId)
    {
        var condition = await _unitOfWork.ItemCondition.GetByIdAsync(conditionId, includeProperties: c => c.Item);
        if (condition == null)
            throw new ApiException(HttpStatusCode.NotFound, "Laptop condition not found.");

        return new ItemConditionResModel
        {
            ConditionId = condition.ConditionId,
            ItemId = condition.ItemId,
            ItemName = condition.Item.ItemName,
            ConditionType = condition.ConditionType,
            Description = condition.Description,
            Status = condition.Status,
            ImageUrl = condition.ImageUrl,
            CheckedBy = condition.CheckedBy,
            CheckedDate = condition.CheckedDate
        };
    }

    // Create a new item condition
    public async Task CreateItemCondition(string token, CreateItemConditionReqModel requestModel)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.Unauthorized, "Unauthorized user.");

        var item = await _unitOfWork.DonateItem.GetByIdAsync(requestModel.ItemId);
        if (item == null)
            throw new ApiException(HttpStatusCode.NotFound, "Laptop not found.");

        var condition = new ItemCondition
        {
            ItemId = requestModel.ItemId,
            ConditionType = requestModel.ConditionType,
            Description = requestModel.Description,
            Status = ItemConditionStatus.Available.ToString(),
            ImageUrl = requestModel.ImageUrl,
            CheckedBy = user.FullName,
            CheckedDate = DateTime.UtcNow
        };

        await _unitOfWork.ItemCondition.AddAsync(condition);
        await _unitOfWork.SaveAsync();
    }

    // Update item condition
    public async Task UpdateItemCondition(string token, int conditionId, UpdateItemConditionReqModel updateModel)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.Unauthorized, "Unauthorized user.");

        var condition = await _unitOfWork.ItemCondition.GetByIdAsync(conditionId);
        if (condition == null)
            throw new ApiException(HttpStatusCode.NotFound, "Laptop condition not found.");

        condition.ConditionType = updateModel.ConditionType ?? condition.ConditionType;
        condition.Description = updateModel.Description ?? condition.Description;
        condition.Status = updateModel.Status ?? condition.Status;
        condition.ImageUrl = updateModel.ImageUrl ?? condition.ImageUrl;
        condition.CheckedBy = user.FullName;
        condition.CheckedDate = DateTime.UtcNow;

        _unitOfWork.ItemCondition.Update(condition);
        await _unitOfWork.SaveAsync();
    }

    // Delete item condition
    public async Task DeleteItemCondition(string token, int conditionId)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.Unauthorized, "Unauthorized user.");

        var condition = await _unitOfWork.ItemCondition.GetByIdAsync(conditionId);
        if (condition == null)
            throw new ApiException(HttpStatusCode.NotFound, "Laptop condition not found.");

        _unitOfWork.ItemCondition.Delete(condition);
        await _unitOfWork.SaveAsync();
    }
}
