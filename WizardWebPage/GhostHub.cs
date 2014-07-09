using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using WizardWebPage.Classes;

namespace WizardWebPage
{
    public class GhostHub : Hub
    {
        //TODO: Data usage by...
        //1) Map client Ids to sequential numbers and using those for clients to idenitfy themselves
        //2.) Reduce json parameters from "name" and "position" to "n" and "p"
        //3.) Reduce the rate at which data is braodcast (from once per 1 sec to once per 2 sec or sommin)
        //4.) Dynamically increase/decrease update rates depending on current number of players
        //5.) Instead of json, reduce reponse to something like name,x,y,z|name,x,y,z|name,x,y,z

        private readonly GhostTracker _ghostTracker;
        public const string RoomName = "hubzone";


        public GhostHub() : this(GhostTracker.Instance) { }

        public GhostHub(GhostTracker ghostTracker)
        {
            _ghostTracker = ghostTracker;
        }

        public override Task OnConnected()
        {
            //ActivateGhost();
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            DeactivateGhost();
            return base.OnDisconnected();
        }

        //When the player enters the hub they should add their ghost to the list
        public void ActivateGhost()
        {
            Groups.Add(Context.ConnectionId, RoomName);
            _ghostTracker.AddOrUpdateGhost(new GhostPosition(Context.ConnectionId,"(0,0,0)" ));
        }

        // When the player leaves the hub their ghost shouldn't show up in other games
        // and they should stop receiving ghost broadcasts
        public void DeactivateGhost()
        {
            Groups.Remove(Context.ConnectionId, RoomName);
            _ghostTracker.RemoveGhost(Context.ConnectionId);
        }

        public void SetGhostPosition(string transform)
        {
            _ghostTracker.AddOrUpdateGhost(new GhostPosition(Context.ConnectionId, transform));
        }

        
    }
}