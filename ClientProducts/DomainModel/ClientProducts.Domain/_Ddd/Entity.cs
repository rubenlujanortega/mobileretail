using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProducts.Domain._Ddd
{
    public abstract class Entity<T>
    {
        public T Id { get; protected set; }
    }
}
