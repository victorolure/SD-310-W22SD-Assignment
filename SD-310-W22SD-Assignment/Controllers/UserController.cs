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

        
        public IActionResult RateSong(int? songId, int? userId)
        {
            User user = _db.Users.First(u => u.Id == userId);
            ViewBag.Message = user.Id;
            Song song = _db.Songs.Include(s=> s.ArtistNavigation).First(s => s.Id == songId);
            return View(song);

        }
        [HttpPost]
        public IActionResult RateSong(int?songId, int?userId, int rating)
        {
            User user = _db.Users.Include(u => u.Collections).First(u => u.Id == userId);
            Song song = _db.Songs.Include(s => s.ArtistNavigation).First(s => s.Id == songId);

            Collection collection = user.Collections.First(u => u.Song == song && u.User == user );
            if(collection.Rating == 0)
            {
                if(rating <= 5)
                {
                   user.Collections.First(u => u.UserId == userId && u.SongId == songId).Rating += rating;
                    _db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = user.Id;
                    ViewBag.Message2 = "Sorry, Maximum rating is 5";
                    return View("RateSong", song);
                }
            }
            else
            {
                if(rating > 5)
                {
                    ViewBag.Message = user.Id;
                    ViewBag.Message2 = "Sorry, Maximum rating is 5";
                    return View("RateSong", song);
                }
                else
                {
                    user.Collections.First(u => u.UserId == userId && u.SongId == songId).Rating = rating;
                    _db.SaveChanges();
                }                
            }           
            user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
            
            return View("UserCollection",user);

        }
        public IActionResult TopSellingSongs()
        {
            Dictionary<Song, int> dbSongs = new Dictionary<Song, int>();
            List<Collection> dbCollections = _db.Collections.Include(c => c.Song).ThenInclude(s => s.ArtistNavigation).ToList();


            foreach (Collection c in dbCollections)
            {
                if (dbSongs.Any(d=> d.Key == c.Song ))
                {
                    
                    dbSongs[c.Song] += 1;
                }
                else
                {
                    dbSongs.Add(c.Song, 1);
                }
            }
            Dictionary<Song, int> sortedDbSongs = dbSongs.OrderByDescending(c => c.Value).ToDictionary(c=> c.Key, c=> c.Value);
            List<Song> SortedSongs = new List<Song>();
            foreach(KeyValuePair<Song,int>kvp in sortedDbSongs)
            {
                SortedSongs.Add(kvp.Key);
            }
            List<Song> TopSellingSongs = SortedSongs.Take(3).ToList();
            return View(TopSellingSongs);
        }
        public IActionResult TopThreeSellingArtists()
        {
            Dictionary<Artist, int> dbArtists = new Dictionary<Artist, int>();
            List<Collection> dbCollections = _db.Collections.Include(c => c.Song).ThenInclude(s => s.ArtistNavigation).ToList();


            foreach (Collection c in dbCollections)
            {
                if (dbArtists.Any(d => d.Key.Name == c.Song.ArtistNavigation.Name))
                {

                    dbArtists[c.Song.ArtistNavigation] += 1;
                }
                else
                {
                    dbArtists.Add(c.Song.ArtistNavigation, 1);
                }
            }
            Dictionary<Artist, int> sortedArtists = dbArtists.OrderByDescending(c => c.Value).Take(3).ToDictionary(c => c.Key, c => c.Value);       
            return View(sortedArtists);
        }
        public IActionResult TopThreeRatedSongs()
        {
            Dictionary<Song, int> dbSongs = new Dictionary<Song, int>();
            List<Collection> dbCollections = _db.Collections.Include(c => c.Song).ThenInclude(s => s.ArtistNavigation).ToList();


            foreach (Collection c in dbCollections)
            {
                if (dbSongs.Any(d => d.Key == c.Song))
                {

                    dbSongs[c.Song] += c.Rating;
                }
                else
                {
                    dbSongs.Add(c.Song, c.Rating);
                }
            }
            Dictionary<Song, int> sortedDbSongs = dbSongs.OrderByDescending(c => c.Value).ToDictionary(c => c.Key, c => c.Value);
            Dictionary<Song, int> topThreeRatedSongs = sortedDbSongs.Take(3).ToDictionary(c => c.Key, c => c.Value);
           
            return View(topThreeRatedSongs);
        }


        [HttpPost]
        public IActionResult RefundSong(int? songId, int? userId)
        {
            User user = _db.Users.Include(u => u.Collections).First(u => u.Id == userId);
            Song song = _db.Songs.First(s => s.Id == songId);
            Collection collectionToRefund = user.Collections.First(c => c.Song == song && c.User == user);
            TimeSpan dateDifference = DateTime.Now.Subtract(collectionToRefund.PurchaseDate);
            
            if (dateDifference.TotalDays < 30)
            {
                user.Collections.Remove(collectionToRefund);
                user.Wallet += song.Price;
                _db.Collections.Remove(collectionToRefund);
                _db.SaveChanges();
                SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());
                ViewBag.Message = sm.SongSelect;
                user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
                return View("UserCollection", user);
            }
            else
            {
                SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());
                ViewBag.Message = sm.SongSelect;
                ViewBag.Message2 = "Sorry, You can only get a refund within 30 days of your purchase";
                user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
                return View("UserCollection", user);
            }
            
        }

        public IActionResult BuySong(int? userId)
        {
            User user = _db.Users.Include(u=> u.Collections).First(u => u.Id == userId);
            List<Song> list = _db.Songs.Include(s=> s.ArtistNavigation).ToList();
            List<Collection> currentUserCollection = user.Collections.ToList();
            List<Song> absentSongs = new List<Song>();
            ViewBag.Message = user.Id;
            foreach(Song s in list)
            {
                if(!currentUserCollection.Any(c=> c.SongId == s.Id))
                {
                   absentSongs.Add(s); 
                }
            }
            absentSongs = absentSongs.OrderBy(s => s.Title).ToList();
            return View(absentSongs);
            
        }

        [HttpPost]
        public IActionResult BuySong(int?userId, int? songId)
        {
            User user = _db.Users.Include(u => u.Collections).First(u => u.Id == userId);
            Song song = _db.Songs.First(s => s.Id == songId);
            Collection newCollection = new Collection();
            if (!_db.Collections.Any(c => c.UserId == userId && c.SongId == songId))
            {
                if(user.Wallet >= song.Price)
                {
                    newCollection.User = user;
                    newCollection.Song = song;
                    newCollection.UserId = user.Id;
                    newCollection.SongId = song.Id;
                    newCollection.PurchaseDate = DateTime.Now.Date;
                    song.Collections.Add(newCollection);
                    user.Collections.Add(newCollection);
                    _db.Collections.Add(newCollection);
                    user.Wallet -= song.Price;
                    _db.SaveChanges();
                    user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
                    return View("UserCollection", user);
                }
                else
                {
                    ViewBag.Message2 = "Sorry, Insufficient balance, Please fund your wallet";
                    user = _db.Users.Include(u => u.Collections).First(u => u.Id == userId);
                    List<Song> list = _db.Songs.Include(s=> s.ArtistNavigation).ToList();
                    List<Collection> currentUserCollection = user.Collections.ToList();
                    List<Song> absentSongs = new List<Song>();
                    ViewBag.Message = user.Id;
                    foreach (Song s in list)
                    {
                        if (!currentUserCollection.Any(c => c.SongId == s.Id))
                        {
                            absentSongs.Add(s);
                        }
                    }
                    return View("BuySong", absentSongs);
                    
                }
                
            }
            else
            {
                return View("UserCollection", user);
            }
        }

        public IActionResult BalanceError(int?userId)
        {
            User user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
            ViewBag.Message = "Sorry, Insufficient balance, Click button below to fund your wallet";
            return View(user);
        }

        public IActionResult FundWallet(int? userId)
        {
            User user = _db.Users.First(u => u.Id == userId);
            return View(user);
        }
        [HttpPost]
        public IActionResult FundWallet(int? userId, int amount)
        {
            User user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
            SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());
            ViewBag.Message = sm.SongSelect;
            user.Wallet += amount;
            _db.SaveChanges();
           
            return Redirect($"https://localhost:7265/User/UserCollection?userId={user.Id}");
           
        }

        
        public IActionResult Index()
        {
            //UserSelectViewModel am = new UserSelectViewModel(_db.Users.ToList());
            UserSelectViewModel vm = new UserSelectViewModel(_db.Users.ToList());
            return View(vm);
        }

        
        public IActionResult UserCollection(int? userId)
        {
            SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());
            ViewBag.Message = sm.SongSelect;           
            User user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
            return View(user);
        }
        
        [HttpPost]
        public IActionResult UserCollection(int? userId, int? songId)
        {
            Song song = _db.Songs.First(s => s.Id == songId);
            User user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
            Collection newCollection = new Collection();
            SongSelectViewModel sm = new SongSelectViewModel(_db.Songs.ToList());
            ViewBag.Message = sm.SongSelect;
            if (!_db.Collections.Any(c => c.UserId == userId && c.SongId == songId))
            {
                newCollection.User = user;
                newCollection.Song = song;
                newCollection.UserId = user.Id;
                newCollection.SongId = song.Id;
                _db.Collections.Add(newCollection);
                _db.SaveChanges();
                user = _db.Users.Include(u => u.Collections.OrderBy(c => c.Song.Title)).ThenInclude(c => c.Song).ThenInclude(s => s.ArtistNavigation).First(u => u.Id == userId);
                return View("UserCollection", user);
            }
            else
            {
                return View("UserCollection", user);
            }
        }

        

        public IActionResult UserSong()
        {
            UserSongSelectViewModel um = new UserSongSelectViewModel(_db.Songs.ToList(), _db.Users.ToList());
            return View(um);
        }

        [HttpPost]
        public IActionResult UpdateCollection(int? userId, int? songId)
        {
            UserSelectViewModel vm = new UserSelectViewModel(_db.Users.ToList());
            if (userId != null && songId != null)
            {
                try
                {
                    Song song = _db.Songs.First(s => s.Id == songId);
                    User user = _db.Users.First(u => u.Id == userId);
                    Collection newCollection = new Collection();
                    if( !_db.Collections.Any(c => c.UserId== userId && c.SongId== songId))
                    {
                        newCollection.User = user;
                        newCollection.Song = song;
                        newCollection.UserId = user.Id;
                        newCollection.SongId = song.Id;
                        _db.Collections.Add(newCollection);
                        _db.SaveChanges();
                        return View("Index", vm);
                    }
                    else
                    {
                        return View("Index", vm);
                    }
                    
                }
                catch
                {
                    return View("Index");
                }
            }
            else
            {
                return View("UserCollection");
            }
        }

    }
}
