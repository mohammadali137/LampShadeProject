using AccountManagement.Application.Contracts.Account;

namespace ShopManagement.Domain.Services
{
    public interface IShopAccountAcl
    {
        (string name, string mobile) GetAccountBy(long id);
        AccountViewModel GetAccountByEmail(long id);
    }
}
