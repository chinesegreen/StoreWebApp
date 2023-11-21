// $(document).on('change', 'input[type=checkbox]', function () {
let $this = $(this), $chks = $(document.getElementsByName(this.name)), $all = $chks.filter(".chk-all");
if ($this.hasClass('chk-all')) {
    $chks.prop('checked', $this.prop('checked'));
} else switch ($chks.filter(":checked").length) {
    case +$all.prop('checked'):
        $all.prop('checked', false).prop('indeterminate', false)
        break;
    case $chks.length - !!$this.prop('checked'):
        $all.prop('checked', true).prop('indeterminate', false)
        break;
    default:
        $all.prop('indeterminate', true)
}
// })
$('.info-dop_time').each((ind, i) => {
    $(i)
});
$("#all-prod").on('change', (e) => {
    if ($(e.target).prop('checked')) {
        $('.product__radio').each((ind, i) => {
            $(i).prop('checked', true);
        })
    } else {
        $('.product__radio').each((ind, i) => {
            $(i).prop('checked', false);
        })
    }
})

let ids = {
    "ids": []
}

$('.product__radio').on('change', (e) => {
    if ($(e.target).prop('checked')) {
        ids['ids'].push(+$(e.target).val())
    }
})

$('.title-block__btn-remove').on('click', () => {
    if (ids['ids'].length > 0) {
        ids = JSON.stringify(ids);
        var settings = {
            "url": "https://localhost:7214/admin/removefromsale",
            "method": "POST",
            "timeout": 0,
            "processData": false,
            "mimeType": "multipart/form-data",
            "contentType": "application/json",
            "data": ids
        };

        $.ajax(settings).done(function (response) {
            console.log(response);
        });
    }
})