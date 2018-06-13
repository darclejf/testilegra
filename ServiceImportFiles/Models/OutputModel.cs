namespace ServiceImportFiles.Models
{
    public class OutputModel
    {
        public static Salesman WorstSalesman { get; set; }
        public int QuantityClientInputFile { get; set; }
        public int QuantitySalesmanInputFile { get; set; }
        public string IdExpensiveSale { get; set; }

        public string OutputContent => $"Amount of clients in the input file: {QuantityClientInputFile}\n" +
                $"Amount of salesman in the input file: {QuantitySalesmanInputFile}\n" +
                $"ID of the most expensive sale: {IdExpensiveSale}\n" +
                $"Worst salesman ever: {WorstSalesman.Name}-{WorstSalesman.CPF}";
    }
}
