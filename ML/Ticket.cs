using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Ticket
    {
        public string IdTienda { get; set; }
        public string IdRegistradora { get; set; }
        public string FechaHoraTicket { get; set; } = string.Format("yyyy-mm-dd hh:mm:ss");
        public TimeOnly HoraTicket { get; set; }
        public int NoTicket { get; set; }
        public decimal ImporteImpuesto { get; set; }
        public decimal Total { get; set; }
        public string FechaCreacion { get; set; }
    }
}
