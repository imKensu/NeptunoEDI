using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NeptunoMVC01.Context;
using NeptunoMVC01.Models;
using PagedList;

namespace NeptunoMVC01.Controllers
{
    public class CiudadesController : Controller
    {
        private NeptunoDbContext db = new NeptunoDbContext();

        // GET: Ciudades
        public ActionResult Index(int? paisSeleccionadoId=null, int? page=null)
        {
            page = (page ?? 1);

            if (paisSeleccionadoId!=null)
            {
                Session["paisSeleccionadoId"] = paisSeleccionadoId;
            }
            else
            {
                if (Session["paisSeleccionadoId"]!=null)
                {
                    paisSeleccionadoId =(int) Session["paisSeleccionadoId"];
                }
            }

            var ciudades = paisSeleccionadoId.HasValue
                ? db.Ciudades.Where(c => c.PaisId == paisSeleccionadoId)
                : db.Ciudades;

            var listaCiudades = ciudades.Include(c => c.Pais)
                .OrderBy(c=>c.Pais.NombrePais)
                .ThenBy(c=>c.NombreCiudad)
                .ToPagedList((int)page,10);

            var listaPaises = db.Paises.ToList();
            listaPaises.Insert(0, new Pais() { PaisId = 0, NombrePais = "[Seleccione un País]" });
            ViewBag.ListaPaises = new SelectList(listaPaises, "PaisId", "NombrePais",paisSeleccionadoId);

            return View(listaCiudades);
        }

        // GET: Ciudades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = db.Ciudades.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // GET: Ciudades/Create
        public ActionResult Create()
        {
            var listaPaises = db.Paises.ToList();
            listaPaises.Insert(0, new Pais() { PaisId = 0, NombrePais ="[Seleccione un País]"});
            ViewBag.PaisId = new SelectList(listaPaises, "PaisId", "NombrePais");
            return View();
        }

        // POST: Ciudades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CiudadId,NombreCiudad,PaisId")] Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Ciudades.Add(ciudad);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError(string.Empty,"Registro duplicado");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,"Error al intentar dar de alta un registro");
                    }
                }

            }

            var listaPaises = db.Paises.ToList();
            listaPaises.Insert(0, new Pais() { PaisId = 0, NombrePais = "[Seleccione un País]" });
            ViewBag.PaisId = new SelectList(listaPaises, "PaisId", "NombrePais",ciudad.PaisId);
            return View(ciudad);
        }

        // GET: Ciudades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = db.Ciudades.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            var listaPaises = db.Paises.ToList();
            listaPaises.Insert(0, new Pais() { PaisId = 0, NombrePais = "[Seleccione un País]" });
            ViewBag.PaisId = new SelectList(listaPaises, "PaisId", "NombrePais", ciudad.PaisId);
            return View(ciudad);
        }

        // POST: Ciudades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CiudadId,NombreCiudad,PaisId")] Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(ciudad).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    if (ex.InnerException!=null && 
                        ex.InnerException.InnerException!=null && 
                        ex.InnerException.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError(string.Empty,"Registro Duplicado!!!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,"Error al intentar editar un registro");
                    }
                }

            }
            var listaPaises = db.Paises.ToList();
            listaPaises.Insert(0, new Pais() { PaisId = 0, NombrePais = "[Seleccione un País]" });
            ViewBag.PaisId = new SelectList(listaPaises, "PaisId", "NombrePais", ciudad.PaisId);
            return View(ciudad);
        }

        // GET: Ciudades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = db.Ciudades
                .Include(c=>c.Pais)
                .FirstOrDefault(c=>c.CiudadId==id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: Ciudades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ciudad ciudad = db.Ciudades
                .Include(c=>c.Pais)
                .FirstOrDefault(c=>c.CiudadId==id);
            try
            {
                db.Ciudades.Remove(ciudad);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                if (ex.InnerException!=null && 
                    ex.InnerException.InnerException!=null && 
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty,"Registro relacionado... baja denegada");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Error al intentar dar de baja un registro");
                }
            }

            ciudad.Pais = db.Paises.Find(ciudad.PaisId);
            return View(ciudad);
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
