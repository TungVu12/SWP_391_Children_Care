using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;


namespace ASP_NET_MVC_Ver1.Enum
{
    public enum Roles
    {
        Admin,
        //Manager,
        Doctor,
        //Nurse,
        Parent,
        Children
    }
    public enum StatusSending
    {
        [Display(Name = "Chờ phê duyệt")]
        Process,
        [Display(Name = "Lịch hẹn bị từ chối")]
        Rejected,
        [Display(Name = "Đã xác nhận hẹn lịch")]
        Approved,
        [Display(Name = "Huỷ lịch")]
        Cancelled
    }

    public enum StatusExamination
    {
        [Display(Name = "Chưa khám bệnh")]
        NotDone,
        [Display(Name = "Đã khám bệnh")]
        Done,
    }
}
