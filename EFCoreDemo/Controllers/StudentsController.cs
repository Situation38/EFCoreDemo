using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Models;

namespace EFCoreDemo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly EFCoreDemoContext _context;
        //Utiliser pour la lecture et injecter par le controlleur

        public StudentsController(EFCoreDemoContext context) 

        {
            _context = context;
        }

       
 


        // GET: StudentsWithCoursesAndEmail
        public async Task<IActionResult>GetStudentsWithCoursesAndEmails()
        //pour afficher la liste des etudiants et leur mails
        //on va generer une jointure au niveau de sql avec include
        {
            return View("Index", await _context.Students
                .Include(s => s.Emails)
                .Include(s => s.Courses)
                .ToListAsync());

            //duLINQ = EF Traduit du SQL en LINQ
        }





        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
            //Pour recuper un etudiant  grace a son Id
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }



        //**************************************************************************************************
        //                                  student controller creation



        // GET: Students
        public async Task<IActionResult> Index()
        //pour afficher la liste des etudiants
        {
            return View(await _context.Students.ToListAsync());//duLINQ = EF Traduit du SQL en LINQ
        }
        // GET: Students/Create
        public IActionResult Create()
            //CREATION DUN etudiant
        {
            return View();
        }

        // POST : Étudiants/Créer
        // Pour vous protéger des attaques de surpublication, activez les propriétés spécifiques auxquelles vous souhaitez vous lier.
        // Pour plus de détails, voir http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]//Le jeton anti-contrefaçon peut être utilisé pour aider à protéger votre application contre la falsification des requêtes intersites


        public async Task<IActionResult> Create([Bind("Id,Name,Email")] Student student)
           
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        //modification d'un etudiant
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            //Supprimer un etudiant
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
            //pour verifier si un etudiant existe ou pas
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }


    //contrôleur simple qui fait écho à toute chaîne envoyée dans le cadre de l'URL
    

    [ApiController]
    [Route("[controller]")]
    public class EchoController : ControllerBase
    {
        [HttpGet("{str}")]
        public string Get(string str)
        {

            
            return str;
        }
    }  




}
