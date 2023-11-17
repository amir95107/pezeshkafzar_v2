﻿using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class HomeRepository : BaseRepository<Page, Guid>, IHomeRepository
    {
        private readonly DbSet<Slider> _slider;
        private readonly DbSet<ContactForm> _contactForms;
        public HomeRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            _slider = context.Set<Slider>();
            _contactForms = context.Set<ContactForm>();
        }

        public async Task<List<ContactForm>> GetFaqsAsync(CancellationToken cancellationToken)
            => await _contactForms.Where(x => x.IsFaq).ToListAsync(cancellationToken);

        public async Task<Page> GetPageDetailAsync(int pageKey, CancellationToken cancellationToken)
            => await Entities.FirstOrDefaultAsync(x => x.PageKey == pageKey, cancellationToken);

        public async Task<List<Slider>> GetSliderListAsync(CancellationToken cancellationToken)
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return await _slider.Where(s => s.IsActive && s.StartDate <= dt && s.EndDate >= dt).OrderByDescending(s => s.StartDate).ToListAsync(cancellationToken);
        }
    }
}
