using System;
using System.Diagnostics;

namespace CmdMessenger.Test
{
    public class Blocking
    {
        private readonly Stopwatch stopWatch;
        private readonly int seconds;

        public Blocking(int seconds) {
            this.seconds = seconds;
            this.stopWatch = new Stopwatch();
            this.stopWatch.Start();
        }

        public bool Untill(Func<bool> func) {
            bool result = true;

            do {
                if (this.stopWatch.ElapsedMilliseconds > this.seconds * 1000) {
                    result = false;
                    break;
                }
            }
            while (!func());

            return result;
        }
    }
}
