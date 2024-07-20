using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Papara.Base.Response;
using Papara.Data.Domain;
using Papara.Schema.CustomerAddressSchema;
using Papara.Schema.CustomerDetailSchema;
using Papara.Schema.CustomerPhoneSchema;
using Papara.Schema.CustomerSchema;


[Route("api/[controller]")]
[ApiController]
public class CustomerReportController : ControllerBase
{
	private readonly IConfiguration _configuration;

	public CustomerReportController(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	[HttpGet]
	public async Task<IActionResult> GetCustomerReport()
	{
		var connectionString = _configuration.GetConnectionString("MsSqlConnection");

		using var connection = new SqlConnection(connectionString);
		var sql = @"
            SELECT 
                c.Id, c.FirstName, c.LastName, c.IdentityNumber, c.Email, c.CustomerNumber, c.DateOfBirth,
                cd.Id AS CustomerDetailId, cd.FatherName, cd.MotherName, cd.EducationStatus, cd.MonthlyIncome, cd.Occupation,
                ca.Id AS CustomerAddressId, ca.Country, ca.City, ca.AddressLine, ca.ZipCode, ca.IsDefault AS IsAddressDefault,
                cp.Id AS CustomerPhoneId, cp.CountryCode, cp.Phone, cp.IsDefault AS IsPhoneDefault
            FROM Customers c
            LEFT JOIN CustomerDetails cd ON c.Id = cd.CustomerId
            LEFT JOIN CustomerAddresses ca ON c.Id = ca.CustomerId
            LEFT JOIN CustomerPhones cp ON c.Id = cp.CustomerId
            WHERE c.IsActive = 1";

		var customerDictionary = new Dictionary<long, CustomerResponseWithDetail>();

		var result = await connection.QueryAsync<CustomerResponseWithDetail, CustomerDetailResponse, CustomerAddressResponse, CustomerPhoneResponse, CustomerResponseWithDetail>(
			sql,
			(customer, detail, address, phone) =>
			{
				if (!customerDictionary.TryGetValue(customer.Id, out var currentCustomer))
				{
					currentCustomer = customer;
					currentCustomer.CustomerAddresses = new List<CustomerAddressResponse>();
					currentCustomer.CustomerPhones = new List<CustomerPhoneResponse>();
					customerDictionary.Add(currentCustomer.Id, currentCustomer);
				}

				if (detail != null)
					currentCustomer.CustomerDetail = detail;

				if (address != null)
					currentCustomer.CustomerAddresses.Add(address);

				if (phone != null)
					currentCustomer.CustomerPhones.Add(phone);
				

				return currentCustomer;
			},
			splitOn: "CustomerDetailId,CustomerAddressId,CustomerPhoneId");

		if (result == null || !result.Any())
		{
			return NotFound(new ApiResponse<List<CustomerResponseWithDetail>>
			{
				IsSuccess = false,
				Message = "No customers found.",
				Errors = new List<string> { "No data available." }
			});
		}

		var customers = customerDictionary.Values.ToList();
		return Ok(new ApiResponse<List<CustomerResponseWithDetail>>(customers));
	}
}