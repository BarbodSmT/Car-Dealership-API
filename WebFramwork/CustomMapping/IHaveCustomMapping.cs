using AutoMapper;

namespace WebFramwork.CustomMapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}