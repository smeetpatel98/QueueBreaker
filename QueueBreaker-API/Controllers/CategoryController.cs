using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;
using QueueBreaker_API.DTOs;
using QueueBreaker_API.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QueueBreaker_API.Controllers
{
    /// <summary>
    /// Interacts with the Category Table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class CategoryController : Controller
    {
        private readonly ICategoryInterface _categoryRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryInterface categoryRepository,
            ILoggerService logger,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Category
        /// </summary>
        /// <returns>List Of Category</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call");

                string UserID = "", CanteenID = "", Role = "";
                int iUserID = 0, iCanteenID = 0;
                var principal = HttpContext.User;
                if (principal?.Claims != null)
                {
                    CanteenID = principal?.Claims?.FirstOrDefault(p => p.Type == "CanteenID")?.Value;
                    Role = principal?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;

                }
             
                if(Role== RoleName.Customer)
                {
                    _logger.LogInfo($"{location}: Attempted Call");

                    var results = await _categoryRepository.FindAll();
                    var response = _mapper.Map<IList<CategoryDTO>>(results);
                    _logger.LogInfo($"{location}: Successful");
                    return Ok(response);
                }
                else
                {
                    if(CanteenID != "")
                    {
                        iCanteenID = Convert.ToInt32(CanteenID);
                        _logger.LogInfo($"{location}: Attempted Call");

                        var results = await _categoryRepository.FindByCanteenId(iCanteenID);
                        var response = _mapper.Map<IList<CategoryDTO>>(results);
                        _logger.LogInfo($"{location}: Successful");
                        return Ok(response);
                    }
                    else
                    {
                        _logger.LogInfo($"{location}: Attempted Call");

                        var results = await _categoryRepository.FindAll();
                        var response = _mapper.Map<IList<CategoryDTO>>(results);
                        _logger.LogInfo($"{location}: Successful");
                        return Ok(response);
                    }
                   
                }
                

            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }

        }
        /// <summary>
        /// Get An Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Category's record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call for id: {id}");
                var result = await _categoryRepository.FindById(id);
                if (result == null)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var response = _mapper.Map<CategoryDTO>(result);
                _logger.LogInfo($"{location}: Successfully got record with id: {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Creates An Category
        /// </summary>
        /// <param name="modelDTO"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeRoles(RoleName.Administrator)]
       // [AuthorizeRoles(RoleName.SuperAdministrator)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDTO modelDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Create Attempted");
                if (modelDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty Request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                modelDTO.CanteenId = 1;
                var result = _mapper.Map<Category>(modelDTO);
                var isSuccess = await _categoryRepository.Create(result);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Creation failed");
                }
                _logger.LogInfo($"{location}: Creation was successful");
                return Created("Create", new { result });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Updates An Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[AuthorizeRoles(RoleName.SuperAdministrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDTO modelDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Update Attempted on record with id: {id} ");
                if (id < 1 || modelDTO == null || id != modelDTO.Id)
                {
                    _logger.LogWarn($"{location}: Update failed with bad data - id: {id}");
                    return BadRequest();
                }
                var isExists = await _categoryRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                modelDTO.CanteenId = 1;
                var result = _mapper.Map<Category>(modelDTO);
                var isSuccess = await _categoryRepository.Update(result);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Update failed for record with id: {id}");
                }
                _logger.LogInfo($"{location}: Record with id: {id} successfully updated");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Removes an Category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[AuthorizeRoles(RoleName.SuperAdministrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Delete Attempted on record with id: {id} ");
                if (id < 1)
                {
                    _logger.LogWarn($"{location}: Delete failed with bad data - id: {id}");
                    return BadRequest();
                }
                var isExists = await _categoryRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var result = await _categoryRepository.FindById(id);
                var isSuccess = await _categoryRepository.Delete(result);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Delete failed for record with id: {id}");
                }
                _logger.LogInfo($"{location}: Record with id: {id} successfully deleted");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please contact the Administrator");
        }

    }
}
