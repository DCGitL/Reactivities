using System;

namespace Application.Repository.Extensions;

public static class GetMatchingEnumString
{
    public static string GetMatchingEnumStringValue<T>(this string value) where T : Enum
    {
        foreach (var enumValue in Enum.GetValues(typeof(T)))
        {
            if (enumValue == null) continue;
            if (enumValue.ToString()!.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return enumValue.ToString()!;
            }
        }
        return string.Empty;
    }
}
