using Microsoft.AspNetCore.Mvc.Rendering;

namespace SD_310_W22SD_Assignment.Models.ViewModels
{
    public class ArtistSelectViewModel
    {
        public List<SelectListItem> ArtistSelect { get; set; }

        public ArtistSelectViewModel(List<Artist> artists)
        {
            ArtistSelect = new List<SelectListItem>();
            
            
            artists.ForEach(a =>
            {
                ArtistSelect.Add(new SelectListItem(a.Name, a.Id.ToString()));
            });
        }
    }
}
