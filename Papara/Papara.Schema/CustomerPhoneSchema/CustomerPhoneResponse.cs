using Papara.Base.Schema;

namespace Papara.Schema.CustomerPhoneSchema
{
	public class CustomerPhoneResponse : BaseResponse
	{
		public long CustomerId { get; set; }
		public string CountryCode { get; set; }
		public string Phone { get; set; }
		public bool IsDefault { get; set; }

	}
}
