//===========Gốc============//
//var cart = {
//    init: function () {
//        cart.regEvents();
//    },
//    regEvents: function () {
//        $('#btnContinue').off('click').on('click', function () {
//            window.location.href = "/san-pham";
//        });

//        $('#btnPayment').off('click').on('click', function () {
//            window.location.href = "/thanh-toan";
//        });

//        $('#btnUpdate').off('click').on('click', function () {
//            var listProduct = $('.txtQuantity');
//            var cartList = [];
//            $.each(listProduct, function (i, item) {
//                cartList.push({
//                    Quantity: $(this).val(),
//                    Product: {
//                        Id: $(item).data('product-id')
//                    },
//                    Size: {
//                        Id: $(item).data('size-id')
//                    }
//                })
//            });

//            $.ajax({
//                url: 'Cart/Update',
//                data: { cartModel: JSON.stringify(cartList) },
//                dataType: 'json',
//                type: 'POST',
//                success: function (res) {
//                    if (res.status == true) {
//                        window.location.href = "/gio-hang"
//                    }
//                }
//            });
//        });

//        $('#btnDeleteAll').off('click').on('click', function () {
//            if (confirm("Bạn có chắc chắn muốn xóa toàn bộ sản phẩm?")) {
//                $.ajax({
//                    url: 'Cart/DeleteAll',
//                    dataType: 'json',
//                    type: 'POST',
//                    success: function (res) {
//                        if (res.status == true) {
//                            window.location.href = "/gio-hang"
//                        }
//                    }
//                });
//            }
//        });

//        $('.btn-delete').off('click').on('click', function (e) {
//            e.preventDefault();

//            if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này?")) {
//                $.ajax({
//                    url: 'Cart/Delete',
//                    data: { productId: $(this).data('product-id'), sizeId: $(this).data('size-id') },
//                    dataType: 'json',
//                    type: 'POST',
//                    success: function (res) {
//                        if (res.status == true) {
//                            window.location.href = "/gio-hang"
//                        }
//                    }
//                });
//            }
//        });



//    }
//}
//cart.init();



//=================Gốc===============================//



//var cart = {
//    init: function () {
//        cart.regEvents();
//        cart.refreshTotals(); // tính lần đầu khi load trang
//    },
//    regEvents: function () {
//        $('#btnContinue').off('click').on('click', function () {
//            window.location.href = "/san-pham";
//        });

//        $('#btnPayment').off('click').on('click', function () {
//            window.location.href = "/thanh-toan";
//        });

//        $('#btnDeleteAll').off('click').on('click', function () {
//            if (confirm("Bạn có chắc chắn muốn xóa toàn bộ sản phẩm?")) {
//                $.ajax({
//                    url: 'Cart/DeleteAll',
//                    dataType: 'json',
//                    type: 'POST',
//                    success: function (res) {
//                        if (res.status) {
//                            $('.table-shopping-cart').empty();
//                            cart.refreshTotals();
                            
//                            // Reload lại trang để cập nhật ViewBag và các thông tin tổng hợp
//                            setTimeout(function () {
//                                location.reload();
//                            }, 100);
//                        }
//                    }
//                });
//            }
//        });

//        $('.btn-delete').off('click').on('click', function (e) {
//            e.preventDefault();
//            var $row = $(this).closest('tr');
//            if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này?")) {
//                $.ajax({
//                    url: 'Cart/Delete',
//                    data: {
//                        productId: $(this).data('product-id'),
//                        sizeId: $(this).data('size-id')
//                    },
//                    dataType: 'json',
//                    type: 'POST',
//                    success: function (res) {
//                        if (res.status) {
//                            $row.remove();
//                            cart.refreshTotals();
//                            // Reload lại trang để cập nhật ViewBag và các thông tin tổng hợp
//                            if ($('.table-shopping-cart tr.table_row').length === 0) {
//                                setTimeout(function () {
//                                    location.reload();
//                                }, 100);
//                            }
//                        }
//                    }
//                });
//            }
//        });

        
//        $(document)
//            .off('click.cartQty', '.btn-num-product-up, .btn-num-product-down')
//            .on('click.cartQty', '.btn-num-product-up, .btn-num-product-down', function (e) {
//                e.preventDefault();
//                var $inp = $(this).closest('.wrap-num-product').find('.txtQuantity'),
//                    stock = +$inp.data('stock'),
//                    cur = parseInt($inp.val(), 10) || 1,
//                    next = $(this).hasClass('btn-num-product-up') ? cur + 1 : cur - 1;

//                next = Math.max(1, Math.min(next, stock));
//                if (next !== cur) {
//                    $inp.val(next).trigger('change.cartQty');
//                }
//            });

