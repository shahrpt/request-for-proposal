using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IMiscRepository
    {
        Response<int> ViewPage(CoPageView item);
        Response<int> MarkFavorite(CoFavorite item);
    }
}
