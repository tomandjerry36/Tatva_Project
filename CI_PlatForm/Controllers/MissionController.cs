using CI_PlatForm.Entities.Models;
using CI_PlatForm.Entities.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CI_PlatForm.Repository.Interface;

namespace CI_PlatForm.Controllers
{
    public class MissionController : Controller
    {

        public readonly IMissionRepository _MissionRepository;
        public readonly IUserRepository _UserRepository;
        public MissionController(IMissionRepository MissionRepository, IUserRepository userRepository)
        {
            _MissionRepository = MissionRepository;
            _UserRepository = userRepository;
        }

        //------------------Mission drop down---------------------------
        public IActionResult PlatformLanding(long id)
        {

            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));


            var CountryList = _MissionRepository.GetCountryData();
            ViewBag.countryList = CountryList;

            var MissionTheme = _MissionRepository.GetMissionTheme();
            ViewBag.missionTheme = MissionTheme;

            var SkillList = _MissionRepository.GetSkillsList();
            ViewBag.skillList = SkillList;

            /*var missions = _MissionRepository.GetMissionList();
            ViewBag.missions = missions;*/

            



            //-------------------Mission card----------------------------------------------------------


            List<Card> missionCardDetails = _MissionRepository.GetMissionCard(userId);
            var DetailsOfWorker = _UserRepository.UserList().Where(i => i.UserId != userId);
            ViewBag.userDetails = DetailsOfWorker;
            var coWorkerList = _MissionRepository.Recommend;

            ViewBag.totalMission = missionCardDetails.Count();

            ViewBag.CardDetail = missionCardDetails.Skip((1-1)*6).Take(6).ToList();

            //--------------------------Pagnation----------------------------------------

