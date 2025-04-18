using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo( "LMSControllerTests" )]
namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {

        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
                var students = (
                from d in db.Departments
                join c in db.Courses on d.Subject equals c.Department
                join cl in db.Classes on c.CatalogId equals cl.Listing
                join e in db.Enrolleds on cl.ClassId equals e.Class
                join s in db.Students on e.Student equals s.UId
                where d.Subject == subject
                    && c.Number == num
                    && cl.Season == season
                    && cl.Year == year
                select new
                {
                    fname = s.FName,
                    lname = s.LName,
                    uid = s.UId,
                    dob = s.Dob,
                    grade = e.Grade
                }
            ).ToList();

            return Json(students);
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
                var query =
                from d in db.Departments
                join c in db.Courses on d.Subject equals c.Department
                join cl in db.Classes on c.CatalogId equals cl.Listing
                join ac in db.AssignmentCategories on cl.ClassId equals ac.InClass
                join a in db.Assignments on ac.CategoryId equals a.Category
                where d.Subject == subject
                    && c.Number == num
                    && cl.Season == season
                    && cl.Year == year
                    && (category == null || ac.Name == category)
                select new
                {
                    aname = a.Name,
                    cname = ac.Name,
                    due = a.Due,
                    submissions = db.Submissions.Count(s => s.Assignment == a.AssignmentId)
                };

            return Json(query.ToList());
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
                    var categories =
                from d in db.Departments
                join c in db.Courses on d.Subject equals c.Department
                join cl in db.Classes on c.CatalogId equals cl.Listing
                join ac in db.AssignmentCategories on cl.ClassId equals ac.InClass
                where d.Subject == subject
                    && c.Number == num
                    && cl.Season == season
                    && cl.Year == year
                select new
                {
                    name = ac.Name,
                    weight = ac.Weight
                };

            return Json(categories.ToList());
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
                    // Get the class associated with the specified subject, number, season, and year
            var classObj =
                (from d in db.Departments
                join c in db.Courses on d.Subject equals c.Department
                join cl in db.Classes on c.CatalogId equals cl.Listing
                where d.Subject == subject
                    && c.Number == num
                    && cl.Season == season
                    && cl.Year == year
                select cl).FirstOrDefault();

            // If class is not found, return false
            if (classObj == null)
            {
                return Json(new { success = false });
            }

            // Check if a category with the same name already exists for this class
            bool categoryExists = db.AssignmentCategories.Any(ac =>
                ac.InClass == classObj.ClassId &&
                ac.Name == category);

            if (categoryExists)
            {
                return Json(new { success = false });
            }

            // Create and add the new category
            AssignmentCategory newCategory = new AssignmentCategory
            {
                InClass = classObj.ClassId,
                Name = category,
                Weight = (uint)catweight
            };

            db.AssignmentCategories.Add(newCategory);
            db.SaveChanges();

            return Json(new { success = true });
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            // Get the class associated with the specified subject, number, season, and year
            var classObj =
                (from d in db.Departments
                join c in db.Courses on d.Subject equals c.Department
                join cl in db.Classes on c.CatalogId equals cl.Listing
                where d.Subject == subject
                    && c.Number == num
                    && cl.Season == season
                    && cl.Year == year
                select cl).FirstOrDefault();

            // If class not found, return false
            if (classObj == null)
            {
                return Json(new { success = false });
            }

            // Get the assignment category for that class
            var catObj = db.AssignmentCategories.FirstOrDefault(ac =>
                ac.InClass == classObj.ClassId &&
                ac.Name == category);

            // If category not found, return false
            if (catObj == null)
            {
                return Json(new { success = false });
            }

            // Check if assignment with the same name already exists in this category
            bool exists = db.Assignments.Any(a =>
                a.Category == catObj.CategoryId &&
                a.Name == asgname);

            if (exists)
            {
                return Json(new { success = false });
            }

            // Create and add the assignment
            Assignment newAssignment = new Assignment
            {
                Name = asgname,
                Contents = asgcontents,
                Due = asgdue,
                MaxPoints = (uint)asgpoints,
                Category = catObj.CategoryId
            };

            db.Assignments.Add(newAssignment);
            db.SaveChanges();

            return Json(new { success = true });
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
                    var submissions =
                from d in db.Departments
                join c in db.Courses on d.Subject equals c.Department
                join cl in db.Classes on c.CatalogId equals cl.Listing
                join ac in db.AssignmentCategories on cl.ClassId equals ac.InClass
                join a in db.Assignments on ac.CategoryId equals a.Category
                join s in db.Submissions on a.AssignmentId equals s.Assignment
                join st in db.Students on s.Student equals st.UId
                where d.Subject == subject
                    && c.Number == num
                    && cl.Season == season
                    && cl.Year == year
                    && ac.Name == category
                    && a.Name == asgname
                select new
                {
                    fname = st.FName,
                    lname = st.LName,
                    uid = st.UId,
                    time = s.Time,
                    score = s.Score
                };

            return Json(submissions.ToList());
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            // Locate the specific assignment submission using joins and filters
            var submission =
                (from d in db.Departments
                join c in db.Courses on d.Subject equals c.Department
                join cl in db.Classes on c.CatalogId equals cl.Listing
                join ac in db.AssignmentCategories on cl.ClassId equals ac.InClass
                join a in db.Assignments on ac.CategoryId equals a.Category
                join s in db.Submissions on a.AssignmentId equals s.Assignment
                where d.Subject == subject
                    && c.Number == num
                    && cl.Season == season
                    && cl.Year == year
                    && ac.Name == category
                    && a.Name == asgname
                    && s.Student == uid
                select s).FirstOrDefault();

            if (submission == null)
            {
                return Json(new { success = false });
            }

            // Update the score
            submission.Score = (uint)score;
            db.SaveChanges();

            return Json(new { success = true });
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {            
                var classes =
                from cl in db.Classes
                join c in db.Courses on cl.Listing equals c.CatalogId
                join d in db.Departments on c.Department equals d.Subject
                where cl.TaughtBy == uid
                select new
                {
                    subject = d.Subject,
                    number = c.Number,
                    name = c.Name,
                    season = cl.Season,
                    year = cl.Year
                };

            return Json(classes.ToList());
        }


        
        /*******End code to modify********/
    }
}

