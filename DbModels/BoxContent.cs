using System.ComponentModel.DataAnnotations;

namespace ASNParserApp.DbModels
{
    public class BoxContent
    {
        [Key]
        public int ID { get; set; }
        public string PoNumber { get; set; }
        public string Isbn { get; set; }
        public int Quantity { get; set; }

        public int BoxID { get; set; }
        public Box Box { get; set; }

    }
}
