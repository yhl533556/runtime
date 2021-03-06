// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace System.Net.Sockets
{
    internal static class IPAddressExtensions
    {
        public static IPAddress Snapshot(this IPAddress original)
        {
            switch (original.AddressFamily)
            {
                case AddressFamily.InterNetwork:
#pragma warning disable CS0618 // IPAddress.Address is obsoleted, but it's the most efficient way to get the Int32 IPv4 address
                    return new IPAddress(original.Address);
#pragma warning restore CS0618

                case AddressFamily.InterNetworkV6:
                    Span<byte> addressBytes = stackalloc byte[IPAddressParserStatics.IPv6AddressBytes];
                    original.TryWriteBytes(addressBytes, out int bytesWritten);
                    Debug.Assert(bytesWritten == IPAddressParserStatics.IPv6AddressBytes);
                    return new IPAddress(addressBytes, (uint)original.ScopeId);

                default:
                    throw new InternalException(original.AddressFamily);
            }
        }
    }
}
