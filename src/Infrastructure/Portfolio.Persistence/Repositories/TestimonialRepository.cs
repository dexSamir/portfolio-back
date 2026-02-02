using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Domain.Entities;
using Portfolio.Persistence.Contexts;

namespace Portfolio.Persistence.Repositories;

public class TestimonialRepository(PortfolioDbContext context) : GenericRepository<Testimonial>(context), ITestimonialRepository
{
    
}