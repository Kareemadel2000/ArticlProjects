using ArticlProjects.Core.Entityes;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArticlProjects.Controllers
{
    [Authorize("Admin")]
    public class CategorysController : Controller
    {
        private readonly IDataHelper<Category> _dataHelper;
        private int PageItem;

        public CategorysController(IDataHelper<Category> dataHelper)
        {
            _dataHelper = dataHelper;
            PageItem = 10;
        }

       
        // GET: CategorysController
        public ActionResult Index(int? id)
        {
            if (id == 0|| id ==null)
            {
                return View(_dataHelper.GetAll().Take(PageItem));
            }
            else
            {
                var data = _dataHelper.GetAll().Where(x=>x.Id > id).Take(PageItem);
                return View(data);
            }
        }

        // GET: CategorysController
        public ActionResult Search(string SearchItem)
        {
            if (SearchItem == null)
            {
                return View("Index", _dataHelper.GetAll());
            }
            else
            {
                return View("Index", _dataHelper.Search(SearchItem));
            }
        }
       

        // GET: CategorysController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategorysController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category collection)
        {
            try
            {
               var result= _dataHelper.Add(collection);
                if (result==1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorysController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_dataHelper.Find(id));
        }

        // POST: CategorysController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category collection)
        {
            try
            {
                var result = _dataHelper.Update(id,collection);
                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorysController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategorysController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category collection)
        {
            try
            {

                var result = _dataHelper.Delete(id);
                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
