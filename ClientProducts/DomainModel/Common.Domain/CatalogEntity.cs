using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class CatalogEntity
    {
        public CatalogEntity()
        {
            Identifier = string.Empty;
            Name = string.Empty;
        }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                Identifier,
                Name
            };
        }
    }
}
