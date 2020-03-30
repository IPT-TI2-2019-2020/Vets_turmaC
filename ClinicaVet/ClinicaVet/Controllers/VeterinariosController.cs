using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaVet.Data;
using ClinicaVet.Models;

namespace ClinicaVet.Controllers {

   public class VeterinariosController : Controller {

      /// <summary>
      /// este atributo representa uma referência à nossa base de dados
      /// </summary>
      private readonly VetsDB db;




      public VeterinariosController(VetsDB context) {
         db = context;
      }





      // GET: Veterinarios
      public async Task<IActionResult> Index() {


         // LINQ
         // db.Veterinarios.ToListAsync()  <=>    SELECT * FROM Veterinarios;

         return View(await db.Veterinarios.ToListAsync());
      }





      // GET: Veterinarios/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         var veterinarios = await db.Veterinarios.FirstOrDefaultAsync(m => m.ID == id);

         if (veterinarios == null) {
            return NotFound();
         }

         return View(veterinarios);
      }





      // GET: Veterinarios/Create
      /// <summary>
      /// invocar a View de criação de um novo Veterinário
      /// </summary>
      /// <returns></returns>
      public IActionResult Create() {
         return View();
      }




      // POST: Veterinarios/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      /// <summary>
      /// Concretiza a escrita do novo veterinário
      /// </summary>
      /// <param name="veterinario">dados do novo veterinário</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("ID,Nome,NumCedulaProf,Foto")] Veterinarios veterinario) {

         // falta validar os dados





         if (ModelState.IsValid) {
            // adiciona o novo veterinário à BD, mas na memória do servidor ASP .NET
            db.Add(veterinario);
            // consolida os dados no Servidor BD (commit)
            await db.SaveChangesAsync();

            // redireciona a a~ção para a View do Index
            return RedirectToAction(nameof(Index));
         }

         // qd ocorre um erro, reenvio os dados do veterinário para a view da criação
         return View(veterinario);
      }





      // GET: Veterinarios/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var veterinarios = await db.Veterinarios.FindAsync(id);
         if (veterinarios == null) {
            return NotFound();
         }
         return View(veterinarios);
      }





      // POST: Veterinarios/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,NumCedulaProf,Foto")] Veterinarios veterinarios) {
         if (id != veterinarios.ID) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               db.Update(veterinarios);
               await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!VeterinariosExists(veterinarios.ID)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(veterinarios);
      }





      // GET: Veterinarios/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var veterinarios = await db.Veterinarios
             .FirstOrDefaultAsync(m => m.ID == id);
         if (veterinarios == null) {
            return NotFound();
         }

         return View(veterinarios);
      }




      // POST: Veterinarios/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var veterinarios = await db.Veterinarios.FindAsync(id);
         db.Veterinarios.Remove(veterinarios);
         await db.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }




      private bool VeterinariosExists(int id) {
         return db.Veterinarios.Any(e => e.ID == id);
      }



   }
}
