using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Resuman
    {
        public int IdResumen { get; set; }
        public string IdTienda { get; set; } = null!;
        public string IdRegistradora { get; set; } = null!;
        public int? Ticket { get; set; }
    }
}
