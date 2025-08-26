$(function () {

    // Global Modal Açıcı
    // data-modal-url="URL_ADRESI" şeklinde bir attribute'e sahip herhangi bir elemente tıklandığında çalışır.
    $(document).on('click', '[data-modal-url]', function (e) {
        e.preventDefault();

        var url = $(this).data('modal-url');
        var modalContainer = $('#appModal .modal-content');

        // Modal içeriğini boşalt ve yükleniyor göstergesi ekle
        modalContainer.html('<div class="modal-body text-center p-5"><div class="spinner-border text-primary" role="status"><span class="sr-only">Yükleniyor...</span></div></div>');
        $('#appModal').modal('show');

        // Belirtilen URL'den içeriği yükle
        modalContainer.load(url, function (response, status, xhr) {
            if (status == "error") {
                modalContainer.html('<div class="modal-body">İçerik yüklenirken bir hata oluştu. Lütfen tekrar deneyin.</div>');
            }
        });
    });

});
