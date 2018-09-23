using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cachetwo
{
    public class SelfReferencingClass
    {
        public SelfReferencingClass Self { get; set; }
    }

    public class CircularParentClass
    {
        public CircularChildClass Child { get; set; }
    }

    public class CircularChildClass
    {
        public CircularParentClass Parent { get; set; }
    }
}
