using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Ticket
    {
        public int IdTicket { get; set; }
        public string IdTienda { get; set; } = null!;
        public string IdRegistradora { get; set; } = null!;
        public DateTime FechaHoraTicket { get; set; }
        public int Ticket1 { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaHoraCreacion { get; set; }
    }
}
