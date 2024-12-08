$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblTiendas').DataTable({
        "ajax": { url: '/tienda/getall' },
        "columns": [
            { data: 'almacen', "width": "10%" },
            { data: 'tienda', "width": "10%" },
            { data: 'nombre', "width": "30%" },
            { data: 'latitud', "width": "15%" },
            { data: 'longitud', "width": "15%" },
            { data: 'tarimas', "width": "10%" },
            {
                data: null,
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group" style="width:100%">
                            <a href="/tienda/edit?almacen=${data.almacen}&tienda=${data.tienda}" 
                               class="btn btn-primary mx-2"> 
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick="Delete('/tienda/delete?tienda=${data.tienda}')" 
                               class="btn btn-danger mx-2"> 
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
        text: "Esta acción no se puede revertir.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        Swal.fire('¡Eliminado!', data.message, 'success');
                        dataTable.ajax.reload();
                    } else {
                        Swal.fire('Error', data.message, 'error');
                    }
                }
            });
        }
    });
}
