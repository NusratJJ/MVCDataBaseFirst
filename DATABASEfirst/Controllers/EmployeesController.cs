using DATABASEfirst.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DATABASEfirst.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        protected readonly EMPdbEntities _context = new EMPdbEntities();
        public ActionResult Index()
        {
            var employeesList = _context.Employees.Include(q => q.QualificationEntries.Select(s => s.Skill))
               .OrderByDescending(e => e.EmployeeID).ToList();
            return View(employeesList);
        }
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _context.Employees.Where(x => x.EmployeeID == id).FirstOrDefaultAsync();
            if (employee == null)
            {
                return HttpNotFound("No employee found");
            }
            if (!string.IsNullOrEmpty(employee.Picture))
            {
                string imgPath = Path.Combine("Image", employee.Picture);
                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AddSkills(int? id)
        {
            ViewBag.skill = new SelectList(_context.Skills.ToList(),
                "SkillId", "SkillName", (id != null) ? id.ToString() : "");
            return PartialView("_AddSkills");
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(EmployeeViewModel vobj, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    FirstName = vobj.FirstName,
                    lastName = vobj.lastName,
                    DateOfBirth = vobj.DateOfBirth,
                    Salary = vobj.Salary,
                    IsActive = vobj.IsActive,
                    MobileNo = vobj.MobileNo
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                    string filePath = Path.Combine("/Image/", Guid.NewGuid().ToString())
                        + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(filePath));
                    employee.Picture = filePath;
                }
                foreach (int item in skillId)
                {
                    QualificationEntry q = new QualificationEntry
                    {
                        Employee = employee,
                        EmployeeId = employee.EmployeeID,
                        SkillId = item
                    };
                    _context.QualificationEntries.Add(q);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            else { return View(); }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee employee = _context.Employees.First(x => x.EmployeeID == id);
            var skill = _context.QualificationEntries.Where(x => x.EmployeeId == id).ToList();
            EmployeeViewModel vobj = new EmployeeViewModel
            {
                EmployeeID = employee.EmployeeID,
                FirstName = employee.FirstName,
                lastName = employee.lastName,
                DateOfBirth = employee.DateOfBirth,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                MobileNo = employee.MobileNo,
                Picture = employee.Picture,
            };
            if (skill.Count > 0)
            {
                foreach (var item in skill)
                {
                    vobj.SkillList.Add(item.SkillId);
                }
            }
            return View(vobj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(EmployeeViewModel vobj, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    EmployeeID = vobj.EmployeeID,
                    FirstName = vobj.FirstName,
                    lastName = vobj.lastName,
                    Salary = vobj.Salary,
                    DateOfBirth = vobj.DateOfBirth,
                    IsActive = vobj.IsActive,
                    MobileNo = vobj.MobileNo,
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                    string imagePath = Path.Combine("/Image/", Guid.NewGuid().ToString())
                        + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(imagePath));
                    employee.Picture = imagePath;
                }
                else
                    employee.Picture = vobj.Picture;
                var skill = _context.QualificationEntries
                    .Where(x => x.EmployeeId == employee.EmployeeID).ToList();
                foreach (var item in skill)
                {
                    _context.QualificationEntries.Remove(item);
                }
                foreach (var item in skillId)
                {
                    QualificationEntry Q = new QualificationEntry
                    {
                        EmployeeId = employee.EmployeeID,
                        SkillId = item
                    };
                    _context.QualificationEntries.Add(Q);
                }
                _context.Entry(employee).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return View(); }
        }

    }
}