            ViewBag.pg_no = 1;
            ViewBag.Totalpages = Math.Ceiling(missionCardDetails.Count() / 6.0);
            ViewBag.missionCardDetails = missionCardDetails.Skip((1-1)*6).Take(6).ToList();
            ViewBag.pg_no = 1;
            return View();

        }

        
        public JsonResult GetCity(List<string> countryId)
        {
            List<City> city = _MissionRepository.GetCityFromCountry(countryId);
            var json = JsonConvert.SerializeObject(city);

            return Json(json);
        }
        //--------------------------------------search bar-------------------------------------------
        [HttpPost]
        public ActionResult Search(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sort, int paging)
        {
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var DetailsOfWorker = _UserRepository.UserList().Where(i => i.UserId == userId);
            ViewBag.userDetails = DetailsOfWorker;
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<Card> missionCardDetails = _MissionRepository.GetMissionCard(userId);
            List<Card> missions = _MissionRepository.GetMissionList(search, countries, cities, themes, skills, sort,paging, userId);
            ViewBag.cardDetail = missions;
            ViewBag.pg_no = paging;
            ViewBag.TotalPages = Math.Ceiling(missionCardDetails.Count()/6.0);
           
            if (missions == null)
            {
                return PartialView("_MissionNotFound");
            }
           
            return PartialView("_gridView");
        }
        
        public ActionResult SearchList(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int sort, int paging)
        {
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var DetailsOfWorker = _UserRepository.UserList().Where(i => i.UserId == userId);
            ViewBag.userDetails = DetailsOfWorker;
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<Card> missionCardDetails = _MissionRepository.GetMissionCard(userId);
            List<Card> missions = _MissionRepository.GetMissionList(search, countries, cities, themes, skills, sort, paging, userId);
            ViewBag.cardDetail = missions;
            ViewBag.pg_no = paging;
            ViewBag.TotalPages = Math.Ceiling(missionCardDetails.Count()/6.0);

            if (missions == null)
            {
                return PartialView("_MissionNotFound");
            }

            return PartialView("_listView");
        }

        
    /*----------------------------------Volunteer mission-----------------------------------------------------------------------------*/
       public IActionResult MissionVolunteering(long id)
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            List<Card> VolunteerCard = _MissionRepository.GetMissionCard(userId);
            
            var missions = VolunteerCard.FirstOrDefault(i => i.MissionId == id);
            ViewBag.cardData = missions;
            ViewBag.getRating = _MissionRepository.getRating(missions.MissionId, userId);
            
            ViewBag.commentViewBag = _MissionRepository.getComment(missions.MissionId);
            
            ViewBag.getSkill = _MissionRepository.GetSkillName(missions.MissionId);

            ViewBag.userId = userId;
            ViewBag.pg_no = 0;
            
            var userDetails = _UserRepository.UserList().Where(i => i.UserId != userId);
            ViewBag.userDetails = userDetails;

            ViewBag.getVolunteer = _MissionRepository.GetRecentUser(missions.MissionId).Take(9).ToList();
            ViewBag.Totalpages = Math.Ceiling(_MissionRepository.getMissionApplicant().Count() / 9.0);
            ViewBag.totalVol = _MissionRepository.GetRecentUser(missions.MissionId).Count();
            ViewBag.totalApplicant = _MissionRepository.getMissionApplicant().Count();

            ViewBag.checkFav = _MissionRepository.checkFavourite(id, userId);
            ViewBag.checkApplied = _MissionRepository.checkApplied(id, userId);
            
            var coWokerList = _MissionRepository.Recommend;
            ViewBag.ratingVolunteers = _MissionRepository.countVolunteers(missions.MissionId);

            var RelatedMission = VolunteerCard.Where(a => a.MissionId != missions.MissionId && (a.CityId == missions.CityId || a.ThemeId == missions.ThemeId || a.MissionType == missions.MissionType)).Take(3).ToList();   
            ViewBag.relatedMission = RelatedMission;
            
            return View();
        }
        public JsonResult VolunteerList(List<long> Volunteers, long MissionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var voluntees = _MissionRepository.Recommend(userId, MissionId, Volunteers);
            return Json(voluntees);
        }
        public void PostCommentInMission(string comment, long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));

            _MissionRepository.PostComment(comment, userId, missionId);
        }
        public bool AddMissionToFav(int missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            var fav = _MissionRepository.addToFavourite(missionId, userId);
            return fav;
        }
        public int PostRating(byte rate, long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("userId"));
            bool UserValid = _MissionRepository.checkApplied(missionId,userId);
            if(UserValid == true)
            {
                bool result = _MissionRepository.PostRating(rate, missionId, userId);
                return 1;
            }
            else if(UserValid == false)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        public void AddToRecentVolunteer(long missionId, long userId)
        {
            bool check = _MissionRepository.checkApplied(missionId, userId);

            if (check != true)
            {
                _MissionRepository.AddToRecent(missionId, userId);
            }
        }
        public ActionResult Voluntee‎rPaging(int paging, long mission_id)
        {
            double total_pages = Math.Ceiling(_MissionRepository.GetRecentUser(mission_id).Count() / 9.0);
            if(paging > 0 && paging <= total_pages)
            {
                ViewBag.getVolunteer = _MissionRepository.GetRecentUser(mission_id).Skip(9 * paging).Take(9).ToList();
                ViewBag.totalVol = _MissionRepository.GetRecentUser(mission_id).Count();
                ViewBag.totalApplicant = _MissionRepository.getMissionApplicant().Count();
                ViewBag.pg_no = paging;
                return PartialView("_RecentVolunteer");
            }
            else
            {
                ViewBag.getVolunteer = _MissionRepository.GetRecentUser(mission_id).Skip(9 * paging).Take(9).ToList();
                ViewBag.totalVol = _MissionRepository.GetRecentUser(mission_id).Count();
                ViewBag.totalApplicant = _MissionRepository.getMissionApplicant().Count();
                ViewBag.pg_no = paging;
                return PartialView("_RecentVolunteer");
            }
        }

        /* ------------------------Volunteer story---------------------------------------------------*/
        public IActionResult VolunteeringStory()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            List<StoryViewModel> storyCardDetails = _MissionRepository.GetStoryDetails().ToList();
            var CountryList = _MissionRepository.GetCountryData();
            ViewBag.CountryList = CountryList;
            var MissionTheme = _MissionRepository.GetMissionTheme();
            ViewBag.missionTheme = MissionTheme;

            var SkillList = _MissionRepository.GetSkillsList();
            ViewBag.skillList = SkillList;

            ViewBag.pg_no = 1;
            ViewBag.Totalpages = Math.Ceiling(storyCardDetails.Count() / 6.0);
            ViewBag.cardDetail = storyCardDetails.Take(6).ToList();
            
            return View();
        }

        [HttpPost]
        public ActionResult SearchStory(string? search, string[] countries, string[] cities, string[] themes, string[] skills, int paging)
        {
            long user = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<StoryViewModel> storyCardDetails = _MissionRepository.GetStoryDetails();
            List<StoryViewModel> storyCard = _MissionRepository.GetStoryList(search, countries, cities, themes, skills, paging);
            ViewBag.cardDetail = storyCard;
            ViewBag.pg_no = paging;
            ViewBag.Totalpages = Math.Ceiling(storyCardDetails.Count() / 6.0);
            return View("_storyCard");

        }




        //---------------------Mission not found --------------------------------------------------------------------------------


        public IActionResult ShareStory()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            ViewBag.missionStoryList = _MissionRepository.getStoryMission(userId);

            return View();
        }
        [HttpPost]
        
        public IActionResult ShareStory(StoryViewModel model, string submit)
        {
            
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
           _MissionRepository.AddStory(model, userId, submit);
            ViewBag.missionStoryList = _MissionRepository.getStoryMission(userId);
            return View("ShareStory");
        }
        public IActionResult StoryDetail(long id)
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");
            
            long userId = (long)Convert.ToInt64(HttpContext.Session.GetString("userId"));
            
            StoryViewModel model = _MissionRepository.getStory(id, userId);
            var userDetails = _UserRepository.UserList().Where(i => i.UserId != userId);
            ViewBag.userDetails = userDetails;
            List<Card> VolunteerCard = _MissionRepository.GetMissionCard(userId);
            var missions = VolunteerCard.FirstOrDefault(i => i.MissionId == id);
            ViewBag.cardData = missions;
           
            
            return View(model);
        }
       
        public IActionResult UserDetail()
        {
            ViewBag.sessionValue = HttpContext.Session.GetString("username");


            var CountryList = _MissionRepository.GetCountryData();
            ViewBag.countryList = CountryList;
            return View();
        }
    }
}        
   
        
