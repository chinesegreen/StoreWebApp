window.addEventListener('load', (e) => {
    e.preventDefault();
});
const requestUrl = 'https://localhost:7214/Admin/Product/Create'
$('.main-nav__link').each((indLink, iLink) => {
    $(iLink).on('click', (e) => {
        $('.form__view').each((indSection, iSection) => {
            if (indLink == indSection) {
                $(iSection).removeClass('main__form--none')
                $('.main-nav__link').addClass('main-nav__link--disable')
                $(iLink).removeClass('main-nav__link--disable')
                $('.main-nav__span').each((indSpan, iSpan) => {
                    if (indSpan == indSection) {
                        $(iSpan).removeClass('main-nav__span--disable')
                    } else {
                        $(iSpan).addClass('main-nav__span--disable')
                    }
                })
            } else {
                $(iSection).addClass('main__form--none')
            }
        })
    })
})

let sendRequest = (method, url, body = null) => {
    const headers = {
        'Content-Type': 'application/json'
    }
    return fetch(url, {
        method: method,
        body: JSON.stringify(body),
        headers: headers
    }).then(response => {
        if (response.ok) {
            return response.json()
        }
    })
}
$.fn.serializeObject = function () {
    let o = {}
    let a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]]
            }
            o[this.name].push(this.value || '')
        } else {
            o[this.name] = this.value || ''
        }
    });
    return o;
}
$('.form__btn-preview').on('click', () => {
    $('.form__view').addClass('main__form--none')
    $($('.form__view')[$('.form__view').length - 1]).removeClass('main__form--none')
    $('.main-nav__link').addClass('main-nav__link--disable')
    $('.main-nav__span').addClass('main-nav__span--disable')
    $($('.main-nav__link')[$('.main-nav__link').length - 1]).removeClass('main-nav__link--disable')
    $($('.main-nav__span')[$('.main-nav__link').length - 1]).removeClass('main-nav__span--disable')

})

// console.log($('.main__form').serializeObject()['type'])

 let photoOrd = []
 function readURL(input) {
   if (input.files && input.files[0]) {
     let reader = new FileReader();
     reader.onload = function(e) {
       if (photoOrd.length < 15) {
         $($('.form__photo-label')[photoOrd.length]).css('background-image', `url(${e.target.result})`)
         $($('.form__photo-label')[photoOrd.length+15]).css('background-image', `url(${e.target.result})`)
         $('.form__title--numcount').text(photoOrd.length+1)
         photoOrd.push($('.form__input--file').val())
       } else {
         $('.form__title--count').css('color', '#ff0000')
       }
     }
     reader.readAsDataURL(input.files[0]);
   }
 }

$("#photo").change(function () {
    console.log($('#photo')[0].files);
     readURL(this);
     console.log($('#photo')[0].files);
     if ($('#photo')[0].files.length <= 15) {
       console.log($('#photo')[0].files);
     }
})

let photoArr = []
$('#photo').on('change', () => {
    photoArr.push($('#photo')[0].files[0])
})

const form = document.querySelector('#form')
let saveData = (event) => {
    event.preventDefault()
    var data = new FormData();

    data.append("Price", $('#form').serializeObject()['Price']);
    data.append("Name", $('#form').serializeObject()['Name']);

    photoArr.forEach((item, index, arr) => {
        data.append("Images", item);
    });

/*    data.append("Tags", $('#form').serializeObject()['Tags']);*/
    data.append("VendorCode", $('#form').serializeObject()['VendorCode']);
    data.append("ValueTax", $('#form').serializeObject()['ValueTax']);

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
            console.log(this.responseText);
        }
    });

    xhr.open("POST", "https://localhost:7214/Admin/Product/Create");

    xhr.send(data);
}

form.addEventListener('submit', saveData)
$('.form__btn-preview').on('click', saveData)
