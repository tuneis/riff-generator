using MidiSharp;
using RiffGeneratorWeb.RiffGenerator;
using System.Collections.Generic;

namespace RiffGeneratorWeb.ViewModels
{
    public class RiffGeneratorViewModel
    {
        public int AmountToGenerate { get; set; }
        public List<int> AllowableDurations { get; set; } = new List<int>();
        public List<int> AllowableOctaves { get; set; } = new List<int>();
        public List<int> AllowablePitches { get; set; } = new List<int>();
        public GeneralMidiInstrument Instrument { get; set; }
        public int Tempo { get; set; }
        public TimeSignature Time { get; set; }
        public string TrackName { get; set; }
    }
}
