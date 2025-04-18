using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
[assembly: InternalsVisibleTo("LMSControllerTests")]
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
            from department in db.Departments
            join course in db.Courses on department.Subject equals course.Department
            join cl in db.Classes on course.CatalogId equals cl.Listing
            join enrolled in db.Enrolleds on cl.ClassId equals enrolled.Class
            join student in db.Students on enrolled.Student equals student.UId
            where department.Subject == subject
                && course.Number == num
                && cl.Season == season
                && cl.Year == year
            select new
            {
                fname = student.FName,
                lname = student.LName,
                uid = student.UId,
                dob = student.Dob,
                grade = enrolled.Grade
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
            from department in db.Departments
            join course in db.Courses on department.Subject equals course.Department
            join cl in db.Classes on course.CatalogId equals cl.Listing
            join assignmentcategory in db.AssignmentCategories on cl.ClassId equals assignmentcategory.InClass
            join assignment in db.Assignments on assignmentcategory.CategoryId equals assignment.Category
            where department.Subject == subject
                && course.Number == num
                && cl.Season == season
                && cl.Year == year
                && (category == null || assignmentcategory.Name == category)
            select new
            {
                aname = assignment.Name,
                cname = assignmentcategory.Name,
                due = assignment.Due,
                submissions = db.Submissions.Count(s => s.Assignment == assignment.AssignmentId)
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
        from department in db.Departments
        join course in db.Courses on department.Subject equals course.Department
        join cl in db.Classes on course.CatalogId equals cl.Listing
        join assignmentcategories in db.AssignmentCategories on cl.ClassId equals assignmentcategories.InClass
        where department.Subject == subject
            && course.Number == num
            && cl.Season == season
            && cl.Year == year
        select new
        {
            name = assignmentcategories.Name,
            weight = assignmentcategories.Weight
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
            var classObj =
                (from departments in db.Departments
                 join course in db.Courses on departments.Subject equals course.Department
                 join cl in db.Classes on course.CatalogId equals cl.Listing
                 where departments.Subject == subject
                     && course.Number == num
                     && cl.Season == season
                     && cl.Year == year
                 select cl).FirstOrDefault();
            if (classObj == null)
            {
                return Json(new { success = false });
            }

            bool categoryExists = db.AssignmentCategories.Any(ac =>
                ac.InClass == classObj.ClassId &&
                ac.Name == category);

            if (categoryExists)
            {
                return Json(new { success = false });
            }

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
            var classObj =
                (from department in db.Departments
                 join course in db.Courses on department.Subject equals course.Department
                 join cl in db.Classes on course.CatalogId equals cl.Listing
                 where department.Subject == subject
                     && course.Number == num
                     && cl.Season == season
                     && cl.Year == year
                 select cl).FirstOrDefault();
            if (classObj == null)
            {
                return Json(new { success = false });
            }

            var catObj = db.AssignmentCategories.FirstOrDefault(ac =>
                ac.InClass == classObj.ClassId &&
                ac.Name == category);

            if (catObj == null)
            {
                return Json(new { success = false });
            }

            bool exists = db.Assignments.Any(a =>
                a.Category == catObj.CategoryId &&
                a.Name == asgname);

            if (exists)
            {
                return Json(new { success = false });
            }
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
        from department in db.Departments
        join c in db.Courses on department.Subject equals c.Department
        join cl in db.Classes on c.CatalogId equals cl.Listing
        join assignmentcategory in db.AssignmentCategories on cl.ClassId equals assignmentcategory.InClass
        join assignment in db.Assignments on assignmentcategory.CategoryId equals assignment.Category
        join submission in db.Submissions on assignment.AssignmentId equals submission.Assignment
        join st in db.Students on submission.Student equals st.UId
        where department.Subject == subject
            && c.Number == num
            && cl.Season == season
            && cl.Year == year
            && assignmentcategory.Name == category
            && assignment.Name == asgname
        select new
        {
            fname = st.FName,
            lname = st.LName,
            uid = st.UId,
            time = submission.Time,
            score = submission.Score
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
            var classId = (from dept in db.Departments
                           where dept.Subject == subject
                           join course in db.Courses on dept.Subject equals course.Department
                           where course.Number == num
                           join cls in db.Classes on course.CatalogId equals cls.Listing
                           where cls.Season == season && cls.Year == year
                           select cls.ClassId).FirstOrDefault();

            if (classId == 0)
                return Json(new { success = false });

            var catId = (from cat in db.AssignmentCategories
                         where cat.Name == category && cat.InClass == classId
                         select cat.CategoryId).FirstOrDefault();

            if (catId == 0)
                return Json(new { success = false });

            var asgId = (from a in db.Assignments
                         where a.Name == asgname && a.Category == catId
                         select a.AssignmentId).FirstOrDefault();

            if (asgId == 0)
                return Json(new { success = false });

            var submission = db.Submissions
                            .Where(s => s.Assignment == asgId && s.Student == uid)
                            .FirstOrDefault();

            if (submission == null)
            {
                submission = new Submission
                {
                    Assignment = asgId,
                    Student = uid,
                    Score = (uint)score,
                    Time = DateTime.Now,
                    SubmissionContents = ""
                };
                db.Submissions.Add(submission);
            }
            else
            {
                submission.Score = (uint)score;
                submission.Time = DateTime.Now;
            }

            db.SaveChanges();

            //recalculated grade of the current student
            var categoryList = db.AssignmentCategories
                                .Where(cat => cat.InClass == classId)
                                .ToList();

            double totalWeightedScore = 0;
            double totalWeight = 0;

            foreach (var cat in categoryList)
            {
                var assignments = db.Assignments
                                    .Where(a => a.Category == cat.CategoryId)
                                    .ToList();

                double catTotalScore = 0;
                double catMaxPoints = 0;

                foreach (var a in assignments)
                {
                    var sub = db.Submissions
                                .Where(s => s.Assignment == a.AssignmentId && s.Student == uid)
                                .FirstOrDefault();

                    if (sub != null)
                    {
                        catTotalScore += sub.Score;
                        catMaxPoints += a.MaxPoints;
                    }
                }
                if (catMaxPoints > 0)
                {
                    double catPercent = catTotalScore / catMaxPoints;
                    totalWeightedScore += catPercent * cat.Weight;
                    totalWeight += cat.Weight;
                }
            }
            //calculate percentage grade
            double finalPercent = totalWeight > 0 ? totalWeightedScore / totalWeight : 0;

            //gets letter grade
            string letterGrade = "--";
            if (finalPercent >= 0.93) letterGrade = "A";
            else if (finalPercent >= 0.90) letterGrade = "A-";
            else if (finalPercent >= 0.87) letterGrade = "B+";
            else if (finalPercent >= 0.83) letterGrade = "B";
            else if (finalPercent >= 0.80) letterGrade = "B-";
            else if (finalPercent >= 0.77) letterGrade = "C+";
            else if (finalPercent >= 0.73) letterGrade = "C";
            else if (finalPercent >= 0.70) letterGrade = "C-";
            else if (finalPercent >= 0.67) letterGrade = "D+";
            else if (finalPercent >= 0.63) letterGrade = "D";
            else if (finalPercent >= 0.60) letterGrade = "D-";
            else letterGrade = "E";

            //updates enrolled table
            var enrollment = db.Enrolleds
                            .Where(e => e.Class == classId && e.Student == uid)
                            .FirstOrDefault();

            if (enrollment != null)
            {
                enrollment.Grade = letterGrade;
            }
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
            join course in db.Courses on cl.Listing equals course.CatalogId
            join department in db.Departments on course.Department equals department.Subject
            where cl.TaughtBy == uid
            select new
            {
                subject = department.Subject,
                number = course.Number,
                name = course.Name,
                season = cl.Season,
                year = cl.Year
            };

            return Json(classes.ToList());
        }

        /*******End code to modify********/
    }
}

