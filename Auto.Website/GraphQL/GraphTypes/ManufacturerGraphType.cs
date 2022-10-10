using Auto.Data.Entities;
using GraphQL.Types;

namespace Auto.Website.GraphQL.GraphTypes;

public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer> {
    public ManufacturerGraphType() {
        Name = "manufacturer";
        Field(c => c.Name).Description("Имя производителя");
    }
}