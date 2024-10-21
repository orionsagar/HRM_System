using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class Invoice
    {
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBillingAddress { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }

        public IList<InvoiceLine> Lines { get; set; }

        public Invoice()
        {
            Lines = new List<InvoiceLine>();
        }

        public decimal Total
        {
            get
            {
                return Lines.Sum(l => l.Total);
            }
        }

        public decimal Vat
        {
            get
            {
                return Lines.Sum(l => l.Vat);
            }
        }

        public static Invoice GetOne()
        {
            var invoice = new Invoice();
            invoice.CustomerBillingAddress = "Wellington str. 2-311, 10113, NY, USA";
            invoice.CustomerName = "Imaginary Corp.";
            invoice.DueDate = DateTime.Now.AddDays(30);
            invoice.InvoiceDate = DateTime.Now.Date;
            invoice.InvoiceNo = "B383810312-2213";

            var line = new InvoiceLine();
            line.Amount = 12;
            line.LineTitle = "Fancy work desks";
            line.UnitPrice = 800;
            line.VatPercent = 20;
            invoice.Lines.Add(line);

            line = new InvoiceLine();
            line.Amount = 5;
            line.LineTitle = "Espresso machines";
            line.UnitPrice = 200;
            line.VatPercent = 20;
            invoice.Lines.Add(line);

            line = new InvoiceLine();
            line.Amount = 30;
            line.LineTitle = "Meeting room whiteboards";
            line.UnitPrice = 50;
            line.VatPercent = 20;
            invoice.Lines.Add(line);

            return invoice;
        }
    }

    public class InvoiceLine
    {
        public string LineTitle { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount { get; set; }
        public int VatPercent { get; set; }

        public decimal Net
        {
            get
            {
                return UnitPrice * Amount;
            }
        }
        public decimal Total
        {
            get
            {
                return Net * (1 + VatPercent / 100M);
            }
        }
        public decimal Vat
        {
            get
            {
                return Net * (VatPercent / 100M);
            }
        }
    }
}
