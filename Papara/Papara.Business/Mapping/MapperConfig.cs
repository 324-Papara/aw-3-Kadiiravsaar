using AutoMapper;
using Papara.Data.Domain;
using Papara.Schema.CustomerSchema;
using Papara.Schema.CustomerAddressSchema;
using Papara.Schema.CustomerDetailSchema;
using Papara.Schema.CustomerPhoneSchema;


namespace Papara.Business.Mapping
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<Customer, CustomerResponse>();
			CreateMap<CustomerRequest, Customer>();

			CreateMap<CustomerAddress, CustomerAddressResponse>();
			CreateMap<CustomerAddressRequest, CustomerAddress>();


			CreateMap<CustomerDetail, CustomerDetailResponse>();
			CreateMap<CustomerDetailRequest, CustomerDetail>();


			CreateMap<CustomerPhone, CustomerPhoneResponse>();
			CreateMap<CustomerPhoneRequest, CustomerPhone>();
		}
	}
}
