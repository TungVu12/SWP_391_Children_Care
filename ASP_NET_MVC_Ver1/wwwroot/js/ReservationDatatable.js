$(document).ready(function () {
    $("#customerDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/ReservationApi",
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
                "data": "booking_date",
                "name": "booking_date",
                "autoWidth": true,
                "render": function (data, type, row) {
                    // Kiểm tra nếu dữ liệu là kiểu ngày thì định dạng
                    if (type === 'display' && data !== null) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                    return data;
                }
            },
            { "data": "status", "name": "status", "autoWidth": true, "orderable": false, },
            { "data": "examinationStatus", "name": "examinationStatus", "autoWidth": true, "orderable": false, },
            {
                "targets": 1,
                "width": "50px",
                "orderable": false,
                "render": function (data, type, row) {
                    var Id = '';
                    if (type === 'display' && data !== null) {
                        Id = row.id;
                    }
                    return `<a href="/Reservation/Edit/${Id}" class="btn btn-primary center-block m-1">Edit</a>`;
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
                    return `<a href="/Reservation/Approve/${Id}" class="btn btn-success center-block m-1">Approved</a>`;
                }
            },
            {
                "targets": 1,
                "width": "70px",
                "orderable": false,
                "render": function (data, type, row) {
                    var Id = '';
                    if (type === 'display' && data !== null) {
                        Id = row.id;
                    }
                    return `<button type="button" class="btn btn-danger center-block m-1" title="Xóa thông tin này" onclick="if (confirm('Bạn có chắc chắn muốn huỷ lịch không?')) { DeleteEmp('${Id}'); }">Delete</button>`;
                }
            },

        ],
        "lengthMenu": [[5, 10, 20, 50, 100], [5, 10, 20, 50, 100]],
        "pageLength": 5
    });
});
function DeleteEmp(id) {
    $.ajax({
        url: '/api/ReservationApi/DeleteEmp?id=' + id,
        type: 'DELETE',
        success: function (result) {
            debugger;
            // Xử lý kết quả trả về từ server (nếu cần)
            location.reload();
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi (nếu có)
            console.log(xhr.responseText);
        }
    });
}
function EditEmp(id) {

}