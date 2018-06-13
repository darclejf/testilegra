using ServiceImportFiles.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceImportFiles.Service
{
    public class SalesService
    {
        private static IList<Customer> _customers = new List<Customer>();
        private static IList<Salesman> _salesmen = new List<Salesman>();
        private IList<Sale> _sales = new List<Sale>();
        private int _totalCustomersFile = 0;
        private int _totalSalesmanFile = 0;

        public void ReadLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return;

            var lineSplit = line.Split('ç');

            switch (lineSplit[0])
            {
                case "001":
                    this.ReadSalesman(lineSplit);
                    break;
                case "002":
                    this.ReadCustomer(lineSplit);
                    break;
                case "003":
                    this.ReadSale(lineSplit);
                    break;
            }
        }

        public OutputModel GenerateOutput()
        {
            OutputModel.WorstSalesman = this.WorstSalesman();
            return new OutputModel
            {
                QuantityClientInputFile = _totalCustomersFile,
                QuantitySalesmanInputFile = _totalSalesmanFile,
                IdExpensiveSale = this.ExpensiveSale().Id
            };
        }

        private void ReadCustomer(string[] line)
        {
            try
            {
                var customer = _customers.FirstOrDefault(q => q.CNPJ == line[1]);
                if (customer == null)
                {
                    customer = new Customer();
                    customer.CNPJ = line[1];
                    _customers.Add(customer);
                }
                customer.Name = line[2];
                customer.BusinessArea = line[3];
                _totalCustomersFile++;
            }
            catch
            {
                throw new Exception("Invalid Customer");
            }
        }

        private void ReadSalesman(string[] line)
        {
            try
            {
                var salesman = _salesmen.FirstOrDefault(q => q.CPF == line[1]);
                if (salesman == null)
                {
                    salesman = new Salesman();
                    salesman.CPF = line[1];
                    _salesmen.Add(salesman);
                }
                salesman.Name = line[2];
                salesman.Salary = ParseDecimal(line[3]);
                _totalSalesmanFile++;
            }
            catch
            {
                throw new Exception("Invalid Salesman");
            }
        }

        private void ReadSale(string[] line)
        {
            try
            {
                var sale = new Sale();
                sale.Itens = this.StringToSaleItem(line[2]);
                sale.Id = line[1];
                sale.Salesman = this.FindByName(line[3]);
                sale.Salesman.UpdateTotalSalesValue(sale.Total);
                this._sales.Add(sale);
            }
            catch
            {
                throw new Exception("Invalid Sale");
            }
        }

        private List<SaleItem> StringToSaleItem(string itens)
        {
            string[] itensSplit = itens.Replace("[", "").Replace("]", "").Split(',');
            var result = new List<SaleItem>();

            foreach (var item in itensSplit)
            {
                string[] itemArray = item.Split('-');
                result.Add(new SaleItem()
                {
                    Id = itemArray[0],
                    Quantity = int.Parse(itemArray[1]),
                    Price = ParseDecimal(itemArray[2])
                });
            }
            return result;
        }

        private Salesman FindByName(string name)
        {
            return _salesmen.First(q => q.Name.ToLower() == name.ToLower());
        }

        private Sale ExpensiveSale()
        {
            return this._sales.OrderByDescending(x => x.Total).FirstOrDefault();
        }

        private Salesman WorstSalesman()
        {
            return _salesmen.OrderBy(q => q.TotalSalesValue).FirstOrDefault();
        }

        private decimal ParseDecimal(string value)
        {
            return decimal.Parse(value, System.Globalization.CultureInfo.GetCultureInfo("en"));
        }
    }
}
