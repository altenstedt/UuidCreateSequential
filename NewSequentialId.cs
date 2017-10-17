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

            // SQL Server's internal NEWSEQUENTIALID function is a wrapper over the
            // Windows UuidCreateSequential function.  However, NEWSEQUENTIALID does
            // perform some byte shuffling.
            //
            // Since we want to generate identifiers that can be used interchangeably
            // with id values genereated by SQL Server, we need to perform the same
            // byte shuffling.
            //
            // https://blogs.msdn.microsoft.com/dbrowne/2012/07/03/how-to-generate-sequential-guids-for-sql-server-in-net/
            var bytes = guid.ToByteArray();
            var shuffled = new byte[16];
            shuffled[3] = bytes[0];
            shuffled[2] = bytes[1];
            shuffled[1] = bytes[2];
            shuffled[0] = bytes[3];
            shuffled[5] = bytes[4];
            shuffled[4] = bytes[5];
            shuffled[7] = bytes[6];
            shuffled[6] = bytes[7];
            shuffled[8] = bytes[8];
            shuffled[9] = bytes[9];
            shuffled[10] = bytes[10];
            shuffled[11] = bytes[11];
            shuffled[12] = bytes[12];
            shuffled[13] = bytes[13];
            shuffled[14] = bytes[14];
            shuffled[15] = bytes[15];

            return new Guid(shuffled);
        }

        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);
    }
}