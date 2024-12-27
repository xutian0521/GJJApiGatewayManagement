using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// 获取所有授权规则
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorizationViewModel>>> GetAll()
        {
            var authorizations = await _authorizationService.GetAllAsync();
            return Ok(authorizations);
        }

        /// <summary>
        /// 根据 ID 获取授权规则
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorizationViewModel>> GetById(int id)
        {
            var authorization = await _authorizationService.GetByIdAsync(id);
            if (authorization == null)
            {
                return NotFound();
            }
            return Ok(authorization);
        }

        /// <summary>
        /// 创建新的授权规则
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AuthorizationViewModel>> Create(CreateAuthorizationDto dto)
        {
            var authorization = await _authorizationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = authorization.Id }, authorization);
        }

        /// <summary>
        /// 更新授权规则
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorizationViewModel>> Update(int id, UpdateAuthorizationDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var updatedAuthorization = await _authorizationService.UpdateAsync(dto);
            if (updatedAuthorization == null)
            {
                return NotFound();
            }

            return Ok(updatedAuthorization);
        }

        /// <summary>
        /// 删除授权规则
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorizationService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
