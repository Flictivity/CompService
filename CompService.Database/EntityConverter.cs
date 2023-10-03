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
            SparePartId = sparePart.SparePartId,
            Name = sparePart.Name,
            Article = sparePart.Article,
            CategoryId = sparePart.Category.CategoryId,
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
            Count = sparePart.Count,
            PurchasePrice = sparePart.PurchasePrice,
            RetailPrice = sparePart.RetailPrice
        };
    }

    public static UserDb ConvertUser(User user)
    {
        return new UserDb
        {
            UserId = user.UserId,
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
            UserId = user.UserId,
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
            ClientId = client.ClientId,
            Name = client.Name,
            Email = client.Email,
            Surname = client.Surname,
            PhoneNumber = client.PhoneNumber,
            SourceId = client.Source?.SourceId
        };
    }

    public static Client ConvertClient(ClientDb client)
    {
        return new Client
        {
            ClientId = client.ClientId,
            Name = client.Name,
            Email = client.Email,
            Surname = client.Surname,
            PhoneNumber = client.PhoneNumber
        };
    }

    public static TransactionDb ConvertTransaction(Transaction transaction)
    {
        return new TransactionDb
        {
            TransactionId = transaction.TransactionId,
            TransactionTime = transaction.TransactionTime,
            ArrivalMoney = transaction.ArrivalMoney,
            ExpenseMoney = transaction.ExpenseMoney,
            PaymentMethod = (int) transaction.PaymentMethod,
            TransactionBasis = (int) transaction.TransactionBasis,
            OrderId = transaction.OrderId,
            UserId = transaction.UserId,
            Comment = transaction.Comment
        };
    }

    public static Transaction ConvertTransaction(TransactionDb transaction)
    {
        return new Transaction
        {
            TransactionId = transaction.TransactionId,
            TransactionTime = transaction.TransactionTime,
            ArrivalMoney = transaction.ArrivalMoney,
            ExpenseMoney = transaction.ExpenseMoney,
            PaymentMethod = (PaymentMethods) transaction.PaymentMethod,
            TransactionBasis = (TransactionBasis) transaction.TransactionBasis,
            OrderId = transaction.OrderId,
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
            IsOccupied = devicePlace.IsOccupied
        };
    }
    public static DevicePlace ConvertDevicePlace(DevicePlaceDb devicePlace)
    {
        return new DevicePlace
        {
            PlaceId = devicePlace.PlaceId,
            Info = devicePlace.Info,
            IsOccupied = devicePlace.IsOccupied
        };
    }

    public static Order ConvertOrder(OrderDb order)
    {
        return new Order
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            ClientId = order.ClientId,
            DefectId = order.DefectId,
            AppearanceId = order.AppearanceId,
            DeviceTypeId = order.DeviceTypeId,
            BrandId = order.BrandId,
            Model = order.Model,
            DevicePassword = order.DevicePassword,
            OperatorId = order.OperatorId,
            MasterId = order.MasterId,
            Status = (OrdersStatuses)order.Status,
            RepairType = (RepairTypes)order.RepairType,
            Money = order.Money,
            DevicePlaceId = order.DevicePlaceId
        };
    }

    public static OrderDb ConvertOrder(Order order)
    {
        return new OrderDb
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            ClientId = order.ClientId,
            DefectId = order.DefectId,
            AppearanceId = order.AppearanceId,
            DeviceTypeId = order.DeviceTypeId,
            BrandId = order.BrandId,
            Model = order.Model,
            DevicePassword = order.DevicePassword,
            OperatorId = order.OperatorId,
            MasterId = order.MasterId,
            Status = (int)order.Status,
            RepairType = (int)order.RepairType,
            Money = order.Money,
            DevicePlaceId = order.DevicePlaceId
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
            Name = deviceType.Name
        };
    }

    public static SourceDb ConvertSource(Source source)
    {
        return new SourceDb
        {
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
            Name = sparePartCategory.Name
        };
    }

    #endregion
}