using AutoMapper;
using MediatR;
using Papara.Base.Response;
using Papara.Data.UnitOfWork;
using Papara.Schema.CustomerSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Business.Query.CustomerQuery.GetParameterQuery
{
	public record GetCustomerByParameterQuery(string Name) : IRequest<ApiResponse<List<CustomerResponse>>>;


	public class GetCustomerByParameterQueryHandler : IRequestHandler<GetCustomerByParameterQuery, ApiResponse<List<CustomerResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetCustomerByParameterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetCustomerByParameterQuery request, CancellationToken cancellationToken)
		{
			var customers = await _unitOfWork.CustomerRepository
		   .Where(c => (c.FirstName == request.Name || string.IsNullOrEmpty(request.Name)));
						  
			var customerResponses = _mapper.Map<List<CustomerResponse>>(customers);

			return new ApiResponse<List<CustomerResponse>>(customerResponses);
			
		}
	}
}
