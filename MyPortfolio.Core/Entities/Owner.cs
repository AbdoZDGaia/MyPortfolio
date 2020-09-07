namespace MyPortfolio.Core.Entities
{
    public class Owner : BaseEntity
    {
        public string FullName { get; set; }
        public string ProfileNote { get; set; }
        public string AvatarUrl { get; set; }
        public Address Address { get; set; }
    }
}