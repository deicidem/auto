using Auto.Data.Entities;
using GraphQL.Types;

namespace Auto.Website.GraphQL.GraphTypes;

public sealed class OwnerGraphType : ObjectGraphType<Owner> {
    public OwnerGraphType() {
        Name = "owner";
        Field(c => c.OwnerVehicle, nullable: false, type: typeof(VehicleGraphType))
            .Description("Номер автомобиля");
        Field(c => c.FirstName);
        Field(c => c.LastName);
        Field(c => c.Email);
    }
}