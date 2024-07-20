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
			CreateMap<Customer, CustomerResponseWithDetail>()
			.ForMember(dest => dest.CustomerPhones, opt => opt.MapFrom(src => src.CustomerPhones)) // CustomerPhones maplendi
			.ForMember(dest => dest.CustomerAddresses, opt => opt.MapFrom(src => src.CustomerAddresses)); // CustomerAddresses maplendi


			CreateMap<CustomerAddress, CustomerAddressResponse>();
			CreateMap<CustomerAddressRequest, CustomerAddress>();


			CreateMap<CustomerAddress, CustomerAddressResponseWithDetail>() // Customer'ı CustomerResponse olarak mapledim
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer));


			CreateMap<CustomerDetail, CustomerDetailResponse>();
			CreateMap<CustomerDetailRequest, CustomerDetail>();

			CreateMap<CustomerDetail, CustomerDetailResponseWithInclude>() // Customer'ı CustomerResponse olarak mapledim
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer));


			CreateMap<CustomerPhone, CustomerPhoneResponse>();
			CreateMap<CustomerPhoneRequest, CustomerPhone>();

			CreateMap<CustomerPhone, CustomerPhoneResponseWithDetail>()
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer));
		}
	}
}
