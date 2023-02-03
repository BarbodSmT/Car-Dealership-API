using WebFramework.Api;

namespace BareProject.Dto.Test;

public class TestDto : BaseDto<long>
{
    public string Test { get; set; }      
}