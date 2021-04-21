using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IMiscFacade
    {
        Response<int> ViewPage(CoPageView item);
        Response<int> MarkFavorite(CoFavorite item);
    }
}
