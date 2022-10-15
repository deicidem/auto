namespace Auto.Messages;

public class NewOwnerMessage
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set;}
    public string VehicleRegistation { get; set; }
    public string VehicleModelCode { get; set; }
    public string VehicleModelName { get; set; }
    public DateTime ListedAtUtc { get; set; }
}