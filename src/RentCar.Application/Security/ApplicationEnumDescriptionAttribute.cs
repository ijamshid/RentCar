namespace RentCar.Application.Security;

[AttributeUsage(AttributeTargets.Enum)]
public class ApplicationEnumDescriptionAttribute : Attribute
{
    public ApplicationEnumDescriptionAttribute()
    {
    }
}
