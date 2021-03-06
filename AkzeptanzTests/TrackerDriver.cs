using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace AkzeptanzTests
{
    public class TrackerDriver
    {
        private Process _tracker;
        private string _antwort;

        public void Starte()
        {
            var assemble = Assembly.GetExecutingAssembly();
            var path = new DirectoryInfo(Path.GetDirectoryName(assemble.Location));
            Console.WriteLine(path);

            _tracker = Process.Start(new ProcessStartInfo
                                         {
                                             FileName = "NerdGolfTracker.exe",
                                             UseShellExecute = false,
                                             RedirectStandardOutput = true,
                                             RedirectStandardInput = true,
                                             CreateNoWindow = true,
                                         });
            SpeichereAntwort();
        }

        public void Beende()
        {
            _tracker.Kill();
        }

        public void EmpfangeAnweisung(string anweisung)
        {
            _tracker.StandardInput.WriteLine(anweisung);
            SpeichereAntwort();
        }

        private void SpeichereAntwort()
        {
            _antwort = _tracker.StandardOutput.ReadLine();
            while (_tracker.StandardOutput.Peek() >= 0)
                _antwort += _tracker.StandardOutput.ReadLine();
        }

        public void AssertThatAntwortContains(string format, params object[] objects)
        {
            Assert.That(_antwort, Contains.Substring(string.Format(format, objects)));
        }
    }
}