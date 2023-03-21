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
            Category = ConvertSparePartCategory(sparePart.Category),
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
            Category = ConvertSparePartCategory(sparePart.Category),
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
            Roles = (Roles) user.Role
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
            Source = client.Source is null ? null : ConvertSource(client.Source)
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
            Source = client.Source is null ? null : ConvertSource(client.Source)
        };
    }

    public static TransactionDb ConvertTransaction(Transaction transaction)
    {
        return new TransactionDb
        {
            TransactionTime = transaction.TransactionTime,
            ArrivalMoney = transaction.ArrivalMoney,
            ExpenseMoney = transaction.ExpenseMoney,
            PaymentMethod = (int) transaction.PaymentMethod,
            TransactionBasis = (int) transaction.TransactionBasis,
            Order = transaction.Order is null
                ? null
                : ConvertOrder(transaction.Order),
            User = ConvertUser(transaction.User),
            Comment = transaction.Comment
        };
    }

    public static Transaction ConvertTransaction(TransactionDb transaction)
    {
        return new Transaction
        {
            TransactionTime = transaction.TransactionTime,
            ArrivalMoney = transaction.ArrivalMoney,
            ExpenseMoney = transaction.ExpenseMoney,
            PaymentMethod = (PaymentMethods) transaction.PaymentMethod,
            TransactionBasis = (TransactionBasis) transaction.TransactionBasis,
            Order = transaction.Order is null
                ? null
                : ConvertOrder(transaction.Order),
            User = ConvertUser(transaction.User),
            Comment = transaction.Comment
        };
    }

    public static Facility ConvertFacility(FacilityDb facility)
    {
        return new Facility
        {
            FacilityId = facility.FacilityId,
            Name = facility.Name,
            Cost = facility.Cost
        };
    }

    public static FacilityDb ConvertFacility(Facility facility)
    {
        return new FacilityDb
        {
            FacilityId = facility.FacilityId,
            Name = facility.Name,
            Cost = facility.Cost
        };
    }

    public static DevicePlaceDb ConvertDevicePlace(DevicePlace devicePlace)
    {
        return new DevicePlaceDb
        {
            PlaceId = devicePlace.PlaceId,
            Info = devicePlace.Info,
            Order = ConvertOrder(devicePlace.Order),
            IsOccupied = devicePlace.IsOccupied
        };
    }
    public static DevicePlace ConvertDevicePlace(DevicePlaceDb devicePlace)
    {
        return new DevicePlace
        {
            PlaceId = devicePlace.PlaceId,
            Info = devicePlace.Info,
            Order = ConvertOrder(devicePlace.Order),
            IsOccupied = devicePlace.IsOccupied
        };
    }

    public static Order ConvertOrder(OrderDb order)
    {
        return new Order
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            Client = ConvertClient(order.Client),
            Defect = ConvertDefect(order.Defect),
            Appearance = ConvertAppearance(order.Appearance),
            DeviceType = ConvertDeviceType(order.DeviceType),
            Brand = ConvertBrand(order.Brand),
            Model = order.Model,
            DevicePassword = order.DevicePassword,
            Operator = ConvertUser(order.Operator),
            Master = ConvertUser(order.Master),
            Status = (OrdersStatuses) order.Status,
            SpareParts = order.SpareParts?.Select(ConvertSparePart).ToList(),
            Facilities = order.Facilities?.Select(ConvertFacility).ToList(),
            Money = order.Money,
        };
    }

    public static OrderDb ConvertOrder(Order order)
    {
        return new OrderDb
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            Client = ConvertClient(order.Client),
            Defect = ConvertDefect(order.Defect),
            Appearance = ConvertAppearance(order.Appearance),
            DeviceType = ConvertDeviceType(order.DeviceType),
            Brand = ConvertBrand(order.Brand),
            Model = order.Model,
            DevicePassword = order.DevicePassword,
            Operator = ConvertUser(order.Operator),
            Master = ConvertUser(order.Master),
            Status = (int) order.Status,
            SpareParts = order.SpareParts?.Select(ConvertSparePart).ToList(),
            Facilities = order.Facilities?.Select(ConvertFacility).ToList(),
            Money = order.Money
        };
    }

    #region ReferencesConvert

    public static Defect ConvertDefect(DefectDb defect)
    {
        return new Defect
        {
            DefectId = defect.DefectId,
            Name = defect.Name
        };
    }

    public static DefectDb ConvertDefect(Defect defect)
    {
        return new DefectDb
        {
            DefectId = defect.DefectId,
            Name = defect.Name
        };
    }

    public static Appearance ConvertAppearance(AppearanceDb appearance)
    {
        return new Appearance
        {
            AppearanceId = appearance.AppearanceId,
            Name = appearance.Name
        };
    }

    public static AppearanceDb ConvertAppearance(Appearance appearance)
    {
        return new AppearanceDb
        {
            AppearanceId = appearance.AppearanceId,
            Name = appearance.Name
        };
    }

    public static Brand ConvertBrand(BrandDb brand)
    {
        return new Brand
        {
            BrandId = brand.BrandId,
            Name = brand.Name
        };
    }

    public static BrandDb ConvertBrand(Brand brand)
    {
        return new BrandDb
        {
            BrandId = brand.BrandId,
            Name = brand.Name
        };
    }

    public static DeviceType ConvertDeviceType(DeviceTypeDb deviceType)
    {
        return new DeviceType
        {
            DeviceTypeId = deviceType.DeviceTypeId,
            Name = deviceType.Name
        };
    }

    public static DeviceTypeDb ConvertDeviceType(DeviceType deviceType)
    {
        return new DeviceTypeDb
        {
            DeviceTypeId = deviceType.DeviceTypeId,
            Name = deviceType.Name
        };
    }

    public static SourceDb ConvertSource(Source source)
    {
        return new SourceDb
        {
            SourceId = source.SourceId,
            Name = source.Name
        };
    }

    public static Source ConvertSource(SourceDb source)
    {
        return new Source
        {
            SourceId = source.SourceId,
            Name = source.Name
        };
    }

    public static SparePartCategory ConvertSparePartCategory(SparePartCategoryDb sparePartCategory)
    {
        return new SparePartCategory
        {
            CategoryId = sparePartCategory.CategoryId,
            Name = sparePartCategory.Name
        };
    }

    public static SparePartCategoryDb ConvertSparePartCategory(SparePartCategory sparePartCategory)
    {
        return new SparePartCategoryDb
        {
            CategoryId = sparePartCategory.CategoryId,
            Name = sparePartCategory.Name
        };
    }

    #endregion
}