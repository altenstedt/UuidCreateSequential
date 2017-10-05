using System;
using System.Runtime.InteropServices;

namespace Test
{
    public static class NewSequentialId
    {
        public static Guid NewGuid()
        {
            var result = UuidCreateSequential(out var guid);

            if (result != 0)
            {
                throw new InvalidOperationException();
            }

            return guid;
        }

        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);
    }
}