$(document).ready(function () {
    debugger;
    $("#customerDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/ExaminationApi",
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "id", "name": "Id", "autoWidth": true },
            {
                "data": null,
                "name": "STT",
                "width": "50px",
                "autoWidth": true,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "doctorName", "name": "doctorName", "autoWidth": true },
            { "data": "parentName", "name": "parentName", "autoWidth": true },
            { "data": "childFullName", "name": "childFullName", "autoWidth": true },
            { "data": "categoryTitle", "name": "categoryTitle", "autoWidth": true },
            {
                "data": "registrationDate",
                "name": "registrationDate",
                "autoWidth": true,
                "render": function (data, type, row) {
                    // Kiểm tra nếu dữ liệu là kiểu ngày thì định dạng
                    if (type === 'display' && data !== null) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                    return data;
                }
            },
            {
                "targets": 1,
                "width": "50px",
                "orderable": false,
                "render": function (data, type, row) {
                    var Id = '';
                    if (type === 'display' && data !== null) {
                        Id = row.id;
                    }
                    return `<a href="/Examination/Edit/${Id}" class="btn btn-primary center-block m-1">Edit</a>`;
                }
            },

            //{
            //    "targets": 1,
            //    "width": "70px",
            //    "orderable": false,
            //    "render": function (data, type, row) {
            //        var Id = '';
            //        if (type === 'display' && data !== null) {
            //            Id = row.id;
            //        }
            //        return `<button type="button" class="btn btn-danger center-block m-1" title="Xóa thông tin này" onclick="if (confirm('Bạn có chắc chắn muốn xóa nhân viên này?')) { DeleteEmp('${Id}'); }">Delete</button>`;
            //        //return `<a href="#" class="btn btn-danger" onclick="DeleteEmp('${Id}');">Delete</a>`;
            //        //return `<button type="button" class="btn btn-danger" onclick="if (confirm('Bạn có chắc chắn muốn xóa nhân viên này?')) { DeleteEmp('${Id}'); }">Delete</button>`;
            //    }
            //},

        ],
        "lengthMenu": [[5, 10, 20, 50, 100], [5, 10, 20, 50, 100]],
        "pageLength": 5
    });
});
//function DeleteEmp(id) {
//    $.ajax({
//        url: '/api/ChildrenApi/DeleteEmp?id=' + id,
//        type: 'DELETE',
//        success: function (result) {
//            debugger;
//            // Xử lý kết quả trả về từ server (nếu cần)
//            location.reload();
//        },
//        error: function (xhr, status, error) {
//            // Xử lý lỗi (nếu có)
//            console.log(xhr.responseText);
//        }
//    });
//}
function EditEmp(id) {

}