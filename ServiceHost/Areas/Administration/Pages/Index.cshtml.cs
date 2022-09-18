using System.Collections.Generic;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Dashboard;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages
{
    public class IndexModel : PageModel
    {
        public OrderSearchModel SearchModel;
        public SelectList Accounts;
        public DashboardViewModel Dashboard;

        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;
        private readonly IProductApplication _productApplication;

        public IndexModel(IOrderApplication orderApplication, IAccountApplication accountApplication, IProductApplication productApplication)
        {
            _orderApplication = orderApplication;
            _accountApplication = accountApplication;
            _productApplication = productApplication;
        }
        public Chart DoughnutDataSet { get; set; }
        public List<Chart> BarLineDataSet { get; set; }

        public void OnGet()
        {
            Dashboard = new DashboardViewModel();
            Dashboard.OrderCount = _orderApplication.GetSaleCount();
            Dashboard.UsersCount = _accountApplication.GetAccountsCount();
            Dashboard.ProductCount = _productApplication.GetProductCount();
            Dashboard.VisitSiteCount = 230;
            BarLineDataSet = new List<Chart>
            {
                new Chart
                {
                    Label = "Apple",
                    Data = new List<int> {100, 200, 250, 170, 50},
                    BackgroundColor = new[] {"#ffcdb2"},
                    BorderColor = "#b5838d"
                },
                new Chart
                {
                    Label = "Samsung",
                    Data = new List<int> {200, 300, 350, 270, 100},
                    BackgroundColor = new[] {"#ffc8dd"},
                    BorderColor = "#ffafcc"
                },
                new Chart
                {
                    Label = "Total",
                    Data = new List<int> {300, 500, 600, 440, 150},
                    BackgroundColor = new[] {"#0077b6"},
                    BorderColor = "#023e8a"
                },
            };
            DoughnutDataSet = new Chart
            {
                Label = "Apple",
                Data = new List<int> { 100, 200, 250, 170, 50 },
                BorderColor = "#ffcdb2",
                BackgroundColor = new[] { "#b5838d", "#ffd166", "#7f4f24", "#ef233c", "#003049" }
            };
        }
    }

    public class Chart
    {
        [JsonProperty(PropertyName = "label")] public string Label { get; set; }

        [JsonProperty(PropertyName = "data")] public List<int> Data { get; set; }

        [JsonProperty(PropertyName = "backgroundColor")]
        public string[] BackgroundColor { get; set; }

        [JsonProperty(PropertyName = "borderColor")]
        public string BorderColor { get; set; }
    }
}