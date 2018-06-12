using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.OracleHelpers.Conventions
{
    /// <summary>
    /// Convention to set maximum length of 2000 for properties of type String so Oracle Data Type will be NVARCHAR2(2000)
    /// </summary>
    public class StringPropertyMaxLengthConvention : Convention
    {
        private const int DefaultLength = 2000;

        /// <summary>
        /// Initializes a new instance with the default length of 2000
        /// </summary>
        public StringPropertyMaxLengthConvention() : this(DefaultLength)
        {
        }

        public StringPropertyMaxLengthConvention(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Invalid Max Length Size");
            }

            Properties<String>().Configure(x => x.HasMaxLength(length));
        }
    }
}