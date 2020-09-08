using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NeptunoMVC01.Context;
using NeptunoMVC01.Models;
using NeptunoMVC01.ViewModels.Pais;
using PagedList;

namespace NeptunoMVC01.Controllers
{
    public class PaisesController : Controller
    {
        private NeptunoDbContext db = new NeptunoDbContext();

        [HttpGet]
        public ActionResult AddCity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = db.Paises.Find(id);
            if (pais == null)
            {
                return HttpNotFound();
            }

            Ciudad ciudad = new Ciudad
            {
                PaisId = pais.PaisId,
                Pais = pais
            };
            return View(ciudad);
        }

        [HttpPost]
        public ActionResult AddCity(Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Ciudades.Add(ciudad);
                    db.SaveChanges();
                    return RedirectToAction($"Details/{ciudad.PaisId}");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError(string.Empty, "Registro repetido!!!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error al intentar guardar un registro!!!");
                    }
                }
            }

            ciudad.Pais = db.Paises.Find(ciudad.PaisId);
            return View(ciudad);
        }
        // GET: Paises
        public ActionResult Index(int? page=null)
        {
            page = (page ?? 1);

            var listaPaises = db.Paises.OrderBy(p=>p.NombrePais).ToList();
            List<PaisListVm> listaVm=new List<PaisListVm>();
            foreach (var pais in listaPaises)
            {
                PaisListVm vm = new PaisListVm
                {
                    PaisId = pais.PaisId,
                    NombrePais = pais.NombrePais,
                    CantidadDeCiudades = db.Ciudades.Count(c => c.PaisId == pais.PaisId)
                };
                listaVm.Add(vm);
            }

            
            return View(listaVm.ToPagedList((int)page,10));
        }

        // GET: Paises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = db.Paises.Find(id);
            if (pais == null)
            {
                return HttpNotFound();
            }

            PaisDetailVm paisVm = new PaisDetailVm
            {
                PaisId = pais.PaisId,
                NombrePais = pais.NombrePais,
                Ciudades = db.Ciudades.Where(c => c.PaisId == pais.PaisId).ToList()
            };
            return View(paisVm);
        }

        // GET: Paises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Paises/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaisId,NombrePais")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Paises.Add(pais);
                    db.SaveChanges();
                    TempData["Msg"] = "Registro agregado!!!";
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {

                    if (ex.InnerException!=null && ex.InnerException.InnerException!=null &&
                        ex.InnerException.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError(string.Empty,"Registro repetido!!!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,"Error al intentar guardar un registro!!!");
                    }
                }

            }

            return View(pais);
        }

        // GET: Paises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = db.Paises.Find(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // POST: Paises/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaisId,NombrePais")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(pais).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Msg"] = "Registro editado!!!";

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError(string.Empty, "Registro repetido!!!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error al intentar guardar un registro!!!");
                    }
                }

            }
            return View(pais);
        }

        // GET: Paises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = db.Paises.Find(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // POST: Paises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pais pais = db.Paises.Find(id);
            try
            {
               
                db.Paises.Remove(pais);
                db.SaveChanges();
                TempData["Msg"] = "Registro Borrado!!!";

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                if (ex.InnerException!=null 
                    && ex.InnerException.InnerException!=null
                    && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty,"Registro relacionado... baja denegada");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Error al intentar dar de baja un registro");
                }
            }

            return View(pais);
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
