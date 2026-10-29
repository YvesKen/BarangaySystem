using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BarangaySystem.Models;

namespace BarangaySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public IActionResult Index()
        {
            List<Resident> residentList = new List<Resident>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT ResidentID, FirstName, MiddleName, LastName, Suffix, Gender, BirthDate, CivilStatus, AddressLine, Purok, ContactNumber, VoterStatus FROM Residents ORDER BY ResidentID DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            residentList.Add(new Resident
                            {
                                ResidentID = Convert.ToInt32(reader["ResidentID"]),
                                FirstName = reader["FirstName"].ToString() ?? "",
                                MiddleName = reader["MiddleName"]?.ToString(),
                                LastName = reader["LastName"].ToString() ?? "",
                                Suffix = reader["Suffix"]?.ToString(),
                                Gender = reader["Gender"].ToString() ?? "Male",
                                BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                                CivilStatus = reader["CivilStatus"]?.ToString() ?? "Single",
                                AddressLine = reader["AddressLine"]?.ToString() ?? "",
                                Purok = reader["Purok"]?.ToString() ?? "",
                                ContactNumber = reader["ContactNumber"]?.ToString(),
                                VoterStatus = reader["VoterStatus"]?.ToString() ?? "Registered Voter"
                            });
                        }
                    }
                }
            }

            return View(residentList);
        }

        [HttpPost]
        public IActionResult CreateResident(Resident resident)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Residents (FirstName, MiddleName, LastName, Suffix, Gender, BirthDate, CivilStatus, AddressLine, Purok, ContactNumber, VoterStatus) 
                               VALUES (@FirstName, @MiddleName, @LastName, @Suffix, @Gender, @BirthDate, @CivilStatus, @AddressLine, @Purok, @ContactNumber, @VoterStatus)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", resident.FirstName ?? "");
                    cmd.Parameters.AddWithValue("@MiddleName", (object?)resident.MiddleName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", resident.LastName ?? "");
                    cmd.Parameters.AddWithValue("@Suffix", (object?)resident.Suffix ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", resident.Gender ?? "Male");
                    cmd.Parameters.AddWithValue("@BirthDate", resident.BirthDate);
                    cmd.Parameters.AddWithValue("@CivilStatus", resident.CivilStatus ?? "Single");
                    cmd.Parameters.AddWithValue("@AddressLine", (object?)resident.AddressLine ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Purok", resident.Purok ?? "1");
                    cmd.Parameters.AddWithValue("@ContactNumber", (object?)resident.ContactNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@VoterStatus", resident.VoterStatus ?? "Registered Voter");

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditResident(Resident resident)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE Residents 
                       SET FirstName = @FirstName, 
                           MiddleName = @MiddleName, 
                           LastName = @LastName, 
                           Suffix = @Suffix, 
                           Gender = @Gender, 
                           BirthDate = @BirthDate, 
                           CivilStatus = @CivilStatus, 
                           AddressLine = @AddressLine, 
                           Purok = @Purok, 
                           ContactNumber = @ContactNumber, 
                           VoterStatus = @VoterStatus 
                       WHERE ResidentID = @ResidentID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ResidentID", resident.ResidentID);
                    cmd.Parameters.AddWithValue("@FirstName", resident.FirstName ?? "");
                    cmd.Parameters.AddWithValue("@MiddleName", (object?)resident.MiddleName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", resident.LastName ?? "");
                    cmd.Parameters.AddWithValue("@Suffix", (object?)resident.Suffix ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", resident.Gender ?? "Male");
                    cmd.Parameters.AddWithValue("@BirthDate", resident.BirthDate);
                    cmd.Parameters.AddWithValue("@CivilStatus", resident.CivilStatus ?? "Single");
                    cmd.Parameters.AddWithValue("@AddressLine", (object?)resident.AddressLine ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Purok", resident.Purok ?? "1");
                    cmd.Parameters.AddWithValue("@ContactNumber", (object?)resident.ContactNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@VoterStatus", resident.VoterStatus ?? "Registered Voter");

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteResident(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Residents WHERE ResidentID = @ResidentID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ResidentID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult IssueClearance(int residentId, string documentType, string purpose, decimal amountPaid)
        {
            string controlNumber = "CLR-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
            string orNumber = "OR-" + new Random().Next(100000, 999999);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Clearances (ControlNumber, ResidentID, DocumentType, Purpose, ORNumber, AmountPaid, IssuedBy, DateIssued) 
                               VALUES (@ControlNumber, @ResidentID, @DocumentType, @Purpose, @ORNumber, @AmountPaid, @IssuedBy, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ControlNumber", controlNumber);
                    cmd.Parameters.AddWithValue("@ResidentID", residentId);
                    cmd.Parameters.AddWithValue("@DocumentType", documentType ?? "Barangay Clearance");
                    cmd.Parameters.AddWithValue("@Purpose", purpose ?? "For Employment");
                    cmd.Parameters.AddWithValue("@ORNumber", orNumber);
                    cmd.Parameters.AddWithValue("@AmountPaid", amountPaid);
                    cmd.Parameters.AddWithValue("@IssuedBy", "Admin");

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["SuccessMessage"] = $"Document generated successfully! Control No: {controlNumber}";
            return RedirectToAction("Index");
        }
    }
}