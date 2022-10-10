using Auto.Data.Entities;
using GraphQL.Types;

namespace Auto.Website.GraphQL.GraphTypes;

public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
    public VehicleGraphType() {
        Name = "vehicle";
        Field(c => c.VehicleModel, nullable: false, type: typeof(ModelGraphType))
            .Description("Модель автомобиля");
        Field(c => c.VehicleOwner, nullable: false, type: typeof(OwnerGraphType))
            .Description("Владелец автомобиля");
        Field(c => c.Registration);
        Field(c => c.Color);
        Field(c => c.Year);
    }
}