using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Model
{
    [Table("ProductStockSync")]
    public class ProductStockSync
    {
        public int id { get; set; }
        public string Barcode { get; set; }
        public int IdMenu { get; set; }
        public int IdKiotviet { get; set; }
        public int Idbranch { get; set; }
        public DateTime DateCreated { get; set; }
        public int OnHand { get; set; }
    }

    public class ProductStockSyncMapping
    {
        public class TonKho
        {
            public string Barcode { get; set; }
            public int IdMenu { get; set; }
            public int Idbranch { get; set; }
            public int OnHand { get; set; }
        }
    }
}
