using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_310_W22SD_Assignment.Models;
using SD_310_W22SD_Assignment.Models.ViewModels;

namespace SD_310_W22SD_Assignment.Controllers
{
    public class ArtistController : Controller
    {
        private MyTunesContext _db { get; set; }

        public ArtistController(MyTunesContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            ArtistSelectViewModel vm = new ArtistSelectViewModel(_db.Artists.ToList());

            return View(vm);
        }
        public IActionResult ArtistCollection(int? artistId)
        {
            if (artistId != null)
            {
                try
                {
                    Artist artist = _db.Artists.Include(a => a.Songs).ThenInclude(s=>s.Collections).First(a => a.Id == artistId);
                    return View(artist);
                }
                catch
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
