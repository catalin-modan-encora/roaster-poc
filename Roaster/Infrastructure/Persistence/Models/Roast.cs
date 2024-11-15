namespace Roaster.Infrastructure.Persistence.Models
{
    public class Roast
    {
        public int Id { get; init; }
        public string Name { get; private set; } = null!;

        private Roast()
        {
        }

        public static Roast Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new Roast { Name = name };
        }
    }
}
