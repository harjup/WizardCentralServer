using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WizardWebPage.Classes
{
    public class GhostPosition
    {
        public string name;
        public string position;
        
        /*TODO: Ensure this actually compiles, then copy it on over to the Untiy side for converting back to an object
        public GhostPosition(string _name, string _position)
        {
            name = _name;
            position = _position;
        }
        
        
        public static string ConvertToString(){
        //TODO: Use the fact that ids will only be letters to remove delimiter between name and position
            return String.Format("{0},{1}|", name, position);
        }
        
        public static List<GhostPostiion> FromGroupString(string ghostString){
            
            List<GhostPosition> ghostPositions = new List<GhostPosition>();
            
            string[] ghosts = ghostString.split('|');
            foreach (string s in ghosts)
            {
                ghostPositions.Add(FromString(s));
            }
            
            return ghostPositions;
        }
        
        private static GhostPosition FromString(string str){
            string[] ghostProperties = str.split(',');
            foreach (string s in ghostProperties)
            {
                return new GhostPositon(ghostProperties[0],
                                        String.Format("{0},{1},{2}" 
                                            ghostProperties[1], 
                                            ghostProperties[2], 
                                            ghostProperties[3]
                                            )
                );
            }
        }
        */
        
    }
}
