using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task GuidsShouldBeSequential()
        {
            var guids = new List<SqlGuid>();

            const int Count = 1000;

            for (var i = 0; i < Count; i++)
            {
                var guid = new SqlGuid(NewGuid());

                guids.Add(guid);

                // The SQL Server DATETIME structure values are rounded to increments of
                // .000, .003, or .007 seconds.  This test assumes that the GUID
                // generation algorithm target the DATETIME structure and we delay the
                // execution with .004 seconds to make sure we get sequential values.
                //
                // Note that increasing this value does not cause the test to pass.
                await Task.Delay(TimeSpan.FromSeconds(0.004));
            }

            var current = new SqlGuid(Guid.Empty);

            Assert.True(guids.Any());

            var result = new List<SqlBoolean>();
            foreach (var guid in guids)
            {
                result.Add(SqlGuid.LessThan(current, guid));

                current = guid;
            }

            Assert.Equal(0, result.Count(item => item.Equals(SqlBoolean.False)));
        }

        private static Guid NewGuid()
        {
            return NewSequentialId.NewGuid();
        }
    }
}
