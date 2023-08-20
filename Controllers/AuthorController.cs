using ArticlProjects.Code;
using ArticlProjects.Core.Entityes;
using ArticlProjects.CoreView;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ArticlProjects.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IDataHelper<Author> _dataHelper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IWebHostEnvironment _webHost;
        private readonly FilesHelper _filesHelper;
        private int PageItem;

        public AuthorController(
            IDataHelper<Author> dataHelper,
            IAuthorizationService authorizationService,
            IWebHostEnvironment webHost)
        {
            _dataHelper = dataHelper;
            _authorizationService = authorizationService;
            _webHost = webHost;
            _filesHelper = new FilesHelper(_webHost);
            PageItem = 10;
        }

        #region Indexs
        // GET: AuthorController
        [Authorize("Admin")]
        public ActionResult Index(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(_dataHelper.GetAll().Take(PageItem));
            }
            else
            {
                var data = _dataHelper.GetAll().Where(x => x.Id > id).Take(PageItem);
                return View(data);
            }
        } 
        #endregion

        #region search
        // GET: AuthorController
        [Authorize("Admin")]
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
        #endregion

        #region Edit

        // GET: AuthorController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var author = _dataHelper.Find(id);
            AuthorView authorView = new AuthorView
            {
                Id = author.Id,
                Bio = author.Bio,
                Facebook = author.Facebook,
                FullName = author.FullName,
                Instagram = author.Instagram,
                Twitter = author.Twitter,
                UserId = author.UserId,
                UserName = author.UserName,
                //ProfileImageUrl = _filesHelper.UploadFile(author.ProfileImageUrl, "images")
            };
            return View(authorView);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AuthorView collection)
        {
            try
            {
                var author = new Author
                {
                    Id = collection.Id,
                    UserId = collection.UserId,
                    Facebook = collection.Facebook,
                    FullName = collection.FullName,
                    Bio = collection.Bio,
                    Instagram = collection.Instagram,
                    UserName = collection.UserName,
                    Twitter = collection.Twitter,
                    ProfileImageUrl = _filesHelper.UploadFile(collection.ProfileImageUrl, "images"),
                };
                _dataHelper.Update(id, author);

                var result = _authorizationService.AuthorizeAsync(User, "Admin");
                if (result.Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect("/AdminIndex");
                }

            }
            catch
            {

                return View();
            }
        }
        #endregion

        #region Delete
        // GET: AuthorController/Delete/5
        [Authorize("Admin")]
        public ActionResult Delete(int id)
        {
            var author = _dataHelper.Find(id);
            AuthorView authorView = new AuthorView
            {
                Id = author.Id,
                UserId = author.UserId,
                Facebook = author.Facebook,
                FullName = author.FullName,
                Bio = author.Bio,
                Instagram = author.Instagram,
                UserName = author.UserName,
                Twitter = author.Twitter,
            };
            return View(authorView);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [Authorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Author collection)
        {
            try
            {
                _dataHelper.Delete(id);
                string filePath = "~/images/" + collection.ProfileImageUrl;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
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
