using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;



namespace TaskManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TaskManagementContext db;

        public CustomerController(TaskManagementContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
        }


        public IActionResult Customer()
        {
            var customers = db.MstrCustomers.ToList();
            if (customers == null || !customers.Any())
            {
                Console.WriteLine("No customers found in the database.");
            }
            return View(customers);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MstrCustomer mstrCustomer)
        {
            if (ModelState.IsValid)
            {
                mstrCustomer.CustomerId = Guid.NewGuid();
                //mstrCustomer.Status = true;

                db.Entry(mstrCustomer).State = EntityState.Added;
                db.SaveChanges();

                return RedirectToAction("Customer");
            }
            return View(mstrCustomer);
        }


        public IActionResult Edit(Guid id)
        {
            var customer = db.MstrCustomers.FirstOrDefault(c => c.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, MstrCustomer mstrCustomer)
        {
            if (id != mstrCustomer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                db.Entry(mstrCustomer).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Customer");
            }
            return View(mstrCustomer);
        }


        //public IActionResult Delete(Guid id)
        //{
        //    var customer = db.MstrCustomers.FirstOrDefault(c => c.CustomerId == id);
        //    if (customer != null)
        //    {
        //        customer.Status = false;
        //        db.Entry(customer).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Customer");
        //}



        public IActionResult Details(Guid id)
        {
            var customer = db.MstrCustomers
                //.Include(c => c.Project)
                .FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
    }
}