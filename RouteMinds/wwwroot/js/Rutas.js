$(document).ready(function () {
    loadDataTable();

    // Vincula el botón con la función ejecutarRutas
    $('#btnEjecutarRutas').on('click', function () {
        ejecutarRutas();
    });
});

function loadDataTable() {
    dataTable = $('#tblRutas').DataTable({
        "ajax": { url: '/ruta/getall' },
        "columns": [
            { data: 'almacen', "width": "15%" },
            { data: 'ruta', "width": "15%" },
            { data: 'tienda', "width": "15%" },
            { data: 'tarimas', "width": "15%" },
            { data: 'distancia', "width": "40%" }
        ],
        "paging": false, // Deshabilita la paginación
        "info": true,   // Opcional: elimina el contador de registros
        "searching": true, // Mantiene la funcionalidad de búsqueda
        "responsive": true // Permite que la tabla sea responsive
    });
}

function ejecutarRutas() {
    $("#spinner").show(); // Mostrar el spinner
    $.ajax({
        url: '/ruta/ejecutarrutas',
        type: 'POST',
        success: function (data) {
            $("#spinner").hide(); // Ocultar el spinner
            if (data.success) {
                Swal.fire(
                    '¡Éxito!',
                    data.message,
                    'success'
                );
                dataTable.ajax.reload(); // Recargar la DataTable
            } else {
                Swal.fire(
                    'Error',
                    data.message,
                    'error'
                );
            }
        },
        error: function () {
            $("#spinner").hide(); // Ocultar el spinner
            Swal.fire(
                'Error',
                'Hubo un problema al ejecutar las rutas.',
                'error'
            );
        }
    });
}
