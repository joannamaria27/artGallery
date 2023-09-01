using ArtGallery.DataAcess.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArtGallery.Models.ViewModels;
using System.Collections.Generic;
using ArtGallery.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ArtGallery.DataAcess.Repository;
using Microsoft.AspNetCore.Hosting;

namespace ArtGalleryWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyController(ICompanyRepository content)
        {
            _companyRepository = content;

        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _companyRepository.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company CompanyObj = _companyRepository.Get(u => u.Id == id);
                return View(CompanyObj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {
                if (CompanyObj.Id != 0)
                {
                    _companyRepository.Update(CompanyObj);
                }
                else
                {
                    _companyRepository.Add(CompanyObj);
                }
                _companyRepository.Save();
                TempData["success"] = "Company edit successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company companyFromDb = _companyRepository.Get(u => u.Id == id);
            if (companyFromDb == null)
            {
                return NotFound();
            }
            return View(companyFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company obj = _companyRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _companyRepository.Remove(obj);
            _companyRepository.Save();
            TempData["success"] = "Company deleted successfully";
            return RedirectToAction("Index");
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _companyRepository.GetAll().ToList();
            return View(objCompanyList);
        }
        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{

        //    Company CompanyToBeDelete = _companyRepository.Get(u => u.Id == id);
        //    if (CompanyToBeDelete == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }
        //    _companyRepository.Remove(CompanyToBeDelete);
        //    _companyRepository.Save();
        //    return Json(new { success = true, message = "Delete successful" });
        //}
        #endregion
    }

}
