﻿using Cognizant.Hackathon.RestClient.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Request;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<LinCUser>> GetOrCreateUser(Guid userId, string accessToken = null);
        Task<ServiceResponse<LinCUser>> CreateUserAsync(string deviceDensity, string deviceType, LinCUser newUser);
        Task<ServiceResponse<(LinCUser, bool)>> GetUserAsync(string deviceDensity, string deviceType, string companyCode, string userId, string UserCode);
        Task<ServiceResponse<MasterData>> GetProductCategoryByUser(int userId);
        Task<ServiceResponse<(List<Product>, bool)>> GetUserProducts(int? userId, int? supplierId, int productTypeMasterId);
        Task<ServiceResponse<(List<LinCUser>, bool)>> GetSuppliers(int userId, int prdTypeId, int searchWithin);
        Task<ServiceResponse<bool>> SaveProduct(List<Product> productsToAdd, int? userId);
        Task<ServiceResponse<List<Order>>> GetOrders(LinCUser user, int? orderId = null);
        Task<ServiceResponse<List<Order>>> SaveOrders(List<Product> ordersToAdd, LinCUser user, int? supplierId);
    }
}
