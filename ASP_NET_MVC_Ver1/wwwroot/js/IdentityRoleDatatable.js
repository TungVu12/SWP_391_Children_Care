$(document).ready(function () {
    debugger;
    $("#customerDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/UserRolesApi",
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
            { "data": "userId", "name": "userId", "autoWidth": true },
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
            { "data": "email", "name": "email", "autoWidth": true, "orderable": false },
            { "data": "firstName", "name": "firstName", "autoWidth": true, "orderable": false },
            { "data": "lastName", "name": "lastName", "autoWidth": true, "orderable": false },

            {
                "data": "roles", "name": "roles", "autoWidth": true, "orderable": false,
                render: function (data, type, row) {
                    // Kết hợp các phần tử trong mảng thành một chuỗi, ngăn cách bởi dấu ','
                    return data.join(', ');
                }
            },
          
            {
                "targets": 1,
                "width": "50px",
                "orderable": false,
                "render": function (data, type, row) {
                    var Id = '';
                    if (type === 'display' && data !== null) {
                        Id = row.userId;
                    }
                    return `<a href="/UserRoles/Manage/${Id}" class="btn btn-primary center-block m-1">Edit</a>`;
                }
            },

        ],
        "lengthMenu": [[5, 10, 20, 50, 100], [5, 10, 20, 50, 100]],
        "pageLength": 5
    });
});
