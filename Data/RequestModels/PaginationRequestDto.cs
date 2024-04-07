namespace course_edu_api.Data.RequestModels;

public class PaginationRequestDto<T> 
{
    public T? Where { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}