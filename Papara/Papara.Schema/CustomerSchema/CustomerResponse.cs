using Papara.Base.Schema;

namespace Papara.Schema.CustomerSchema
{
	public class CustomerResponse : BaseResponse
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string IdentityNumber { get; set; }
		public string Email { get; set; }
		public int CustomerNumber { get; set; }
		public DateTime DateOfBirth { get; set; }

	}
}
