const apiBaseUrl = "https://gorest.co.in/public/v2/users";
const apiToken = "Bearer 9625f3ce793514d2ffb6dcaec7144fb0a5e19e2def092eca89daa1026ad84568";

const tableSelector = "#usersTable";
const tbodySelector = "#userTableBody";

const columnKeyMap = {
    "adı": "name",
    "e-posta": "email",
    "cinsiyet": "gender",
    "durum": "status"
};

function loadAllUsers() {
    $.ajax({
        url: `${apiBaseUrl}?page=1&per_page=50`,
        method: "GET",
        dataType: "json",
        headers: { Authorization: apiToken },
        success: function (users) {
            renderUsers(users);
        },
        error: function () {
            showToast("Kullanıcılar yüklenemedi.", "danger");
        }
    });
}

function renderUsers(users) {
    let rows = "";
    users.forEach(user => {
        rows += `
            <tr>
                <td>${user.id}</td>
                <td>${user.name}</td>
                <td>${user.email}</td>
                <td>${user.gender}</td>
                <td>${user.status}</td>
                <td>
                    <div class="btn-group">
                        <button class="btn btn-primary btn-sm edit-btn">Düzenle</button>
                        <button class="btn btn-danger btn-sm delete-btn">Sil</button>
                    </div>
                </td>
            </tr>`;
    });

    if ($.fn.DataTable.isDataTable(tableSelector)) {
        $(tableSelector).DataTable().clear().destroy();
    }
    $(tbodySelector).html(rows);

    $(tableSelector).DataTable({
        pagingType: "full_numbers",
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Turkish.json",
            search: "",
            searchPlaceholder: "Kullanıcı ara...",
            paginate: {
                first: "İlk",
                last: "Son",
                previous: "<i class='bi bi-arrow-left'></i>",
                next: "<i class='bi bi-arrow-right'></i>"
            }
        },
        columnDefs: [
            { targets: 0, width: "50px" },
            { targets: 1, width: "150px", searchable: true },
            { targets: 3, width: "80px", searchable: true },
            { targets: 4, width: "80px" },
            { targets: "_all", className: "text-start" }
        ],
        dom: "<'row mb-3'<'col-sm-6'l><'col-sm-6 text-end'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row mt-3'<'col-sm-12 d-flex justify-content-center'p>>" +
            "<'row'<'col-sm-12 text-center'i>>",
        initComplete: function () {
            const $searchInput = $(tableSelector + "_filter input");
            $searchInput.addClass("form-control form-control-sm").css({
                "border": "2px solid #007bff",
                "box-shadow": "0 0 5px rgba(0, 123, 255, 0.5)"
            });
            $(tableSelector + "_filter label").contents().filter(function () {
                return this.nodeType === 3;
            }).remove();
        }
    });
}

function showToast(message, type = "info") {
    const toastId = `toast-${Date.now()}`;
    const toastHtml = `
        <div id="${toastId}" class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true"
             style="position: fixed; bottom: 1rem; right: 1rem; z-index: 9999;">
            <div class="d-flex">
                <div class="toast-body">${message}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Kapat"></button>
            </div>
        </div>`;
    const $toast = $(toastHtml).appendTo("body");
    const toast = new bootstrap.Toast($toast[0]);
    toast.show();
    setTimeout(() => $toast.remove(), 5000);
}

function getApiErrorMessage(xhr) {
    const errors = xhr.responseJSON;
    if (Array.isArray(errors)) {
        return errors.map(e => `${capitalize(e.field)} ${e.message}.`).join("\n");
    }
    return "Bir hata oluştu.";
}

function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

