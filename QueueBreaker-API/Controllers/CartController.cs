﻿using System;
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
    /// Interacts with the Cart Table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class CartController : Controller
    {
        private readonly ICartInterface _cartRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public CartController(ICartInterface cartRepository,
            ILoggerService logger,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Cart
        /// </summary>
        /// <returns>List Of Cart Items</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var location = GetControllerActionNames();
            try
            {

                string UserID = "", CanteenID = "", Role = "";
                int iUserID = 0, iCanteenID = 0;
                var principal = HttpContext.User;
                if (principal?.Claims != null)
                {
                    UserID = principal?.Claims?.FirstOrDefault(p => p.Type == "UserID")?.Value;
                    CanteenID = principal?.Claims?.FirstOrDefault(p => p.Type == "CanteenID")?.Value;
                    Role = principal?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;

                }
                if (UserID != "")
                {
                    iUserID = Convert.ToInt32(UserID);
                }
                if (CanteenID != "")
                {
                    iCanteenID = Convert.ToInt32(CanteenID);
                }

                _logger.LogInfo($"{location}: Attempted Call");
                if (Role == RoleName.Customer && iUserID > 0)
                {
                    var results = await _cartRepository.FindAll();
                    results = results.Where(x => x.UserId == iUserID).ToList();
                    var response = _mapper.Map<IList<CartDTO>>(results);
                    _logger.LogInfo($"{location}: Successful");
                    return Ok(response);
                }
                else
                {
                    var results = await _cartRepository.FindByCanteenId(iCanteenID);
                    var response = _mapper.Map<IList<CartDTO>>(results);
                    _logger.LogInfo($"{location}: Successful");
                    return Ok(response);
                }



                //_logger.LogInfo($"{location}: Attempted Call");
                //var results = await _cartRepository.FindAll();
                //var response = _mapper.Map<IList<CartDTO>>(results);
                //_logger.LogInfo($"{location}: Successful");
                //return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }

        }
        /// <summary>
        /// Get An Cart by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Cart's record</returns>
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
                var result = await _cartRepository.FindById(id);
                if (result == null)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var response = _mapper.Map<CartDTO>(result);
                _logger.LogInfo($"{location}: Successfully got record with id: {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> orderplace()
        //{
        //    CartCreateDTO modelDTO = new CartCreateDTO();
        //    var Userid = 1;
        //    var CanteenId = 1;
        //    var location = GetControllerActionNames();
        //    try
        //    {
        //        _logger.LogInfo($"{location}: Attempted Call for id: {Userid}");
        //        var result = await _cartRepository.FindById(Userid);
        //        modelDTO.CanteenId = result.CanteenId;
        //        var result1 = _mapper.Map<Cart>(modelDTO);
        //        if (result == null)
        //        {
        //            _logger.LogWarn($"{location}: Failed to retrieve record with id: {Userid}");
        //            return NotFound();
        //        }
        //        var response = _mapper.Map<CartDTO>(result);
        //        _logger.LogInfo($"{location}: Successfully got record with id: {Userid}");
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return InternalError($"{location}: {e.Message} - {e.InnerException}");
        //    }
        //}
        /// <summary>
        /// Creates An Cart
        /// </summary>
        /// <param name="modelDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeRoles(RoleName.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CartCreateDTO modelDTO)
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
                var result = _mapper.Map<Cart>(modelDTO);
                var isSuccess = await _cartRepository.Create(result);
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
        /// Updates An Cart
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthorizeRoles(RoleName.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CartUpdateDTO modelDTO)
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
                var isExists = await _cartRepository.isExists(id);
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
                var result = _mapper.Map<Cart>(modelDTO);
                var isSuccess = await _cartRepository.Update(result);
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
        /// Removes an Cart by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthorizeRoles(RoleName.Customer)]
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
                var isExists = await _cartRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var result = await _cartRepository.FindById(id);
                var isSuccess = await _cartRepository.Delete(result);
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
