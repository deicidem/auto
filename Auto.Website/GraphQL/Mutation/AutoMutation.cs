using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Mutation;

public class AutoMutation: ObjectGraphType
{
    private readonly IAutoDatabase _db;

    public AutoMutation(IAutoDatabase db)
    {
        this._db = db;
        
        Field<VehicleGraphType>(
            "createVehicle",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "registration"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "color"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "year"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "modelCode"}
            ),
            resolve: context =>
            {
                var registration = context.GetArgument<string>("registration");
                var color = context.GetArgument<string>("color");
                var year = context.GetArgument<int>("year");
                var modelCode = context.GetArgument<string>("modelCode");
                var ownerEmail = context.GetArgument<string>("ownerEmail");

                var vehicleModel = db.FindModel(modelCode);
                var vehicleOwner = db.FindOwner(ownerEmail);
                var vehicle = new Vehicle
                {
                    Registration = registration,
                    Color = color,
                    Year = year,
                    VehicleModel = vehicleModel,
                    VehicleOwner = vehicleOwner,
                    OwnerEmail = vehicleOwner.Email,
                    ModelCode = vehicleModel.Code
                };
                _db.CreateVehicle(vehicle);
                return vehicle;
            }
        );
        
        Field<OwnerGraphType>(
            "createOwner",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "firstName"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "lastName"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "vehicleRegistration"}
            ),
            resolve: context =>
            {
                var email = context.GetArgument<string>("email");
                var firstName = context.GetArgument<string>("firstName");
                var lastName = context.GetArgument<string>("lastName");
                var vehicleRegistration = context.GetArgument<string>("vehicleRegistration");

                var ownerVehicle = db.FindVehicle(vehicleRegistration);
                var owner = new Owner
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    OwnerVehicle = ownerVehicle,
                    VehicleRegistration = ownerVehicle.Registration
                };
                _db.CreateOwner(owner);
                return owner;
            }
        );
    }
}