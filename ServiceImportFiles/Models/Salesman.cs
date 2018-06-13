namespace ServiceImportFiles.Models
{
    public class Salesman : Person
    {
        public string CPF { get; set; }
        public decimal Salary { get; set; } 
        public decimal TotalSalesValue { get; private set; }

        public void UpdateTotalSalesValue(decimal value)
        {
            this.TotalSalesValue += value;
        }
    }
}
