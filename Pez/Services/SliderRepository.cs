using DataLayer.Data;
using DataLayer.Models;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class SliderRepository : BaseRepository<Slider, Guid>, ISliderRepository
    {
        public SliderRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
        }
    }
}
