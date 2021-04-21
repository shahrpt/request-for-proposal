function Save() {
    var par = $(this).parent().parent(); //tr
    var tdName = par.children("td:nth-child(1)");
    var tdButtons = par.children("td:nth-child(2)");
    var id = $(par).data("id");
    var parentid = $(par).data("parentid");
    var name = tdName.children("input[type=text]").val();

    if (parentid == undefined) {
        $.getJSON('/ProductType/UpdateType?id=' + id + '&name=' + name, null, function (result) {
            if (result.IsSuccess) {
                tdName.html(tdName.children("input[type=text]").val());
                tdButtons.html("<img src='/content/images/edit.png' class='btnEdit'/>&nbsp;&nbsp;&nbsp;<img src='/content/images/delete.png' class='btnDelete'/>");

                $(".btnEdit").bind("click", Edit);
                $(".btnDelete").bind("click", Delete);
                showSuccess("Type updated successfully!");
            }
            else {
                showError(result.Message);
            }
        });
    }
    else {
        $.getJSON('/ProductType/UpdateSubtype?id=' + id + '&name=' + name + '&parentid=' + parentid, null, function (result) {
            if (result.IsSuccess) {
                tdName.html(tdName.children("input[type=text]").val());
                tdButtons.html("<img src='/content/images/edit.png' class='btnEdit'/>&nbsp;&nbsp;&nbsp;<img src='/content/images/delete.png' class='btnDelete'/>");

                $(".btnEdit").bind("click", Edit);
                $(".btnDelete").bind("click", Delete);
                showSuccess("Sub-type updated successfully!");
            }
            else {
                showError(result.Message);
            }
        });
    }
}; 

function Edit() {
    var par = $(this).parent().parent(); //tr
    var tdName = par.children("td:nth-child(1)");
    var tdButtons = par.children("td:nth-child(2)");

    tdName.html("<input class='form-control' type='text' id='txtName' value='" + tdName.html() + "'/>");
    tdButtons.html("<img src='/content/images/save.png' class='btnSave'/>");

    $(".btnSave").bind("click", Save);
    $(".btnEdit").bind("click", Edit);
    $(".btnDelete").bind("click", Delete);
};

function Delete() {
    var par = $(this).parent().parent(); //tr
    var id = $(par).data("id");
    var parentid = $(par).data("parentid");

    if (parentid == undefined) {
        $.getJSON('/ProductType/DeleteType?id=' + id, null, function (result) {
            if (result.IsSuccess) {
                par.remove();
                showSuccess("Type deleted successfully!");
            }
            else {
                showError(result.Message);
            }
        });
    }
    else {
        $.getJSON('/ProductType/DeleteSubtype?id=' + id, null, function (result) {
            if (result.IsSuccess) {
                par.remove();
                showSuccess("Subtype deleted successfully!");
            }
            else {
                showError(result.Message);
            }
        });
    }
}; 

function selectRow(row) {
    $("#tblTypes tbody tr").removeClass("selected");
    $("#tblSubTypes tbody tr").remove();
    $(row).addClass("selected");
    var typeId = $(row).data("id");

    $.getJSON('/ProductType/GetSubTypes?id=' + typeId , null, function (result) {
        if (result.IsSuccess && result.DataCollection.length > 0) {
            $.each(result.DataCollection, function (index, item) {
                $("#tblSubTypes tbody").append(
                    "<tr data-id='" + item.Id + "' data-parentid='" + item.ParentID + "'>" +
                    "<td>" + item.Name + "</td>" +
                    "<td  class='text-center' style='vertical-align: middle;'><img src='/content/images/edit.png' class='btnEdit'/>&nbsp;&nbsp;&nbsp;<img src='/content/images/delete.png' class='btnDelete'/></td>" +
                    "</tr>");
            });

            $(".btnEdit").bind("click", Edit);
            $(".btnDelete").bind("click", Delete);
        }
    });
}

