using MediatR;
using Microsoft.AspNetCore.Mvc;
using Papara.Base.Response;
using Papara.Business.Command.CustomerPhoneCommand.Create;
using Papara.Business.Command.CustomerPhoneCommand.Delete;
using Papara.Business.Command.CustomerPhoneCommand.Update;
using Papara.Business.Query.CustomerPhoneQuery.GetById;
using Papara.Business.Query.CustomerPhoneQuery.GetList;
using Papara.Schema.CustomerPhoneSchema;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerPhonesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CustomerPhonesController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[HttpGet]
		public async Task<ApiResponse<List<CustomerPhoneResponse>>> Get()
		{
			var operation = new GetAllCustomerPhoneQuery();
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpGet("{customerPhoneId}")]
		public async Task<ApiResponse<CustomerPhoneResponse>> Get([FromRoute] long customerPhoneId)
		{
			var operation = new GetCustomerPhoneByIdQuery(customerPhoneId);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPost]
		public async Task<ApiResponse<CustomerPhoneResponse>> Post([FromBody] CustomerPhoneRequest value)
		{
			var operation = new CreateCustomerPhoneCommand(value);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPut("{customerPhoneId}")]
		public async Task<ApiResponse> Put(long customerPhoneId, [FromBody] CustomerPhoneRequest value)
		{
			var operation = new UpdateCustomerPhoneCommand(customerPhoneId, value);
			var result = await _mediator.Send(operation);
			return result;
		}


		[HttpDelete("{customerPhoneId}")]
		public async Task<ApiResponse> Delete(long customerPhoneId)
		{
			var operation = new DeleteCustomerPhoneCommand(customerPhoneId);
			var result = await _mediator.Send(operation);
			return result;
		}
	}
}
