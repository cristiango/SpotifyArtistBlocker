using System;
using System.Threading;

namespace BieberBlocker
{
    class Program
    {
        static void Main(string[] args)
        {
            new SpotifyHelper();

            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}