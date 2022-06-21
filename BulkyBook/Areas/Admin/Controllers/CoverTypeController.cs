﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitofWork _unitOfWork;

        public CoverTypeController(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();

            if (id == null)
            {
                return View(coverType);
            }

            coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.CoverType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                if (coverType.Id == 0)
                {
                    _unitOfWork.CoverType.Add(coverType);
                }
                else
                {
                    _unitOfWork.CoverType.Update(coverType);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(coverType);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.CoverType.Get(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.CoverType.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successfully" });
        }

        #endregion
    }
}
