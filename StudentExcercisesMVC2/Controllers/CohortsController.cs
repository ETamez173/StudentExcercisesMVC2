using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentExcercisesMVC2.Models;
using StudentExcercisesMVC2.Models.ViewModels;

namespace StudentExcercisesMVC2.Controllers
{
    public class CohortsController : Controller
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }


        // GET: CohortsController
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort";


                    var reader = cmd.ExecuteReader();
                    var cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        var cohort = new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        cohorts.Add(cohort);
                    }

                    reader.Close();
                    return View(cohorts);
                }

            }

        }

        // GET: CohortsController/Details/5
        // this works
        public ActionResult Details(int id)
        {
            var cohort = GetCohortById(id);
            return View(cohort);
        }

        // GET: CohortsController/Create
        public ActionResult Create()
        {
            var cohortOptions = GetCohortOptions();
            var viewModel = new CohortEditViewModel()
            {

            };
            return View();
        }

        // POST: CohortsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CohortEditViewModel cohort)

        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Cohort (Name)
                                            OUTPUT INSERTED.Id
                                            VALUES (@Name)";

                        cmd.Parameters.Add(new SqlParameter("@name", cohort.Name));

                        var id = (int)cmd.ExecuteScalar();
                        cohort.CohortId = id;

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: CohortsController/Edit/5
        public ActionResult Edit(int id)
        {
            var cohort = GetCohortById(id);
            var cohortOptions = GetCohortOptions();
            var viewModel = new CohortEditViewModel()
            {
                //CohortId = cohort.Id,
                Name = cohort.Name,
                //CohortOptions = cohortOptions
            };

            return View(viewModel);
        }

        // POST: CohortsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CohortEditViewModel cohort)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Cohort
                                            SET Name = @name                                                
                                            WHERE Id  = @id";

                        cmd.Parameters.Add(new SqlParameter("@name", cohort.Name));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        var rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected < 1)
                        {
                            return NotFound();
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CohortsController/Delete/5
        public ActionResult Delete(int id)
        {
            var cohort = GetCohortById(id);
            return View(cohort);
        }

        // POST: CohortsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Cohort cohort)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM Cohort WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteScalar();
                    }
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private List<SelectListItem> GetCohortOptions()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort";

                    var reader = cmd.ExecuteReader();
                    var options = new List<SelectListItem>();

                    while (reader.Read())
                    {
                        var option = new SelectListItem()
                        {
                            Text = reader.GetString(reader.GetOrdinal("Name")),
                            Value = reader.GetInt32(reader.GetOrdinal("Id")).ToString()
                        };
                        options.Add(option);
                    }
                    reader.Close();
                    return options;
                }
            }
        }


        private Cohort GetCohortById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    var reader = cmd.ExecuteReader();
                    Cohort cohort = null;

                    if (reader.Read())
                    {
                        cohort = new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                            //CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),


                        };

                    }
                    reader.Close();
                    return cohort;
                }
            }
        }

    }
}
