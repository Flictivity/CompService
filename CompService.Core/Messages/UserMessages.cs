namespace CompService.Core.Messages;

public static class UserMessages
{
    public const string UserNotFound = "Пользователь с таким адресом почты не найден";
    public const string UserAlreadyExisting = "Пользователь с таким адресом почты уже существует";
    public const string UserVerifyCodeExpire = "Истек срок действия кода";
    public const string WrongUserVerifyCode = "Неверный код";
    public const string WrongUserPassword = "Неверный пароль";
}