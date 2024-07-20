using MediatR;
using Microsoft.AspNetCore.Mvc;
using Papara.Base.Response;
using Papara.Business.Command.CustomerAddressCommand.Create;
using Papara.Business.Command.CustomerAddressCommand.Delete;
using Papara.Business.Command.CustomerAddressCommand.Update;
using Papara.Business.Query.CustomerAddressQuery.GetById;
using Papara.Business.Query.CustomerAddressQuery.GetList;
using Papara.Business.Query.CustomerAddressQuery.GetListWithInclude;
using Papara.Schema.CustomerAddressSchema;


namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerAddressesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CustomerAddressesController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[HttpGet("GetListWithDetail")]

		public async Task<ApiResponse<List<CustomerAddressResponseWithDetail>>> GetListCustomerAddressesWithDetail()
		{
			var operation = new GetListCustomerAddressesWithDetailQuery();
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpGet("GetList")]

		public async Task<ApiResponse<List<CustomerAddressResponse>>> GetListCustomerAddresses()
		{
			var operation = new GetListCustomerAddressesQuery();
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpGet("{customerAddressId}")]
		public async Task<ApiResponse<CustomerAddressResponse>> Get([FromRoute] long customerAddressId)
		{
			var operation = new GetCustomerAddressByIdQuery(customerAddressId);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPost]
		public async Task<ApiResponse<CustomerAddressResponse>> Post([FromBody] CustomerAddressRequest value)
		{
			var operation = new CreateCustomerAddressCommand(value);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPut("{customerAddressId}")]
		public async Task<ApiResponse> Put(long customerAddressId, [FromBody] CustomerAddressRequest value)
		{
			var operation = new UpdateCustomerAddressCommand(customerAddressId, value);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpDelete("{customerAddressId}")]
		public async Task<ApiResponse> Delete(long customerAddressId)
		{
			var operation = new DeleteCustomerAddressCommand(customerAddressId);
			var result = await _mediator.Send(operation);
			return result;
		}
	}
}
