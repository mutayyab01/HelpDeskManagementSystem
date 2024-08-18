$(document).on("change", "#CategoryId.get-subcategories", function (e) {
    var $dropdown = $(this);  // Store the reference to the dropdown
    var working = "<span id='spin'><i class='fa fa-spinner fa-spin fa-3x fa-fw'></i></span>";

    if ($dropdown.val() !== "") {
        $.ajax({
            url: "/Data/GetTicketSubCategories/" + $dropdown.val(),
            dataType: "json",
            crossDomain: true,
            beforeSend: function () {
                $dropdown.parent().append(working);
                $("#SubCategoryId").html("");
                $("#SubCategoryId").append("<option value=''>--Select Sub-Categories --</option>");
            },
            success: function (json) {
                var data = json;
                console.log(data);
                $(data).map(function () {
                    $("#SubCategoryId").append($('<option></option>').val(this.id).html(this.name));
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("Error: " + thrownError);
            },
            complete: function () {
                $("#spin").remove();
            }
        });
    }
});
