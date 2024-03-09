using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using EpiHotel.Models;
using System.Web.Security;

namespace EpiHotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly string connectionString = "Data Source=GABRIELE-PORTAT\\SQLEXPRESS;Initial Catalog=EpiHotel;Integrated Security=True;"; 

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EffettuaLogin(DipendenteModel model)
        {
            if (AutenticazioneUtente(model.Matricola, model.PasswordDipendente))
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(model.Matricola, false);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Matricola o password non validi");
            return View("Index", model);
        }


        private bool AutenticazioneUtente(string matricola, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id FROM Dipendenti WHERE Matricola = @Matricola AND PasswordDipendente = @PasswordDipendente";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Matricola", matricola);
                    cmd.Parameters.AddWithValue("@PasswordDipendente", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Login");
        }
    }
}