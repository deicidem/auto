using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Auto.Website.Models {
	public class VehicleDto {

		public string ModelName { get; set; }

		[HiddenInput(DisplayValue = false)]
		public string ModelCode { get; set; }
		
		[HiddenInput(DisplayValue = false)]
		public string OwnerEmail { get; set; }

		private string registration;

		private static string NormalizeRegistration(string reg) {
			return reg == null ? reg : Regex.Replace(reg.ToUpperInvariant(), "[^A-Z0-9]", "");
		}

		[Required]
		[DisplayName("Registration Plate")]
		public string Registration {
			get => NormalizeRegistration(registration);
			set => registration = value;
		}

		[Required]
		[DisplayName("Year of first registration")]
		[Range(1950, 2022)]
		public int Year { get; set; }

		[Required]
		[DisplayName("Colour")]
		public string Color { get; set; }
	}
}

