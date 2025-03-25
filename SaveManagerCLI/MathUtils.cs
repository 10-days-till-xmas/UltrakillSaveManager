using System;

namespace SaveManagerCLI;

internal static class MathUtils
{
    /// <summary>
    /// Limits an <see cref="Array"/> to a smaller array, centred around <paramref name="index"/>, with <paramref name="radius"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array to be limited</param>
    /// <param name="index">the index to center the bounding around</param>
    /// <param name="radius">the offset, on both sides of the index, of the bounds</param>
    /// <returns>a new limited array</returns>
    public static T[] Limit<T>(this T[] array, int index, int radius)
    {
        // TODO: write better documentation
        var range = BoundedRange(array.Length, index, radius);
        return array[range];
    }

    public static Range BoundedRange(int length, int index, int radius)
    {
        if (index > length - 1)
            throw new ArgumentOutOfRangeException(nameof(index));
        if ((radius * 2) + 1 > length - 1)
            return 0..length;

        int lowerBoundRaw = index - radius; // inclusive
        int upperBoundRaw = index + radius + 1; // exclusive, so needs to be 1 bigger
        
        if (lowerBoundRaw < 0)
        {
            int error = 0 - lowerBoundRaw;
            upperBoundRaw += error;
            lowerBoundRaw += error;
        }
        else if (upperBoundRaw > length)
        {
            int error = upperBoundRaw - length;
            lowerBoundRaw -= error;
            upperBoundRaw -= error;
        }
        return lowerBoundRaw..upperBoundRaw;
    }

    public static bool Contains(this Range range, int index)
    {
        return index >= range.Start.Value && index < range.End.Value;
    }
}
