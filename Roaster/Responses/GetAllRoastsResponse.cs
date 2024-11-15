namespace Roaster.Responses
{
    public record GetAllRoastsResponse(IEnumerable<SingleRoast> Roasts);
    public record SingleRoast(string Id, string Name);
}
