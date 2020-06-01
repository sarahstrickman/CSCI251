using System;
using System.Collections.Generic;

namespace ssWriter
{
    public enum Location
    {
        top,
        bottom
    }
    public static class splitWriter
    {

        private static Object lk;
        private static Dictionary<Location, List<string>> buffers;
        private static Dictionary<Location, List<string>> prevbuffers;
        private static int areaHeights = (Console.WindowHeight - 2) / 2;
        private static string emptyString;
        public static void Init() {
            lk = new Object();
            for (var i=0; i<Console.WindowWidth; i++){
                emptyString += " ";
            }
            buffers = new Dictionary<Location, List<string>>();
            prevbuffers = new Dictionary<Location, List<string>>();
            buffers[Location.top] = new List<string>();
            buffers[Location.bottom] = new List<string>();
            for (var i=0; i<areaHeights-1; i++){
                buffers[Location.top].Add("");
                buffers[Location.bottom].Add("");
            }
            Console.Clear();
            for (int i = 0; i < Console.BufferWidth; i++)
            {
                Console.SetCursorPosition(i, areaHeights);
                Console.Write('=');
            }
        }

        public static void Write(Location loc, string data)
        {
    
            buffers[loc].Add(data);
            while (buffers[loc].Count >= areaHeights-1){
                buffers[loc].RemoveAt(0);
            }

            if (loc == Location.top) {
                drawScreenTop(true);
            } 
            else 
            {
                drawScreenBottom(true);
            }
            //Console.ReadKey();
        }

        public static void Write(Location loc, char data)
        {
            bool didScroll = false;
            buffers[loc][buffers[loc].Count-1] += data;
            if (data == '\n') {
                buffers[loc].Add("");
            }
            while (buffers[loc].Count >= areaHeights-1){
                buffers[loc].RemoveAt(0);
                didScroll = true;
            }
            if (loc == Location.top) {
                drawScreenTop(didScroll);
            } else {
                drawScreenBottom(didScroll);
            }
        }

        private static void drawScreenTop(bool didScroll)
        {

            lock (lk) {
                // Draw the area divider
                //int currentLine = areaHeights - 1;

                if (didScroll) {
                    for (int i = 0; i < buffers[Location.top].Count; i++)
                    {
                        Console.SetCursorPosition(0, (i ));
                        Console.Write(emptyString);
                        Console.SetCursorPosition(0, (i));
                        Console.Write(buffers[Location.top][i].Trim());

                    }
                } else {
                    var i = buffers[Location.top].Count -1;
                    Console.SetCursorPosition(0, (i ));
                    Console.Write(emptyString);
                    Console.SetCursorPosition(0, (i));
                    Console.Write(buffers[Location.top][i].Trim());
                }
            
            }
        }
        private static void drawScreenBottom(bool didScroll)
        {
            lock(lk) {
                int currentLine = areaHeights+1;
                if (didScroll) {

                    for(int i = 0; i < buffers[Location.bottom].Count; i++)
                    {
                        Console.SetCursorPosition(0,  (i + currentLine));
                        Console.Write(emptyString);
                        Console.SetCursorPosition(0, (i+currentLine));
                        Console.Write(buffers[Location.bottom][i].Trim());
                        
                    }
                }
                else {
                    int i = buffers[Location.bottom].Count -1;
                    Console.SetCursorPosition(0,  (i + currentLine));
                    Console.Write(emptyString);
                    Console.SetCursorPosition(0, (i+currentLine));
                    Console.Write(buffers[Location.bottom][i].Trim());
                }
            }
        }
    }
}