using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AppMvc.Models;
using System;

namespace AppMvc.Controllers
{
   [Authorize]
   public class AlunosController : Controller
   {
      private ApplicationDbContext db = new ApplicationDbContext();

      [HttpGet]
      [OutputCache(Duration = 60)]
      [Route("listar-alunos")]
      public async Task<ActionResult> Index()
      {
         return View(await db.Alunos.ToListAsync());
      }

      [Authorize]
      [HttpGet]
      [Route("aluno-detalhe/{id:int}")]
      public async Task<ActionResult> Details(int id)
      {
         var aluno = await db.Alunos.FindAsync(id);
         if (aluno == null)
         {
            return HttpNotFound();
         }
         return View(aluno);
      }

      [HttpGet]
      [AllowAnonymous]
      [Route("novo-aluno")]
      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [Route("novo-aluno")]
      [HandleError(ExceptionType = typeof(NullReferenceException), View = "Erro")]
      [ValidateInput(false)]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Create([Bind(Include = "Id,Nome,Email,CPF,Descricao,DataMatricula,Ativo")] Aluno aluno)
      {
         if (ModelState.IsValid)
         {
            db.Alunos.Add(aluno);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
         }

         return View(aluno);
      }

      [HttpGet]
      [Route("editar-aluno/{id:int}")]
      public async Task<ActionResult> Edit(int id)
      {
         var aluno = await db.Alunos.FindAsync(id);
         if (aluno == null)
         {
            return HttpNotFound();
         }
         return View(aluno);
      }

      
      [HttpPost]
      [Route("editar-aluno/{id:int}")]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Edit([Bind(Include = "Id,Nome,Email,CPF,Descricao,DataMatricula,Ativo")] Aluno aluno)
      {
         if (ModelState.IsValid)
         {
            db.Entry(aluno).State = EntityState.Modified;
            db.Entry(aluno).Property(a => a.DataMatricula).IsModified = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
         }
         return View(aluno);
      }

      [HttpGet]
      [Route("excluir-aluno/{id:int}")]
      public async Task<ActionResult> Delete(int? id)
      {
         var aluno = await db.Alunos.FindAsync(id);
         if (aluno == null)
         {
            return HttpNotFound();
         }
         return View(aluno);
      }

      
      [HttpPost]
      [Route("excluir-aluno/{id:int}")]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> DeleteConfirmed(int id)
      {
         Aluno aluno = await db.Alunos.FindAsync(id);
         db.Alunos.Remove(aluno);
         await db.SaveChangesAsync();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
