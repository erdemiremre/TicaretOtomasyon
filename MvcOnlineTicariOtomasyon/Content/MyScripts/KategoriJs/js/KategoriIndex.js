$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

function AddProduct() {
    var categoryName = $('#KategoriAd').val();
    if (categoryName == "") {
        alertify.danger("Kategori adı girilmedi.");
    }
    else {
        $.ajax({
            url: "/Kategori/KategoriEkleJson",
            type: "POST",
            dataType: "json",
            data: { KategoriAd: categoryName },
            beforeSend: function () {
                $('body').loadingModal({ text: "Kategori ekleniyor..", animation: 'cubeGrid', backgroundColor: 'black' });
            },
            success: function (data) {
                if (data.Result == false) {
                    alertify.error(data.FeedBack);
                }
                else {
                    if ($('tr').length < 5) {
                        var newRowContent = '<tr><td>' + data.Object.KategoriID + '</td>';
                        newRowContent += '<td data-id="' + data.Object.KategoriID + '" > ' + data.Object.KategoriAd + '</td > ';
                        newRowContent += '<td><a href="/Kategori/KategoriSil/' + data.Object.KategoriID + '" class="btn btn-danger">Sil</a></td>';
                        newRowContent += '<td><a href="/Kategori/KategoriGetir/' + data.Object.KategoriID + '" class="btn btn-success">Güncelle</a></td></tr>';
                        $("#tableIndex tbody").append(newRowContent);
                    }

                    $('[data-dismiss="modal"]').click();
                    alertify.success(data.FeedBack);
                    $('#KategoriAd').val("");
                }
                $('body').loadingModal('destroy');

            }
        });
    }
}
$('.modalAc').on('click', function (e) {
    var kategoriAdi = $(this).attr('data-adi');
    var kategoriID = $(this).attr('data-id');
    $('#KategoriAdUpdate').val(kategoriAdi);
    $('#updateButton').attr('data-update', kategoriID);
    debugger;
})

$('#updateButton').on('click', function () {
    var kategoriID = $(this).attr('data-update');
    var kategoriAdi = $('#KategoriAdUpdate').val();
    debugger;
    $.ajax({
        url: "/Kategori/KategoriGuncelleJson",
        type: "POST",
        dataType: "json",
        data: { KategoriAd: kategoriAdi, KategoriID: kategoriID },
        success: function (data) {
            debugger;
            if (data.Result == false) {
                alertify.error(data.FeedBack);
            }
            else {
                debugger;
                $('[data-name="' + data.Object.KategoriID + '"]').html(data.Object.KategoriAd);
                alertify.success(data.FeedBack);
                $('#KategoriAdUpdate').val("");
                debugger;
                $('[data-id="' + data.Object.KategoriID + '"]').attr('data-adi', data.Object.KategoriAd);
                debugger;
            }
            $('[data-dismiss="modal"]').click();
            $('body').loadingModal('destroy');
        }
    });


})

$('.modalSilAc').click(function () {
    var kategoriAdi = $(this).data("adi");
    $(".modal-body>p").html(kategoriAdi + " kategorisini silmek istiyor musun?");
    var kategoriid = $(this).data("id");
    debugger
    $(".kategoriSil").data("id", kategoriid);
    debugger
});

$(".kategoriSil").click(function () {
    debugger;
    var kategoriid = $(this).data("id");
    debugger;
    $.ajax({
        url: "/Kategori/KategoriSilJson?id=" + kategoriid,
        type: "POST",
        dataType: "json",
        success: function (data) {
            debugger;
            if (data.Result == false) {
                alertify.error(data.FeedBack);
                debugger;
            }
            else {
                $('[data-name="' + data.Object.id + '"]').parent('tr').remove();
            }
        }

    });


});