$(document).ready(function () {
    loadAllUsers();

    $(tbodySelector).on("click", ".edit-btn", function () {
        const $btn = $(this);
        const $row = $btn.closest("tr");

        if ($btn.text().trim() === "Düzenle") {
            let originalData = [];
            $row.find("td").each((i, td) => {
                if (i > 0 && i < 5) originalData.push($(td).text().trim());
            });
            $row.data("originalData", originalData);

            $row.find("td").each((i, td) => {
                const text = $(td).text().trim();
                if (i > 0 && i < 5) {
                    if (i === 3) {
                        $(td).html(`
                            <select class="form-select">
                                <option value="male" ${text === "male" ? "selected" : ""}>Erkek</option>
                                <option value="female" ${text === "female" ? "selected" : ""}>Kadın</option>
                            </select>`);
                    } else if (i === 4) {
                        $(td).html(`
                            <select class="form-select">
                                <option value="active" ${text === "active" ? "selected" : ""}>active</option>
                                <option value="inactive" ${text === "inactive" ? "selected" : ""}>inactive</option>
                            </select>`);
                    } else {
                        $(td).html(`<input type="text" class="form-control" value="${text}">`);
                    }
                }
            });
            $btn.text("Kaydet").removeClass("btn-primary").addClass("btn-success");
            $row.find(".delete-btn").hide();
            if (!$row.find(".cancel-btn").length) {
                $btn.after('<button class="btn btn-secondary btn-sm cancel-btn">İptal</button>');
            }
        } else {
            const updatedData = {};
            $row.find("td").each((i, td) => {
                if (i === 0) {
                    updatedData.id = $(td).text().trim();
                } else if (i > 0 && i < 5) {
                    const label = $("table thead th").eq(i).text().toLowerCase();
                    const key = columnKeyMap[label];
                    updatedData[key] = $(td).find("input, select").val().trim();
                    $(td).html(updatedData[key]);
                }
            });

            if (!['male', 'female'].includes(updatedData.gender)) {
                showToast("Cinsiyet hatalı.", "danger");
                return;
            }
            if (!['active', 'inactive'].includes(updatedData.status)) {
                showToast("Durum hatalı.", "danger");
                return;
            }

            $btn.text("Düzenle").removeClass("btn-success").addClass("btn-primary");
            $row.find(".cancel-btn").remove();
            $row.find(".delete-btn").show();

            $.ajax({
                url: `${apiBaseUrl}/${updatedData.id}`,
                method: "PUT",
                headers: { Authorization: apiToken },
                data: updatedData,
                success: () => {
                    $row.addClass("table-success");
                    $row.removeData("originalData");
                    const table = $(tableSelector).DataTable();
                    table.row($row).invalidate().draw(false);
                    setTimeout(() => $row.removeClass("table-success"), 1000);
                    showToast("Kullanıcı güncellendi.", "success");
                },
                error: (xhr) => {
                    $row.addClass("table-danger");
                    const originalData = $row.data("originalData");
                    if (originalData && originalData.length === 4) {
                        $row.find("td").each((i, td) => {
                            if (i > 0 && i < 5) $(td).html(originalData[i - 1]);
                        });
                    }
                    $row.find(".cancel-btn").remove();
                    $btn.text("Düzenle").removeClass("btn-success").addClass("btn-primary");
                    $row.find(".delete-btn").show();
                    setTimeout(() => $row.removeClass("table-danger"), 1000);
                    showToast(getApiErrorMessage(xhr), "danger");
                }
            });
        }
    });

    $(tbodySelector).on("click", ".cancel-btn", function () {
        const $row = $(this).closest("tr");
        const originalData = $row.data("originalData");
        if (originalData && originalData.length === 4) {
            $row.find("td").each((i, td) => {
                if (i > 0 && i < 5) $(td).html(originalData[i - 1]);
            });
        }
        $row.find(".edit-btn").text("Düzenle").removeClass("btn-success").addClass("btn-primary");
        $row.find(".delete-btn").show();
        $(this).remove();
        $row.removeData("originalData");
    });

    $(tbodySelector).on("click", ".cancel-new-btn", function () {
        $(this).closest("tr").remove();
    });

    $(tbodySelector).on("click", ".delete-btn", function () {
        if (!confirm("Silmek istediğinizden emin misiniz?")) return;
        const $row = $(this).closest("tr");
        const id = $row.find("td").eq(0).text().trim();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: "DELETE",
            headers: { Authorization: apiToken },
            success: () => {
                $row.remove();
                showToast("Kullanıcı silindi.", "success");
            },
            error: (xhr) => {
                showToast(getApiErrorMessage(xhr), "danger");
            }
        });
    });

    $("#addUserBtn").on("click", function () {
        if ($(`${tableSelector} tbody tr.new-user-row`).length) return;

        const newRow = `
            <tr class="new-user-row">
                <td>—</td>
                <td><input type="text" class="form-control" placeholder="Adı"></td>
                <td><input type="email" class="form-control" placeholder="E-posta"></td>
                <td>
                    <select class="form-select">
                        <option value="male">Erkek</option>
                        <option value="female">Kadın</option>
                    </select>
                </td>
                <td>
                    <select class="form-select">
                        <option value="active">active</option>
                        <option value="inactive">inactive</option>
                    </select>
                </td>
                <td>
                    <button class="btn btn-success btn-sm save-new-btn">
                        <i class="bi bi-check2-circle"></i> Kaydet
                    </button>
                    <button class="btn btn-secondary btn-sm cancel-new-btn">İptal</button>
                </td>
            </tr>`;
        $(`${tableSelector} tbody`).prepend(newRow);
    });

    $(tbodySelector).on("click", ".save-new-btn", function () {
        const $row = $(this).closest("tr");
        const name = $row.find("td:eq(1) input").val().trim();
        const email = $row.find("td:eq(2) input").val().trim();
        const gender = $row.find("td:eq(3) select").val();
        const status = $row.find("td:eq(4) select").val();

        if (!name || !email) {
            showToast("Ad ve e-posta zorunludur.", "danger");
            return;
        }

        const newUser = { name, email, gender, status };

        $.ajax({
            url: apiBaseUrl,
            method: "POST",
            headers: { Authorization: apiToken },
            data: newUser,
            success: () => {
                $row.addClass("table-success");
                setTimeout(() => $row.removeClass("table-success"), 1000);
                showToast("Yeni kullanıcı eklendi.", "success");
                loadAllUsers();
            },
            error: (xhr) => {
                $row.addClass("table-danger");
                setTimeout(() => $row.removeClass("table-danger"), 1000);
                showToast(getApiErrorMessage(xhr), "danger");
            }
        });
    });
});
