using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
    public class OrderController : Controller
    {
        private readonly IOrderInterface _orderRepository;
        private readonly IUsersInterface _usersRepository;
        private readonly IOrderItemsInterface _orderItemsRepository;
        private readonly ICartInterface _cartRepository;
        private readonly IItemsInterface _itemsRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public OrderController(IOrderInterface orderRepository,
            IOrderItemsInterface orderItemsRepository, IUsersInterface usersRepository,
            ICartInterface cartRepository, IItemsInterface itemsRepository,
            ILoggerService logger,
            IMapper mapper)
        {
            _orderItemsRepository = orderItemsRepository;
            _usersRepository = usersRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository; _itemsRepository = itemsRepository;
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
                string UserID = "", CanteenID = "", Role = "";
                int iUserID = 0, iCanteenID = 0;
                var principal = HttpContext.User;
                if (principal?.Claims != null)
                {
                     UserID = principal?.Claims?.FirstOrDefault(p => p.Type == "UserID")?.Value;
                     CanteenID = principal?.Claims?.FirstOrDefault(p => p.Type == "CanteenID")?.Value;
                     Role = principal?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;

                }
               if(UserID != "")
                {
                    iUserID = Convert.ToInt32(UserID);
                }
               if(CanteenID != "")
                {
                    iCanteenID = Convert.ToInt32(CanteenID);
                }

                _logger.LogInfo($"{location}: Attempted Call");
                if(Role== RoleName.Customer && iUserID > 0)
                {
                    var results = await _orderRepository.FindAll();
                    results = results.Where(x => x.UserId == iUserID).ToList();
                    var response = _mapper.Map<IList<OrderDTO>>(results);
                    _logger.LogInfo($"{location}: Successful");
                    return Ok(response);
                }
                else
                {
                    var results = await _orderRepository.FindByCanteenId(iCanteenID);
                    var response = _mapper.Map<IList<OrderDTO>>(results);
                    _logger.LogInfo($"{location}: Successful");
                    return Ok(response);
                }
              
              
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }

        }
        /// <summary>
        /// Get An Order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Order record</returns>
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
                var result = await _orderRepository.FindById(id);
                if (result == null)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var response = _mapper.Map<OrderDTO>(result);
                _logger.LogInfo($"{location}: Successfully got record with id: {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Creates An Order
        /// </summary>
        /// <param name="modelDTO"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeRoles(RoleName.Administrator)]
       // [AuthorizeRoles(RoleName.SuperAdministrator)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] OrderCreateDTO modelDTO)
        {
            var location = GetControllerActionNames();
            modelDTO.OrderDate = DateTime.Now;
            modelDTO.Status = 1;
            //var results = await _cartRepository.FindAll();
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
              
                var result = _mapper.Map<Order>(modelDTO);
                var isSuccess = await _orderRepository.Create(result);
               
                
                if (!isSuccess)
                {
                    return InternalError($"{location}: Creation failed");
                }
                var reurnvalue = result.Id;
              //  var Itemlist = await _itemsRepository.FindAll();
                var cartlist = await _cartRepository.FindAll();
                var list = cartlist.Where(x => x.UserId == modelDTO.UserId && x.CanteenId == modelDTO.CanteenId).ToList();
                OrderItem OI = new OrderItem();
                foreach (var item in list)
                {
                    OI.OrderId = reurnvalue;
                    OI.ItemId = item.ItemId;
                    OI.Quantity = item.Count;
                    OI.ItemPrice = item.Item.Price;
                    OI.Status = true;
                    var result1 = _mapper.Map<OrderItem>(OI);
                    var Success = await _orderItemsRepository.Create(result1);
                    var SuccessRemove = await _cartRepository.Delete(item);
                }

                if (isSuccess == true)
                {
                  var UserId=modelDTO.UserId;
                    var result1 = await _usersRepository.FindById(UserId); 
                   // var p = result1.Where(x => x.UserId == UserId).Select(x=>x.PhoneNumber);
                    var Phone = result1.PhoneNumber.ToString();
                    if (Phone != null) {
                        await SendSMS(Phone, "Your order has been Placed");
                            }
                }

                _logger.LogInfo($"{location}: Creation was successful");
                return Created("Create", new { result });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        [HttpPost("SendSMS")]
        public Task SendSMS(string Phone, string Message)
        {
            // Plug in your SMS service here to send a text message.
            // Twilio Begin
            var accountSid = "AC98f4bb7ee7a9480551d25ce206498d9b";
            var authToken = "398161345dbce7967895e98b3a95b0c8";
            var fromNumber = "+17738393665";

            TwilioClient.Init(accountSid, authToken);

            MessageResource result = MessageResource.Create(
            new PhoneNumber(Phone),
            from: new PhoneNumber(fromNumber),
            body: Message
            );

            //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.TraceInformation(result.Status.ToString());
            //Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);
            // Twilio End
        }
        /// <summary>
        /// Updates An Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[AuthorizeRoles(RoleName.SuperAdministrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDTO modelDTO)
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
                var isExists = await _orderRepository.isExists(id);
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
                var result = _mapper.Map<Order>(modelDTO);
                var isSuccess = await _orderRepository.Update(result);
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
                var isExists = await _orderRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var result = await _orderRepository.FindById(id);
                var isSuccess = await _orderRepository.Delete(result);
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
