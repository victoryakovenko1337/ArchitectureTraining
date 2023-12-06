namespace CodeBase.Infrastructure.Services.Randomizer
{
    public interface IRandomService : IService
    {
        int Next(int min, int max);
    }
}