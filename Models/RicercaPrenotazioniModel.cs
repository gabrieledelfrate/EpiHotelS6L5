using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiHotel.Models
{
    public class RicercaPrenotazioniModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdCamera { get; set; }
        public DateTime DataPrenotazione { get; set; }
        public int NumeroProgressivoAnno { get; set; }
        public int Anno { get; set; }
        public DateTime PeriodoDal { get; set; }
        public DateTime PeriodoAl { get; set; }
        public decimal CaparraConfirmatoria { get; set; }
        public decimal TariffaApplicata { get; set; }
        public bool PernottamentoConColazione { get; set; }
        public bool MezzaPensione { get; set; }
        public bool PensioneCompleta { get; set; }
    }
}