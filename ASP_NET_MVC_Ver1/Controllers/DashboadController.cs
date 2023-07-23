using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Enum;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class DashboadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _uid;

        public DashboadController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _uid = userManager;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<Reservation> ReservationLst = _context.Reservations.ToList();
            List<Examination> examinations = _context.Examination.ToList();
            var getData = await _uid.GetUsersInRoleAsync(Roles.Parent.ToString());
            var DoctorL = await _uid.GetUsersInRoleAsync(Roles.Doctor.ToString());
            var ParentLst = getData.Where(c => c.Email != "admin@gmail.com").ToList();
            var DoctorLst = DoctorL.Where(c => c.Email != "admin@gmail.com").ToList();

            ViewBag.Reservation = ReservationLst.ToList().Count;
            ViewBag.Examination = examinations.ToList().Count;
            ViewBag.ParentLst = ParentLst.ToList().Count;
            ViewBag.DoctorLst = DoctorLst.ToList().Count;


            Dictionary<int, int> monthlyCounts = new Dictionary<int, int>();

            // Lặp qua danh sách ReservationLst và tính tổng số lượt đặt chỗ của mỗi tháng
            foreach (var reservation in ReservationLst)
            {
                if (reservation.booking_date.HasValue)
                {
                    // Lấy tháng từ booking_date (sử dụng thuộc tính Value để lấy giá trị từ nullable DateTime)
                    int month = reservation.booking_date.Value.Month;

                    if (monthlyCounts.ContainsKey(month))
                    {
                        monthlyCounts[month] += 1; // Cộng thêm 1 lượt đặt chỗ vào tháng đã tồn tại trong Dictionary
                    }
                    else
                    {
                        monthlyCounts[month] = 1; // Thêm tháng mới vào Dictionary với số lượt đặt chỗ là 1
                    }
                }
            }

            int[] data = new int[12]; // Khởi tạo mảng data với 12 phần tử tương ứng với 12 tháng trong năm
            for (int i = 1; i <= 12; i++)
            {
                if (monthlyCounts.ContainsKey(i))
                {
                    data[i - 1] = monthlyCounts[i]; // Lấy số lượt đặt chỗ của tháng i từ Dictionary và gán vào mảng data
                }
                else
                {
                    data[i - 1] = 0; // Nếu tháng i không có lượt đặt chỗ nào, gán giá trị là 0
                }
            }
            // Đưa dữ liệu vào ViewBag
            ViewBag.ChartData = data;

            return View();
        }
    }
}
