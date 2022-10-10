using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Auto.Website.Models {
	public class OwnerDto {
		private string registration;

		private static string NormalizeRegistration(string reg) {
			return reg == null ? reg : Regex.Replace(reg.ToUpperInvariant(), "[^A-Z0-9]", "");
		}

		[DisplayName("Vehicle Registration Plate")]
		[HiddenInput(DisplayValue = false)]
		public string VehicleRegistration {
			get => NormalizeRegistration(registration);
			set => registration = value;
		}
		
		[Required]
		[DisplayName("Email")]
		public string Email { get; set; }

		[Required]
		[DisplayName("FirstName")]
		public string FirstName { get; set; }

		[Required]
		[DisplayName("LastName")]
		public string LastName { get; set; }
	}
}

