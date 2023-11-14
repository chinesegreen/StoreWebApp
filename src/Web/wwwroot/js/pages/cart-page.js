const requestUrl = 'https://jsonplaceholder.typicode.com/users'
let checkCart = ()=>{
  if (!$('.cart__item--order').length) {
    $('.cart__item--end').css({'display' : 'block'})
    $('.cart-result__subtotal-price').text(0)
    $('.cart-result__total-price').text(0)
    $('input[name=shipping-radio]').css({'pointerEvents' : 'none'})
    $('.shipping__label').css({'pointerEvents' : 'none', 'color' : '#999999'})
    $('.shipping__price').css({'color' : '#999999'})
    $('input[name=shipping-radio]').removeAttr("checked");
  } else {
    $('.cart__item--end').css({'display' : 'none'})
    
    $('input[name=shipping-radio]').css({'pointerEvents' : 'none'})
    $('.shipping__label').css({'pointerEvents' : 'visibly', 'color' : '#333333'})
    $('.shipping__price').css({'color' : '#333333'})
    $('input[name=shipping-radio]').attr("checked");
  }
}
let checkSubtotalPrice = ()=>{
  let orderPrice = 0
  $('.cart__part-item__total-price-span').each((index, item)=>{
    orderPrice = orderPrice + +$(item).text()
    $('.cart-result__subtotal-price').text(orderPrice)
  })
  return orderPrice
}
let checkVarShipping = ()=>{
  switch ($('input[name=shipping-radio]:checked').val()) {
    case 'free':
      return 0
    case 'standart':
      return 10
    case 'express':
      return 20
    default:
      console.log($('input[name=shipping-radio]:checked').val());
      console.log('Где-то косяк');
      break;
  }
}
let checkTotalPrice = ()=>{
  let orderPrice = checkVarShipping() + checkSubtotalPrice()
  $('.cart-result__total-price').text(orderPrice)
}
let deleteOrder = (ind)=>{
  $($('.cart__item--order')[ind]).remove()
  // checkTotalPrice()
}
let checkOrder = ()=>{
  $('.cart__part-input').each((index, item)=>{
    $(item).change(()=>{
      $(item).val(Math.floor($(item).val()))
      if (Math.floor($(item).val()) <= 10 && Math.floor($(item).val()) > 0) {
        let orderPrice = $($('.cart__part-item__price-span')[index]).text()*Math.floor($(item).val())
        $($('.cart__part-item__total-price-span')[index]).text(orderPrice)
      } else if (Math.floor(+$(item).val()) <= 0) {
        $(item).val(1)
        let orderPrice = $($('.cart__part-item__price-span')[index]).text()*+$(item).val()
        $($('.cart__part-item__total-price-span')[index]).text(orderPrice)
      } else {
        $(item).val(10)
        let orderPrice = $($('.cart__part-item__price-span')[index]).text()*Math.floor($(item).val())
        $($('.cart__part-item__total-price-span')[index]).text(orderPrice)
      }
      $('.cart-result__subtotal-price').text()
      checkSubtotalPrice()
      checkTotalPrice()
    })
  })
}

checkOrder()
checkCart()
checkSubtotalPrice()
checkVarShipping()
checkTotalPrice()

$('.cart__part-item--delete').on('click', (e)=>{
  $('.cart__part-item--delete').each((index, item)=>{
    if (e.target == item) {
      deleteOrder(index)
    }
  })
  checkSubtotalPrice()
  checkTotalPrice()
  checkOrder()
  checkCart()
})
$('.shipping__input').on('click', ()=>{
  checkTotalPrice()
  return checkVarShipping()
})



let sendRequest = (method, url, body = null)=>{
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
$('.cart-result__btn').on('click', (e)=>{
  sendRequest('post', requestUrl, {
  productCode: +$('.cart-result__total-price').text()
  })
    .then(data => console.log(data))
    .catch(err => console.log(err))
})