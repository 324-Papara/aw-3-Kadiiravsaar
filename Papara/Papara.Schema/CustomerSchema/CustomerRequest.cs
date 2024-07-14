﻿using Papara.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Papara.Schema.CustomerSchema
{
	public class CustomerRequest : BaseRequest
	{
		public int CustomerNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string IdentityNumber { get; set; }
		public string Email { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
