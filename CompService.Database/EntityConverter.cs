using CompService.Core.Enums;
using CompService.Core.Models;
using CompService.Database.Models;

namespace CompService.Database;

public static class EntityConverter
{
    public static SparePartDb ConvertSparePart(SparePart sparePart)
    {
        return new SparePartDb
            {
                Name = sparePart.Name,
                Article = sparePart.Article,
                Category = new SparePartCategoryDb
                {
                    CategoryId = sparePart.Category.CategoryId,
                    Name = sparePart.Category.Name
                },
                Count = sparePart.Count,
                PurchasePrice = sparePart.PurchasePrice,
                RetailPrice = sparePart.RetailPrice
            };
    }
    
    public static SparePart ConvertSparePart(SparePartDb sparePart)
    {
        return new SparePart
        {
            SparePartId = sparePart.SparePartId,
            Name = sparePart.Name,
            Article = sparePart.Article,
            Category = new SparePartCategory
            {
                CategoryId = sparePart.Category.CategoryId,
                Name = sparePart.Category.Name
            },
            Count = sparePart.Count,
            PurchasePrice = sparePart.PurchasePrice,
            RetailPrice = sparePart.PurchasePrice
        }; 
    }

    public static UserDb ConvertUser(User user)
    {
        return new UserDb
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Patronymic = user.Patronymic,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber,
            Role = (int) user.Roles
        };
    }
    
    public static User ConvertUser(UserDb user)
    {
        return new User
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Patronymic = user.Patronymic,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber,
            Roles = (Roles)user.Role
        };
    }

    public static ClientDb ConvertClient(Client client)
    {
        return new ClientDb
        {
            Name = client.Name,
            Email = client.Email,
            Surname = client.Surname,
            PhoneNumber = client.PhoneNumber,
            Source = client.Source is null ? null : new SourceDb
            {
                SourceId = client.Source.SourceId,
                Name = client.Source.Name
            }
        };
    }

    public static Client ConvertClient(ClientDb client)
    {
        return new Client
        {
            Name = client.Name,
            Email = client.Email,
            Surname = client.Surname,
            PhoneNumber = client.PhoneNumber,
            Source = client.Source is null ? null : new Source
            {
                SourceId = client.Source.SourceId,
                Name = client.Source.Name
            }
        };
    }
    public static TransactionDb ConvertTransaction(Transaction transaction)
    {
        return new TransactionDb
        {
            TransactionTime = transaction.TransactionTime,
            ArrivalMoney = transaction.ArrivalMoney,
            ExpenseMoney = transaction.ExpenseMoney,
            PaymentMehod = (int)transaction.PaymentMehod,
            TransactionBasis = (int)transaction.TransactionBasis,
            OrderDb = transaction.Order is null
                ? null
                : new OrderDb
                {
                    OrderId = transaction.Order.OrderId,
                    OrderDate = transaction.Order.OrderDate
                },
            User = new UserDb
            {
                UserId = transaction.User.UserId,
                Name = transaction.User.Name,
                Surname = transaction.User.Surname,
                Patronymic = transaction.User.Patronymic,
                Email = transaction.User.Email,
                Password = transaction.User.Password,
                PhoneNumber = transaction.User.PhoneNumber,
                Role = (int) transaction.User.Roles
            }
        };
    }
    
    public static Transaction ConvertTransaction(TransactionDb transaction)
    {
        return new Transaction
        {
            TransactionTime = transaction.TransactionTime,
            ArrivalMoney = transaction.ArrivalMoney,
            ExpenseMoney = transaction.ExpenseMoney,
            PaymentMehod = (PaymentMethods)transaction.PaymentMehod,
            TransactionBasis = (TransactionBasis)transaction.TransactionBasis,
            Order = transaction.OrderDb is null
                ? null
                : new Order
                {
                    OrderId = transaction.OrderDb.OrderId,
                    OrderDate = transaction.OrderDb.OrderDate
                },
            User = new User
            {
                UserId = transaction.User.UserId,
                Name = transaction.User.Name,
                Surname = transaction.User.Surname,
                Patronymic = transaction.User.Patronymic,
                Email = transaction.User.Email,
                Password = transaction.User.Password,
                PhoneNumber = transaction.User.PhoneNumber,
                Roles = (Roles)transaction.User.Role
            }
        };
    }
}