using ArticlProjects.Core.Entityes;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArticlProjects.Controllers
{
    public class AuthorController : Controller
    {
        private IDataHelper<Author> _dataHelper;
        public AuthorController(IDataHelper<Author> dataHelper)
        {
            _dataHelper = dataHelper;
        }

        // GET: AuthorController
        public ActionResult Index()
        {
            return View(_dataHelper.GetAll());
        }

        #region Edit

        // GET: AuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Delete
        // GET: AuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        } 
        #endregion
    }
}
