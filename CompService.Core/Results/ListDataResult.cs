namespace CompService.Core.Results;

public class ListDataResult<T> : BaseResult
{
    public List<T>? Items { get; init; }
    public int TotalItemsCount { get; init; }
}