using EpiHotel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiHotel.Controllers
{
    public class RicercaPrenotazioniController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult PrenotazioniPerCliente(string codiceFiscale)
        {
            List<RicercaPrenotazioniModel> prenotazioniList = new List<RicercaPrenotazioniModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = @"
                            SELECT *
                            FROM Prenotazioni
                            WHERE IdCliente = (SELECT Id FROM Clienti WHERE CodiceFiscale = @CodiceFiscale)
                        ";

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    RicercaPrenotazioniModel prenotazione = new RicercaPrenotazioniModel
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                        IdCamera = Convert.ToInt32(reader["IdCamera"]),
                                        DataPrenotazione = Convert.ToDateTime(reader["DataPrenotazione"]),
                                        NumeroProgressivoAnno = Convert.ToInt32(reader["NumeroProgressivoAnno"]),
                                        Anno = Convert.ToInt32(reader["Anno"]),
                                        PeriodoDal = Convert.ToDateTime(reader["PeriodoDal"]),
                                        PeriodoAl = Convert.ToDateTime(reader["PeriodoAl"]),
                                        CaparraConfirmatoria = Convert.ToDecimal(reader["CaparraConfirmatoria"]),
                                        TariffaApplicata = Convert.ToDecimal(reader["TariffaApplicata"]),
                                        PernottamentoConColazione = Convert.ToBoolean(reader["PernottamentoConColazione"]),
                                        MezzaPensione = Convert.ToBoolean(reader["MezzaPensione"]),
                                        PensioneCompleta = Convert.ToBoolean(reader["PensioneCompleta"])
                                    };

                                    prenotazioniList.Add(prenotazione);
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Errore durante l'apertura della connessione al database";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Errore durante il recupero delle prenotazioni per il cliente: " + ex.Message;
            }

            return View(prenotazioniList);
        }

        public ActionResult NumeroPrenotazioniPensioneCompleta()
        {
            int numeroPrenotazioniPensioneCompleta = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = @"
                            SELECT COUNT(*) AS NumeroPrenotazioni
                            FROM Prenotazioni
                            WHERE PensioneCompleta = 1
                        ";

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    numeroPrenotazioniPensioneCompleta = Convert.ToInt32(reader["NumeroPrenotazioni"]);
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Errore durante l'apertura della connessione al database";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Errore durante il recupero del numero di prenotazioni per pensione completa: " + ex.Message;
            }

            return View(numeroPrenotazioniPensioneCompleta);
        }
    }
}