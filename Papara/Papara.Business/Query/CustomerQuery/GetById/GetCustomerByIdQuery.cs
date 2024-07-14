using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Papara.Base.Response;
using Papara.Data.UnitOfWork;
using Papara.Schema.CustomerSchema;

namespace Papara.Business.Query.CustomerQuery.GetById
{
	public record GetCustomerByIdQuery(long CustomerId) : IRequest<ApiResponse<CustomerResponse>>;


	public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
		{
			//var entity = await _unitOfWork.CustomerRepository.GetById(request.CustomerId);

			var entity = await _unitOfWork.CustomerRepository.GetInclude(
				request.CustomerId,
				include:c=>c.Include(c=>c.CustomerDetail));

			var mapped = _mapper.Map<CustomerResponse>(entity);
			return new ApiResponse<CustomerResponse>(mapped);
		}
	}





}