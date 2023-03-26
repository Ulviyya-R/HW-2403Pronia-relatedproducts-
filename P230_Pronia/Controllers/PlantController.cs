using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Controllers
{
    public class PlantController:Controller
    {
        private readonly ProniaDbContext _context;

        public PlantController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Plant> plants = _context.Plants.
                 Include(p => p.PlantTags).ThenInclude(pt => pt.Tag).
                 Include(p => p.PlantCategories).ThenInclude(pc => pc.Category).
                 Include(p => p.PlantDeliveryInformation).
                  Include(p => p.PlantImages)
                 .ToList();
            return View(plants);
          
        }

        public IActionResult Detail(int id,int CategoryId)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Plant? plant = _context.Plants
                  .Include(p => p.PlantTags).ThenInclude(pt => pt.Tag)
                  .Include(p => p.PlantCategories).ThenInclude(pc => pc.Category)
                  .Include(p => p.PlantDeliveryInformation)
                  .Include(p => p.PlantImages)
                  .FirstOrDefault(p => p.Id == id);



            ViewBag.RelatedPlant = _context.Plants
                .Include(p => p.PlantImages)
                .Include(pc => pc.PlantCategories).ThenInclude(pc => pc.Category)

                .Where(p => p.PlantCategories.Any(pc => pc.Category.Id == CategoryId && pc.Category.Id != id && p.Id != id))
                .ToList();

            return View(plant);
        }

        

        }
    }

