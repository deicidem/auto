using System.Collections.Generic;
using Auto.Data.Entities;

namespace Auto.Data {
	public interface IAutoDatabase {
		
		public int CountVehicles();
		public int CountOwners();
		public IEnumerable<Vehicle> ListVehicles();
		public IEnumerable<Manufacturer> ListManufacturers();
		public IEnumerable<Model> ListModels();
		public IEnumerable<Owner> ListOwners();

		public Vehicle FindVehicle(string registration);
		public Model FindModel(string code);
		public Manufacturer FindManufacturer(string code);
		public Owner FindOwner(string email);

		public void CreateVehicle(Vehicle vehicle);
		public void UpdateVehicle(Vehicle vehicle);
		public void DeleteVehicle(Vehicle vehicle);
		public void CreateOwner(Owner owner);
		public void UpdateOwner(Owner owner);
		public void DeleteOwner(Owner owner);
	}
}
