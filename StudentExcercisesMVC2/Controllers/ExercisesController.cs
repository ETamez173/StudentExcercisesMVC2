using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentExcercisesMVC2.Models;
using StudentExcercisesMVC2.Models.ViewModels;

namespace StudentExcercisesMVC2.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IConfiguration _config;

        public ExercisesController(IConfiguration config)
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


        // GET: ExercisesController
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Language FROM Exercise";

                    var reader = cmd.ExecuteReader();
                    var exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        var exercise = new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language")),
                        };

                        exercises.Add(exercise);

                    }
                    reader.Close();

                    return View(exercises);
                }
            }
        }

        // GET: ExercisesController/Details/5
        public ActionResult Details(int id)
        {
            var exercise = GetExerciseById(id);
            return View(exercise);
        }

        // GET: ExercisesController/Create
        public ActionResult Create()
        {
            var viewModel = new ExerciseEditViewModel();
            {

            };
        
            return View(viewModel);
        }

        // POST: ExercisesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExerciseEditViewModel exercise)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Exercise (Name, Language)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name, @language)";

                        cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                        cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));

                        var id = (int)cmd.ExecuteScalar();
                        exercise.ExerciseId = id;

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: ExercisesController/Edit/5
        public ActionResult Edit(int id)
        {
            var exercise = GetExerciseById(id);
            var viewModel = new ExerciseEditViewModel()
            {

                ExerciseId = exercise.Id,
                Name = exercise.Name,
                Language = exercise.Language,


            };


            return View(viewModel);
        }

        // POST: ExercisesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ExerciseEditViewModel exercise)
        {
            try
            {

                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Exercise 
                                           SET Name = @name,
                                               Language = @language
                                           
                                            WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                        cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));
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
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: ExercisesController/Delete/5
        public ActionResult Delete(int id)
        {
            var exercise = GetExerciseById(id);
            return View(exercise);
        }

        // POST: ExercisesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Exercise exercise)
        {
            try
            {

                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM Exercise WHERE Id = @id";
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

        private Exercise GetExerciseById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Language FROM Exercise WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    var reader = cmd.ExecuteReader();
                    Exercise exercise = null;

                    while (reader.Read())
                    {
                        exercise = new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language")),


                        };

                    }
                    reader.Close();
                    return exercise;
                }
            }
        }
    }
}
