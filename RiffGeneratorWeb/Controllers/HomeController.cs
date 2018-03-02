using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidiSharp;
using RiffGeneratorWeb.Models;
using RiffGeneratorWeb.RiffGenerator;
using RiffGeneratorWeb.ViewModels;

namespace RiffGeneratorWeb.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IHostingEnvironment _env;
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var rgVM = new RGViewModel
            {
                Instruments = new SelectList(RG.Instruments, "Item2", "Item1", RG.Instruments[(int)GeneralMidiInstrument.DistortionGuitar].ToString()),
                Pitches = new SelectList(RG.Pitches, "Item2", "Item1"),
                Durations = new SelectList(RG.Durations, "Item2", "Item1"),
                Octaves = new SelectList(RG.Octaves, "Item2", "Item1"),
                TimeSignatures = new SelectList(RG.TimeSignatures, "Item1", "Item1")
            };
            return View(rgVM);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Generate(RGViewModel vm)
        {

            // get file location for saving and reading
            string directory = Path.Combine(_env.WebRootPath, "midi");

            // check if directory exists if not create it
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // instantiate the riff generator
            var rg = new RG(vm.TrackName, RG.Instruments[vm.SelectedInstrument].Item3, vm.Tempo, RG.TimeSignatures[vm.SelectedTimeSignature].Item3, vm.TotalNotes);

            // generate the midi based on selected values
            var midi = rg.Generate(vm.SelectedOctaves, vm.SelectedPitches, vm.SelectedDurations);

            // get date for file parsing
            var now = DateTime.Now;
            var filePath = Path.Combine(directory, $"{vm.TrackName.Replace(" ", string.Empty)}_{Guid.NewGuid().ToString()}.mid");

            // save to file
            using (Stream file = System.IO.File.Create(filePath))
            {
                midi.Save(file);
            }

            // generate memory stream for download
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/x-midi", Path.GetFileName(filePath));
        }
    }
}
