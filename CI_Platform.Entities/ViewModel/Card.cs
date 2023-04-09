using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Entities.ViewModel
{
    public class Card
    {
        public bool checkFav;
        public string? aboutOrganization;
        public object? skillId;

        public long MissionId { get; set; }
        public long SkillId { get; set; }
        public long ThemeId { get; set; }
        public string? Theme { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; } = null!;
        public string SkillName { get; set; } = null!;
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool MissionType { get; set; }
        public string? OrganizationName { get; set; }
        public string? MediaName { get; set; }
        public int Rating { get; set; }
        public string? GoalObjectiveText { get; set; }
        public int GoalValue { get; set; }
        public int? Avaibility { get; set; }
        
        public DateTime? Deadline { get; set; }
      
        public long FavouriteMissionId { get; set; }

        public string? MissionIntro { get; set; }
        public string? Description { get; set; }

        public string? OrganizationDetail { get; set; }
        public string? DocumentPath { get; set; }
        public string? AboutOrganization { get; set; }
        public bool CheckFav { get; set; }

    }
}
