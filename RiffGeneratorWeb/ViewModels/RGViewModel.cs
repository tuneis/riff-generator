using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RiffGeneratorWeb.ViewModels
{
    public class RGViewModel
    {
        public SelectList Instruments { get; set; }
        public SelectList Pitches { get; set; }
        public SelectList Octaves { get; set; }
        public SelectList Durations { get; set; }
        public SelectList TimeSignatures { get; set; }

        [Required(ErrorMessage = "Track Name is required.")]
        public string TrackName { get; set; }

        [Required(ErrorMessage = "Tempo is required.")]
        public int Tempo { get; set; }

        [Required(ErrorMessage = "Total Notes is required.")]
        public int TotalNotes { get; set; }

        [Required(ErrorMessage = "Instrument is required.")]
        public int SelectedInstrument { get; set; }

        [Required(ErrorMessage = "Time Signature is required.")]
        public int SelectedTimeSignature { get; set; }

        [Required(ErrorMessage = "You must select at least one pitch.")]
        public int[] SelectedPitches { get; set; }

        [Required(ErrorMessage = "You must select at least one duration.")]
        public int[] SelectedDurations { get; set; }

        [Required(ErrorMessage = "You must select at least one octave.")]
        public int[] SelectedOctaves { get; set; }
    }
}
