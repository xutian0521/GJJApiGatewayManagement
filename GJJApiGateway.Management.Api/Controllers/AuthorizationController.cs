using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers
{
    /// <summary>
    /// 授权控制器，负责处理授权规则的 CRUD 操作。
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        /// <summary>
        /// 授权服务接口，用于处理授权规则的业务逻辑。
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// 构造函数，注入授权服务。
        /// </summary>
        /// <param name="authorizationService">授权服务接口实例。</param>
        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// 获取所有授权规则。
        /// </summary>
        /// <returns>包含所有授权规则视图模型的响应。</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorizationViewModel>>> GetAll()
        {
            var authorizations = await _authorizationService.GetAllAsync();
            return Ok(authorizations); // 返回 200 OK 响应，包含授权规则数据
        }

        /// <summary>
        /// 根据 ID 获取指定的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>指定的授权规则视图模型的响应，如果不存在则返回 404 Not Found。</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorizationViewModel>> GetById(int id)
        {
            var authorization = await _authorizationService.GetByIdAsync(id);
            if (authorization == null)
            {
                return NotFound(); // 授权规则不存在，返回 404
            }
            return Ok(authorization); // 返回 200 OK 响应，包含授权规则数据
        }

        /// <summary>
        /// 创建新的授权规则。
        /// </summary>
        /// <param name="dto">创建授权规则的数据传输对象。</param>
        /// <returns>创建后的授权规则视图模型的响应，状态码为 201 Created。</returns>
        [HttpPost]
        public async Task<ActionResult<AuthorizationViewModel>> Create(CreateAuthorizationDto dto)
        {
            var authorization = await _authorizationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = authorization.Id }, authorization); // 返回 201 Created 响应，包含新创建的授权规则数据
        }

        /// <summary>
        /// 更新指定 ID 的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <param name="dto">更新授权规则的数据传输对象。</param>
        /// <returns>更新后的授权规则视图模型的响应，如果授权规则不存在则返回 404 Not Found。</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorizationViewModel>> Update(int id, UpdateAuthorizationDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch"); // 请求的 ID 与 DTO 中的 ID 不匹配，返回 400 Bad Request
            }

            var updatedAuthorization = await _authorizationService.UpdateAsync(dto);
            if (updatedAuthorization == null)
            {
                return NotFound(); // 授权规则不存在，返回 404
            }

            return Ok(updatedAuthorization); // 返回 200 OK 响应，包含更新后的授权规则数据
        }

        /// <summary>
        /// 删除指定 ID 的授权规则。
        /// </summary>
        /// <param name="id">授权规则的唯一标识符。</param>
        /// <returns>删除成功则返回 204 No Content，否则返回 404 Not Found。</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorizationService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(); // 授权规则不存在，返回 404
            }
            return NoContent(); // 删除成功，返回 204 No Content
        }
    }
}
