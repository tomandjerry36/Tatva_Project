using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Entities.ViewModel
{
    public  class CommentViewModel
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public string Time { get; set; }

        public string WeekDay { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string Avatar { get; set; }

        public string Day { get; set; }

        public long MissionId { get; set; }
    }
}
