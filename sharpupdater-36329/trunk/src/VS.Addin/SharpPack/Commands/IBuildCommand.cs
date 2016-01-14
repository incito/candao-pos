using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

namespace CnSharp.Delivery.VisualStudio.PackingTool.Commands
{
    interface IBuildCommand
    {
        string CommandName { get; set; }
        DTE2 Dte { get; set; }
        AddIn AddIn { get; set; }
        CommandBarControl Connect();
        bool Build();
    }
}
