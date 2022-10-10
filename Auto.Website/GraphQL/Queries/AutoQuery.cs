using System;
using System.Collections.Generic;
using System.Linq;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Queries;

public class AutoQuery : ObjectGraphType {
    private readonly IAutoDatabase _db;

    public AutoQuery(IAutoDatabase db) {
        this._db = db;

        // Vehicles
        Field<ListGraphType<VehicleGraphType>>("Vehicles", "Запрос возвращающий все автомобили",
            resolve: GetAllVehicles);

        Field<VehicleGraphType>("Vehicle", "Запрос к конкретному автомобилю",
            new QueryArguments(MakeNonNullStringArgument("registration", "Номера машины")),
            resolve: GetVehicle);

        Field<ListGraphType<VehicleGraphType>>("VehiclesByColor", "Запрос возвращающий все машины с выбранным цветом",
            new QueryArguments(MakeNonNullStringArgument("color", "Имя цвета")),
            resolve: GetVehiclesByColor);
        
        // Owners
        Field<ListGraphType<OwnerGraphType>>("Owners", "Запрос возвращающий всех владельцев автомобилей",
            resolve: GetAllOwners);

        Field<OwnerGraphType>("Owner", "Запрос к конкретному владельцу",
            new QueryArguments(MakeNonNullStringArgument("email", "E-mail")),
            resolve: GetOwner);

    }
    

    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }
    
    // Vehicles
    private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => _db.ListVehicles();

    private Vehicle GetVehicle(IResolveFieldContext<object> context) {
        var registration = context.GetArgument<string>("registration");
        return _db.FindVehicle(registration);
    }

    private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
        var color = context.GetArgument<string>("color");
        var vehicles = _db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        return vehicles;
    }
    

    // Owners
    private IEnumerable<Owner> GetAllOwners(IResolveFieldContext<object> context) => _db.ListOwners();

    private Owner GetOwner(IResolveFieldContext<object> context) {
        var email = context.GetArgument<string>("email");
        return _db.FindOwner(email);
    }
}