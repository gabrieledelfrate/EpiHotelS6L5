using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiHotel.Models
{
    public class ClientePrenotazioneModel
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Citta { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        [Display(Name = "Id Camera")]
        public int IdCamera { get; set; }
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