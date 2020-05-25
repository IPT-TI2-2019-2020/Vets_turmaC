using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaVet.Data;
using ClinicaVet.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaVet.Controllers {

   [Authorize]
   public class VeterinariosController : Controller {

      /// <summary>
      /// este atributo representa uma referência à nossa base de dados
      /// </summary>
      private readonly VetsDbContext db;

      /// <summary>
      /// atributo que recolhe nele os dados do Servidor
      /// </summary>
      private readonly IWebHostEnvironment _caminho;


      public VeterinariosController(VetsDbContext context, IWebHostEnvironment caminho) {
         this.db = context;
         this._caminho = caminho;
      }





      // GET: Veterinarios
      [AllowAnonymous]
      public async Task<IActionResult> Index() {

         // LINQ
         // db.Veterinarios.ToListAsync()  <=>    SELECT * FROM Veterinarios;

         return View(await db.Veterinarios.ToListAsync());
      }


      // GET: Veterinarios
      [AllowAnonymous]
      public async Task<IActionResult> Index2() {

         // LINQ
         // db.Veterinarios.ToListAsync()  <=>    SELECT * FROM Veterinarios;

         return View(await db.Veterinarios.ToListAsync());
      }


      // GET: Veterinarios/Details/5
      /// <summary>
      /// Mostra os detalhes de um Veterinário, usando Lazy Loading
      /// </summary>
      /// <param name="id">valor da PK do veterinário. Admite um valor Null, por causa do sinal ? </param>
      /// <returns></returns>
      [AllowAnonymous]
      public async Task<IActionResult> Details(int? id) {

         if (id == null) {
            // se o ID é null, é porque o meu utilizador está a testar a minha aplicação
            // redireciono para o método INDEX deste mesmo controller
            return RedirectToAction("Index");
         }

         // esta expressão db.Veterinarios.FirstOrDefaultAsync(m => m.ID == id)
         // é uma forma diferente de escrever o seguinte comando
         // SELECT * FROM db.Veterinarios v WHERE v.ID = id
         // esta expressão é escrita em LINQ
         var veterinario = await db.Veterinarios.FirstOrDefaultAsync(v => v.ID == id);

         if (veterinario == null) {
            // se o ID é null, é porque o meu utilizador está a testar a minha aplicação
            // ele introduziu manualmente um valor inexistente
            // redireciono para o método INDEX deste mesmo controller
            return RedirectToAction("Index");
         }

         return View(veterinario);
      }



      // GET: Veterinarios/Details/5
      /// <summary>
      /// Mostra os detalhes de um Veterinário, usando Eager Loading
      /// </summary>
      /// <param name="id">valor da PK do veterinário. Admite um valor Null, por causa do sinal ? </param>
      /// <returns></returns>

      [Authorize]
      public async Task<IActionResult> Details2(int? id) {

         if (id == null) {
            return RedirectToAction("Index");
         }

         // é uma forma diferente de escrever o seguinte comando
         /// SELECT * 
         /// FROM db.Veterinarios v, db.Consultas c, db.Animais a, db.Donos d
         /// WHERE c.VeterinarioFK=v.ID AND
         ///       c.AnimalFK=a.ID AND
         ///       a.AnimalFK=d.ID AND
         ///       v.ID = id

         // esta expressão é escrita em LINQ
         var veterinario = await db.Veterinarios
                                   .Include(v => v.Consultas)
                                   .ThenInclude(a => a.Animal)
                                   .ThenInclude(d => d.Dono)
                                   .FirstOrDefaultAsync(v => v.ID == id);


         var vvv = from v in db.Veterinarios
                   select v;



         if (veterinario == null) {
            return RedirectToAction("Index");
         }

         return View(veterinario);
      }



      // GET: Veterinarios/Create
      /// <summary>
      /// invocar a View de criação de um novo Veterinário
      /// </summary>
      /// <returns></returns>

      [Authorize(Roles = "Administrativo")]
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
      [Authorize(Roles = "Administrativo")]
      public async Task<IActionResult> Create([Bind("ID,Nome,NumCedulaProf,Foto")] Veterinarios veterinario, IFormFile fotoVet) {

         //***************************************
         // processar a imagem
         //***************************************

         // vars. auxiliares
         bool haFicheiro = false;
         string caminhoCompleto = "";

         // será q há imagem?
         if (fotoVet == null) {
            // o utilizador não fez upload de um ficheiro
            veterinario.Foto = "avatar.png";
         }
         else {
            // existe fotografia.
            // Mas, será boa?
            if (fotoVet.ContentType == "image/jpeg" ||
                fotoVet.ContentType == "image/png") {
               // estamos perante uma boa foto
               // temos de gerar um nome para o ficheiro
               Guid g;
               g = Guid.NewGuid();
               // obter a extensão do ficheiro
               string extensao = Path.GetExtension(fotoVet.FileName).ToLower();
               string nomeFicheiro = g.ToString() + extensao;
               // onde guardar o ficheiro
               caminhoCompleto = Path.Combine(_caminho.WebRootPath, "imagens\\vets", nomeFicheiro);
               // atribuir o nome do ficheiro ao Veterinário
               veterinario.Foto = nomeFicheiro;
               // marcar q existe uma fotografia
               haFicheiro = true;
            }
            else {
               // o ficheiro não é válido
               veterinario.Foto = "avatar.png";
            }
         }

         try {
            if (ModelState.IsValid) {
               // adiciona o novo veterinário à BD, mas na memória do servidor ASP .NET
               db.Add(veterinario);
               // consolida os dados no Servidor BD (commit)
               await db.SaveChangesAsync();
               // será q há foto para gravar?
               if (haFicheiro) {
                  using var stream = new FileStream(caminhoCompleto, FileMode.Create);
                  await fotoVet.CopyToAsync(stream);
               }
               // redireciona a ação para a View do Index
               return RedirectToAction(nameof(Index));
            }
         }
         catch (Exception) {

            throw;
         }

         // qd ocorre um erro, reenvio os dados do veterinário para a view da criação
         return View(veterinario);
      }





      // GET: Veterinarios/Edit/5
      //  [AllowAnonymous]  // anula a obrigação de o User estar autenticado
      //  [Authorize]       // qq pessoa autenticada, tem acesso a este recurso

      [Authorize(Roles = "Administrativo,Veterinario")]  // qq pessoa que pertenca a UM destes Roles, pode aceder ao recurso

      ////  [Authorize(Roles = "Administrativo")]   // se se quiser que o User seja EM SIMULTÂNEO Veterinário e Administrativo
      ////  [Authorize(Roles = "Veterinario")]      // é preciso adicionar estas duas anotações
      public async Task<IActionResult> Edit(int? id) {

         if (id == null) {
            // se o ID é null, é porque o meu utilizador está a testar a minha aplicação
            // redireciono para o método INDEX deste mesmo controller
            return RedirectToAction("Index");
         }

         // esta expressão db.Veterinarios.FindAsync(id)
         // é uma forma diferente de escrever o seguinte comando
         // SELECT * FROM db.Veterinarios v WHERE v.ID = id
         // esta expressão é escrita em LINQ
         var veterinario = await db.Veterinarios.FindAsync(id);

         if (veterinario == null) {
            // se o ID é null, é porque o meu utilizador está a testar a minha aplicação
            // ele introduziu manualmente um valor inexistente
            // redireciono para o método INDEX deste mesmo controller
            return RedirectToAction("Index");
         }
         return View(veterinario);
      }





      // POST: Veterinarios/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize(Roles = "Administrativo,Veterinario")]
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
      [Authorize(Roles = "Administrativo")]
      public async Task<IActionResult> Delete(int? id) {

         if (id == null) {
            // se o ID é null, é porque o meu utilizador está a testar a minha aplicação
            // redireciono para o método INDEX deste mesmo controller
            return RedirectToAction("Index");
         }

         // esta expressão db.Veterinarios.FirstOrDefaultAsync(m => m.ID == id)
         // é uma forma diferente de escrever o seguinte comando
         // SELECT * FROM db.Veterinarios v WHERE v.ID = id
         // esta expressão é escrita em LINQ
         var veterinario = await db.Veterinarios.FirstOrDefaultAsync(v => v.ID == id);

         if (veterinario == null) {
            // se o ID é null, é porque o meu utilizador está a testar a minha aplicação
            // ele introduziu manualmente um valor inexistente
            // redireciono para o método INDEX deste mesmo controller
            return RedirectToAction("Index");
         }

         return View(veterinario);
      }




      // POST: Veterinarios/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      [Authorize(Roles = "Administrativo")]
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
