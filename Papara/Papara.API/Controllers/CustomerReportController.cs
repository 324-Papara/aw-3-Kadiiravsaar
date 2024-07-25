using MediatR;
using Microsoft.AspNetCore.Mvc;
using Papara.Business.Query.CustomerReport;



[Route("api/[controller]")]
[ApiController]
public class CustomerReportController : ControllerBase
{
	private readonly IMediator _mediator;

	public CustomerReportController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> GetCustomerReport()
	{
		var operation = await _mediator.Send(new GetCustomerReportQuery());
		return Ok(operation);
	}
}