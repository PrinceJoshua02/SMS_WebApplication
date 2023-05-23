using SMS_WebApplication.Models;
using SMS_WebApplication.Repositories;
using SMS_WebApplication.ViewModel;
using Microsoft.AspNetCore.Mvc;


namespace SMS_WebApplication.Controllers
{
    public class StudentController : Controller
    {
        IStudentRepository _repo;

        public StudentController(IStudentRepository repo)
        {
            this._repo = repo;
        }
        // Get All
        public IActionResult GetAllStudents()
        {
            string token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case when the token is not available
                return RedirectToAction("Login", "Account");
            }

            var contacts = _repo.GetStudents(token);

            return View(contacts);
        }

        // Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(StudentViewModel newStudent)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("JWToken");
                var info = _repo.AddStudents(newStudent, token);
                return RedirectToAction("GetAllStudents");
            }
            ViewData["Message"] = "Data is not valid to create the Students";
            return View();
        }

        // Update

        [HttpGet]
        public IActionResult Update(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var wishlist = _repo.GetStudentById(id, token);
            if (wishlist == null)
                return NotFound();

            return View(wishlist);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Student updatedStudent)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (id != updatedStudent.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var getEmp = await _repo.UpdateStudent(id, updatedStudent, token);
                if (getEmp != null)
                {
                    if (getEmp.Id != null)
                    {
                        return RedirectToAction(nameof(GetAllStudents), new { id = getEmp.Id, token });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to update the selected student";
                        return View(updatedStudent);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to update the selected student";
                    return View(updatedStudent);
                }
            }

            return View(updatedStudent);
        }

        // Get by ID

        public IActionResult Details(int id)
        {

            var token = HttpContext.Session.GetString("JWToken");
            var emp = _repo.GetStudentById(id, token);

            if (emp is null)
                return NotFound();

            return View(emp);
        }

        public IActionResult Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _repo.DeleteStudent(id, token);
            return RedirectToAction("GetAllStudents");
        }

    }
}