//        // 2) Bind change input chỉ 1 lần
//        $(document)
//            .off('change.cartQty', '.txtQuantity')
//            .on('change.cartQty', '.txtQuantity', function () {
//                var $inp = $(this),
//                    stock = +$inp.data('stock'),
//                    val = parseInt($inp.val(), 10) || 1;

//                val = Math.max(1, Math.min(val, stock));
//                $inp.val(val);

//                cart.updateItem($inp);
//            });

        
//    },

//    // gửi Ajax cập nhật session trên server, sau đó recalc client
//    updateItem: function ($inp) {
//        var productId = $inp.data('product-id'),
//            sizeId = $inp.data('size-id'),
//            quantity = parseInt($inp.val(), 10);
//        $.ajax({
//            url: 'Cart/Update',
//            type: 'POST',
//            data: {
//                cartModel: JSON.stringify([
//                    { Product: { Id: productId }, Size: { Id: sizeId }, Quantity: quantity }
//                ])
//            },
//            dataType: 'json',
//            success: function (res) {
//                if (res.status) {
//                    cart.refreshTotals();
//                } else {
//                    alert('Cập nhật thất bại');
//                }
//            }
//        });
//    },

//    // 2–6: Tính lại từng row và summary
//    refreshTotals: function () {
//        var orderValue = 0,    // Tổng giá gốc * qty
//            promotion = 0,     // Tổng tiền giảm giá
//            subTotal = 0;      // Tổng tiền thực chi = orderValue - promotion

//        $('.table_row').each(function () {
//            var $tr = $(this),
//                qty = parseInt($tr.find('.txtQuantity').val(), 10) || 1,

//                // Giá khuyến mãi (nếu có .text-danger), ngược lại lấy giá gốc
//                promoText = $tr.find('.column-3 .text-danger').first().text(),
//                promoPrice = parseInt(promoText.replace(/[^\d]/g, ''), 10) ||
//                    parseInt($tr.find('.column-3 .stext-105').first().text().replace(/[^\d]/g, ''), 10),

//                // Giá gốc
//                origText = $tr.find('.column-3 .stext-105').first().text(),
//                origPrice = parseInt(origText.replace(/[^\d]/g, ''), 10) || promoPrice,

//                rowOrigTotal = origPrice * qty,
//                rowTotal = promoPrice * qty;

//            orderValue += rowOrigTotal;
//            promotion += (origPrice - promoPrice) * qty;
//            subTotal += rowTotal;

//            // Cập nhật cột Tổng tiền (promoPrice * qty)
//            $tr.find('.row-total').text(rowTotal.toLocaleString());
//        });

//        // 3. Giá trị đơn hàng = orderValue
//        $('.subtotal').text(orderValue.toLocaleString());

//        // 4. Giảm giá
//        $('.promotion').text('-' + promotion.toLocaleString());

//        // 5. Phí giao hàng
//        var shipFee = subTotal > 1000000 ? 0 : 21000;
//        $('.shipping').text(shipFee ? shipFee.toLocaleString() : 'Miễn phí');

//        // 6. Tổng cuối = orderValue - promotion + shipFee
//        var total = orderValue - promotion + shipFee;
//        $('.total').text(total.toLocaleString());
//    }
//};

//$(function () {
//    cart.init();
//});

