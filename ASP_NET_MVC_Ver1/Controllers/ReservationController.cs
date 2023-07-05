using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Collections.Generic;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;
        bool isAdmin = false;
        bool isDoctor = false;
        bool isParent = false;
        string idUser;
        int ApproveTotal = -1;
        //bool isDoctor = User.IsInRole(Roles.Doctor.ToString());
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            isAdmin = User.IsInRole(Roles.Admin.ToString());
            isDoctor = User.IsInRole(Roles.Doctor.ToString());
            isParent = User.IsInRole(Roles.Parent.ToString());
            idUser = _uid.GetUserId(HttpContext.User);
            ViewBag.Admin = isAdmin;
            ViewBag.Doctor = isDoctor;
            ViewBag.Parent = isParent;
            List<SelectListItem> lstSlot = new List<SelectListItem>();

            TimeSpan startTime = new TimeSpan(8, 0, 0); // Giờ bắt đầu (08:00)
            TimeSpan endTime = new TimeSpan(20, 0, 0); // Giờ kết thúc (20:00)

            TimeSpan slotDuration = new TimeSpan(1, 0, 0); // Thời gian mỗi slot (1 giờ)

            DateTime currentDate = DateTime.Now.Date; // Lấy ngày hiện tại

            int slotCount = (int)(endTime - startTime).TotalHours; // Số lượng slot trong khoảng thời gian

            for (int i = 1; i < slotCount; i++)
            {
                TimeSpan slotStart = startTime.Add(new TimeSpan(i, 0, 0)); // Giờ bắt đầu của slot hiện tại
                TimeSpan slotEnd = slotStart.Add(slotDuration); // Giờ kết thúc của slot hiện tại

                string slotName = $"{slotStart:hh\\:mm} - {slotEnd:hh\\:mm}";

                lstSlot.Add(new SelectListItem { Text = i.ToString(), Value = "Ca khám số " + i.ToString() + " ( Thời gian: " + slotName + ")" });
            }

            ViewBag.lstSlot = lstSlot;

            base.OnActionExecuting(context);
        }
        public ReservationController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _uid = uid;
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public async Task<IActionResult> Index()
        {
            List<Reservation> ReservationLst = _context.Reservations.ToList();

            if (isAdmin)
            {
                ReservationLst = ReservationLst;
            }
            else if (isDoctor)
            {
                ReservationLst = ReservationLst.Where(c => c.doctor_id == idUser).ToList();
            }
            else if (isParent)
            {
                ReservationLst = ReservationLst.Where(c => c.parent_id == idUser).ToList();
            }



            List<Children> ChildrenLst = _context.Childrens.ToList();
            List<Category> CategoryLst = _context.Categories.ToList();
            var getData = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
            var DoctorL = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
            var ParentLst = getData.Where(c => c.Email != "admin@gmail.com").ToList();
            var DoctorLst = DoctorL.Where(c => c.Email != "admin@gmail.com").ToList();


            var joinTable = from reservation in ReservationLst
                            join children in ChildrenLst on reservation.child_id.ToString() equals children.Id.ToString()
                            join parent in ParentLst on reservation.parent_id.ToString() equals parent.Id.ToString()
                            join doctor in DoctorLst on reservation.doctor_id.ToString() equals doctor.Id.ToString()
                            join category in CategoryLst on reservation.category.ToString() equals category.Id.ToString()
                            select new Reservation
                            {
                                // Gán các thuộc tính khác của Reservation
                                Id = reservation.Id,
                                parent_id = parent.FirstName + parent.LastName,
                                child_id = children.FullName,
                                doctor_id = doctor.FirstName + doctor.LastName,
                                category = category.Title,
                                r_title = reservation.r_title,
                                r_content = reservation.r_content,
                                create_date = reservation.create_date,
                                booking_date = reservation.booking_date,
                                slot = reservation.slot,
                                Desciption = reservation.Desciption,
                                status = reservation.status,

                                // Gán tên của Children vào thuộc tính ChildrenName
                                // Hoặc có thể tạo một thuộc tính mới trong Reservation để lưu trữ tên của Children
                            };

            List<Reservation> updatedReservationLst = joinTable.ToList();



            var result = new List<Reservation>();
            result = updatedReservationLst.ToList();
            return View(result);

        }

        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public async Task<IActionResult> Create()
        {
            if (isAdmin || isParent)
            {
                var getData = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
                var filterParent = getData.Where(c => c.Email != "admin@gmail.com").ToList();
                List<SelectListItem> lstDoctor = new List<SelectListItem>();
                foreach (var doctor in filterParent)
                {
                    if (doctor.Id.ToString() != null)
                    {
                        lstDoctor.Add(new SelectListItem { Text = doctor.Id.ToString(), Value = doctor.FirstName.ToString() + " " + doctor.LastName.ToString() });

                    }
                }
                ViewBag.doctor_lst = lstDoctor;
            }
            else if (isDoctor)
            {
                var resultModel = _uid.GetUserAsync(HttpContext.User);
                ViewBag.doctorName = resultModel.Result.FirstName + resultModel.Result.LastName;
            }

            if (isAdmin || isDoctor)
            {
                var getData = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
                var filterParent = getData.Where(c => c.Email != "admin@gmail.com").ToList();
                List<SelectListItem> lstParents = new List<SelectListItem>();
                foreach (var parent in filterParent)
                {
                    if (parent.Id.ToString() != null)
                    {
                        lstParents.Add(new SelectListItem { Text = parent.Id.ToString(), Value = parent.FirstName.ToString() + " " + parent.LastName.ToString() + " - " + parent.Email.ToString() });

                    }
                }
                ViewBag.parent_lst = lstParents;
            }
            else if (isParent)
            {
                var resultModel = _uid.GetUserAsync(HttpContext.User);
                ViewBag.parentName = resultModel.Result.FirstName + resultModel.Result.LastName;
            }

            List<Children> childrens = _context.Childrens.ToList();
            List<SelectListItem> lstChildrens = new List<SelectListItem>();
            foreach (var child in childrens)
            {
                if (child.Id.ToString() != null)
                {
                    if (isAdmin || isDoctor)
                    {
                        lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                    }
                    else if (isParent)
                    {
                        if (child.parent_id == idUser)
                        {
                            lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                        }
                    }
                }
            }
            ViewBag.child_lst = lstChildrens;

            List<Category> categoryLst = _context.Categories.ToList();
            List<SelectListItem> category = new List<SelectListItem>();
            foreach (var item in categoryLst)
            {
                if (item.Id.ToString() != null)
                {
                    category.Add(new SelectListItem { Text = item.Id.ToString(), Value = item.Title.ToString() });
                }
            }
            ViewBag.category_lst = category;


            var reservationSend = new Reservation();
            if (!isAdmin || !isDoctor)
            {
                reservationSend.status = StatusSending.Process;
            }
            return View(reservationSend);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Create(Reservation empobj)
        {
            empobj.create_date = DateTime.Now;
            if (isParent)
            {
                empobj.parent_id = idUser;
            }
            else if (isDoctor)
            {
                empobj.doctor_id = idUser;
            }
            if (ModelState.IsValid)
            {
                _context.Reservations.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Yêu cầu đặt lịch đã được gửi đi!";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public async Task<IActionResult> Approve(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var empfromdb = _context.Reservations.Find(Id);
            ApproveTotal = (int)empfromdb.status;
            ViewBag.ApproveTotal = ApproveTotal;

            List<Children> childrens = _context.Childrens.ToList();
            List<SelectListItem> lstChildrens = new List<SelectListItem>();
            foreach (var child in childrens)
            {
                if (child.Id.ToString() != null)
                {
                    if (isAdmin || isDoctor)
                    {
                        lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                    }
                    else if (isParent)
                    {
                        if (child.parent_id == idUser)
                        {
                            lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                        }
                    }
                }
            }
            ViewBag.child_lst = lstChildrens;

            List<Category> categoryLst = _context.Categories.ToList();
            List<SelectListItem> category = new List<SelectListItem>();
            foreach (var item in categoryLst)
            {
                if (item.Id.ToString() != null)
                {
                    category.Add(new SelectListItem { Text = item.Id.ToString(), Value = item.Title.ToString() });
                }
            }
            ViewBag.category_lst = category;


            var getData = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
            var filterParent = getData.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstDoctor = new List<SelectListItem>();
            foreach (var doctor in filterParent)
            {
                if (doctor.Id.ToString() != null)
                {
                    lstDoctor.Add(new SelectListItem { Text = doctor.Id.ToString(), Value = doctor.FirstName.ToString() + " " + doctor.LastName.ToString() });

                }
            }
            ViewBag.doctor_lst = lstDoctor;

            var getData1 = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
            var filterParent1 = getData1.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstParents = new List<SelectListItem>();
            foreach (var parent in filterParent1)
            {
                if (parent.Id.ToString() != null)
                {
                    lstParents.Add(new SelectListItem { Text = parent.Id.ToString(), Value = parent.FirstName.ToString() + " " + parent.LastName.ToString() + " - " + parent.Email.ToString() });

                }
            }
            ViewBag.parent_lst = lstParents;
            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Doctor,Nurse,Parent,Children")]
        public IActionResult Approve(Reservation empobj)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Update(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Lịch khám cho bé cập nhật thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }


        [HttpGet]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var empfromdb = _context.Reservations.Find(Id);
            ApproveTotal = (int)empfromdb.status;
            ViewBag.ApproveTotal = ApproveTotal;

            List<Children> childrens = _context.Childrens.ToList();
            List<SelectListItem> lstChildrens = new List<SelectListItem>();
            foreach (var child in childrens)
            {
                if (child.Id.ToString() != null)
                {
                    if (isAdmin || isDoctor)
                    {
                        lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                    }
                    else if (isParent)
                    {
                        if (child.parent_id == idUser)
                        {
                            lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                        }
                    }
                }
            }
            ViewBag.child_lst = lstChildrens;

            List<Category> categoryLst = _context.Categories.ToList();
            List<SelectListItem> category = new List<SelectListItem>();
            foreach (var item in categoryLst)
            {
                if (item.Id.ToString() != null)
                {
                    category.Add(new SelectListItem { Text = item.Id.ToString(), Value = item.Title.ToString() });
                }
            }
            ViewBag.category_lst = category;


            var getData = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
            var filterParent = getData.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstDoctor = new List<SelectListItem>();
            foreach (var doctor in filterParent)
            {
                if (doctor.Id.ToString() != null)
                {
                    lstDoctor.Add(new SelectListItem { Text = doctor.Id.ToString(), Value = doctor.FirstName.ToString() + " " + doctor.LastName.ToString() });

                }
            }
            ViewBag.doctor_lst = lstDoctor;

            var getData1 = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
            var filterParent1 = getData1.Where(c => c.Email != "admin@gmail.com").ToList();
            List<SelectListItem> lstParents = new List<SelectListItem>();
            foreach (var parent in filterParent1)
            {
                if (parent.Id.ToString() != null)
                {
                    lstParents.Add(new SelectListItem { Text = parent.Id.ToString(), Value = parent.FirstName.ToString() + " " + parent.LastName.ToString() + " - " + parent.Email.ToString() });

                }
            }
            ViewBag.parent_lst = lstParents;

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parent")]
        public IActionResult Edit(Reservation empobj)
        {
            empobj.create_date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Reservations.Update(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Lịch khám cho bé cập nhật thành công !";
                return RedirectToAction("Index");
            }

            return View(empobj);
        }
    }
}
