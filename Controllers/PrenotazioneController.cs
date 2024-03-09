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
            string query = @"INSERT INTO Clienti (Nome, Cognome, Citta, Provincia, Email, Telefono, CodiceFiscale)
                         VALUES (@Nome, @Cognome, @Citta, @Provincia, @Email, @Telefono, @CodiceFiscale);
                         SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Nome", model.Nome);
                cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                cmd.Parameters.AddWithValue("@Citta", model.Citta);
                cmd.Parameters.AddWithValue("@Provincia", model.Provincia);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Telefono", model.Telefono);
                cmd.Parameters.AddWithValue("@CodiceFiscale", model.CodiceFiscale);

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

        [HttpPost]
        public ActionResult InserisciServizio(int idPrenotazione, string tipoServizio, DateTime data, int quantita, decimal prezzo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO Servizi (IdPrenotazione, TipoServizio, Data, Quantita, Prezzo)
                             VALUES (@IdPrenotazione, @TipoServizio, @Data, @Quantita, @Prezzo)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);
                        cmd.Parameters.AddWithValue("@TipoServizio", tipoServizio);
                        cmd.Parameters.AddWithValue("@Data", data);
                        cmd.Parameters.AddWithValue("@Quantita", quantita);
                        cmd.Parameters.AddWithValue("@Prezzo", prezzo);

                        cmd.ExecuteNonQuery();
                    }
                }

                return Json(new { success = true, message = "Servizio inserito con successo." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Errore durante l'inserimento del servizio." });
            }
        }

        public ActionResult MostraDettaglioPrenotazione(int idPrenotazione)
        {
            string dettaglioPrenotazioneMarkup = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = "mostradettaglioprenotazione";

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        dettaglioPrenotazioneMarkup = $@"
                                    <p>Numero Camera: {reader["NumeroCamera"]}</p>
                                    <p>Nome Cliente: {reader["ClienteNome"]}</p>
                                    <p>Cognome Cliente: {reader["ClienteCognome"]}</p>
                                    <p>Email Cliente: {reader["ClienteEmail"]}</p>
                                    <p>Telefono Cliente: {reader["ClienteTelefono"]}</p>
                                    <p>Periodo Dal: {Convert.ToDateTime(reader["PeriodoDal"]).ToShortDateString()}</p>
                                    <p>Periodo Al: {Convert.ToDateTime(reader["PeriodoAl"]).ToShortDateString()}</p>
                                    <p>Tariffa Applicata: {Convert.ToDecimal(reader["TariffaApplicata"])}</p>                                   
                                    <p>Caparra Confirmatoria: {Convert.ToDecimal(reader["CaparraConfirmatoria"])}</p>
                                    <p>Servizi Aggiuntivi: {reader["ServiziAggiuntivi"]}</p>
                                    <p>Somma Servizi: {Convert.ToDecimal(reader["SommaServizi"])}</p>
                                    <p>Importo da Saldare: {Convert.ToDecimal(reader["ImportoDaSaldare"])}</p>                                    
                                    ";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorMessage = "Errore durante l'esecuzione della stored procedure: " + ex.Message;
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
                ViewBag.ErrorMessage = "Errore durante il recupero dei dettagli della prenotazione: " + ex.Message;
            }

            return Content(dettaglioPrenotazioneMarkup, "text/html");
        }




    }
}