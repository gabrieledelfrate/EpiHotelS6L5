﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiHotel.Models
{
    public class DettaglioPrenotazioneViewModel
    {
        public string NumeroCamera { get; set; }
        public DateTime PeriodoDal { get; set; }
        public DateTime PeriodoAl { get; set; }
        public decimal TariffaApplicata { get; set; }
        public string TipoServizio { get; set; }
        public DateTime? DataServizio { get; set; }
        public int? Quantita { get; set; }
        public decimal? Prezzo { get; set; }
        public string NomeCliente { get; set; }
        public string CognomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public decimal ImportoDaSaldare { get; set; } 
    }
}