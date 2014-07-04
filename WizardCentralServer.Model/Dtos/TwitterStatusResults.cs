using System.Collections.Generic;
using Newtonsoft.Json;

namespace WizardCentralServer.Model.Dtos
{
    public class TwitterStatusResults
    {
        [JsonProperty("statuses")]
        public List<TwitterStatus> Statuses;
    }

    public class TwitterStatus
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("text")]
        public string Text;

        [JsonProperty("user")]
        public TwitterUser User;

    }

    public class TwitterUser
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("screen_name")]
        public string ScreenName;
    }

}