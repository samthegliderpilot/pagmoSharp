using System;

namespace pagmo;

internal static class SizeTInterop
{
    internal const ulong ManagedSizeMax = uint.MaxValue;

    internal static uint ToNativeUInt32(ulong value, string parameterName)
    {
        if (value > ManagedSizeMax)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                value,
                $"Value exceeds supported v1 size_t range (max {ManagedSizeMax}).");
        }

        return (uint)value;
    }

    internal static UIntPtr ToNativeUIntPtr(ulong value, string parameterName)
    {
        return new UIntPtr(ToNativeUInt32(value, parameterName));
    }

    internal static ulong ToManagedUInt64(uint value)
    {
        return value;
    }
}
