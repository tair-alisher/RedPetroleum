using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;
using RedPetroleum.Models.Entities;
using System.Threading.Tasks;
using System.Net;

namespace RedPetroleum.Controllers.CRUD
{
    public class MarksController : Controller
    {
        UnitOfWork unitOfWork;

        public MarksController()
        {
            this.unitOfWork = new UnitOfWork();
        }

        public MarksController(UnitOfWork unit)
        {
            this.unitOfWork = unit;
        }

        public ActionResult Index(int? page, string searching)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var mark = unitOfWork.Marks.GetAllIndex(pageNumber, pageSize, searching);
            return View(mark);
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = await unitOfWork.Marks.GetAsync(id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            return View(mark);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Mark mark)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    mark.Id = Guid.NewGuid();
                    unitOfWork.Marks.Create(mark);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                return View(mark);
            }
            catch (Exception)
            {
                ViewBag.Message = "Такая запись уже существует!";
                return View(mark);
            }

        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = await unitOfWork.Marks.GetAsync(id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            return View(mark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Mark mark)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Marks.Update(mark);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                return View(mark);

            }
            catch (Exception)
            {
                ViewBag.Message = "Такая запись уже существует!";
                return View(mark);
            }
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = await unitOfWork.Marks.GetAsync(id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            return View(mark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            unitOfWork.Marks.Delete(id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}