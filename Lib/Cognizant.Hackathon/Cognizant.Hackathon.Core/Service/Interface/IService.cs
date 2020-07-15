using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cognizant.Hackathon.Core.Common.Helpers;
using Cognizant.Hackathon.Core.Common.Enum;
//using Cognizant.Hackathon.Core.Configuration.Constants;
using Cognizant.Hackathon.Core.Interface;
using Cognizant.Hackathon.Core.Model;
using Cognizant.Hackathon.Core.Model.Enums;
using Cognizant.Hackathon.Core.Model.Interface;
using Cognizant.Hackathon.Core.Model.Internal;
using Cognizant.Hackathon.RestClient;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.Core.Service.Interface
{
    /// <summary>
    /// The i service.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public interface IService<TEntity>
        where TEntity : ICoreObject
    {
        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        Guid MemberID { get; set; }
        
        #region Public Methods and Operators

        /// <summary>
        /// The get by identity id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        ServiceResponse<TEntity> GetByIdentityId(Guid id);

        /// <summary>
        /// The get by identity id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> GetByIdentityIdAsync(Guid id);

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <param name="getUnitOfWork"></param>
        /// <param name="unitOfWork"></param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        //ServiceResponse<TEntity> Create(TEntity entity, IUnitOfWork unitOfWork);

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> CreateAsync(TEntity entity);

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <param name="unitOfWork"></param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        //ServiceResponse<List<TEntity>> CreateMany(List<TEntity> entities, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// The create many async.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<List<TEntity>>> CreateManyAsync(List<TEntity> entities);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        ServiceResponse<TEntity> Delete(TEntity entity);

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> DeleteAsync(TEntity entity);

        /// <summary>
        /// The delete by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        ServiceResponse<TEntity> DeleteById(Guid id);

        /// <summary>
        /// The delete by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> DeleteByIdAsync(Guid id);

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> GetAsync(TEntity entity, string[] includes = null);

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceResponse"/>.
        /// </returns>
        ServiceResponse<TEntity> Get(Expression<Func<TEntity, bool>> where, string order = "", string[] includes = null);

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where, string order = "", string[] includes = null);
        
        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> GetAsync(string where, string order = "", string[] includes = null);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// List
        /// </returns>
        ServiceResponse<IEnumerable<TEntity>> GetAll(int amount, int start, string order, string[] includes);


        /// <summary>
        /// The get all async.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<List<TEntity>>> GetAllAsync(int amount = 0, int start = 0, string order = "", string[] includes = null);

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        ServiceResponse<TEntity> GetById(Guid id, string[] includes = null, bool useCache = true);

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> GetByIdAsync(Guid id, string[] includes = null);

        /// <summary>
        /// The get many.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The List
        /// </returns>
        ServiceResponse<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> @where, int amount = 0, int start = 0, string order = "", string[] includes = null, bool useCache = true);

        ServiceResponse<IEnumerable<TEntity>> GetMany(IQueryable<TEntity> query, int amount, int start, string order, string[] includes = null);

        /// <summary>
        /// The get many async.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<List<TEntity>>> GetManyAsync(Expression<Func<TEntity, bool>> @where, int amount = 0, int start = 0, string order = "", string[] includes = null);

        /// <summary>
        /// The get many async.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<List<TEntity>>> GetManyAsync(string where, int amount = 0, int start = 0, string order = "", string[] includes = null);

        /// <summary>
        /// The get recent.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The amount
        /// </returns>
        ServiceResponse<IEnumerable<TEntity>> GetRecent(int amount, string[] includes = null);

        ServiceResponse<IEnumerable<TEntity>> GetManyByTimestamp(DateTime timestamp, string[] includes = null);

        ServiceResponse<IEnumerable<TEntity>> GetManyByTimestamp(Guid targetId, DateTime timestamp, string[] includes = null);

        ServiceResponse<IEnumerable<TEntity>> GetManyByTimestamp(Guid targetId, ObjectType targetType, DateTime timestamp, string[] includes = null);

        /// <summary>
        /// The get recent async.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<List<TEntity>>> GetRecentAsync(int amount, string[] includes = null);
        
        /// <summary>
        /// The count.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        ServiceResponse<long> Count(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// The count async.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<long>> CountAsync(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// The count.
        /// </summary>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        ServiceResponse<long> Count();

        /// <summary>
        /// The count async.
        /// </summary>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<long>> CountAsync();

        /// <summary>
        ///     The save.
        /// </summary>
        /// <returns>
        ///     The <see cref="ServiceResponse{T}" />.
        /// </returns>
        ServiceResponse<TEntity> Save();

        /// <summary>
        /// The save async.
        /// </summary>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> SaveAsync();

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        ServiceResponse<TEntity> Save(TEntity entity);

        /// <summary>
        /// The save async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> SaveAsync(TEntity entity);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <param name="unitOfWork"></param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        //ServiceResponse<TEntity> Update(TEntity entity, IUnitOfWork unitOfWork, bool bypassMerge = false);

        //ServiceResponse<TEntity> Update<TProperty>(TEntity model, IUnitOfWork unitOfWork = null,
            //Expression<Func<CoreObject, TProperty>> excludePropertyExpression = null);

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TEntity>> UpdateAsync(TEntity entity);

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="httpVerb">
        /// The http verb.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <param name="timeoutInMilliseconds">
        /// The timeout in milliseconds.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <typeparam name="TResult">
        /// </typeparam>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TResult>> ExecuteAsync<TResult>(HttpVerb httpVerb = HttpVerb.GET, TEntity body = default(TEntity), Guid id = default(Guid), 
            Dictionary<string, object> parameters = null, 
            int amount = -1, int start = -1, string order = "", string[] includes = null, string methodName = "");

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="httpVerb">
        /// The http verb.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <param name="timeoutInMilliseconds">
        /// The timeout in milliseconds.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <typeparam name="TResult">
        /// </typeparam>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        Task<ServiceResponse<TResult>> ExecuteAsync<TResult, TBody>(HttpVerb httpVerb = HttpVerb.GET, TBody body = default(TBody), Guid id = default(Guid),
            Dictionary<string, object> parameters = null,
            int amount = -1, int start = -1, string order = "", string[] includes = null, string methodName = "", string apiRoutePrefix = null);

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="httpVerb">
        /// The http verb.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <param name="timeoutInMilliseconds">
        /// The timeout in milliseconds.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <typeparam name="TResult">
        /// </typeparam>
        /// <typeparam name="TBody">
        /// </typeparam>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        ServiceResponse<TResult> Execute<TResult>(HttpVerb httpVerb = HttpVerb.GET, TEntity body = default(TEntity), Guid id = default(Guid), 
            Dictionary<string, object> parameters = null, 
            int amount = -1, int start = -1, string order = "", string[] includes = null, string methodName = "", string apiRoutePrefix = null);

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="httpVerb">
        /// The http verb.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <param name="includes">
        /// The includes.
        /// </param>
        /// <param name="timeoutInMilliseconds">
        /// The timeout in milliseconds.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <typeparam name="TResult">
        /// </typeparam>
        /// <typeparam name="TBody">
        /// </typeparam>
        /// <returns>
        /// The <see cref="async Task"/>.
        /// </returns>
        ServiceResponse<TResult> Execute<TResult, TBody>(HttpVerb httpVerb = HttpVerb.GET, TBody body = default(TBody), Guid id = default(Guid),
            Dictionary<string, object> parameters = null,
            int amount = -1, int start = -1, string order = "", string[] includes = null, string methodName = "", string apiRoutePrefix = null);

        IEnumerable<TEntity> GetManyByForeignKey(Type foreignKeyType, Guid foreignKeyId, bool useCache = true);

        #endregion

    }

}