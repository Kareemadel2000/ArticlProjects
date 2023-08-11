using ArticlProjects.Code;
using ArticlProjects.Core.Entityes;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticlProjects.Controllers
{
    public class PostController : Controller
    {
        private readonly IDataHelper<Author> _dataHelperForAuthor;
        private readonly IDataHelper<Category> _dataHelperForCategory;
        private readonly IDataHelper<AuthorPost> _dataHelper;
        private readonly IWebHostEnvironment _webHost;
        private readonly FilesHelper _filesHelper;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Task<AuthorizationResult> result;
        private int PageItem;
        private string UserId;

        public PostController(
            IDataHelper<Author> dataHelperForAuthor, 
            IDataHelper<Category> dataHelperForCategory,
            IDataHelper<AuthorPost> dataHelper,
            IWebHostEnvironment webHost,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _dataHelperForAuthor = dataHelperForAuthor;
            _dataHelperForCategory = dataHelperForCategory;
            _dataHelper = dataHelper;
            _webHost = webHost;
            _filesHelper = new FilesHelper(_webHost);
            _authorizationService = authorizationService;
            _userManager = userManager;
            _signInManager = signInManager;
            PageItem = 10;
            result = authorizationService.AuthorizeAsync(User, "Admin");
            //UserId = User.FindFirst(ClaimTypes.NameIdentifier).ToString();
        }
        #region Index
        // GET: PostController
        public ActionResult Index(int? id)
        {
            if (result.Result.Succeeded)
            {
                //Admin
                if (id == 0 || id is null)
                {
                    return View(_dataHelper.GetAll().Take(PageItem));
                }
                else
                {
                    var data = _dataHelper.GetAll().Where(x => x.Id > id).Take(PageItem);
                    return View(data);
                }
            }
            else
            {
                // User => UserId   
                if (id == 0 || id is null)
                {
                    return View(_dataHelper.GetDataByUserId(UserId).Take(PageItem));
                }
                else
                {
                    var data = _dataHelper.GetDataByUserId(UserId).Where(x => x.Id > id).Take(PageItem);
                    return View(data);
                }
            }



        }
        #endregion

        #region Details
        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            return View(_dataHelper.Find(id));
        }
        #endregion

        #region Create
        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoreView.AuthorPostView collection)
        {
            try
            {
                var post = new AuthorPost
                {
                    AddedDate= DateTime.Now,
                    Author = collection.Author,
                    AuthorId = _dataHelperForAuthor.GetAll().Where(x=>x.UserId== UserId).Select(x=>x.Id).First(),
                    Category = collection.Category,
                    CategoryId =_dataHelperForCategory.GetAll().Where(x => x.Name == collection.PostCategory).Select(x => x.Id).First(),
                    FullName = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.FullName).First(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.UserName).First(),
                    PostImageUrl= _filesHelper.UploadFile(collection.PostImageUrl, "images")
                };
                _dataHelper.Add(post);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Edit
        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
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
        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
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
