using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using Reactive.Bindings;

using Runner.Models;

namespace Runner.ViewModels
{
    class RunItemViewModel : VmBase
    {
        private readonly OptionItem _optionItem;
        private System.Diagnostics.Process? cmd;
        private IObservable<bool> canRun;

        public RunItemViewModel(OptionItem optionItem)
        {
            _optionItem = optionItem;

            Parameters = _optionItem.Parameters
                .Select(i => new RunParameterViewModel(_optionItem, i))
                .ToArray();

            canRun = Parameters.Select(i => i.IsValid)
                .Merge().Select(CanActRun); //  ok
            //.ToObservable().Subscribe(i => CanActRun());  //  bad (( схуяли?
            //.CombineLatest(i => i.All(CanActRun));

            RunPath = canRun
                .Select(CreateRunPath)
                .ToReadOnlyReactiveProperty();

            CmdRefresh = CommandWrapper.Create("Refresh", ActRefresh);
            CmdRun = CommandWrapper.Create("RUN", ActRun, canRun);
        }


        public bool IsExpanded { get; set; }
        public string? Description => _optionItem.Description;
        public string? ProgramPath => _optionItem.ProgramPath;
        public RunParameterViewModel[] Parameters { get; }
        public IReadOnlyReactiveProperty<string?> RunPath { get; }


        #region actions

        public CommandWrapper CmdRefresh { get; private set; }
        public CommandWrapper CmdRun { get; private set; }

        private void ActRefresh()
        {
            foreach(var p in Parameters)
            {
                p.Refresh();
            }
        }

        private void ActRun()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                //Arguments = "/k " + RunPath,
                //Arguments = "/k ipconfig",
                //CreateNoWindow = false,
                FileName = "cmd.exe",
                //RedirectStandardError = true,
                RedirectStandardInput = true,
                //RedirectStandardOutput = true,
                //Verb = "runas",
                WorkingDirectory = System.IO.Path.GetDirectoryName(_optionItem.ProgramPath)
            };

            cmd = System.Diagnostics.Process.Start(psi);
            cmd.StandardInput.WriteLine(RunPath);
            //cmd.StandardInput.Flush();

            //cmd.WaitForExit();
        }

        private bool CanActRun(bool val)
            => !string.IsNullOrEmpty(ProgramPath) && Parameters.All(i => i.IsValid.Value);

        private string? CreateRunPath(bool isValid)
        {
            if (isValid)
            {
                var paramsList = Parameters
                    .Select(i => i.Value.Value)
                    .Select(NormalizeItem);
                return $"{NormalizeItem(_optionItem.ProgramPath)} {string.Join(" ", paramsList)}";
            }

            return null;
        }

        private static string NormalizeItem(string value)
        {
            return value.Contains(" ")
                ? $"\"{value}\""  //  экранирование
                : value;
        }

        #endregion
    }
}
