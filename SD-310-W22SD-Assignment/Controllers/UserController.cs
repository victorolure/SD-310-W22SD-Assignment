using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_310_W22SD_Assignment.Models;
using SD_310_W22SD_Assignment.Models.ViewModels;

namespace SD_310_W22SD_Assignment.Controllers
{
    public class UserController : Controller
    {
        private MyTunesContext _db { get; set; }
        public UserController(MyTunesContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            UserSelectViewModel vm = new UserSelectViewModel(_db.Users.ToList());
            return View(vm);
        }

        
        public IActionResult UserCollection(int? userId)
        {
            SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());
            ViewBag.Message = sm.SongSelect;
            
            if(userId != null)
            {
                try
                {
                    User user = _db.Users.Include(u => u.Collections.OrderBy(c=>c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s=>s.ArtistNavigation).First(u => u.Id == userId);
                    return View(user);
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

        //[HttpPost]
        //public IActionResult UserCollection(int?userId,int? songId)
        //{
        //    if (userId != null && songId != null)
        //    {
        //        try
        //        {
        //            Song song = _db.Songs.First(s => s.Id == songId);
        //            User user = _db.Users.First(u => u.Id == userId);
        //            Collection newCollection = new Collection();
        //            //newCollection.User = user;
        //            // newCollection.Song = song;
        //            newCollection.UserId = user.Id;
        //            newCollection.SongId = song.Id;
        //            _db.Collections.Add(newCollection);
        //            _db.SaveChanges();
        //            return View("UserCollection");
        //        }
        //        catch
        //        {
        //            return View("UserCollection");
        //        }
        //    }
        //    else
        //    {
        //        return View("UserCollection");
        //    }
        //}

    }
}
