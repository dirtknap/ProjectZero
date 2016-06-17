using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Composite.Interfaces
{
    public abstract class BaseCompositeDal<T> : DataSource
    {
        protected readonly string schema;
        internal readonly string connectionString;

        protected BaseCompositeDal(string connectionString, string schema = null)
        {
            this.connectionString = connectionString;
            this.schema = schema;
        }
    }
}
