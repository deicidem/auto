using Auto.Data.Entities;
using GraphQL.Types;

namespace Auto.Website.GraphQL.GraphTypes;

public sealed class ModelGraphType : ObjectGraphType<Model> {
    public ModelGraphType() {
        Name = "model";
        Field(m => m.Name).Description("Имя модели автомобиля");
        Field(m => m.Manufacturer, type: typeof(ManufacturerGraphType)).Description("Производитель");
    }
}