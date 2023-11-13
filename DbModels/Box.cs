using System.ComponentModel.DataAnnotations;

namespace ASNParserApp.DbModels
{
    public class Box
    {
        [Key]
        public int ID { get; set; }
        public string SupplierIdentifier { get; set; }
        public string Identifier { get; set; }

        public ICollection<BoxContent> Contents { get; set; }
    }
}
