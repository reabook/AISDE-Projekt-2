using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_Projekt_2 {
    class Simulator {
        Player player { get; set; }

        Event lastEvent;
        Event lastDownload;
        List<Event> events = new List<Event>();

        List<int> chartTime = new List<int>();
        List<int> chartBuffer = new List<int>();
        List<int> chartSpeed = new List<int>();
        List<string> chartQuality = new List<string>();

        const int maxBuffer = 40;
        const int minBuffer = 30;
        const double lambda = 0.1;

        const int speedChanges = 9;

        public void Simulate(int time) {

            int currentTime = 0;
            int connectionSpeed = 8;

            player = new Player();

            chartTime.Add(currentTime);
            chartBuffer.Add(player.Buffer);
            chartSpeed.Add(connectionSpeed);
            chartQuality.Add(player.Quality);

            events.Add(new Event("startdownloading", currentTime));
            player.Buffer = 0;

            var chunk = new Chunk(2, 10);

            for(int i = 0; i < speedChanges; i++) {
                var temp = getSpeedChangeTime();
                while (temp > time)
                    temp = getSpeedChangeTime();

                events.Add(new Event("speedchange", (int)temp));
            }

            while(currentTime < time && events.Count != 0) {
                lastEvent = getEvent();

                switch(lastEvent.Type) {
                    case "startdownloading":
                        var evt = new Event("downloaded", new Chunk(2, 10), connectionSpeed, currentTime);
                        events.Add(evt);
                        lastDownload = evt;
                    break;

                    case "downloaded":
                        player.Buffer += lastEvent.Chunk.Length;
          
                        var chk = new Chunk(2, 10);
                        events.Add(new Event("startdownloading", currentTime));

                    break;

                    case "speedchange":
                        var speed = randomSpeed();
                        int elapsed2 = lastEvent.EndTime - lastDownload.Time;

                        lastDownload.EndTime = (int) ((lastDownload.Chunk.Size - elapsed2*connectionSpeed)/speed) + lastEvent.Time;

                    break;
                    
                }

               

                int elapsed = lastEvent.EndTime - currentTime;

                if (elapsed > player.Buffer) {
                    player.TimePlayed += player.Buffer;
                    player.Buffer = 0;
                } else {
                    player.TimePlayed += elapsed;
                    player.Buffer -= elapsed;
                }

                currentTime = lastEvent.EndTime;
                
                chartTime.Add(currentTime);
                chartBuffer.Add(player.Buffer);
                chartSpeed.Add(connectionSpeed);
                chartQuality.Add(player.Quality);

            } 
        }

        Event getEvent() {
            events.Sort((e1, e2) => e1.EndTime.CompareTo(e2.EndTime));
            var evt = events.First();
            events.Remove(evt);
            return evt;
        }
        
        double getSpeedChangeTime() {
            Random rdm = new Random();
            double x = rdm.NextDouble();

            return (-1.0 / lambda) * Math.Log10(x);
        }

        int randomSpeed() {
            Random rdm = new Random();
            int x = rdm.Next(0, 10);

            return x < 5 ? 2 : 8;
        }
    }
}
