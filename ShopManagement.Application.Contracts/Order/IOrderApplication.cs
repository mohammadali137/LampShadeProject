using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        long PlaceOrder(Cart cart);
        
        double GetAmountBy(long id);
        void Cancel(long orderId);
        void SendEmailToUser(long id);
        string PaymentSucceeded(long orderId, long refId);
        string PaymentSucceeded2(long orderId);
        List<OrderItemViewModel> GetItems(long orderId);
        List<OrderViewModel> Search(OrderSearchModel searchModel);
        double GetSaleCount();
    }
}