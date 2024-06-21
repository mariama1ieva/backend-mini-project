(function ($) {
    "use strict"

    $(document).on('click', '.category', function (e) {
        $(this).addClass('active').siblings().removeClass('active');
        e.preventDefault();
        const category = $(this).attr('category-id');
        const products = $('.fruite-item');

        products.each(function () {
            if (category === $(this).attr('category-id')) {
                $(this).parent().fadeIn();
            }
            else {
                $(this).parent().hide();
            }
        })

        if (category == 'All') {
            products.parent().fadeIn();
        }
    });


    // Add
    $(document).on("click", ".add-basket", function () {
        const btn = $(this);
        const productId = $(this).attr('product-id');
        let quantity = null;

        if (btn.prev().find("input").length !== 0) {
            quantity = btn.prev().find("input").val();
        }

        $.ajax({
            type: "Post",
            url: `/Basket/Add?productId=${productId}&quantity=${quantity}`,
            success: (res) => {
                console.log(res.redirectUrl)
                if (!res.redirectUrl) {
                    Swal.fire({
                        icon: "success",
                        title: "Added to cart!",
                        showConfirmButton: false,
                        timer: 1500
                    });

                    const count = $(".basket-count");

                    if (quantity !== null) {
                        count.text(parseInt(count.text()) + parseInt(quantity));
                    } else {
                        count.text(parseInt(count.text()) + 1);
                    }
                } else {
                    window.location.href = res.redirectUrl;
                }
            }
        });
    });

    // Increase
    $(document).on("click", ".btn-plus", function () {
        const id = $(this).attr('id');
        const btn = $(this);

        $.ajax({
            type: "Post",
            url: `/Basket/Increase?id=${id}`,
            success: function (res) {
                btn.closest(".basket-item").find(".total").text(res.totalPrice + ' $');

                const count = $(".basket-count");
                count.text(parseInt(count.text()) + 1);

                $(".total-price").text('$' + res.total);
            }
        });
    });

    // Decrease
    $(document).on("click", ".btn-minus", function () {
        const btn = $(this);
        const id = $(this).attr('id');
        const basketItem = btn.closest(".basket-item");
        const quantityInput = basketItem.find(".quantity input");
        const quantity = basketItem.find(".quantity input").val();

        if (quantity == 0) {
            Swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your product has been deleted.",
                        icon: "success"
                    });

                    $.ajax({
                        type: "Post",
                        url: `/Basket/Decrease?id=${id}`,
                        success: function (res) {
                            basketItem.remove();
                            const count = $(".basket-count");
                            count.text(parseInt(count.text()) - 1);

                            $(".total-price").text('$' + res.total);
                        }
                    });
                } else {
                    quantityInput.val(1);
                }
            });
        } else {
            $.ajax({
                type: "Post",
                url: `Basket/Decrease?id=${id}`,
                success: function (res) {

                    basketItem.find(".total").text(res.totalPrice + ' $');

                    const count = $(".basket-count");
                    count.text(parseInt(count.text()) - 1);

                    $(".total-price").text('$' + res.total);
                }
            });
        }
    });

    // Change quantity
    $(document).on("keyup", ".quantity input", function () {
        const input = $(this);
        const quantity = input.val();
        const basketItem = input.closest(".basket-item");
        const id = basketItem.attr('id');
        const count = $(".basket-count");


        if (quantity == 0) {
            Swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your product has been deleted.",
                        icon: "success"
                    });

                    $.ajax({
                        type: "Post",
                        url: `/Basket/ChangeQuantity?id=${id}&quantity=${quantity}`,
                        success: function (res) {
                            basketItem.remove();
                            count.text(res.basketCount);

                            $(".total-price").text('$' + res.total);
                        }
                    });
                } else {
                    input.val(1);
                }
            });
        } else {
            $.ajax({
                type: "Post",
                url: `Basket/ChangeQuantity?id=${id}&quantity=${quantity}`,
                success: function (res) {
                    basketItem.find(".total").text(res.totalPrice + ' $');
                    count.text(res.basketCount);

                    $(".total-price").text('$' + res.total);
                }
            });
        }
    });






    // Delete
    $(document).on("click", ".basket-delete", function () {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: "Deleted!",
                    text: "Your product has been deleted.",
                    icon: "success"
                });

                const id = $(this).attr('id');
                const btn = $(this);

                $.ajax({
                    type: "Post",
                    url: `Basket/Delete?id=${id}`,
                    success: function (res) {
                        btn.closest(".basket-item").remove();

                        const count = $(".basket-count");
                        count.text(parseInt(count.text()) - 1);

                        $(".total-price").text("$" + res);
                    }
                });
            }
        });
    });

    // Category filter
    $('.category-filter').on('click', function () {
        const categoryId = $(this).attr('category-id');

        $(".paginate").css("display", "none");

        $(".products .product-item").slice(0).remove();

        $.ajax({
            type: "Get",
            url: `Shop/CategoryFilter?id=${categoryId}`,
            success: function (res) {
                $('.products').append(res);
            },
        });
    });

    //Price filter
    $('.form-range').on('change', function () {
        const value = $(this).val().trim();

        $(".paginate").css("display", "none");

        $(".products .product-item").slice(0).remove();

        $.ajax({
            type: "Get",
            url: `Shop/PriceFilter?price=${value}`,
            success: function (res) {
                $('.products').append(res);
            },
        });
    });

    // Search
    $(document).on("keyup", ".search", function () {
        const value = $(this).val().trim();

        if (value !== "") {
            $(".paginate").css("display", "none");
        } else {
            $(".paginate").css("display", "block");
        }

        $(".products .product-item").slice(0).remove();

        $.ajax({
            type: "Get",
            url: `Shop/Search?searchText=${value}`,
            success: function (res) {
                $(".products").append(res);
            }
        });
    });

    //Sorting
    $('#fruits').on('change', function () {
        const value = $(this).val().trim();

        $(".paginate").css("display", "none");

        $(".products .product-item").slice(0).remove();

        $.ajax({
            type: "Get",
            url: "Shop/Sorting",
            data: { sort: value },
            success: function (res) {
                $('.products').append(res);
            },
        });
    });

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner(0);


    // Fixed Navbar
    $(window).scroll(function () {
        if ($(window).width() < 992) {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow');
            } else {
                $('.fixed-top').removeClass('shadow');
            }
        } else {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow').css('top', -55);
            } else {
                $('.fixed-top').removeClass('shadow').css('top', 0);
            }
        }
    });


    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });


    // Testimonial carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 2000,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav: true,
        navText: [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 1
            },
            768: {
                items: 1
            },
            992: {
                items: 2
            },
            1200: {
                items: 2
            }
        }
    });


    // vegetable carousel
    $(".vegetable-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav: true,
        navText: [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 1
            },
            768: {
                items: 2
            },
            992: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    });


    // Modal Video
    $(document).ready(function () {
        var $videoSrc;
        $('.btn-play').click(function () {
            $videoSrc = $(this).data("src");
        });
        console.log($videoSrc);

        $('#videoModal').on('shown.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc + "?autoplay=1&amp;modestbranding=1&amp;showinfo=0");
        })

        $('#videoModal').on('hide.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc);
        })
    });



    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });

})(jQuery);