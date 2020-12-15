$(document).ready(function () {
    Listele();
});



function Listele() {
    $.ajax({
        url: "/Kategori/KategoriListele/",
        type: "POST",
        dataType: "json",
        beforeSend: function () {
            $('body').loadingModal({ text: "Kategoriler listeleniyor..", animation: 'cuberGrid', bacgroundColor: 'black' });
        },
        success: function (data) {
            var icerik = "";
            $.each(data.Object, function (i, item) {
                icerik += "<tr>";
                icerik += "<td>" + item.KategoriID + "</td>";
                icerik += "<td>" + item.KategoriAd + "</td>";
                icerik += "<td><button type='button'  class='btn btn-warning modalAc'    data-adi='" + item.KategoriAd + "' data-id='" + item.KategoriID + "' data-toggle='modal' data-target='#UpdateProduct'><i title='Düzenle' data-toggle='tooltip' data-placement='top' class='fa fa-edit'></i></button>";
                icerik += "<button type='button'  class='btn btn-danger modalSilAc'   data-adi='" + item.KategoriAd + "' data-id='" + item.KategoriID + "' data-toggle='modal' data-target='#DeleteProduct'><i title='Sil' data-toggle='tooltip' data-placement='top' class='fa fa-trash'></i></button></td>";
                icerik += "</tr>";
            })
            $("#tbodyList").html(icerik);
            $('body').loadingModal('destroy');
        }
    });
}

$('#updateButton').on('click', function () {
    var kategoriID = $(this).attr('data-update');
    var kategoriAdi = $('#KategoriAdUpdate').val();
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
                Listele();
                alertify.success(data.FeedBack);
                $('#KategoriAdUpdate').val("");
            }
            $('[data-dismiss="modal"]').click();
            $('body').loadingModal('destroy');
        }
    })
});

$(document)
    .on('click', '.modalAc', function () {
        var kategoriAdi = $(this).attr('data-adi');
        var kategoriID = $(this).attr('data-id');
        $('#KategoriAdUpdate').val(kategoriAdi);
        $('#updateButton').attr('data-update', kategoriID);
    })
    .on('click', '.modalSilAc', function () {
        var kategoriAdi = $(this).data("adi");
        $(".modal-body>p").html(kategoriAdi + " kategorisini silmek istiyor musun?");
        var kategoriid = $(this).data("id");
        $(".kategoriSil").data("id", kategoriid);
    });

$(".kategoriSil").click(function () {
    var kategoriid = $(this).data("id");
    $.ajax({
        url: "/Kategori/KategoriSilJson?id=" + kategoriid,
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Result == false) {
                alertify.error(data.FeedBack);
            }
            else {
                Listele();
                alertify.success(data.FeedBack);
            }
            $('[data-dismiss="modal"]').click();
        }

    });
});

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
                    Listele();
                    $('[data-dismiss="modal"]').click();
                    alertify.success(data.FeedBack);
                    $('#KategoriAd').val("");
                }
                $('body').loadingModal('destroy');

            }
        });
    }
}