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

$('.search-block__btn').on('click', () => {
    if ($('.search-block__inp').val()) {
        $('.search-block__btn').attr('href', `https://localhost:7214/Admin/Search/${$('.search-block__inp').val()}`)
    }
})

$('.title-block__btn-restore').on('click', () => {
    $('.title-block__btn-restore').attr('href', `https://localhost:7214/Admin`)
})

$('.title-block__btn-delete').on('click', () => {
    $('.title-block__btn-delete').attr('href', `https://localhost:7214/Admin`)
})

$('.title-block__btn-remove').on('click', () => {
    $('.title-block__btn-remove').attr('href', `https://localhost:7214/Admin`)
})

let ids = {
    "ids": []
}
let checkCount = (length) => {
    if (length <= 0) {
        $('.title-block__btn-remove').addClass('title-block__btn--disable')
        $('.title-block__btn-delete').addClass('title-block__btn--disable')
        $('.title-block__btn-restore').addClass('title-block__btn--disable')
    } else {
        $('.title-block__btn-remove').removeClass('title-block__btn--disable')
        $('.title-block__btn-delete').removeClass('title-block__btn--disable')
        $('.title-block__btn-restore').removeClass('title-block__btn--disable')
    }
}
let deleteItem = (arr, to) => {
    if (arr.length > 0) {
        for (let i = 0, len = arr.length; i < len; i++) {
            if (arr[i] === to) {
                arr.splice(i, 1);
                break;
            }
        }
    } else if (arr.length == 0) {
        ids['ids'].pop()
    }
}
let changeAllProduct = (e) => {
    ids['ids'].length = 0;
    $('.product__radio').prop('checked', false)
    $('.product__radio').each((ind, i) => {
        if ($(e.target).prop('checked')) {
            $(i).prop('checked', true)
            ids['ids'].push(+$(i).val())
        } else {
            $(i).prop('checked', false)
            ids['ids'].pop()
        }
    })
    checkCount(ids['ids'].length)
    return ids['ids']
}
let changeProduct = (e) => {
    if ($("#all-prod").prop('checked')) {
        $("#all-prod").prop('checked', false)
    }
    if ($(e.target).prop('checked')) {
        $(e.target).prop('checked', true)
        ids['ids'].push(+$(e.target).val())
    } else {
        $(e.target).prop('checked', false)
        deleteItem(ids['ids'], +$(e.target).val())
    }
    checkCount(ids['ids'].length)
    return ids['ids']
}
checkCount(ids['ids'].length)
$('#all-prod').on('change', changeAllProduct)
$('.product__radio').on('change', changeProduct)

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
        $('#all-prod').prop('checked', false)
        $('.product__radio').prop('checked', false)
        ids['ids'].length = 0;
    } else {
        alert('Вы не выбрали ни одного продукта')
    }
    checkCount(ids['ids'].length)
})
$('.title-block__btn-delete').on('click', () => {
    let daun = confirm(`Вы точно хотите навсегда удалить выбранные элементы?`)
    if (daun) {
        if (ids['ids'].length > 0) {
            ids = JSON.stringify(ids);
            var settings = {
                "url": "https://localhost:7214/admin/delete",
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
            $('#all-prod').prop('checked', false)
            $('.product__radio').prop('checked', false)
            ids['ids'].length = 0;
        } else {
            alert('Вы не выбрали ни одного продукта')
        }
        checkCount(ids['ids'].length)
    }
})

$('.title-block__btn-restore').on('click', () => {
    if (ids['ids'].length > 0) {
        ids = JSON.stringify(ids);
        var settings = {
            "url": "https://localhost:7214/admin/restoreforsale",
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
        $('#all-prod').prop('checked', false)
        $('.product__radio').prop('checked', false)
        ids['ids'].length = 0;
    } else {
        alert('Вы не выбрали ни одного продукта')
    }
    checkCount(ids['ids'].length)
})
