using ArticlProjects.Code;
using ArticlProjects.Core.Entityes;
using ArticlProjects.CoreView;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticlProjects.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IDataHelper<Author> _dataHelperForAuthor;
        private readonly IDataHelper<Category> _dataHelperForCategory;
        private readonly IDataHelper<AuthorPost> _dataHelper;
        private readonly IWebHostEnvironment webHost;
        private readonly FilesHelper _filesHelper;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private Task<AuthorizationResult> result;
        private int PageItem;
        private string UserId;

        public PostController(
            IDataHelper<Author> dataHelperForAuthor,
            IDataHelper<Category> dataHelperForCategory,
            IDataHelper<AuthorPost> dataHelper,
            IWebHostEnvironment webHost,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
           )
        {
            _dataHelperForAuthor = dataHelperForAuthor;
            _dataHelperForCategory = dataHelperForCategory;
            _dataHelper = dataHelper;
            this.webHost = webHost;
            _filesHelper = new FilesHelper(this.webHost);
            _authorizationService = authorizationService;
            _userManager = userManager;
            _signInManager = signInManager;
            PageItem = 10;
          
        }
        #region Index
        // GET: PostController
        public ActionResult Index(int? id)
        {
            SetUser();
            if (result.Result.Succeeded)
            {
                //Admin
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
            else
            {
                // User => UserId   
                if (id == 0 || id == null)
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
            SetUser();
            return View(_dataHelper.Find(id));
        }
        #endregion

        #region Create
        // GET: PostController/Create
        public ActionResult Create()
        {
            SetUser();
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AuthorPostView collection)
        {
            SetUser();
            try
            {
                var post = new AuthorPost
                {
                    AddedDate = DateTime.Now,
                    Author = collection.Author,
                    AuthorId = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.Id).First(),
                    Category = collection.Category,
                    CategoryId = _dataHelperForCategory.GetAll().Where(x => x.Name == collection.PostCategory).Select(x => x.Id).First(),
                    FullName = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.FullName).First(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.UserName).First(),
                    PostImageUrl = _filesHelper.UploadFile(collection.PostImageUrl, "images")

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
            var authorPost = _dataHelper.Find(id);
            AuthorPostView authorPostView = new AuthorPostView
            {
                AddedDate = authorPost.AddedDate,
                Author = authorPost.Author,
                AuthorId = authorPost.AuthorId,
                Category = authorPost.Category,
                CategoryId = authorPost.CategoryId,
                FullName = authorPost.FullName,
                PostCategory = authorPost.PostCategory,
                PostDescription = authorPost.PostDescription,
                PostTitle = authorPost.PostTitle,
                UserId = authorPost.UserId,
                UserName = authorPost.UserName,
                Id = authorPost.Id,
            };
            return View(authorPostView);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AuthorPostView collection)
        {
            SetUser();
            try
            {

                var post = new AuthorPost
                {
                    AddedDate = DateTime.Now,
                    Author = collection.Author,
                    AuthorId = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.Id).First(),
                    Category = collection.Category,
                    CategoryId = _dataHelperForCategory.GetAll().Where(x => x.Name == collection.PostCategory).Select(x => x.Id).First(),
                    FullName = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.FullName).First(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = _dataHelperForAuthor.GetAll().Where(x => x.UserId == UserId).Select(x => x.UserName).First(),
                    PostImageUrl = _filesHelper.UploadFile(collection.PostImageUrl, "images"),
                    Id = collection.Id,
                };

                _dataHelper.Update(id, post);
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
            
            return View(_dataHelper.Find(id));
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AuthorPost collection)
        {
            try
            {
                _dataHelper.Delete(id);
                string filePath = "~/images/" + collection.PostImageUrl;
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

        private void SetUser()
        {
            result = _authorizationService.AuthorizeAsync(User, "Admin");
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
