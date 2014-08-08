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

        /* 
        public void GetIdentifier(){
            _ghostTracker.AddClientIdentifier(Context.ConnectionId);
        }
        */
        
        //When the player enters the hub they should add their ghost to the list
        public void ActivateGhost()
        {
            Groups.Add(Context.ConnectionId, RoomName);
            GhostTracker.Instance.AddClientIdentifier(Context.ConnectionId);
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
            /*
            var id = _ghostTracker.GetClientId(Context.ConnectionId);
            _ghostTracker.AddOrUpdateGhost(new GhostPosition(id, transform));
            */
            
            _ghostTracker.AddOrUpdateGhost(new GhostPosition(Context.ConnectionId, transform));
        }

        public void Broadcast(string message)
        {
            Clients.Others.RecieveMessage(message);
        }

        public void SubmitScore(string name, int score)
        {
            _ghostTracker.ScoreBroadcast(name, score.ToString());
        }

    }
}
