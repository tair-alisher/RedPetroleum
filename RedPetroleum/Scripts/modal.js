//$(function () {
//    $('.element').click(function (e) {

//        e.preventDefault();
//        $('#delete').modal('show');
//        console.log($(this).data('id'));

//        var id = $(this).data('id').toString();
//        $('#itemid').val(id);
//        console.log($('#itemid').val());
//    });


//    $('#Delete').click(function () {
//        var id = $('#itemid').val().toString();
//        console.log(id);
//        $.post("@Url.Action("Delete", "Roles", new { id = "+ id +"})", { id: id }, function (data) {

//        });

//        $('#exampleModalCenter').modal('hide');
//    });
//});

$(document).ready(function () {
    $('.element').click(function (e) {
        e.preventDefault();
        var itemid = $(this).data('id');
        $('#itemid').val(id);
        console.log(id);
    });

    //$('#action').click(function () {
    //    var itemId = $('#itemid').val();

    //    var actionLink = "/Roles/Delete/" + itemId.toString();
    //    window.location.href = actionLink;
    //    console.log(itemId);
    //    console.log(actionLink);
    //});
    $('#action').click(function () {
        var itemId = $('#itemid').val();
        //var urlAction = @Url.Action("Delete", "Roles");
        $.post("/Roles/Delete", { itemId: itemId }, function (data) {

            alert("Deleted");

        });
        console.log(actionUrl);
    $('#exampleModalCenter').modal('hide');
});
});