using System.Collections.Generic;
using System.Linq;

namespace ServiceImportFiles.Models
{
    public class Sale
    {
        public string Id { get; set; }
        public Salesman Salesman { get; set; }
        public List<SaleItem> Itens { get; set; }
        public decimal Total
        {
            get
            {
                if (Itens == null)
                    return 0;

                return Itens.Sum(x => x.Quantity * x.Price);
            }
        }
    }
}
