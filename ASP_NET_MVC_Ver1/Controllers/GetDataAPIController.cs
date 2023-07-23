using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace ASP_NET_MVC_Ver1.Controllers
{
    [ApiController]
    [Route("api/GetData")]
    public class GetDataAPIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;
        public GetDataAPIController(ApplicationDbContext context, UserManager<ApplicationUser> uid)
        {
            _context = context;
            _uid = uid;
        }
        [HttpGet]
        [Route("GetChildren")]
        public IActionResult ActionName(string parentId)
        {
            try
            {
                List<SelectListItem> lstChildrens = new List<SelectListItem>();
                List<Children> childrens = _context.Childrens.ToList();
                foreach (var child in childrens)
                {
                    if (child.Id.ToString() != null && child.parent_id.ToString() == parentId)
                    {
                        lstChildrens.Add(new SelectListItem { Text = child.Id.ToString(), Value = child.FullName.ToString() });
                    }
                }
                return Ok(lstChildrens);

                // Xử lý logic dựa trên giá trị childId từ request

            }
            catch (Exception)
            {
                throw new Exception("Lỗi dữ liệu");
            }

        }


        [HttpGet]
        [Route("GetReservation")]
        public IActionResult GetReservation(string doctorId)
        {
            try
            {
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


                List<SelectListItem> uniqueSlots = new List<SelectListItem>();
                List<Reservation> reservations = _context.Reservations.ToList();
                uniqueSlots.Add(new SelectListItem { Text = null, Value = "Chọn lịch khám" });
                if (doctorId != null)
                {
                    var result = reservations.Where(c => c.examination_status == StatusExamination.NotDone).ToList();
                    foreach (var res in result)
                    {
                        if (res.Id.ToString() != null)
                        {
                            uniqueSlots.Add(new SelectListItem { Text = res.Id.ToString(), Value = "Slot " + res.slot.ToString() + " Date: " + res.booking_date?.ToString("MM/dd/yyyy") });
                        }
                    }
                    //uniqueSlots = lstSlot.Where(s => result.Any(r => r.slot.ToString() == s.Text)).ToList();

                }




                return Ok(uniqueSlots);

                // Xử lý logic dựa trên giá trị childId từ request

            }
            catch (Exception)
            {
                throw new Exception("Lỗi dữ liệu");
            }

        }

        [HttpGet]
        [Route("callAPIBooking")]
        public IActionResult callAPIBooking(DateTime bookingDate, string idDoctor)
        {
            try
            {
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
                List<Reservation> reservation = _context.Reservations.ToList();
                List<SelectListItem> uniqueSlots = new List<SelectListItem>();
                if (bookingDate != null && idDoctor != null)
                {
                    var result = reservation.Where(c => c.booking_date == bookingDate && c.doctor_id == idDoctor).ToList();
                    uniqueSlots = lstSlot.Where(s => !result.Any(r => r.slot.ToString() == s.Text)).ToList();

                }
                return Ok(uniqueSlots);

                // Xử lý logic dựa trên giá trị childId từ request

            }
            catch (Exception)
            {
                throw new Exception("Lỗi dữ liệu");
            }

        }


        [HttpGet]
        [Route("GetReservationFull")]
        public async Task<IActionResult> GetReservationFull(string reservationId)
        {
            try
            {
                List<SelectListItem> lstReservation = new List<SelectListItem>();
                List<Children> ChildrenLst = _context.Childrens.ToList();
                List<Reservation> reservation = _context.Reservations.ToList();
                var result = reservation.FirstOrDefault(c => c.Id.ToString() == reservationId);
                var parentData = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
                var filterParent = parentData.Where(c => c.Email != "admin@gmail.com").ToList();
                List<Category> categoryLst = _context.Categories.ToList();
                foreach (var res in categoryLst)
                {
                    if (res.Id.ToString() != null && res.Id.ToString() == result.category)
                    {
                        lstReservation.Add(new SelectListItem { Text = res.Id.ToString(), Value = res.Title.ToString() });
                    }
                }
                foreach (var res in filterParent)
                {
                    if (res.Id.ToString() != null && res.Id.ToString() == result.parent_id)
                    {
                        lstReservation.Add(new SelectListItem { Text = res.Id.ToString(), Value = res.FirstName.ToString() + " " + res.LastName.ToString() });
                    }
                }
                foreach (var res in ChildrenLst)
                {
                    if (res.Id.ToString() != null && res.Id.ToString() == result.child_id)
                    {
                        lstReservation.Add(new SelectListItem { Text = res.Id.ToString(), Value = res.FullName.ToString() });
                    }
                }
                return Ok(lstReservation);

                // Xử lý logic dựa trên giá trị childId từ request

            }
            catch (Exception)
            {
                throw new Exception("Lỗi dữ liệu");
            }



        }
        public class ReservationData
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}
