using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Auto.Data.Entities;
using static System.Int32;

namespace Auto.Data {
    public class AutoCsvFileDatabase : IAutoDatabase {
        private static readonly IEqualityComparer<string> Collation = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, Manufacturer> _manufacturers = new Dictionary<string, Manufacturer>(Collation);
        private readonly Dictionary<string, Model> _models = new Dictionary<string, Model>(Collation);
        private readonly Dictionary<string, Vehicle> _vehicles = new Dictionary<string, Vehicle>(Collation);
        private readonly Dictionary<string, Owner> _owners = new Dictionary<string, Owner>(Collation);
        private readonly ILogger<AutoCsvFileDatabase> _logger;

        public AutoCsvFileDatabase(ILogger<AutoCsvFileDatabase> logger) {
            this._logger = logger;
            ReadManufacturersFromCsvFile("manufacturers.csv");
            ReadModelsFromCsvFile("models.csv");
            ReadVehiclesFromCsvFile("vehicles.csv");
            ReadOwnersFromCsvFile("owners.csv");
            ResolveReferences();
        }

        private void ResolveReferences() {
            foreach (var mfr in _manufacturers.Values) {
                mfr.Models = _models.Values.Where(m => m.ManufacturerCode == mfr.Code).ToList();
                foreach (var model in mfr.Models) model.Manufacturer = mfr;
            }

            foreach (var model in _models.Values) {
                model.Vehicles = _vehicles.Values.Where(v => v.ModelCode == model.Code).ToList();
                foreach (var vehicle in model.Vehicles) vehicle.VehicleModel = model;
            }
            
            foreach (var owner in _owners.Values)
            {
                owner.OwnerVehicle = _vehicles.Values.FirstOrDefault(v => v.OwnerEmail == owner.Email);
                if (owner.OwnerVehicle != null) owner.OwnerVehicle.VehicleOwner = owner;
            }
        }

        private string ResolveCsvFilePath(string filename) {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvFilePath = Path.Combine(directory, "csv-data");
            return Path.Combine(csvFilePath, filename);
        }

        private void ReadVehiclesFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var vehicle = new Vehicle {
                    Registration = tokens[0],
                    ModelCode = tokens[1],
                    Color = tokens[2],
                    OwnerEmail = tokens.Length > 4 ? tokens[4] : null
                };
                if (TryParse(tokens[3], out var year)) vehicle.Year = year;
                _vehicles[vehicle.Registration] = vehicle;
            }
            _logger.LogInformation($"Loaded {_vehicles.Count} models from {filePath}");
        }
        
        private void ReadOwnersFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var owner = new Owner {
                    Email = tokens[0],
                    FirstName = tokens[1],
                    LastName = tokens[2],
                    VehicleRegistration = tokens[3]
                };
                _owners[owner.Email] = owner;
            }
            _logger.LogInformation($"Loaded {_owners.Count} models from {filePath}");
        }

        private void ReadModelsFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var model = new Model {
                    Code = tokens[0],
                    ManufacturerCode = tokens[1],
                    Name = tokens[2]
                };
                _models.Add(model.Code, model);
            }
            _logger.LogInformation($"Loaded {_models.Count} models from {filePath}");
        }

        private void ReadManufacturersFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var mfr = new Manufacturer {
                    Code = tokens[0],
                    Name = tokens[1]
                };
                _manufacturers.Add(mfr.Code, mfr);
            }
            _logger.LogInformation($"Loaded {_manufacturers.Count} manufacturers from {filePath}");
        }

        public int CountVehicles() => _vehicles.Count;
        public int CountOwners() => _owners.Count;

        public IEnumerable<Vehicle> ListVehicles() => _vehicles.Values;

        public IEnumerable<Manufacturer> ListManufacturers() => _manufacturers.Values;

        public IEnumerable<Model> ListModels() => _models.Values;
        public IEnumerable<Owner> ListOwners() => _owners.Values;

        public Vehicle FindVehicle(string registration) => _vehicles.GetValueOrDefault(registration);

        public Model FindModel(string code) => _models.GetValueOrDefault(code);
        public Owner FindOwner(string code) => _owners.GetValueOrDefault(code);

        public Manufacturer FindManufacturer(string code) => _manufacturers.GetValueOrDefault(code);

        public void CreateVehicle(Vehicle vehicle) {
            vehicle.ModelCode = vehicle.VehicleModel.Code;
            vehicle.OwnerEmail = vehicle.VehicleOwner?.Email;
            
            vehicle.VehicleModel.Vehicles.Add(vehicle);
            if (vehicle.VehicleOwner != null) vehicle.VehicleOwner.OwnerVehicle = vehicle;

            UpdateVehicle(vehicle);
        }

        public void UpdateVehicle(Vehicle vehicle) {
            _vehicles[vehicle.Registration] = vehicle;
        }

        public void DeleteVehicle(Vehicle vehicle) {
            var model = FindModel(vehicle.ModelCode);
            model.Vehicles.Remove(vehicle);
            
            var owner = FindOwner(vehicle.OwnerEmail);
            owner.OwnerVehicle = null;
            
            _vehicles.Remove(vehicle.Registration);
        }
        
        public void CreateOwner(Owner owner) {
            owner.OwnerVehicle = FindVehicle(owner.VehicleRegistration);
            UpdateOwner(owner);
        }

        public void UpdateOwner(Owner owner) {
            _owners[owner.Email] = owner;
        }

        public void DeleteOwner(Owner owner)
        {
            if (owner.OwnerVehicle != null)
            {
                owner.OwnerVehicle.OwnerEmail = null;
                owner.OwnerVehicle.VehicleOwner = null;
            }

            _owners.Remove(owner.Email);
        }
    }
}