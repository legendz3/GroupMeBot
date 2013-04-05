using System;
using System.Text;
using KeBot.Service.Utils;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace KeBot.Service.Bots.CheckinBot
{
    public class CheckinBot : IBot
    {
        private IRepository<CheckinBotEntity> _repository;

        public CheckinBot(IRepository<CheckinBotEntity> repository)
        {
            _repository = repository;
        }

        public string Process(dynamic input)
        {
            var checkInKeyword = "kebot checkin";
            var locationKeyword = "kebot locations";
            string text = input.text;
            if(string.IsNullOrEmpty(text))
                return null;
            if (text.ToLower().Contains(checkInKeyword))
            {
                text = text.Remove(text.ToLower().IndexOf(checkInKeyword), checkInKeyword.Length + 1);
                var checkin = new CheckinBotEntity();
                checkin.Id = input.id;
                checkin.GroupId = input.group_id;
                checkin.Location = text;
                checkin.UserId = input.user_id;
                checkin.UserName = input.name;
                checkin.CheckInTime = ((long)input.created_at).FromUnixTime();
                _repository.Insert(checkin);
                return string.Format("Checkin recorded: {0} checked in at {1}", checkin.UserName,
                                     checkin.Location);
            }
            if (text.ToLower().Contains(locationKeyword))
            {
                var result = new StringBuilder();
                var checkins = _repository.FindAll(x => x.GroupId == input.group_id);
                var groups = checkins.GroupBy(x => x.UserId);
                foreach (var group in groups)
                {
                    var lastCheckin = @group.OrderByDescending(c => c.CheckInTime).First();
                    result.AppendLine(string.Format("- {0} last checked in at {1} {2}", lastCheckin.UserName,
                                                    lastCheckin.Location, lastCheckin.CheckInTime.TimeAgo()));
                }
                return result.ToString();
            }
            return null;
        }
    }

    public class CheckinBotEntity
    {
        [BsonId]
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Location { get; set; }
        public DateTime CheckInTime { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
