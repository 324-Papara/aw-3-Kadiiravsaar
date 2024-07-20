using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Papara.Base.Response;
using Papara.Business.Command.CustomerDetailCommand.Create;
using Papara.Business.Command.CustomerDetailCommand.Delete;
using Papara.Business.Command.CustomerDetailCommand.Update;
using Papara.Business.Query.CustomerDetailQuery.GetById;
using Papara.Business.Query.CustomerDetailQuery.GetList;
using Papara.Business.Query.CustomerDetailQuery.GetListWithInclude;
using Papara.Schema.CustomerDetailSchema;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerDetailsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CustomerDetailsController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[HttpGet("GetListWithInclude")]
		public async Task<ApiResponse<List<CustomerDetailResponseWithInclude>>> GetListCustomerDetailsInclude()
		{
			var operation = new GetListCustomerDetailsWithIncludeQuery();
			var result = await _mediator.Send(operation);
			return result;
		}


		[HttpGet("GetListCustomerDetails")]
		public async Task<ApiResponse<List<CustomerDetailResponse>>> GetListCustomerDetails()
		{
			var operation = new GetListCustomerDetailsQuery();
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpGet("{customerDetailId}")]
		public async Task<ApiResponse<CustomerDetailResponseWithInclude>> Get([FromRoute] long customerDetailId)
		{
			var operation = new GetCustomerDetailByIdQuery(customerDetailId);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPost]
		public async Task<ApiResponse<CustomerDetailResponse>> Post([FromBody] CustomerDetailRequest value)
		{
			var operation = new CreateCustomerDetailCommand(value);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpPut("{customerDetailId}")]
		public async Task<ApiResponse> Put([FromRoute] long customerDetailId, [FromBody] CustomerDetailRequest value)
		{
			var operation = new UpdateCustomerDetailCommand(customerDetailId, value);
			var result = await _mediator.Send(operation);
			return result;
		}

		[HttpDelete("{customerDetailId}")]
		public async Task<ApiResponse> Delete([FromRoute] long customerDetailId)
		{
			var operation = new DeleteCustomerDetailCommand(customerDetailId);
			var result = await _mediator.Send(operation);
			return result;
		}
	}
}
