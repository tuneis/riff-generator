using MidiSharp;
using MidiSharp.Events;
using MidiSharp.Events.Meta;
using MidiSharp.Events.Meta.Text;
using MidiSharp.Events.Voice;
using MidiSharp.Events.Voice.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiffGeneratorWeb.RiffGenerator
{
    /// <summary>
    /// Code borrow from https://github.com/stephentoub/MidiSharp.
    /// </summary>
    public class RG
    {
        /// <summary>
        /// Name of the track.
        /// </summary>
        private string _trackName;

        /// <summary>
        /// The instrument used in the track.
        /// </summary>
        private GeneralMidiInstrument _instrument;

        /// <summary>
        /// The tempo used in the midi file.
        /// </summary>
        private int _tempo;

        /// <summary>
        /// The time signature chosen.
        /// </summary>
        private TimeSignature _timeSignature;

        /// <summary>
        /// The amount of notes you want to generate.
        /// </summary>
        private int _totalNotes;

        /// <summary>
        /// All possible pitches.
        /// </summary>
        public static List<Tuple<string, int>> Pitches = new List<Tuple<string, int>>
        {
            new Tuple<string, int>("E", 0),
            new Tuple<string, int>("F", 1),
            new Tuple<string, int>("F#", 2),
            new Tuple<string, int>("G", 3),
            new Tuple<string, int>("G#", 4),
            new Tuple<string, int>("A", 5),
            new Tuple<string, int>("A#", 6),
            new Tuple<string, int>("B", 7),
            new Tuple<string, int>("C", 8),
            new Tuple<string, int>("C#", 9),
            new Tuple<string, int>("D", 10),
            new Tuple<string, int>("D#", 11)
        };

        /// <summary>
        /// All possible octaves.
        /// </summary>
        public static List<Tuple<string, int>> Octaves = new List<Tuple<string, int>>
        {
            new Tuple<string, int>("C0", 0),
            new Tuple<string, int>("C1", 1),
            new Tuple<string, int>("C2", 2),
            new Tuple<string, int>("C3", 3),
            new Tuple<string, int>("C4", 4),
            new Tuple<string, int>("C5", 5),
            new Tuple<string, int>("C6", 6),
            new Tuple<string, int>("C7", 7),
            new Tuple<string, int>("C8", 8),
            new Tuple<string, int>("C9", 9),
        };

        /// <summary>
        /// All possible octaves.
        /// </summary>
        public static List<Tuple<string, int, int>> Durations = new List<Tuple<string, int, int>>
        {
            new Tuple<string, int, int>("1/64", 0, 30),
            new Tuple<string, int, int>("1/32", 1, 60),
            new Tuple<string, int, int>("1/16", 2, 120),
            new Tuple<string, int, int>("1/8", 3, 240),
            new Tuple<string, int, int>("1/4", 4, 480),
            new Tuple<string, int, int>("1/2", 5, 960),
            new Tuple<string, int, int>("1/1", 6, 1920),
        };

        /// <summary>
        /// All possible time signatures.
        /// </summary>
        public static List<Tuple<string, int, TimeSignature>> TimeSignatures = new List<Tuple<string, int, TimeSignature>>
        {
            new Tuple<string, int, TimeSignature>("3/4", 0, new TimeSignature(3,4)),
            new Tuple<string, int, TimeSignature>("4/4", 1, new TimeSignature(4,4)),
            new Tuple<string, int, TimeSignature>("5/4", 2, new TimeSignature(5,4)),
            new Tuple<string, int, TimeSignature>("6/4", 3, new TimeSignature(6,4)),
        };

        /// <summary>
        /// All possible instruments.
        /// </summary>
        public static List<Tuple<string, int, GeneralMidiInstrument>> Instruments = Enum.GetValues(typeof(GeneralMidiInstrument))
            .OfType<GeneralMidiInstrument>()
            .Select(x => new Tuple<string, int, GeneralMidiInstrument>(x.ToString(), (int)x, x))
            .OrderBy(s => s.Item1)
            .ToList();

        /// <summary>
        /// The almighty constructor. Fear and respect the.
        /// </summary>
        /// <param name="trackName"></param>
        /// <param name="instrument"></param>
        /// <param name="tempo"></param>
        /// <param name="timeSignature"></param>
        /// <param name="noteCountToGenerate"></param>
        public RG(string trackName, GeneralMidiInstrument instrument, int tempo, TimeSignature timeSignature, int noteCountToGenerate)
        {
            _trackName = trackName;
            _instrument = instrument;
            _tempo = tempo;
            _timeSignature = timeSignature;
            _totalNotes = noteCountToGenerate;
        }


        //http://www.deluge.co/?q=midi-tempo-bpm
        /// <summary>
        /// Converts tempo into the value the library accepts as a tempo.
        /// </summary>
        /// <param name="tempo"></param>
        /// <returns></returns>
        private int ConvertToTempo(int tempo)
        {
            return 60000000 / tempo;
        }

        /// <summary>
        /// Sets the tempo.
        /// </summary>
        /// <param name="tempo"></param>
        /// <returns></returns>
        private TempoMetaMidiEvent SetTempo(int tempo)
        {
            return new TempoMetaMidiEvent(0, ConvertToTempo(tempo));
        }

        /// <summary>
        /// Sets time signature.
        /// </summary>
        /// <param name="time">Use the numerator to set the time signature.</param>
        /// <returns></returns>
        private TimeSignatureMetaMidiEvent SetTimeSignature(TimeSignature time)
        {
            return new TimeSignatureMetaMidiEvent(0, time.Numerator, time.Denominator, 24, 8);
        }

        /// <summary>
        /// Sets instrument.
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        private ProgramChangeVoiceMidiEvent SetInstrument(GeneralMidiInstrument instrument)
        {
            return new ProgramChangeVoiceMidiEvent(0, 0, instrument);
        }

        /// <summary>
        /// Sets the end of the track.
        /// </summary>
        /// <returns></returns>
        private EndOfTrackMetaMidiEvent SetEndOfTrack()
        {
            return new EndOfTrackMetaMidiEvent(0);
        }

        /// <summary>
        /// Sets the name of the track.
        /// </summary>
        /// <param name="trackName"></param>
        /// <returns></returns>
        private SequenceTrackNameTextMetaMidiEvent SetTrackName(string trackName)
        {
            return new SequenceTrackNameTextMetaMidiEvent(0, trackName);
        }

        /// <summary>
        /// Creates the MIDI sequence.
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        private MidiSequence CreateSequence(List<MidiTrack> tracks)
        {
            MidiSequence sequence = new MidiSequence(Format.One, 480);

            foreach (var track in tracks)
            {
                sequence.Tracks.Add(track);
            }

            return sequence;
        }

        /// <summary>
        /// Creates the midi track to add to the sequence.
        /// </summary>
        /// <param name="noteEvents"></param>
        /// <returns></returns>
        private MidiTrack CreateTrack(List<MidiEvent> noteEvents)
        {
            MidiTrack track = new MidiTrack();
            MidiEventCollection events = track.Events;

            // Set the track name
            events.Add(SetTrackName(_trackName));

            // Set the instrument
            events.Add(SetInstrument(_instrument));

            // Set the tempo
            events.Add(SetTempo(_tempo));

            // Set the time signature
            events.Add(SetTimeSignature(_timeSignature));

            // add on off note events to track
            foreach (var note in noteEvents)
            {
                events.Add(note);
            }

            // Set the end of track flag
            events.Add(SetEndOfTrack());

            return track;
        }

        /// <summary>
        /// Give me something completely random.
        /// </summary>
        /// <returns></returns>
        public MidiSequence Randomize()
        {
            try
            {
                // create list of tracks
                List<MidiTrack> tracks = new List<MidiTrack>();

                // randomized values for notes
                Random random = new Random();

                // event list for on and off notes
                List<MidiEvent> events = new List<MidiEvent>();

                for (int i = 0; i < _totalNotes; i++)
                {
                    // create the not and set the pitch and octave
                    Note note = new Note
                    {
                        Octave = random.Next(1, 8),
                    };
                    note.Pitch = note.Pitches[random.Next(0, 11)];
                    note.Duration = note.Durations[random.Next(0, 13)];

                    // create the note on and off from properties inside class
                    OnNoteVoiceMidiEvent on = note.CreateNoteOn();
                    OffNoteVoiceMidiEvent off = note.CreateNoteOff();

                    // add the events to the midievent list
                    events.Add(on);
                    events.Add(off);
                }

                // list of denoms
                int[] denominators = { 1, 2, 4, 8, 16, 32 };
                // create track with the randomly generate midi events
                MidiTrack track = CreateTrack(events);

                // add track to list
                tracks.Add(track);

                // create sequence
                MidiSequence midi = CreateSequence(tracks);

                return midi;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Generates a midi sequence to be written to a file.
        /// </summary>
        /// <param name="allowableOctaves"></param>
        /// <param name="allowablePitches"></param>
        /// <param name="allowableDurations"></param>
        /// <returns></returns>
        public MidiSequence Generate(int[] allowableOctaves, int[] allowablePitches, int[] allowableDurations)
        {
            try
            {
                // create list of tracks
                List<MidiTrack> tracks = new List<MidiTrack>();

                // randomized values for notes
                Random octaveRandom = new Random();
                Random pitchRandom = new Random();
                Random durationRandom = new Random();

                // event list for on and off notes
                List<MidiEvent> events = new List<MidiEvent>();

                for (int i = 0; i < _totalNotes; i++)
                {
                    // create the not and set the pitch and octave
                    Note note = new Note
                    {
                        Octave = allowableOctaves[octaveRandom.Next(0, allowableOctaves.Length)]
                    };
                    note.Pitch = note.Pitches[allowablePitches[pitchRandom.Next(0, allowablePitches.Length)]];
                    note.Duration = note.Durations[allowableDurations[durationRandom.Next(0, allowableDurations.Length)]];

                    // create the note on and off from properties inside class
                    OnNoteVoiceMidiEvent on = note.CreateNoteOn();
                    OffNoteVoiceMidiEvent off = note.CreateNoteOff();

                    // add the events to the midievent list
                    events.Add(on);
                    events.Add(off);
                }

                // create track 
                MidiTrack track = CreateTrack(events);

                // add track to list
                tracks.Add(track);

                // create sequence
                MidiSequence midi = CreateSequence(tracks);

                return midi;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
