using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Hangman.Windows8.Utilities
{
    public static class LiveTileHandler
    {
        public static void UpdateTile()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            // create a string with the tile template xml
            string tileXmlString = "<tile>"
                              + "<visual>"
                              + "<binding template='TileWideText09'>"
                              + "<text id='1'>Бесеница</text>"
                              + "<text id='2'>Познати думи: " + roamingSettings.Values["guessedWords"] + "</text>"
                              + "</binding>"
                              + "<binding template='TileSquareBlock'>"
                              + "<text id='1'>" + roamingSettings.Values["guessedWords"] + "</text>"
                              + "<text id='2'>познати думи</text>"
                              + "</binding>"
                              + "</visual>"
                              + "</tile>";

            // create a DOM
            Windows.Data.Xml.Dom.XmlDocument tileDom = new Windows.Data.Xml.Dom.XmlDocument();

            // load the xml string into the DOM, catching any invalid xml characters 
            tileDom.LoadXml(tileXmlString);

            // create a tile notification
            TileNotification tile = new TileNotification(tileDom);

            // send the notification to the app's application tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tile);
        }
    }
}
