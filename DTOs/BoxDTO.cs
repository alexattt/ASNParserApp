using ASNParserApp.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNParserApp.DTOs
{
    public class BoxDTO
    {
        public string SupplierIdentifier { get; set; }
        public string Identifier { get; set; }

        public List<BoxContentDTO> Contents { get; set; } = new List<BoxContentDTO>();
    }
}
