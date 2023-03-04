namespace CompService.Client.Data.ValidationMessages;

public static class UserValidation
{
    public const string NullablePassword = "Пароль не может быть пустым";
    public const string LeastPasswordLength = "Пароль не может содержать менее 8 символов";
    public const string CapitalLetterInPassword = "Пароль должен содержать хотябы одну заглавную букву";
    public const string LowerLetterInPassword = "Пароль должен содержать хотябы одну строчную букву"; 
    public const string LeastDigitInPassword = "Пароль должен содержать хотябы одну цифру"; 
}