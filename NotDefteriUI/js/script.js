var apiUrl = "https://localhost:44367/";
var path = window.location.pathname
function getAccessToken() {
    var loginDataJson = sessionStorage["login"] || localStorage["login"];
    var loginData;

    try {
        loginData = JSON.parse(loginDataJson)
    } catch (error) {
        window.location.href = "login2.html";
        return null;
    }

    if (!loginData || !loginData.token) {
        return window.location.href = "login2.html";
    }

    return loginData.token;
}
function getAuthHeaders() {
    return { Authorization: "Bearer " + getAccessToken() };
}

function loginControl() {
    if (path.endsWith("/login2.html")) return;

    var accessToken = getAccessToken();

    if (!accessToken) {
        window.location.href = "login2.html"
        return;
    }

    $.ajax({
        type: "get",
        url: apiUrl + "api/Login",
        data: { token: accessToken },
        success: function (data) {
            notlariListele();
            $("#loginAd").text("Hoşgeldin " + data);
        },
        error: function () {
            window.location.href = "login2.html";
        }
    });
};


$("#frmGiris").submit(function (e) {
    e.preventDefault();
    $.ajax({
        type: "post",
        url: apiUrl + "api/Login",
        datatype: "JSON",
        data: $("#frmGiris").serialize(),
        success: function (data) {
            localStorage.removeItem("login");
            sessionStorage.removeItem("login");
            var hatirla = $("#rememberme").prop("checked");
            var storage = hatirla ? localStorage : sessionStorage;
            storage["login"] = JSON.stringify(data);
            toastr.success("Giriş başarılı Not Defterinize yönlendiriliyorsunuz...");
            setTimeout(function () {
                window.location.href = "index.html";
            }, 1000);
            frmGiris.reset();
        },
        error: function () {
            toastr.error("Giriş yapılırken hata oluştu lütfen bilgilerinizi kontrol edin...")
        }
    });
});

$("#frmRegister").submit(function (e) {
    e.preventDefault()
    var pwdReg = $("#inputPasswordRegister");
    var pwdRegConfirm = $("#inputPasswordRegisterDoğrula");
    $.ajax({
        type: "post",
        url: apiUrl + "api/Register",
        datatype: "JSON",
        data: $("#frmRegister").serialize(),
        success: function (data) {
            toastr.success("Hesabınız başarıyla oluşturuldu lütfen giriş ekranından giriş yapın.");
            pwdReg.val("");
            pwdRegConfirm.val("");
        },
        error: function () {
            if (pwdReg.val() != pwdRegConfirm.val()) {
                toastr.error("Parolonız eşleşmiyor...")
                pwdReg.val("");
                pwdRegConfirm.val("");

            }
            else {
                toastr.error("Hesap oluştururken bir hata oluştu");
            }
            pwdReg.val("");
            pwdRegConfirm.val("");
        }
    });
});

$("body").on("click", "#createStickyBtn", function (e) {
    e.preventDefault();
    $.ajax({
        type: "post",
        headers: getAuthHeaders(),
        url: apiUrl + "api/NotDefteri",
        data: { Icerik: "" },
        success: function (data) {
            $("#stickerTime").text(tarihBicimlendir(data.time));
            notuDuvaraEkle(data);

            toastr.success("Bir Post-it yapıştırdın.");
        },
        error: function () {
            toastr.error("Sanırım kötü bişey oldu Post-it bitmiş olabilir :(")
        }
    });
});

$("body").on("click", "#createStickyBtnKucuk", function (e) {
    e.preventDefault();
    $.ajax({
        type: "post",
        headers: getAuthHeaders(),
        url: apiUrl + "api/NotDefteri",
        data: { Icerik: "" },
        success: function (data) {
            $("#stickerTime").text(tarihBicimlendir(data.time));
            notuDuvaraEkle(data);
            toastr.success("Bir Post-it daha yapıştırdın.")
        },
        error: function () {
            toastr.error("Sanırım kötü bişey oldu Post-it bitmiş olabilir :(")
        }
    });
});

$("body").on("click", "[data-save-id]", function (e) {
    e.preventDefault();
    var saveId = $(this).data("save-id");
    console.log(saveId);
    $.ajax({
        type: "put",
        headers: getAuthHeaders(),
        url: apiUrl + "api/NotDefteri",
        data: { Id: saveId, Icerik: $("#content-" + saveId).val() },
        success: function (data) {
            toastr.success("Post-it yazı ekledin.")
        },
        error: function () {
            toastr.error("Sanırım kötü bişey oldu :( ")
        }
    });
});

$("body").on("click", "[data-delete-id]", function (e) {
    e.preventDefault();
    var deleteId = $(this).data("delete-id");
    console.log(deleteId);
    $.ajax({
        type: "delete",
        headers: getAuthHeaders(),
        url: apiUrl + "api/NotDefteri/" + deleteId,
        success: function (data) {
            toastr.success("Post-it çöpe atıldı");
            $("#burayaEkle").html("")
            notlariListele();
        },
        error: function () {
            toastr.error("Sanırım bir sorun oldu:( ")
        }
    });
});

$(document).ajaxStart(function () {
    $("#loading").removeClass("d-none");
});

$(document).ajaxStop(function () {
    $("#loading").addClass("d-none");
});

function tarihBicimlendir(isoTarih) {
    if (!isoTarih) {
        return "Belirtilmedi";
    }
    var tarih = new Date(isoTarih);
    return tarih.toLocaleDateString();
}

function notlariListele() {
    $.ajax({
        type: "get",
        headers: getAuthHeaders(),
        url: apiUrl + "api/NotDefteri",
        success: function (data) {
            console.log(data);
            notlariTabloyaEkle(data);
        },
        error: function () {
        }

    });
}

function notlariTabloyaEkle(notdefterleri) {
    for (var i = 0; i < notdefterleri.length; i++) {
        notuDuvaraEkle(notdefterleri[i]);
    }
};
function notuDuvaraEkle(notdefteri) {
    var html = "<div>'" + notdefteri.id + "'</div>"
    var html = '<div class="sticky" style="display:block;"><div class="sticky-header"><span class="sticky-header-menu add-button fas fa-plus" title="new sticky" id="createStickyBtnKucuk"></span><span class="sticky-header-menu drop-button fas fa-check" data-save-id=' + notdefteri.id + '></span><span class="sticky-header-menu remove-button fas fa-trash-alt" title="delete sticky" data-delete-id=' + notdefteri.id + '></span></div><span id="stickerTime">' + tarihBicimlendir(notdefteri.time) + '</span><textarea class="sticky-content" id="content-' + notdefteri.id + '">"' + (notdefteri.icerik ? notdefteri.icerik : "") + '"</textarea></div>';



    $("#burayaEkle").append(html);
};

loginControl();