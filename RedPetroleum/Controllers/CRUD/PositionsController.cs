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
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace RedPetroleum.Controllers.CRUD
{
    public class PositionsController : Controller
    {
        UnitOfWork unitOfWork;

        public PositionsController()
        {
            this.unitOfWork = new UnitOfWork();
        }

        public PositionsController(UnitOfWork unit)
        {
            this.unitOfWork = unit;
        }
        
        public ActionResult Index(int? page, string searching)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            var position = unitOfWork.Positions.GetAllIndex(pageNumber, pageSize, searching);
            return View(position);
        }
        
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = await unitOfWork.Positions.GetAsync(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Position position)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    position.PositionId = Guid.NewGuid();
                    unitOfWork.Positions.Create(position);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                return View(position);
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors.Count > 0)
                    {
                        switch (sqlException.Errors[0].Number)
                        {
                            case 2601:
                                ViewBag.Message = "Такая запись уже существует!";
                                return View(position);
                            default:
                                return View(position);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

        }
        
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = await unitOfWork.Positions.GetAsync(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PositionId,Name")] Position position)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Positions.Update(position);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                return View(position);

            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors.Count > 0)
                    {
                        switch (sqlException.Errors[0].Number)
                        {
                            case 2601:
                                ViewBag.Message = "Такая запись уже существует!";
                                return View(position);
                            default:
                                return View(position);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
        }
        
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = await unitOfWork.Positions.GetAsync(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            unitOfWork.Positions.Delete(id);
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