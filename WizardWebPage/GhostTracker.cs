using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using WizardWebPage.Classes;

namespace WizardWebPage
{
    public class GhostTracker
    {
        private readonly static Lazy<GhostTracker> _instance = new Lazy<GhostTracker>(() => new GhostTracker(GlobalHost.ConnectionManager.GetHubContext<GhostHub>().Clients));

        private readonly ConcurrentDictionary<string, GhostPosition> positions = new ConcurrentDictionary<string, GhostPosition>();

        
        private static int clientNumber = 0;
        private readonly ConcurrentDictionary<string, int> _clientIdentifiers = new ConcurrentDictionary<string, int>();
        
        
        private  readonly  object _updatePositionsLock = new object();

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Timer _timer;

        private GhostTracker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;

            _timer = new Timer(UpdatePositions, null, _updateInterval, _updateInterval);
        }

        public static GhostTracker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }
        
    
        public void AddClientIdentifier(string clientId){
            clientNumber++;
            //This should be ok as long as we don't have one person stayin the client for 9999 connections
            if (clientNumber > 999)
            {
                clientNumber = 0;
            }
            var shortId = clientNumber;
            
            _clientIdentifiers.AddOrUpdate(clientId, shortId, (key, oldValue) => shortId);
            Clients.Client(clientId).setIdentifier(shortId);
        }

        public void AddOrUpdateGhost(GhostPosition ghost)
        {
            positions.AddOrUpdate(ghost.name, ghost, (s, position) => ghost);
        }

        public void RemoveGhost(string clientId)
        {
            GhostPosition position;
            int shortId;
            _clientIdentifiers.TryRemove(clientId, out shortId);
            positions.TryRemove(clientId, out position);
        }

        private void UpdatePositions(object state)
        {
            lock (_updatePositionsLock)
            {
                if (positions.Count == 0)
                    return;


                //TODO: Ensure this works, then apply it. Maybe sure the Unity endpoint is expecting this as well
                List<string> positionList = positions.Select(ghostPosition =>
                {
                    if (!_clientIdentifiers.ContainsKey(ghostPosition.Value.name)) return null;
                    return String.Format("{0},{1}|", _clientIdentifiers[ghostPosition.Value.name], ghostPosition.Value.position);
                }).ToList();
                //TODO: Do this with string builder
                var payload = "";
                foreach (var ghostString in positionList)
                {
                    payload += ghostString;
                }

                //Clients.All.updatePositions(payload);

                //List<GhostPosition> positionList = positions.Select(ghostPosition => ghostPosition.Value).ToList();
                //var payload = JsonConvert.SerializeObject(positionList);  
                Clients.Group(GhostHub.RoomName).updatePositions(payload);
            }
        }
    }
}
