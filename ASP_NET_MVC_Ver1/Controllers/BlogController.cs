using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;
        bool isAdmin = false;
        bool isDoctor = false;
        bool isParent = false;
        string idUser;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult BlogsMXH()
        {
            IEnumerable<Post> objCatlist = _context.Posts;
            if (objCatlist == null)
            {
                return View("Create");
            }
            return View(objCatlist);

        }

        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult BlogDetail(Guid? Id)
        {
            if (Id == null )
            {
                return NotFound();
            }
            var empfromdb = _context.Posts.Find(Id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            var newBlog = new Post();
            newBlog.Image = new byte[0];
            return View(newBlog);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(Post empobj)
        {
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    file.CopyToAsync(dataStream);
                    empobj.Image = dataStream.ToArray();
                }
            }
            if (ModelState.IsValid)
            {
                empobj.CreateDate = DateTime.Now;
                _context.Posts.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Thêm một blog thành công !";
                return RedirectToAction("BlogsMXH");
            }

            return View(empobj);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(Guid?id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var empfromdb = _context.Posts.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(Post empobj)
        {
            if (ModelState.IsValid)
            {
                var oldData = _context.Posts.Find(empobj.Id);
                if (oldData != null)
                {
                    oldData.CreateDate = DateTime.Now;
                    if (empobj.Title.ToString() != oldData.Title.ToString())
                    {
                        oldData.Title = empobj.Title;
                    }
                    if (empobj.Description.ToString() != oldData.Description.ToString())
                    {
                        oldData.Description = empobj.Description;
                    }
                    if (Request.Form.Files.Count > 0)
                    {
                        IFormFile file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            file.CopyToAsync(dataStream);
                            empobj.Image = dataStream.ToArray();
                            oldData.Image = empobj.Image;
                        }
                    }

                    _context.Posts.Update(oldData);
                    _context.SaveChanges();
                }
                TempData["ResultOk"] = "Cập nhật thông tin blog thành công !";
                return RedirectToAction("BlogsMXH");
            }

            return View(empobj);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var empfromdb = _context.Posts.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult DeleteEmp(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var deleterecord = _context.Posts.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Posts.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Xóa blog thành công !";
            return RedirectToAction("BlogsMXH");
        }
    }
}