$(function () {
    LoadTypes();

    //Add, Save, Edit and Delete functions code
    $(".btnEdit").bind("click", Edit);
    $(".btnDelete").bind("click", Delete);
    $("#btnAddType").click(function () {
        if ($("#txtNewType").val().trim().length > 0) {
            $.getJSON('/ProductType/AddType?name=' + $("#txtNewType").val().trim(), null, function (result) {
                if (result.IsSuccess) {
                    $("#tblTypes tbody").append(
                        "<tr onclick='selectRow(this);' data-id='" + result.Data.Id + "'>" +
                        "<td>" + $("#txtNewType").val() + "</td>" +
                        "<td  class='text-center' style='vertical-align: middle;'><img src='/content/images/edit.png' class='btnEdit'/>&nbsp;&nbsp;&nbsp;<img src='/content/images/delete.png' class='btnDelete'/></td>" +
                        "</tr>");
                    $("#txtNewType").val("");
                    $(".btnEdit").bind("click", Edit);
                    $(".btnDelete").bind("click", Delete);
                    showSuccess("Record saved successfully!");
                }
                else {
                    showError("Could not save record!");
                }
            });
        }
        else {
            showError("Please enter name.");
        }
    });

    $("#btnAddSubType").click(function () {
        if ($("#tblTypes tbody tr.selected").length != 1) {
            showError("Please select type!");
            return false;
        }

        if ($("#txtNewSubType").val().trim().length > 0) {
            var parentId = $("#tblTypes tbody tr.selected").data("id");
            $.getJSON('/ProductType/AddSubtype?name=' + $("#txtNewSubType").val().trim() + '&parentid=' + parentId, null, function (result) {
                if (result.IsSuccess) {
                    $("#tblSubTypes tbody").append(
                        "<tr data-id='" + result.Data.Id + "' data-parentid='" + result.Data.ParentID + "'>" +
                        "<td>" + $("#txtNewSubType").val() + "</td>" +
                        "<td  class='text-center' style='vertical-align: middle;'><img src='/content/images/edit.png' class='btnEdit'/>&nbsp;&nbsp;&nbsp;<img src='/content/images/delete.png' class='btnDelete'/></td>" +
                        "</tr>");
                    $("#txtNewSubType").val("");
                    $(".btnEdit").bind("click", Edit);
                    $(".btnDelete").bind("click", Delete);
                    showSuccess("Record saved successfully!");
                }
                else {
                    showError("Could not save record!");
                }
            });
        }
        else {
            showError("Please enter name.");
        }
    });
});

function showError(msg) {
    $(".alert span.message").html("");
    $(".alert").addClass("alert-danger").removeClass("hide alert-success");
    $(".alert span.message").html(msg);
    setTimeout(function () {
        $(".alert span.message").html("");
        $(".alert").addClass("hide");
    }, 3000)
}

function showSuccess(msg) {
    $(".alert span.message").html("");
    $(".alert").addClass("alert-success").removeClass("hide alert-danger");
    $(".alert span.message").html(msg);
    setTimeout(function () {
        $(".alert span.message").html("");
        $(".alert").addClass("hide");
    }, 3000)
}

function LoadTypes() {
    $.getJSON('/lookup/GetAllTypes', null, function (result) {
        if (result.IsSuccess && result.DataCollection.length > 0) {

            $.each(result.DataCollection, function (index, item) {
                $("#tblTypes tbody").append(
                    "<tr onclick='selectRow(this);' data-id=" + item.Id + ">" +
                    "<td>" + item.Name + "</td>" +
                    "<td  class='text-center' style='vertical-align: middle;'><img src='/content/images/edit.png' class='btnEdit'/>&nbsp;&nbsp;&nbsp;<img src='/content/images/delete.png' class='btnDelete'/></td>" +
                    "</tr>");
            });

            $(".btnEdit").bind("click", Edit);
            $(".btnDelete").bind("click", Delete);
        }
    });
}