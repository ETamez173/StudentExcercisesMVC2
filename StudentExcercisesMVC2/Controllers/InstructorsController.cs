﻿using System;
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
    public class InstructorsController : Controller
    {

        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
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





        // GET: InstructorsController
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, CohortId, SlackHandle, Specialty FROM Instructor";

                    var reader = cmd.ExecuteReader();
                    var instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        var instructor = new Instructor()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),


                        };
                        instructors.Add(instructor);
                    }
                    reader.Close();
                    return View(instructors);
                }

            }
        }

        // GET: InstructorsController/Details/5
        public ActionResult Details(int id)
        {
            var instructor = GetInstructorById(id);
            return View(instructor);
        }

        // GET: InstructorsController/Create
        public ActionResult Create()
        {
            var cohortOptions = GetCohortOptions();
            var viewModel = new InstructorEditViewModel()
            {
                CohortOptions = cohortOptions
            };
            return View(viewModel);
        }

        // POST: InstructorsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstructorEditViewModel instructor)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Instructor (FirstName, LastName, SlackHandle, Specialty, CohortId)
                                            OUTPUT INSERTED.Id
                                            VALUES (@firstName, @lastName, @slackHandle, @specialty, @cohortId)";

                        cmd.Parameters.Add(new SqlParameter("@firstName", instructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", instructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@slackHandle", instructor.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@specialty", instructor.Specialty));
                        cmd.Parameters.Add(new SqlParameter("@cohortId", instructor.CohortId));

                        var id = (int)cmd.ExecuteScalar();
                        instructor.InstructorId = id;

                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: InstructorsController/Edit/5
        public ActionResult Edit(int id)
        {
            var instructor = GetInstructorById(id);
            var cohortOptions = GetCohortOptions();
            var viewModel = new InstructorEditViewModel()
            {
                InstructorId = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                CohortId = instructor.CohortId,
                Specialty = instructor.Specialty,
                SlackHandle = instructor.SlackHandle,
                CohortOptions = cohortOptions

            };
            return View(viewModel);
        }

        // POST: InstructorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Instructor instructor)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Instructor
                                            SET FirstName = @firstName,
                                                LastName = @lastName,
                                                Specialty = @specialty,
                                                SlackHandle = @slackHandle,
                                                CohortId = @cohortId
                                            WHERE Id  = @id";

                        cmd.Parameters.Add(new SqlParameter("@firstName", instructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", instructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@slackHandle", instructor.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@specialty", instructor.Specialty));
                        cmd.Parameters.Add(new SqlParameter("@cohortId", instructor.CohortId));
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

        // GET: InstructorsController/Delete/5
        public ActionResult Delete(int id)
        {
            var instructor = GetInstructorById(id);
            return View(instructor);
        }

        // POST: InstructorsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Instructor instructor)
        {
            try

            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM Instructor WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
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


        private Instructor GetInstructorById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, CohortId, SlackHandle, Specialty FROM Instructor WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    var reader = cmd.ExecuteReader();
                    Instructor instructor = null;

                    while (reader.Read())
                    {
                        instructor = new Instructor()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),

                        };

                    }
                    reader.Close();
                    return instructor;
                }
            }
        }
    }
}
