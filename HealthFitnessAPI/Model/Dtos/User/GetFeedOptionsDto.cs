namespace HealthFitnessAPI.Model.Dtos.User;

public class GetFeedOptionsDto
{
    public PaginationDto Pagination { get; set; }
    public string FeedOrderBy { get; set; }
    public string? QueryString { get; set; }
}