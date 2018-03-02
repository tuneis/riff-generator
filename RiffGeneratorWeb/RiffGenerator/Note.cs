using MidiSharp.Events.Voice.Note;
using System.Collections.Generic;

namespace RiffGeneratorWeb.RiffGenerator
{
    public class Note
    {
        // pitches
        public const string A = "A";
        public const string ASharp = "A#";
        public const string B = "B";
        public const string C = "C";
        public const string CSharp = "C#";
        public const string D = "D";
        public const string DSharp = "D#";
        public const string E = "E";
        public const string F = "F";
        public const string FSharp = "F#";
        public const string G = "G";
        public const string GSharp = "G#";

        // durations
        public const int _64th = 30;
        public const int _32nd = 60;
        public const int _16th = 120;
        public const int _8th = 240;
        public const int _Quarter = 480;
        public const int _Half = 960;
        public const int _Whole = 1920;
        public const int _64th_T = _64th - (_64th / 3);
        public const int _32nd_T = _32nd - (_32nd / 3);
        public const int _16th_T = _16th - (_16th / 3);
        public const int _8th_T = _8th - (_8th / 3);
        public const int _Quarter_T = _Quarter - (_Quarter / 3);
        public const int _Half_T = _Half - (_Half / 3);

        // fields
        public const int _Whole_T = _Whole - (_Whole / 3);
        public string Pitch { get; set; }
        public int Duration { get; set; }
        public byte Velocity { get; set; }
        public int Octave { get; set; }

        public Dictionary<int, string> Pitches = new Dictionary<int, string>()
        {

            { 0, E },
            { 1, F },
            { 2, FSharp },
            { 3, G },
            { 4, GSharp },
            { 5, A },
            { 6, ASharp },
            { 7, B },
            { 8, C },
            { 9, CSharp },
            { 10, D },
            { 11, DSharp },
        };

        public Dictionary<int, int> Durations = new Dictionary<int, int>()
        {
            { 0, _64th },
            { 1, _32nd },
            { 2, _16th },
            { 3, _8th },
            { 4, _Quarter },
            { 5, _Half },
            { 6, _Whole },
            { 7, _64th_T },
            { 8, _32nd_T },
            { 9, _16th_T },
            { 10, _8th_T },
            { 11, _Quarter_T },
            { 12, _Half_T },
            { 13, _Whole_T }
        };

        public Note()
        {
            this.Velocity = 72;
        }

        public Note(string pitch, int duration, int octave)
        {
            Duration = duration;
            Pitch = pitch;
            Octave = octave;
            Velocity = 72;
        }

        public OnNoteVoiceMidiEvent CreateNoteOn()
        {
            return new OnNoteVoiceMidiEvent(0, 0, this.Pitch + this.Octave, this.Velocity);
        }

        public OffNoteVoiceMidiEvent CreateNoteOff()
        {
            return new OffNoteVoiceMidiEvent(this.Duration, 0, this.Pitch + this.Octave, 64);
        }
    }
}
