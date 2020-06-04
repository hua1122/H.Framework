using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lambda.Extension
{
    public class SqlProvider<T> : ISqlProvider<T>
    {
        public string SqlString { get; private set; }


    }

}
