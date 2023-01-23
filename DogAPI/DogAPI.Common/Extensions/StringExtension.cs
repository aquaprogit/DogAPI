namespace DogAPI.Common.Extensions;
public static class StringExtension
{
    public static string Capitalize(this string self)
    {
        return char.ToUpper(self[0]) + self[1..];
    }
}
