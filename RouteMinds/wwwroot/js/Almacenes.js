$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblAlmacenes').DataTable({
        "ajax": { url: '/almacen/getall' },
        "columns": [
            { data: 'almacenid', "width": "10%" },
            { data: 'nombre', "width": "20%" },
            { data: 'latitud', "width": "20%" },
            { data: 'longitud', "width": "20%" },
            {
                data: 'almacenid',
                "render": function (data) {
                    return `<div class="btn-group" role="group" style="width:100%">
                                <a href="/almacen/edit?id=${data}" class="btn btn-primary mx-2"> 
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a onClick="Delete('/almacen/delete/${data}')" class="btn btn-danger mx-2"> 
                                    <i class="bi bi-trash-fill"></i> Delete 
                                </a>
                            </div>`;
                },
                "width": "30%"
            }
        ],
        "paging": false, // Deshabilita la paginación
        "info": true,   // Opcional: elimina el contador de registros
        "searching": true, // Mantiene la funcionalidad de búsqueda
        "responsive": true // Permite que la tabla sea responsive
    });
}

function Delete(url) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esta acción!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        Swal.fire(
                            '¡Eliminado!',
                            data.message,
                            'success'
                        );
                        dataTable.ajax.reload();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: data.message
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Ocurrió un error al intentar eliminar el almacén.'
                    });
                }
            });
        }
    });
}
