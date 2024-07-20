using MediatR;
using Microsoft.AspNetCore.Mvc;
using Papara.Base.Response;
using Papara.Business.Command.CustomerCommand.Create;
using Papara.Business.Command.CustomerCommand.Delete;
using Papara.Business.Command.CustomerCommand.Update;
using Papara.Business.Query.CustomerQuery.GetById;
using Papara.Business.Query.CustomerQuery.GetListWithInclude;
using Papara.Business.Query.CustomerQuery.GetParameterQuery;
using Papara.Schema.CustomerSchema;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CustomersController(IMediator mediator)
		{
			_mediator = mediator;
		}
			

		[HttpGet("GetListWithDetail")]
		public async Task<ApiResponse<List<CustomerResponseWithDetail>>> GetListCustomersWithDetail()
		{
			var operation = new GetListCustomersWithDetailQuery();
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpGet("GetList")]
		public async Task<ApiResponse<List<CustomerResponse>>> GetListCustomers()
		{
			var operation = new GetListCustomersQuery();
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpGet("Parameter")]
		public async Task<ApiResponse<List<CustomerResponse>>> GetParameter(string name)
		{
			var operation = new GetCustomerByParameterQuery(name);
			var result = await _mediator.Send(operation);
			return result;
		}


		[HttpGet("{customerId}")]
		public async Task<ApiResponse<CustomerResponse>> Get([FromRoute] long customerId)
		{
			var operation = new GetCustomerByIdQuery(customerId);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPost]	
		public async Task<ApiResponse<CustomerResponse>> Post([FromBody] CustomerRequest value)
		{
			var operation = new CreateCustomerCommand(value);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPut("{customerId}")]
		public async Task<ApiResponse> Put(long customerId, [FromBody] CustomerRequest value)
		{
			var operation = new UpdateCustomerCommand(customerId, value);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpDelete("{customerId}")]
		public async Task<ApiResponse> Delete(long customerId)
		{
			var operation = new DeleteCustomerCommand(customerId);
			var result = await _mediator.Send(operation);
			return result;
		}
	}
}
