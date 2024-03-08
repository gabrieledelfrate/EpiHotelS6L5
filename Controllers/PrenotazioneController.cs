using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using EpiHotel.Models;
using System.Configuration;
using System.Data;

namespace EpiHotel.Controllers
{
    public class PrenotazioneController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        
        public ActionResult Prenotazione()
        {
            var model = new ClientePrenotazioneModel();
            return View(model);
        }    
       
        [HttpPost]
        public ActionResult Prenotazione(ClientePrenotazioneModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        if (connection.State == ConnectionState.Open)
                        {
                            using (SqlTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    int idCliente = InserisciCliente(connection, transaction, model);

                                    int idCamera = int.Parse(Request.Form["IdCamera"]);

                                    InserisciPrenotazione(connection, transaction, model, idCliente, idCamera);

                                    transaction.Commit();

                                    return RedirectToAction("Index", "Home");
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    ModelState.AddModelError("", "Errore durante il salvataggio dei dati");
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Errore durante l'apertura della connessione al database");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Errore durante l'apertura della connessione al database");
                }
            }

            return View(model);
        }



        [HttpPost]
        private int InserisciCliente(SqlConnection connection, SqlTransaction transaction, ClientePrenotazioneModel model)
        {
            string query = @"INSERT INTO Clienti (Nome, Cognome, Citta, Provincia, Email, Telefono)
                         VALUES (@Nome, @Cognome, @Citta, @Provincia, @Email, @Telefono);
                         SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Nome", model.Nome);
                cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                cmd.Parameters.AddWithValue("@Citta", model.Citta);
                cmd.Parameters.AddWithValue("@Provincia", model.Provincia);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Telefono", model.Telefono);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        [HttpPost]
        private void InserisciPrenotazione(SqlConnection connection, SqlTransaction transaction, ClientePrenotazioneModel model, int idCliente, int idCamera)
        {
            string query = @"INSERT INTO Prenotazioni (IdCliente, IdCamera, DataPrenotazione, NumeroProgressivoAnno, Anno,
                 PeriodoDal, PeriodoAl, CaparraConfirmatoria, TariffaApplicata, PernottamentoConColazione,
                 MezzaPensione, PensioneCompleta)
                 VALUES (@IdCliente, @IdCamera, @DataPrenotazione, @NumeroProgressivoAnno, @Anno,
                 @PeriodoDal, @PeriodoAl, @CaparraConfirmatoria, @TariffaApplicata, @PernottamentoConColazione,
                 @MezzaPensione, @PensioneCompleta);";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                cmd.Parameters.AddWithValue("@IdCamera", idCamera); 
                cmd.Parameters.AddWithValue("@DataPrenotazione", DateTime.Now);
                cmd.Parameters.AddWithValue("@NumeroProgressivoAnno", model.NumeroProgressivoAnno);
                cmd.Parameters.AddWithValue("@Anno", model.Anno);
                cmd.Parameters.AddWithValue("@PeriodoDal", model.PeriodoDal);
                cmd.Parameters.AddWithValue("@PeriodoAl", model.PeriodoAl);
                cmd.Parameters.AddWithValue("@CaparraConfirmatoria", model.CaparraConfirmatoria);
                cmd.Parameters.AddWithValue("@TariffaApplicata", model.TariffaApplicata);
                cmd.Parameters.AddWithValue("@PernottamentoConColazione", model.PernottamentoConColazione);
                cmd.Parameters.AddWithValue("@MezzaPensione", model.MezzaPensione);
                cmd.Parameters.AddWithValue("@PensioneCompleta", model.PensioneCompleta);

                cmd.ExecuteNonQuery();
            }
        }

        public ActionResult ElencoPrenotazioni()
        {
            List<PrenotazioneViewModel> prenotazioniList = new List<PrenotazioneViewModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = @"
                    SELECT 
                        Prenotazioni.Id, 
                        Prenotazioni.IdCliente, 
                        Clienti.Nome AS ClienteNome, 
                        Clienti.Cognome AS ClienteCognome, 
                        Prenotazioni.IdCamera,
                        Prenotazioni.DataPrenotazione,
                        Prenotazioni.NumeroProgressivoAnno,
                        Prenotazioni.Anno,
                        Prenotazioni.PeriodoDal,
                        Prenotazioni.PeriodoAl,
                        Prenotazioni.CaparraConfirmatoria,
                        Prenotazioni.TariffaApplicata,
                        Prenotazioni.PernottamentoConColazione,
                        Prenotazioni.MezzaPensione,
                        Prenotazioni.PensioneCompleta
                    FROM Prenotazioni
                    JOIN Clienti ON Prenotazioni.IdCliente = Clienti.Id";

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    PrenotazioneViewModel prenotazione = new PrenotazioneViewModel
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                        ClienteNome = reader["ClienteNome"].ToString(),
                                        ClienteCognome = reader["ClienteCognome"].ToString(),
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
                ViewBag.ErrorMessage = "Errore durante il recupero delle prenotazioni";
            }

            return View("~/Views/Prenotazione/ElencoPrenotazioni.cshtml", prenotazioniList);
        }


    }
}