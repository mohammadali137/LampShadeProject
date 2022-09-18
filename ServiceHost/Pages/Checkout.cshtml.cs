using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using _0_Framework.Application;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery.Contracts;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;
using ZarinpalSandbox;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        public Cart Cart;
        public const string CookieName = "cart-items";
        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IOrderApplication _orderApplication;
        private readonly ICartCalculatorService _cartCalculatorService;

        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService,
            IProductQuery productQuery, IOrderApplication orderApplication, IZarinPalFactory zarinPalFactory,
            IAuthHelper authHelper)
        {
            Cart = new Cart();
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _productQuery = productQuery;
            _orderApplication = orderApplication;
            _zarinPalFactory = zarinPalFactory;
            _authHelper = authHelper;
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);
        }

        public IActionResult OnPostPay(int paymentMethod)
        {
            var cart = _cartService.Get();
            cart.SetPaymentMethod(paymentMethod);

            var result = _productQuery.CheckInventoryStatus(cart.Items);
            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("/Cart");

            var orderId = _orderApplication.PlaceOrder(cart);
            if (paymentMethod == 1)
            {
                //var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
                //    cart.PayAmount.ToString(CultureInfo.InvariantCulture), "", "",
                //    "خرید از درگاه لوازم خانگی و دکوری", orderId);

                //return Redirect(
                //    $"https://https://zarinpal.com/pg/StartPay/{paymentResponse.Authority}");

                var payment = new Payment(Convert.ToInt32(cart.PayAmount));

                var res = payment.PaymentRequest(" خرید ", $"https://localhost:44344/Checkout?handler=CallBack&id=" + orderId, "sheikhimohammadali571@gmail.com", "+989172663608");

                if (res.Result.Status == 100)
                {
                    // return Redirect("https://zarinpal.com/pg/StartPay/" + res.Result.Authority);
                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
                }
            }

            var paymentResult = new PaymentResult();
            Response.Cookies.Delete("cart-items");
            _orderApplication.SendEmailToUser(orderId);
            return RedirectToPage("/PaymentResult",
                paymentResult.Succeeded(
                    "سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما و پرداخت وجه، سفارش ارسال خواهد شد.", null));
        }

        // public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status,[FromQuery] long oId)
        public IActionResult OnGetCallBack(int id)
        {
            var result = new PaymentResult();
            if (HttpContext.Request.Query["Status"] != "" &&
             HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
             && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var wallet = _orderApplication.GetAmountBy(id);
                var payment = new Payment((int)wallet);
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    var issueTrackingNo = _orderApplication.PaymentSucceeded2(id);
                    Response.Cookies.Delete("cart-items");
                    result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
                    return RedirectToPage("/PaymentResult", result);
                }

            }
            //var orderAmount = _orderApplication.GetAmountBy(oId);
            //var verificationResponse =
            //    _zarinPalFactory.CreateVerificationRequest(authority,
            //        orderAmount.ToString(CultureInfo.InvariantCulture));

            //var result = new PaymentResult();
            //if (status == "OK" && verificationResponse.Status >= 100)
            //{
            //    var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.RefID);
            //    Response.Cookies.Delete("cart-items");
            //    result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
            //    return RedirectToPage("/PaymentResult", result);
            //}

            result = result.Failed(
                "پرداخت با موفقیت انجام نشد. درصورت کسر وجه از حساب، مبلغ تا 24 ساعت دیگر به حساب شما بازگردانده خواهد شد.");
            return RedirectToPage("/PaymentResult");
        }
    }
}