; (function ($) {
    "use strict";

    var cart = {
        init: function () {
            cart.regEvents();
            cart.refreshTotals(); // tính lần đầu khi load trang
        },

        regEvents: function () {
            // Điều hướng mua hàng tiếp tục
            $('#btnContinue')
                .off('click')
                .on('click', function () {
                    window.location.href = "/san-pham";
                });

            // Điều hướng thanh toán
            $('#btnPayment')
                .off('click')
                .on('click', function () {
                    window.location.href = "/thanh-toan";
                });

            // Xóa toàn bộ
            $('#btnDeleteAll')
                .off('click')
                .on('click', function () {
                    if (confirm("Bạn có chắc chắn muốn xóa toàn bộ sản phẩm?")) {
                        $.post('Cart/DeleteAll', function (res) {
                            if (res.status) {
                                $('.table-shopping-cart').empty();
                                cart.refreshTotals();
                                setTimeout(function () { location.reload(); }, 100);
                            }
                        }, 'json');
                    }
                });

            // Xóa từng sản phẩm
            $('.btn-delete')
                .off('click')
                .on('click', function (e) {
                    e.preventDefault();
                    var $row = $(this).closest('tr');
                    if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này?")) {
                        $.post('Cart/Delete', {
                            productId: $(this).data('product-id'),
                            sizeId: $(this).data('size-id')
                        }, function (res) {
                            if (res.status) {
                                $row.remove();
                                cart.refreshTotals();
                                if ($('.table-shopping-cart tr.table_row').length === 0) {
                                    setTimeout(function () { location.reload(); }, 100);
                                }
                            }
                        }, 'json');
                    }
                });

            // 1) Delegation: bind tăng/giảm số lượng chỉ một lần
            $(document)
                .off('click.cartQty', '.btn-num-product-up, .btn-num-product-down')
                .on('click.cartQty', '.btn-num-product-up, .btn-num-product-down', function (e) {
                    e.preventDefault();
                    var $inp = $(this).closest('.wrap-num-product').find('.txtQuantity'),
                        stock = +$inp.data('stock'),
                        current = parseInt($inp.val(), 10) || 1,
                        next = $(this).hasClass('btn-num-product-up') ? current + 1 : current - 1;

                    if (next < 1) next = 1;
                    if (next > stock) {
                        alert('Sản phẩm không được vượt quá số lượng tồn.');
                        next = stock;
                    }

                    if (next !== current) {
                        $inp.val(next).trigger('change.cartQty');
                    }
                });

            // 2) Delegation: bind change input số lượng chỉ một lần
            $(document)
                .off('change.cartQty', '.txtQuantity')
                .on('change.cartQty', '.txtQuantity', function () {
                    var $inp = $(this),
                        stock = +$inp.data('stock'),
                        val = parseInt($inp.val(), 10) || 1;

                    if (val < 1) val = 1;
                    if (val > stock) {
                        alert('Sản phẩm không được vượt quá số lượng tồn.');
                        val = stock;
                    }
                    $inp.val(val);

                    cart.updateItem($inp);
                });
        },

        // Gửi AJAX cập nhật server, sau đó recalc client
        //updateItem: function ($inp) {
        //    var productId = $inp.data('product-id'),
        //        sizeId = $inp.data('size-id'),
        //        quantity = parseInt($inp.val(), 10);

        //    $.ajax({
        //        url: 'Cart/Update',
        //        type: 'POST',
        //        data: {
        //            cartModel: JSON.stringify([
        //                { Product: { Id: productId }, Size: { Id: sizeId }, Quantity: quantity }
        //            ])
        //        },
        //        dataType: 'json',
        //        success: function (res) {
        //            if (res.status) {
        //                cart.refreshTotals();
        //            } else {
        //                alert('Cập nhật thất bại');
        //            }
        //        }
        //    });
        //},

        updateItem: function ($inp) {
            var cartData = [];

            $('.table_row').each(function () {
                var $tr = $(this),
                    productId = $tr.find('.txtQuantity').data('product-id'),
                    sizeId = $tr.find('.txtQuantity').data('size-id'),
                    quantity = parseInt($tr.find('.txtQuantity').val(), 10) || 1;

                cartData.push({
                    Product: { Id: productId },
                    Size: { Id: sizeId },
                    Quantity: quantity
                });
            });

            $.ajax({
                url: 'Cart/Update',
                type: 'POST',
                data: {
                    cartModel: JSON.stringify(cartData)
                },
                dataType: 'json',
                success: function (res) {
                    if (res.status) {
                        cart.refreshTotals();
                    } else {
                        alert('Cập nhật thất bại');
                    }
                }
            });
        },

        // Tính lại tổng của giỏ hàng
        refreshTotals: function () {
            var orderValue = 0,
                promotion = 0,
                subTotal = 0;

            $('.table_row').each(function () {
                var $tr = $(this),
                    qty = parseInt($tr.find('.txtQuantity').val(), 10) || 1,
                    promoText = $tr.find('.column-3 .text-danger').first().text(),
                    promoPrice = parseInt(promoText.replace(/[^\d]/g, ''), 10) ||
                        parseInt($tr.find('.column-3 .stext-105').first().text().replace(/[^\d]/g, ''), 10),
                    origText = $tr.find('.column-3 .stext-105').first().text(),
                    origPrice = parseInt(origText.replace(/[^\d]/g, ''), 10) || promoPrice,
                    rowOrigTot = origPrice * qty,
                    rowTot = promoPrice * qty;

                orderValue += rowOrigTot;
                promotion += (origPrice - promoPrice) * qty;
                subTotal += rowTot;

                $tr.find('.row-total').text(rowTot.toLocaleString());
            });

            $('.subtotal').text(orderValue.toLocaleString());
            $('.promotion').text('-' + promotion.toLocaleString());

            //var shipFee = subTotal > 1000000 ? 0 : 21000;
            //$('.shipping').text(shipFee ? shipFee.toLocaleString() : 'Miễn phí');

            var total = orderValue - promotion /*+ shipFee*/;
            $('.total').text(total.toLocaleString());
        }
    };

    // Khi DOM sẵn sàng, khởi tạo cart
    $(function () {
        cart.init();
    });

})(jQuery);



